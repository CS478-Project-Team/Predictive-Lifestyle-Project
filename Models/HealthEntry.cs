using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Predictive_Lifestyle_Project.Models
{
    public class HealthEntry
    {
        public long Id { get; set; }

        [Required]
        public string UserId { get; set; } = default!;  

        [Range(18, 100)]
        public int Age { get; set; }

        [Required, RegularExpression("M|F|O")]
        public string SexAtBirth { get; set; } = "O";

        [Range(100, 250)]
        public decimal HeightCm { get; set; }

        [Range(30, 300)]
        public decimal WeightKg { get; set; }

        [Range(60, 250)]
        public int? SystolicBp { get; set; }

        [Range(40, 140)]
        public int? DiastolicBp { get; set; }

        [Range(30, 200)]
        public int? RestingHr { get; set; }

        public int? StepsPerDay { get; set; }
        public decimal? SleepHours { get; set; }

        public decimal? Hdl { get; set; }
        public decimal? Ldl { get; set; }
        public decimal? Triglycerides { get; set; }
        public decimal? A1c { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public decimal Bmi => Math.Round(WeightKg / (HeightCm / 100m * HeightCm / 100m), 1);
    }
}
