using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

class DijkstraData
{
    public Point Previous { get; set; }
    public int Price { get; set; }
}

public class DijkstraPathFinder
{
    public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
        IEnumerable<Point> targets)
    {
        var visited = new HashSet<Point>();
        var track = new Dictionary<Point, DijkstraData>();
        track[start] = new DijkstraData { Price = 0, Previous = start };
        visited.Add(start);
        while (visited.Count > 0)
        {
            Point toOpen = start;
            var bestPrice = double.PositiveInfinity;
            foreach (var e in visited)
            {
                if (track.ContainsKey(e) && track[e].Price < bestPrice)
                {
                    bestPrice = track[e].Price;
                    toOpen = e;
                }
            }
            if (targets.Contains(toOpen))
            {
                var path = MakePath(toOpen, track);
                if (path != null)
                    yield return path;
            }
            for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                {
                    if (dx != 0 && dy != 0) continue;
                    visited.Remove(toOpen);
                    var nextPoint = new Point(toOpen.X + dx, toOpen.Y + dy);
                    if (IsPointAvailable(nextPoint, state))
                    {
                        var currentPrice = track[toOpen].Price +
                            state.CellCost[nextPoint.X, nextPoint.Y];
                        if (!track.ContainsKey(nextPoint) || track[nextPoint].Price > currentPrice)
                        {
                            track[nextPoint] = new DijkstraData
                            { Previous = toOpen, Price = currentPrice };
                            visited.Add(nextPoint);
                        }
                    }
                }
        }
    }

    private bool IsPointAvailable(Point point, State state)
    {
        return point.X >= 0 && point.X < state.MapWidth &&
               point.Y >= 0 && point.Y < state.MapHeight &&
               !state.IsWallAt(point);
    }

    private PathWithCost MakePath(Point end,
        Dictionary<Point, DijkstraData> track)
    {
        var list = new List<Point>();
        var cost = track[end].Price;
        while (end != track[end].Previous)
        {
            list.Add(end);
            end = track[end].Previous;
        }
        list.Add(end);
        list.Reverse();
        return new PathWithCost(cost, list.ToArray());
    }
}