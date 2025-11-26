namespace Predictive_Lifestyle_Project.Services.Transport
{
    public sealed class PredictBatchRequest
    {
        public List<Dictionary<string, object?>> rows { get; set; } = new();
    }
}
