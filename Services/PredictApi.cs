using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

public sealed class PredictionServiceOptions
{
    public string BaseUrl { get; set; } = "";
    public string? ApiKey { get; set; }
    public int TimeoutSeconds { get; set; } = 15;
}

public interface IPredictApi
{
    Task<PredictResponse> PredictAsync(List<Dictionary<string, object?>> rows, CancellationToken ct = default);
    Task<List<FeatureImportanceItem>> GetFeatureImportanceAsync(CancellationToken ct = default);
    Task<List<TrainingStatsItem>> GetTrainingStatsAsync(List<Dictionary<string, object?>> rows, CancellationToken ct = default);
}

public sealed class PredictApi : IPredictApi
{
    private readonly HttpClient _http;
    private readonly PredictionServiceOptions _opts;

    public PredictApi(HttpClient http, IOptions<PredictionServiceOptions> opts)
    {
        _http = http;
        _opts = opts.Value;
    }

    private void AddHeaders(HttpRequestMessage req)
    {
        if (!string.IsNullOrWhiteSpace(_opts.ApiKey))
            req.Headers.Add("x-api-key", _opts.ApiKey);
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<PredictResponse> PredictAsync(List<Dictionary<string, object?>> rows, CancellationToken ct = default)
    {
        var req = new PredictRequest { Rows = rows };
        using var httpReq = new HttpRequestMessage(HttpMethod.Post, "/predict")
        {
            Content = JsonContent.Create(req)
        };
        AddHeaders(httpReq);
        using var resp = await _http.SendAsync(httpReq, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<PredictResponse>(cancellationToken: ct)) ?? new();
    }

    public async Task<List<FeatureImportanceItem>> GetFeatureImportanceAsync(CancellationToken ct = default)
    {
        using var httpReq = new HttpRequestMessage(HttpMethod.Get, "/feature-importance");
        AddHeaders(httpReq);
        using var resp = await _http.SendAsync(httpReq, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<List<FeatureImportanceItem>>(cancellationToken: ct)) ?? [];
    }

    public async Task<List<TrainingStatsItem>> GetTrainingStatsAsync(List<Dictionary<string, object?>> rows, CancellationToken ct = default)
    {
        var req = new PredictRequest { Rows = rows };
        using var httpReq = new HttpRequestMessage(HttpMethod.Post, "/training-stats")
        {
            Content = JsonContent.Create(req)
        };
        AddHeaders(httpReq);
        using var resp = await _http.SendAsync(httpReq, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<List<TrainingStatsItem>>(cancellationToken: ct)) ?? [];
    }

    // Factory to get a resilient handler (wired in Program.cs)
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(r => (int)r.StatusCode == 429)
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)));
}
