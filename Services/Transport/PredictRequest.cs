namespace Predictive_Lifestyle_Project.Services.Transport
{
    public sealed class PredictRequest
    {
        public int Age { get; set; }
        public string SexAtBirth { get; set; } = "O";
        public decimal HeightCm { get; set; }
        public decimal WeightKg { get; set; }
        public decimal Bmi { get; set; }
        public int? SystolicBp { get; set; }
        public int? DiastolicBp { get; set; }
        public int? RestingHr { get; set; }
        public int? StepsPerDay { get; set; }
        public decimal? SleepHours { get; set; }
        public decimal? Hdl { get; set; }
        public decimal? Ldl { get; set; }
        public decimal? Triglycerides { get; set; }
        public decimal? A1c { get; set; }
    }
}
