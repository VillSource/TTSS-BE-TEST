using Anirut.Evacuation.Api.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Anirut.Evacuation.Api.Services.Database;

public class DatabaseService : IDatabaseService
{
    private readonly DataContext _data;
    private readonly IConnectionMultiplexer _redis;

    public DatabaseService(DataContext data, IConnectionMultiplexer redis)
    {
        _data = data;
        _redis = redis;
    }

    public async Task ClearAllData(CancellationToken ct = default)
    {
        _redis.GetServers();
        foreach (var server in _redis.GetServers())
        {
            await server.FlushDatabaseAsync();
        }
        await _data.Vehicles.ExecuteDeleteAsync(ct);
        await _data.SaveChangesAsync(ct);
    }
}
