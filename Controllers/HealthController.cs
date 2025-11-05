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

        try
        {
            var req = new PredictRequest
            {
                Age = entry.Age,
                SexAtBirth = entry.SexAtBirth,
                HeightIn = entry.HeightIn,
                WeightLbs = entry.WeightLbs,
                DailyCal = entry.DailyCal,
                RestingHr = entry.RestingHr,
                StepsPerDay = entry.StepsPerDay,
                SleepHours = entry.SleepHours,
                AlcoholicDrinksPerWeek = entry.AlcoholicDrinksPerWeek,
                SmokeOrVape = entry.SmokeOrVape,
            };

            var result = await _predictApi.PredictAsync(req, ct);

            var pred = new Prediction
            {
                HealthEntryId = entry.Id,
                UserId = uid,
                Score = (decimal)result.score,
                Label = result.label,
                DetailsJson = result.details_json
            };

            _db.Predictions.Add(pred);
            await _db.SaveChangesAsync(ct);
            predId = pred.Id;
        }
        catch
        {
            
        }

        return RedirectToAction(nameof(Success), new { id = predId });
    }


    [AllowAnonymous]
    [HttpGet("Result/{id:long}")]
    public async Task<IActionResult> Result(long id, CancellationToken ct)
    {
        var uid = _userManager.GetUserId(User) ?? ANON;
        var p = await _db.Predictions.Include(x => x.HealthEntry)
                                     .FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid, ct);
        if (p == null) return NotFound();
        return View(p);
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
