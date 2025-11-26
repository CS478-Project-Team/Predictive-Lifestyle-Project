using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Predictive_Lifestyle_Project.Services.Transport;

namespace Predictive_Lifestyle_Project.Services
{
    public sealed class PredictApi : IPredictApi
    {
        private readonly HttpClient _http;
        private readonly string _route;

        public PredictApi(HttpClient http, IOptions<PredictionServiceOptions> opts)
        {
            _http = http;
            var o = opts.Value;
            _http.BaseAddress = new System.Uri(o.BaseUrl);
            _http.Timeout = System.TimeSpan.FromSeconds(o.TimeoutSeconds);

            _route = "/predict";
        }

        public async Task<PredictResponse> PredictAsync(PredictRequest request, CancellationToken ct = default)
        {
            // 1) Wrap single record into `rows: [...]`
            var batch = new PredictBatchRequest();
            batch.rows.Add(new Dictionary<string, object?>
            {
                ["Age"] = request.Age,
                ["SexAtBirth"] = request.SexAtBirth,
                ["HeightIn"] = request.HeightIn,
                ["WeightLbs"] = request.WeightLbs,
                ["DailyCal"] = request.DailyCal,
                ["RestingHr"] = request.RestingHr,
                ["StepsPerDay"] = request.StepsPerDay,
                ["SleepHours"] = request.SleepHours,
                ["AlcoholicDrinksPerWeek"] = request.AlcoholicDrinksPerWeek,
                ["SmokeOrVape"] = request.SmokeOrVape
            });

            // 2) Call FastAPI
            var resp = await _http.PostAsJsonAsync(_route, batch, ct);
            resp.EnsureSuccessStatusCode();

            // 3) Read Python-shaped response
            var body = await resp.Content.ReadFromJsonAsync<PythonPredictResponse>(cancellationToken: ct);
            if (body is null || body.results.Count == 0)
                throw new System.InvalidOperationException("Empty prediction response.");

            var first = body.results[0];

            // 4) Map into your existing PredictResponse (used by HealthController)
            var label = first.predicted switch
            {
                0 => "Low",
                1 => "High",
                _ => "Unknown"
            };

            return new PredictResponse
            {
                score = first.confidence,
                label = label,
                details_json = JsonSerializer.Serialize(first.data)
            };
        }
    }
}
