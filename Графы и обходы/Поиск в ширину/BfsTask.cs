using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class BfsTask
{
    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Chest[] chests)
    {
        var queue = new Queue<SinglyLinkedList<Point>>();
        var visited = new HashSet<Point>();
        var chestLocations = new HashSet<Point>(chests.Select(chest => chest.Location));

        queue.Enqueue(new SinglyLinkedList<Point>(start));
        visited.Add(start);

        while (queue.Count != 0)
        {
            var path = queue.Dequeue();
            var point = path.Value;

            if (chestLocations.Contains(point))
                yield return path;

            if (point.X < 0 || point.X >= map.Dungeon.GetLength(0) ||
                point.Y < 0 || point.Y >= map.Dungeon.GetLength(1)) continue;

            if (map.Dungeon[point.X, point.Y] != MapCell.Empty) continue;

            for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                {
                    if (dx != 0 && dy != 0) continue;

                    var nextPoint = new Point(point.X + dx, point.Y + dy);
                    if (visited.Contains(nextPoint)) continue;

                    queue.Enqueue(new SinglyLinkedList<Point>(nextPoint, path));
                    visited.Add(nextPoint);
                }
        }
    }
}