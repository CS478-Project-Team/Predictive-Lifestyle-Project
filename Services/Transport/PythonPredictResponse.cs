using System.Collections.Generic;

namespace Predictive_Lifestyle_Project.Services.Transport
{
    public sealed class PythonPredictResponse
    {
        public List<PythonPredictRow> results { get; set; } = new();
    }

    public sealed class PythonPredictRow
    {
        public int predicted { get; set; }
        public double confidence { get; set; }
        public Dictionary<string, object?> data { get; set; } = new();
    }
}