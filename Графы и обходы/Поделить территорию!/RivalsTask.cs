using System.Collections.Generic;
using System.Linq;

namespace Rivals
{
    public class RivalsTask
    {
        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var queue = new Queue<OwnedLocation>();
            var visited = new HashSet<Point>();
            var chests = map.Chests.ToHashSet();
            for (int i = 0; i < map.Players.Length; i++)
                queue.Enqueue(new OwnedLocation(i, map.Players[i], 0));
            while (queue.Count != 0)
            {
                var location = queue.Dequeue();
                var point = location.Location;
                if (IsPointNotAvailable(map, point, visited)) continue;
                visited.Add(point);
                yield return new OwnedLocation(location.Owner,
                    new Point(point.X, point.Y), location.Distance);
                if (chests.Contains(point) ) continue;
                for (var dy = -1; dy <= 1; dy++)
                    for (var dx = -1; dx <= 1; dx++)
                        if (dx != 0 && dy != 0) continue;
                        else
                            queue.Enqueue(new OwnedLocation(location.Owner,
                            new Point(point.X + dx, point.Y + dy), location.Distance + 1));
            }
        }

        private static bool IsPointNotAvailable(Map map, Point point,
            HashSet<Point> visited)
        {
            return point.X < 0 || point.X >= map.Maze.GetLength(0) ||
                    point.Y < 0 || point.Y >= map.Maze.GetLength(1) ||
                    map.Maze[point.X, point.Y] != MapCell.Empty ||
                    visited.Contains(point);
        }
    }
}