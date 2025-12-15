using Anirut.Evacuation.Api.Data;
using Anirut.Evacuation.Api.Data.Entities;
using Google.OrTools.ConstraintSolver;

namespace Anirut.Evacuation.Api.Services.PlanServices;

public class A
{
    public string ZoneId { get; set; } = string.Empty;
    public string VecicleId { get; set; } = string.Empty;
    public string Eta { get; set; } = string.Empty;
    public int NumberOfPeople { get; set; }

}

public class PlanService
{
    private readonly DataContext _context;

    public PlanService(DataContext context)
    {
        _context = context;
    }

    public List<A> CalculatePlan()
    {
        var vehicles = _context.Vehicles.ToList();
        var zones = _context.Zones.ToList();
        var result = Solve(zones, vehicles);

        var planResults = new List<A>();

        planResults = result.SelectMany(r => 
            r.ZoneIdsVisited.Select(zid => new A 
            { 
                ZoneId = zid, 
                VecicleId = r.VehicleId, 
                Eta = "", 
                NumberOfPeople = zones.First(z => z.ZoneId == zid).NumberOfPeople 
            })
        ).ToList();

        return planResults;
    }

    public class RoutingSolution
    {
        public string VehicleId { get; set; } = string.Empty;
        public List<string> ZoneIdsVisited { get; set; } = [];
        public double TotalTimeSeconds { get; set; }
    }
    public List<RoutingSolution> Solve(List<ZoneEntity> rawZones, List<VehicleEntity> vehicles)
    {
        int splitSize = vehicles.Max(v => v.Capacity);
        var virtualZones = SplitZones(rawZones, splitSize);

        var allLocations = new List<ZoneEntity>();
        allLocations.AddRange(virtualZones);

        int vehicleStartIndex = allLocations.Count;
        allLocations.AddRange(vehicles.Select(v => new ZoneEntity
        {
            Location = v.Location,
            ZoneId = $"VEHICLE_DEPOT_{v.VehicleId}",
        }));

        int[] startIndices = new int[vehicles.Count];
        int[] endIndices = new int[vehicles.Count];

        for (int i = 0; i < vehicles.Count; i++)
        {
            int locIndex = vehicleStartIndex + i;
            startIndices[i] = locIndex;
            endIndices[i] = locIndex;
        }

        var manager = new RoutingIndexManager(
            allLocations.Count,
            vehicles.Count,
            startIndices,
            endIndices
        );

        var routing = new RoutingModel(manager);

        for (int i = 0; i < vehicles.Count; i++)
        {
            int vehicleIndex = i;
            double velocity = vehicles[i].Speed;

            int transitCallbackIndex = routing.RegisterTransitCallback((fromIndex, toIndex) =>
            {
                var fromNode = allLocations[manager.IndexToNode(fromIndex)];
                var toNode = allLocations[manager.IndexToNode(toIndex)];

                bool isFromDepot = fromIndex >= virtualZones.Count;
                bool isToDepot = toIndex >= virtualZones.Count;
                if (!isFromDepot && !isToDepot && (fromIndex != toIndex))
                {
                    return long.MaxValue;
                }


                double distanceKm = fromNode.Location.HaversineDistanceTo(toNode.Location);

                double priorityFactor = 6 - (int)toNode.UrgencyLevel;

                int toNodeIndex = manager.IndexToNode(toIndex);
                if (toNodeIndex < virtualZones.Count)
                {
                    int p = 6 - (int)virtualZones[toNodeIndex].UrgencyLevel;
                    priorityFactor = 0.5 + (p / 5);
                }

                double timeHours = distanceKm / velocity;
                double weightedCost = timeHours * priorityFactor;

                return (long)(weightedCost * 3600);
            });

            routing.SetArcCostEvaluatorOfVehicle(transitCallbackIndex, vehicleIndex);
        }

        int demandCallbackIndex = routing.RegisterUnaryTransitCallback((long fromIndex) =>
        {
            int nodeIndex = manager.IndexToNode(fromIndex);
            if (nodeIndex >= virtualZones.Count) return 0;
            return virtualZones[nodeIndex].NumberOfPeople;
        });

        routing.AddDimensionWithVehicleCapacity(
            demandCallbackIndex,
            0,
            vehicles.Select(v => (long)v.Capacity).ToArray(),
            true,
            "Capacity");

        RoutingSearchParameters searchParameters =
            operations_research_constraint_solver.DefaultRoutingSearchParameters();
        searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;

        Assignment solution = routing.SolveWithParameters(searchParameters);

        return ExtractSolution(manager, routing, solution, vehicles, virtualZones);
    }

    private List<ZoneEntity> SplitZones(List<ZoneEntity> zones, int maxSplitSize)
    {
        var result = new List<ZoneEntity>();
        foreach (var zone in zones)
        {
            int remaining = zone.NumberOfPeople;
            while (remaining > 0)
            {
                int take = Math.Min(remaining, maxSplitSize);
                result.Add(new ZoneEntity
                {
                    ZoneId = zone.ZoneId,
                    Location = zone.Location,
                    NumberOfPeople = take,
                    UrgencyLevel = zone.UrgencyLevel
                });
                remaining -= take;
            }
        }
        return result;
    }

    private List<RoutingSolution> ExtractSolution(RoutingIndexManager manager, RoutingModel routing, Assignment solution, List<VehicleEntity> vehicles, List<ZoneEntity> virtualZones)
    {
        var results = new List<RoutingSolution>();
        if (solution == null) return results;

        for (int i = 0; i < vehicles.Count; ++i)
        {
            var route = new RoutingSolution { VehicleId = vehicles[i].VehicleId, ZoneIdsVisited = new List<string>(), TotalTimeSeconds = 0 };
            long index = routing.Start(i);

            while (!routing.IsEnd(index))
            {
                int nodeIndex = manager.IndexToNode(index);
                if (nodeIndex < virtualZones.Count)
                {
                    route.ZoneIdsVisited.Add(virtualZones[nodeIndex].ZoneId);
                }
                index = solution.Value(routing.NextVar(index));
            }
            results.Add(route);
        }
        return results;
    }
}
