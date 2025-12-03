using System.Collections.Generic;

namespace Predictive_Lifestyle_Project.Services.Transport
{
    public sealed class PredictResponse
    {
        public List<PredictResponseRow> Results { get; set; } = new();
    }
}