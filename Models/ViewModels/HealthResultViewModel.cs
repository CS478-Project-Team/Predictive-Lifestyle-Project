using System.Collections.Generic;

namespace Predictive_Lifestyle_Project.Models.ViewModels
{
    public sealed class HealthResultViewModel
    {
        public HealthEntryViewModel Input { get; set; } = new();

        public string RiskBand { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public List<string> Suggestions { get; set; } = new();

        public long? PredictionId { get; set; }
    }
}
