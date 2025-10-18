using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Predictive_Lifestyle_Project.Data;
using Predictive_Lifestyle_Project.Models;
using Predictive_Lifestyle_Project.Models.ViewModels;
using Predictive_Lifestyle_Project.Services;
using Predictive_Lifestyle_Project.Services.Transport;

namespace Predictive_Lifestyle_Project.Controllers
{
    [Authorize]
    public class HealthController : Controller
    {
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
        [HttpGet]
        public IActionResult Create() => View(new HealthEntryViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HealthEntryViewModel vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.GetUserAsync(User);
            var entry = new HealthEntry
            {
                UserId = user!.Id,
                Age = vm.Age,
                SexAtBirth = vm.SexAtBirth,
                HeightCm = vm.HeightCm,
                WeightKg = vm.WeightKg,
                SystolicBp = vm.SystolicBp,
                DiastolicBp = vm.DiastolicBp,
                RestingHr = vm.RestingHr,
                StepsPerDay = vm.StepsPerDay,
                SleepHours = vm.SleepHours,
                Hdl = vm.Hdl,
                Ldl = vm.Ldl,
                Triglycerides = vm.Triglycerides,
                A1c = vm.A1c
            };

            _db.HealthEntries.Add(entry);
            await _db.SaveChangesAsync(ct);

            var req = new PredictRequest
            {
                Age = entry.Age,
                SexAtBirth = entry.SexAtBirth,
                HeightCm = entry.HeightCm,
                WeightKg = entry.WeightKg,
                Bmi = entry.Bmi,
                SystolicBp = entry.SystolicBp,
                DiastolicBp = entry.DiastolicBp,
                RestingHr = entry.RestingHr,
                StepsPerDay = entry.StepsPerDay,
                SleepHours = entry.SleepHours,
                Hdl = entry.Hdl,
                Ldl = entry.Ldl,
                Triglycerides = entry.Triglycerides,
                A1c = entry.A1c
            };

            var result = await _predictApi.PredictAsync(req, ct);

            var pred = new Prediction
            {
                HealthEntryId = entry.Id,
                UserId = entry.UserId,
                Score = (decimal)result.score,
                Label = result.label,
                DetailsJson = result.details_json
            };

            _db.Predictions.Add(pred);
            await _db.SaveChangesAsync(ct);

            return RedirectToAction(nameof(Result), new { id = pred.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Result(long id, CancellationToken ct)
        {
            var uid = _userManager.GetUserId(User);
            var p = await _db.Predictions
                             .Include(x => x.HealthEntry)
                             .FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid, ct);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpGet]
        public async Task<IActionResult> History(CancellationToken ct)
        {
            var uid = _userManager.GetUserId(User);
            var items = await _db.Predictions
                                 .Include(x => x.HealthEntry)
                                 .Where(x => x.UserId == uid)
                                 .OrderByDescending(x => x.CreatedUtc)
                                 .ToListAsync(ct);
            return View(items);
        }
    }
}
