using Anirut.Evacuation.Api.Common.Enumns;
using Anirut.Evacuation.Api.Data;
using Anirut.Evacuation.Api.Data.Entities;
using Anirut.Evacuation.Api.Services.PlanServices;

namespace Anirut.Evacuation.Api.Test;

public class VehicleRoutingProblemTest
{
    [Fact]
    public void Test()
    {
        var sut = new PlanService(null!);

        List<VehicleEntity> vehicles =
        [
            new()
            {
                VehicleId = "Vehicle1",
                Location=new(0,0),
                Speed = 60,
                Capacity = 40,
                Type = "truck"
            },
            new()
            {
                VehicleId = "Vehicle2",
                Location=new(1000,1000),
                Speed = 60,
                Capacity = 10,
                Type = "truck"
            }
        ];

        List<ZoneEntity> zones =
        [
            new()
            {
                ZoneId = "Zone1",
                Location=new(100,100),
                NumberOfPeople= 20,
                UrgencyLevel = UrgencyLevel.High
            },
            new()
            {
                ZoneId = "Zone2",
                Location=new(0,100),
                NumberOfPeople= 20,
                UrgencyLevel = UrgencyLevel.Low
            }
        ];

        var result = sut.Solve(zones,vehicles);
    }
}
