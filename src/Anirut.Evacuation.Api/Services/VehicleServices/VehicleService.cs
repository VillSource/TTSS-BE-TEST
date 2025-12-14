using Anirut.Evacuation.Api.Data;
using Anirut.Evacuation.Api.Data.Entities;

namespace Anirut.Evacuation.Api.Services.VehicleServices;

public class VehicleService : IVehicleService
{
    private readonly DataContext _data;

    public VehicleService(DataContext data)
    {
        _data = data;
    }

    public Task AddRange(IEnumerable<VehicleEntity> data, CancellationToken ct = default)
    {
        _data.AddRangeAsync(data, ct);
        return _data.SaveChangesAsync(ct);
    }

    public Task<IEnumerable<VehicleEntity>> GetAll(CancellationToken ct = default)
    {
        var vehicles = _data.Vehicles.AsEnumerable();
        return Task.FromResult(vehicles);
    }
}
