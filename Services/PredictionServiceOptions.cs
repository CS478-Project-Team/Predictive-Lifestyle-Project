namespace Predictive_Lifestyle_Project.Services
{
    public sealed class PredictionServiceOptions
    {
        public string BaseUrl { get; set; } = "http://127.0.0.1:5001";
        public int TimeoutSeconds { get; set; } = 30;
    }
}

