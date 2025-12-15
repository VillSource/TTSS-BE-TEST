using Anirut.Evacuation.Api.Services.EvacuationStatusServices.Models;

namespace Anirut.Evacuation.Api.Services.EvacuationStatusServices
{
    public interface IEvacuationStatusService
    {
        Task<List<EvacuationStatusResult>> GetStatus();
        Task UpdateStatus(List<EvacuationStatusUpdate> data);
    }
}