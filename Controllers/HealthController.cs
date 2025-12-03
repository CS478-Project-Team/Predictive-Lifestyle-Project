using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Predictive_Lifestyle_Project.Data;
using Predictive_Lifestyle_Project.Models;
using Predictive_Lifestyle_Project.Models.ViewModels;
using Predictive_Lifestyle_Project.Services;
using Predictive_Lifestyle_Project.Services.Transport;

[Route("Health")]
[Authorize]
public sealed class HealthController : Controller
{
    private const string ANON = "ANON";
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPredictApi _predictApi;

    public HealthController(ApplicationDbContext db, UserManager<IdentityUser> um, IPredictApi predictApi)
    {
        _db = db;
        _userManager = um;
        _predictApi = predictApi;
    }

    [AllowAnonymous]
    [HttpGet("Create")]
    public IActionResult Create() => View(new HealthEntryViewModel());

    [AllowAnonymous]
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HealthEntryViewModel vm, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(vm);

        var uid = _userManager.GetUserId(User) ?? ANON;

        // 1. Save the health entry
        var entry = new HealthEntry
        {
            UserId = uid,
            Age = vm.Age,
            SexAtBirth = vm.SexAtBirth,
            HeightIn = vm.HeightIn,
            WeightLbs = vm.WeightLbs,
            DailyCal = vm.DailyCal,
            RestingHr = vm.RestingHr,
            StepsPerDay = vm.StepsPerDay,
            SleepHours = vm.SleepHours,
            AlcoholicDrinksPerWeek = vm.AlcoholicDrinksPerWeek,
            SmokeOrVape = vm.SmokeOrVape,
        };

        _db.HealthEntries.Add(entry);
        await _db.SaveChangesAsync(ct);

        long? predId = null;
        string riskBand = "Unknown";
        double confidence = 0.0;
        List<string> suggestions = new();

        try
        {
            // 2. Build request for FastAPI model
            var req = new PredictRequest
            {
                Rows = new()
            {
                new Dictionary<string, object?>
                {
                    ["Age"] = entry.Age,
                    ["SexAtBirth"] = entry.SexAtBirth,
                    ["HeightIn"] = entry.HeightIn,
                    ["WeightLbs"] = entry.WeightLbs,
                    ["DailyCal"] = entry.DailyCal,
                    ["RestingHr"] = entry.RestingHr,
                    ["StepsPerDay"] = entry.StepsPerDay,
                    ["SleepHours"] = entry.SleepHours,
                    ["AlcoholicDrinksPerWeek"] = entry.AlcoholicDrinksPerWeek,
                    ["SmokeOrVape"] = entry.SmokeOrVape
                }
            }
            };

            // 3. Call prediction API
            var apiResponse = await _predictApi.PredictAsync(req, ct);
            var row = apiResponse.Results.FirstOrDefault();

            if (row != null)
            {
                riskBand = row.RiskBand;
                confidence = row.Confidence;
                suggestions = row.Suggestions ?? new List<string>();

                // 4. Save Prediction entity
                var pred = new Prediction
                {
                    HealthEntryId = entry.Id,
                    UserId = uid,
                    Score = (decimal)confidence,
                    Label = riskBand,
                    DetailsJson = System.Text.Json.JsonSerializer.Serialize(row)
                };

                _db.Predictions.Add(pred);
                await _db.SaveChangesAsync(ct);
                predId = pred.Id;
            }
        }
        catch
        {
            // TODO: log if you want
        }

        // 5. Build ViewModel for the result screen
        var resultVm = new HealthResultViewModel
        {
            Input = vm,
            RiskBand = riskBand,
            Confidence = confidence,
            Suggestions = suggestions,
            PredictionId = predId
        };

        // Show detailed result page instead of plain success
        return View("Result", resultVm);
    }


    [AllowAnonymous]
    [HttpGet("Result/{id:long}")]
    public async Task<IActionResult> Result(long id, CancellationToken ct)
    {
        var uid = _userManager.GetUserId(User) ?? ANON;

        var p = await _db.Predictions
                         .Include(x => x.HealthEntry)
                         .FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid, ct);

        if (p == null) return NotFound();

        // Try to recover the PredictResponseRow from DetailsJson
        PredictResponseRow? row = null;
        if (!string.IsNullOrWhiteSpace(p.DetailsJson))
        {
            try
            {
                row = JsonSerializer.Deserialize<PredictResponseRow>(p.DetailsJson);
            }
            catch
            {
                row = null; // if it's old-format JSON, just ignore it
            }
        }

        // Map HealthEntry → HealthEntryViewModel for display
        var inputVm = p.HealthEntry == null
            ? new HealthEntryViewModel()
            : new HealthEntryViewModel
            {
                Age = p.HealthEntry.Age,
                SexAtBirth = p.HealthEntry.SexAtBirth,
                HeightIn = p.HealthEntry.HeightIn,
                WeightLbs = p.HealthEntry.WeightLbs,
                DailyCal = p.HealthEntry.DailyCal,
                RestingHr = p.HealthEntry.RestingHr,
                StepsPerDay = p.HealthEntry.StepsPerDay,
                SleepHours = p.HealthEntry.SleepHours,
                AlcoholicDrinksPerWeek = p.HealthEntry.AlcoholicDrinksPerWeek,
                SmokeOrVape = p.HealthEntry.SmokeOrVape
            };

        // Build the unified result VM
        var vm = new HealthResultViewModel
        {
            Input = inputVm,
            PredictionId = p.Id,
            RiskBand = row?.RiskBand ?? p.Label ?? "Unknown",
            Confidence = row?.Confidence ?? (double)p.Score,
            Suggestions = row?.Suggestions ?? new List<string>()
        };

        return View("Result", vm);
    }

    [AllowAnonymous]
    [HttpGet("Success/{id:long?}")]
    public IActionResult Success(long? id)
    {
        var vm = new HealthSuccessViewModel { PredictionId = id };
        return View(vm);
    }

    [AllowAnonymous]
    [HttpGet("History")]
    public async Task<IActionResult> History(CancellationToken ct)
    {
        var uid = _userManager.GetUserId(User) ?? ANON;
        var items = await _db.Predictions.Include(x => x.HealthEntry)
                                         .Where(x => x.UserId == uid)
                                         .OrderByDescending(x => x.CreatedUtc)
                                         .ToListAsync(ct);
        return View(items);
    }
}
