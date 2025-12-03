using System.ComponentModel.DataAnnotations;

namespace Predictive_Lifestyle_Project.Models.ViewModels
{
    public class HealthEntryViewModel
    {
        [Range(18, 100)] public int Age { get; set; }
        [Required, RegularExpression("M|F")] public string SexAtBirth { get; set; } = "M";
        [Range(30, 250)] public decimal HeightIn { get; set; }
        [Range(30, 800)] public decimal WeightLbs { get; set; }
        [Range(60, 10000)] public int? DailyCal { get; set; }
        [Range(30, 200)] public int? RestingHr { get; set; }
        [Range(0, 200)] public decimal? AlcoholicDrinksPerWeek { get; set; }
        public int? StepsPerDay { get; set; }
        public decimal? SleepHours { get; set; }
        [Required, RegularExpression("Y|N")] public string SmokeOrVape { get; set; } = "N";
    }
}
