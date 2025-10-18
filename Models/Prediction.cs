using System.ComponentModel.DataAnnotations;

namespace Predictive_Lifestyle_Project.Models
{
    public class Prediction
    {
        public long Id { get; set; }

        [Required]
        public long HealthEntryId { get; set; }

        [Required]
        public string UserId { get; set; } = default!;

        [Required, MaxLength(100)]
        public string ModelName { get; set; } = "health-risk-v1";

        public decimal Score { get; set; }
        public string? Label { get; set; }
        public string? DetailsJson { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public HealthEntry? HealthEntry { get; set; }
    }
}

