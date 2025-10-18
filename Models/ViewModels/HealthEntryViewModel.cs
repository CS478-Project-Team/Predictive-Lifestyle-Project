using System.ComponentModel.DataAnnotations;

namespace Predictive_Lifestyle_Project.Models.ViewModels
{
    public class HealthEntryViewModel
    {
        [Range(18, 100)] public int Age { get; set; }
        [Required, RegularExpression("M|F|O")] public string SexAtBirth { get; set; } = "O";
        [Range(100, 250)] public decimal HeightCm { get; set; }
        [Range(30, 300)] public decimal WeightKg { get; set; }
        [Range(60, 250)] public int? SystolicBp { get; set; }
        [Range(40, 140)] public int? DiastolicBp { get; set; }
        [Range(30, 200)] public int? RestingHr { get; set; }
        public int? StepsPerDay { get; set; }
        public decimal? SleepHours { get; set; }
        public decimal? Hdl { get; set; }
        public decimal? Ldl { get; set; }
        public decimal? Triglycerides { get; set; }
        public decimal? A1c { get; set; }
    }
}
