namespace Predictive_Lifestyle_Project.Services.Transport
{
    public sealed class PredictRequest
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
    }
}
