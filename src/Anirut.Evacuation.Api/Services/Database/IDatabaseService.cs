
namespace Anirut.Evacuation.Api.Services.Database
{
    public interface IDatabaseService
    {
        Task ClearAllData(CancellationToken ct = default);
    }
}