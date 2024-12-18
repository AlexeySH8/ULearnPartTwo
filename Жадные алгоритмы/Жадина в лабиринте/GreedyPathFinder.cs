using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
    public List<Point> FindPathToCompleteGoal(State state)
    {
        var targets = state.Chests.ToHashSet();
        var result = new List<Point>();
        if (targets.Count < state.Goal) return result;
        var start = state.Position;
        var pathFinder = new DijkstraPathFinder();
        var expendedEnergy = 0;
        var currentScore = 0;
        while (targets.Count > 0 && currentScore != state.Goal)
        {
            var nextPath = pathFinder
                .GetPathsByDijkstra(state, start, targets).FirstOrDefault();
            if (nextPath == null) return result;
            expendedEnergy += nextPath.Cost;
            if (expendedEnergy > state.Energy) return result;
            result.AddRange(nextPath.Path.Skip(1).ToList());
            currentScore++;
            targets.Remove(nextPath.End);
            start = nextPath.End;
        }
        return result;
    }
}