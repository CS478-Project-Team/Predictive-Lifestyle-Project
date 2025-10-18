namespace Predictive_Lifestyle_Project.Services.Transport
{
    public sealed class PredictResponse
    {
        public double score { get; set; }            // 0..1
        public string? label { get; set; }           // "Low"/"Moderate"/"High"
        public string? details_json { get; set; }    // optional explainer payload
    }
}
