using System.ComponentModel.DataAnnotations;

namespace Predictive_Lifestyle_Project.Models.ViewModels
{
    public class HealthEntryViewModel
    {
        [Range(18, 100)] public int Age { get; set; }
        [Required, RegularExpression("M|F|O")] public string SexAtBirth { get; set; } = "O";
        [Range(100, 250)] public decimal HeightIn { get; set; }
        [Range(30, 300)] public decimal WeightLbs { get; set; }
        [Range(60, 250)] public int? DailyCal { get; set; }
        [Range(30, 200)] public int? RestingHr { get; set; }
        [Range(1, 200)] public int? AlcoholicDrinksPerWeek { get; set; }
        public int? StepsPerDay { get; set; }
        public decimal? SleepHours { get; set; }
        [Required, RegularExpression("Y|N")] public string SmokeOrVape { get; set; } = "N";
        public decimal? Triglycerides { get; set; }
        public decimal? A1c { get; set; }
    }
}
