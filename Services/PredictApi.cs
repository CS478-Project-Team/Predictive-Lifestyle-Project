using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
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

            // Adjust if your FastAPI/Flask route differs
            _route = "/predict";
        }

        public async Task<PredictResponse> PredictAsync(PredictRequest request, CancellationToken ct = default)
        {
            var resp = await _http.PostAsJsonAsync(_route, request, ct);
            resp.EnsureSuccessStatusCode();

            var body = await resp.Content.ReadFromJsonAsync<PredictResponse>(cancellationToken: ct);
            if (body is null) throw new System.InvalidOperationException("Empty prediction response.");
            return body;
        }
    }
}
