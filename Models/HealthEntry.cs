using System.ComponentModel.DataAnnotations.Schema;

public sealed class HealthEntry
{
    public long Id { get; set; }
    public string UserId { get; set; } = "ANON";
    public int Age { get; set; }
    public string SexAtBirth { get; set; } = "O";
    public decimal HeightIn { get; set; }
    public decimal WeightLbs { get; set; }
    public int? DailyCal { get; set; }
    public int? RestingHr { get; set; }
    public int? StepsPerDay { get; set; }
    public decimal? SleepHours { get; set; }
    public decimal? AlcoholicDrinksPerWeek { get; set; }
    public string SmokeOrVape { get; set; } = "N";

    public DateTime CreatedUtc { get; set; }

    [NotMapped]
    public decimal Bmi
    {
        get
        {
            // convert to metric and compute BMI safely
            var kg = WeightLbs / 2.2046226218m;
            var m = HeightIn * 0.0254m;
            if (m <= 0) return 0;
            return Math.Round(kg / (m * m), 2);
        }
    }
}


