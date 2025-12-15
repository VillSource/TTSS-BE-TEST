using Anirut.Evacuation.Api.Services.PlanServices.Models;

namespace Anirut.Evacuation.Api.Services.PlanServices;

public interface IPlanService
{
    List<PlanResult> CalculatePlan();
}