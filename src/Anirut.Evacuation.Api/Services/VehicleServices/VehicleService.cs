using Anirut.Evacuation.Api.Data;
using Anirut.Evacuation.Api.Data.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Anirut.Evacuation.Api.Services.VehicleServices;

public class VehicleService : IVehicleService
{
    private readonly DataContext _data;
    private readonly IDistributedCache _cache;

    public VehicleService(DataContext data, IDistributedCache cache)
    {
        _data = data;
        _cache = cache;
    }

    public Task AddRange(IEnumerable<VehicleEntity> data, CancellationToken ct = default)
    {
        _data.AddRangeAsync(data, ct);
        return _data.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<VehicleEntity>> GetAll(CancellationToken ct = default)
    {
        var c = await _cache.GetStringAsync("A", ct);
        var vehicles = _data.Vehicles.AsEnumerable();
        await _cache.SetStringAsync("A", "TMP", ct);
        return vehicles;
    }
}
