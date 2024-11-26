using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class DungeonTask
{
    public static MoveDirection[] FindShortestPath(Map map)
    {
        var pathsToChestsFromStart = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
        var pathsToChestsFromExit = BfsTask.FindPaths(map, map.Exit, map.Chests).ToDictionary(x => x.Value);
        var chestsDictionary = map.Chests.ToDictionary(x => x.Location);
        var minLength = int.MaxValue;
        var paths = new List<(SinglyLinkedList<Point> FromStart, SinglyLinkedList<Point> FromEnd, int ChestValue)>();

        foreach (var pathFromStart in pathsToChestsFromStart)
        {
            var chestPosition = pathFromStart.Value;
            if (!pathsToChestsFromExit.TryGetValue(chestPosition, out var pathFromEnd))
                return Array.Empty<MoveDirection>();
            var totalPathLength = pathFromStart.Length + pathFromEnd.Length;
            var chestValue = chestsDictionary[chestPosition].Value;
            if (totalPathLength <= minLength) paths.Add((pathFromStart, pathFromEnd, chestValue));
        }

        if (paths.Count == 0)
        {
            var path = BfsTask.FindPaths(map, map.InitialPosition, new Chest[] { new Chest(map.Exit, 0) })
                .FirstOrDefault(defaultValue: null);
            if (path != null) { return ToMoveDirections(path); }
            return Array.Empty<MoveDirection>();
        }

        var minPath = paths.MinBy(x => (x.FromStart.Length + x.FromEnd.Length - 1, -x.ChestValue));
        return ToMoveDirections(MergePath(minPath.FromStart, minPath.FromEnd));
    }

    private static MoveDirection[] ToMoveDirections(SinglyLinkedList<Point> path)
    {
        var moveDirections = new MoveDirection[path.Length - 1];
        var index = moveDirections.Length - 1;

        var previousPoint = path.Value;
        path = path.Previous;

        foreach (var point in path)
        {
            var direction = Walker.ConvertOffsetToDirection(previousPoint - point);
            previousPoint = point;
            moveDirections[index--] = direction;
        }
        return moveDirections;
    }

    private static SinglyLinkedList<Point> MergePath(SinglyLinkedList<Point> firstPath,
                                               SinglyLinkedList<Point> secondPath)
    {
        secondPath = secondPath.Previous;
        foreach (var point in secondPath)
            firstPath = new SinglyLinkedList<Point>(point, firstPath);
        return firstPath;
    }
}