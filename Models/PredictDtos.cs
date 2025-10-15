using System.Text.Json.Serialization;

public sealed class PredictRequest
{
    [JsonPropertyName("rows")]
    public List<Dictionary<string, object?>> Rows { get; set; } = new();
}

public sealed class PredictResponse
{
    [JsonPropertyName("results")]
    public List<PredictResponseRow> Results { get; set; } = new();
}

public sealed class PredictResponseRow
{
    [JsonPropertyName("predicted")]
    public int Predicted { get; set; }

    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }

    [JsonPropertyName("data")]
    public Dictionary<string, object?> Data { get; set; } = new();
}

public sealed class FeatureImportanceItem
{
    public string Feature { get; set; } = "";
    public double Importance { get; set; }
}

public sealed class TrainingStatsItem
{
    public string Metric { get; set; } = "";
    public double Training_Average { get; set; }
    public double? Input_Average { get; set; }
    public double? Difference { get; set; }
}
