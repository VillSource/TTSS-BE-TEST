using Anirut.Evacuation.Api.Data.Entities;

namespace Anirut.Evacuation.Api.Services.ZoneServices
{
    public interface IZoneService
    {
        Task AddRange(IEnumerable<ZoneEntity> data, CancellationToken ct = default);
    }
}