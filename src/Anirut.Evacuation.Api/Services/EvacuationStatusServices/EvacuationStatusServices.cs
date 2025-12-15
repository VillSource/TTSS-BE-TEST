using Anirut.Evacuation.Api.Data;

namespace Anirut.Evacuation.Api.Services.EvacuationStatusServices;

public class A
{
    public string ZoneId { get; set; } = string.Empty;
    public int TotalEvacuated { get; set; }
    public int RemainingPeople { get; set; }
    public string LastVehiclesUse { get; set; } = string.Empty;

}

public class B
{
    public string VehicleId { get; set; } = string.Empty;
    public string ZoneId { get; set; } = string.Empty;
    public int TotalMove { get; set; }
}

public class EvacuationStatusServices
{
    private readonly DataContext _data;

    public EvacuationStatusServices(DataContext data)
    {
        _data = data;
    }

    public async Task<List<A>> GetStatus()
    {
        return _data.Zones.Select(z => new A
        {
            RemainingPeople = z.NumberOfPeople,
            LastVehiclesUse = z.LastVehicleId ?? string.Empty,
            TotalEvacuated = z.TotalPeople,
            ZoneId = z.ZoneId
        }).ToList();
    }

    public async Task UpdateStatus(List<B> data)
    {
        var zoneIds = data.Select(d => d.ZoneId).Distinct().ToList();
        var zone = _data.Zones.Where(z => zoneIds.Contains(z.ZoneId));

        foreach (var z in zone)
        {
            var dataUpdateForZone = data.Where(d => d.ZoneId == z.ZoneId);
            var totalMove = dataUpdateForZone.Sum(d => d.TotalMove);
            var lastVehicle = dataUpdateForZone.LastOrDefault()?.VehicleId ?? string.Empty;
            z.NumberOfPeople -= totalMove;
            z.LastVehicleId = lastVehicle;
        }

        _data.Zones.UpdateRange(zone);
        await _data.SaveChangesAsync();
    }
}
