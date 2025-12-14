using Anirut.Evacuation.Api.Data.Entities;

namespace Anirut.Evacuation.Api.Services.VehicleServices
{
    public interface IVehicleService
    {
        Task AddRange(IEnumerable<VehicleEntity> data, CancellationToken ct = default);
    }
}