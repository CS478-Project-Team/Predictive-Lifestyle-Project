using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Predictive_Lifestyle_Project.Services.Transport
{
    public sealed class PredictResponseRow
    {
        [JsonPropertyName("predicted")]
        public int Predicted { get; set; }

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }

        [JsonPropertyName("risk_band")]
        public string RiskBand { get; set; } = string.Empty;

        [JsonPropertyName("suggestions")]
        public List<string> Suggestions { get; set; } = new();

        [JsonPropertyName("data")]
        public Dictionary<string, object?> Data { get; set; } = new();
    }
}
