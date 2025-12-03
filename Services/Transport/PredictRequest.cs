namespace Predictive_Lifestyle_Project.Services.Transport
{
    public sealed class PredictRequest
    {
        public List<Dictionary<string, object?>> Rows { get; set; } = new();
    }
}
