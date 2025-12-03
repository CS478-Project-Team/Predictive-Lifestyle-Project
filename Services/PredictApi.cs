using System;
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
            _http.BaseAddress = new Uri(o.BaseUrl);
            _http.Timeout = TimeSpan.FromSeconds(o.TimeoutSeconds);

            _route = "/predict";
        }

        public async Task<PredictResponse> PredictAsync(PredictRequest request, CancellationToken ct = default)
        {
            // request.Rows should already be filled by the controller
            var resp = await _http.PostAsJsonAsync(_route, request, ct);
            resp.EnsureSuccessStatusCode();

            var body = await resp.Content.ReadFromJsonAsync<PredictResponse>(cancellationToken: ct);
            if (body is null || body.Results is null || body.Results.Count == 0)
            {
                throw new InvalidOperationException("Empty prediction response.");
            }

            return body;
        }
    }
}
