using Anirut.Evacuation.Api.Data;
using Anirut.Evacuation.Api.Data.Entities;
using Anirut.Evacuation.Api.Services.PlanServices.Models;
using Google.OrTools.ConstraintSolver;

namespace Anirut.Evacuation.Api.Services.PlanServices;

public partial class PlanService : IPlanService
{
    private readonly DataContext _context;

    public PlanService(DataContext context)
    {
        _context = context;
    }

    public List<PlanResult> CalculatePlan()
    {
        var vehicles = _context.Vehicles.ToList();
        var zones = _context.Zones.ToList();

        if (vehicles.Count == 0 || zones.Count == 0)
        {
            return [];
        }

        var result = Solve(zones, vehicles);

        var planResults = new List<PlanResult>();

        planResults = result.SelectMany(r =>
            r.ZoneIdsVisited.Select(zid =>
            {
                var zone = zones.First(z => z.ZoneId == zid);
                var vehicle = vehicles.First(v => v.VehicleId == r.VehicleId);
                var distanceKm = vehicle.Location.HaversineDistanceTo(zone.Location);
                var etaHours = distanceKm / vehicle.Speed;
                var etaMinutes = (int)(etaHours * 60);
                return new PlanResult
                {
                    ZoneId = zid,
                    VecicleId = r.VehicleId,
                    Eta = $"{etaMinutes} minutes",
                    NumberOfPeople = zone.NumberOfPeople
                };
            })).ToList();

        return planResults;
    }
    private List<RoutingSolution> Solve(List<ZoneEntity> rawZones, List<VehicleEntity> vehicles)
    {
        int splitSize = vehicles.Max(v => v.Capacity);
        var virtualZones = SplitBigZones(rawZones, splitSize);

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

                double distanceKm = fromNode.Location.HaversineDistanceTo(toNode.Location);

                double priorityFactor = 1;

                int toNodeIndex = manager.IndexToNode(toIndex);
                if (toNodeIndex < virtualZones.Count)
                {
                    int p = 5 - (int)virtualZones[toNodeIndex].UrgencyLevel;
                    priorityFactor = 0.5 + (p / 10);
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

    private List<ZoneEntity> SplitBigZones(List<ZoneEntity> zones, int maxSplitSize)
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
