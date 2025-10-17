using Microsoft.AspNetCore.Mvc;

namespace Predictive_Lifestyle_Project.Controllers
{
    public sealed class PredictionsController : Controller
    {
        private readonly IPredictApi _api;
        public PredictionsController(IPredictApi api) => _api = api;

        [HttpGet]
        public IActionResult Create() => View(new UserInputViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserInputViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // map VM -> Python row
            var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
            {
                ["Category 1"] = vm.Col_Cat_1,
                ["Numeric 1"] = vm.Col_Num_1,
                ["Numeric 2"] = vm.Col_Num_2,
                ["Numeric 3"] = vm.Col_Num_3,
            };

            try
            {
                var response = await _api.PredictAsync(new() { row });
                var importances = await _api.GetFeatureImportanceAsync();
                var stats = await _api.GetTrainingStatsAsync(new() { row });

                ViewBag.Importance = importances;
                ViewBag.Stats = stats;
                return View("Result", response);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Prediction failed: {ex.Message}");
                return View(vm);
            }
        }
    }
}