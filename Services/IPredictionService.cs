using Predictive_Lifestyle_Project.Models;

namespace Predictive_Lifestyle_Project.Services
{
    public interface IPredictionService
    {
        Task<(decimal score, string? label, string? detailsJson)>
            PredictAsync(HealthEntry entry, CancellationToken ct = default);
    }
}

