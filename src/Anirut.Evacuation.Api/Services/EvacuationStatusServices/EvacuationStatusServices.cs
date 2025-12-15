using Anirut.Evacuation.Api.Data;
using Anirut.Evacuation.Api.Services.EvacuationStatusServices.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Anirut.Evacuation.Api.Services.EvacuationStatusServices;

public class EvacuationStatusServices : IEvacuationStatusService
{
    private readonly DataContext _data;
    private readonly IDistributedCache _cache;
    private readonly string _cacheKey = nameof(EvacuationStatusServices);

    public EvacuationStatusServices(DataContext data, IDistributedCache cache)
    {
        _data = data;
        _cache = cache;
    }

    public async Task<List<EvacuationStatusResult>> GetStatus()
    {
        var cachedData = await _cache.GetStringAsync(_cacheKey);

        if (cachedData != null)
        {
            return JsonSerializer.Deserialize<List<EvacuationStatusResult>>(cachedData)
                ?? [];
        }

        var result = _data.Zones.Select(z => new EvacuationStatusResult
        {
            RemainingPeople = z.NumberOfPeople,
            LastVehiclesUse = z.LastVehicleId ?? string.Empty,
            TotalEvacuated = z.TotalPeople,
            ZoneId = z.ZoneId
        }).ToList();

        await _cache.SetStringAsync(_cacheKey, JsonSerializer.Serialize(result), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });

        return result;
    }

    public async Task UpdateStatus(List<EvacuationStatusUpdate> data)
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
            if (z.NumberOfPeople < 0)
            {
                z.NumberOfPeople = 0;
            }
        }

        _data.Zones.UpdateRange(zone);
        await _data.SaveChangesAsync();
        await _cache.RemoveAsync(_cacheKey);
    }
}
