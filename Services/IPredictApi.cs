using System.Threading;
using System.Threading.Tasks;
using Predictive_Lifestyle_Project.Services.Transport;

namespace Predictive_Lifestyle_Project.Services
{
    public interface IPredictApi
    {
        Task<PredictResponse> PredictAsync(PredictRequest request, CancellationToken ct = default);
    }
}
