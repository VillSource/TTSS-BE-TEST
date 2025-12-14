using Anirut.Evacuation.Api.Data;
using Anirut.Evacuation.Api.Data.Entities;

namespace Anirut.Evacuation.Api.Services.ZoneServices;

public class ZoneService : IZoneService
{
    private readonly DataContext _data;

    public ZoneService(DataContext data)
    {
        _data = data;
    }

    public async Task AddRange(IEnumerable<ZoneEntity> data, CancellationToken ct = default)
    {
        await _data.AddRangeAsync(data, ct);
        await _data.SaveChangesAsync(ct);
    }
}
