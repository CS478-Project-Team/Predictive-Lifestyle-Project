using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Predictive_Lifestyle_Project.Models;

namespace Predictive_Lifestyle_Project.Services
{
    public class PythonPredictionService : IPredictionService
    {
        private readonly HttpClient _http;
        private readonly string _route;

        public PythonPredictionService(HttpClient http, IOptions<PredictionOptions> opts)
        {
            _http = http;
            _http.BaseAddress = new Uri(opts.Value.EndpointBase);
            _route = opts.Value.ModelRoute;
        }

        public async Task<(decimal score, string? label, string? detailsJson)> PredictAsync(HealthEntry e, CancellationToken ct = default)
        {
            var payload = new
            {
                age = e.Age,
                sex_at_birth = e.SexAtBirth,
                height_cm = e.HeightIn,
                weight_kg = e.WeightLbs,
                bmi = e.Bmi,
                sbp = e.DailyCal,
                dbp = e.DiastolicBp,
                rhr = e.RestingHr,
                steps_per_day = e.StepsPerDay,
                sleep_hours = e.SleepHours,
                hdl = e.Hdl,
                ldl = e.Ldl,
                triglycerides = e.Triglycerides,
                a1c = e.A1c
            };

            var resp = await _http.PostAsJsonAsync(_route, payload, ct);
            resp.EnsureSuccessStatusCode();

            var doc = await resp.Content.ReadFromJsonAsync<PredictionResponse>(cancellationToken: ct);
            if (doc is null) throw new InvalidOperationException("Empty prediction response.");

            return ((decimal)doc.score, doc.label, doc.details_json);
        }

        private sealed class PredictionResponse
        {
            public double score { get; set; }
            public string? label { get; set; }
            public string? details_json { get; set; }
        }
    }

    public class PredictionOptions {
        public string EndpointBase { get; set; } = default!;
        public string ModelRoute { get; set; } = "/predict";
    }
}
