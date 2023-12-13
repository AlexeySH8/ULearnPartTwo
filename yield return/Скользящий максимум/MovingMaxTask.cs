using System.Collections.Generic;

namespace yield;

public static class MovingMaxTask
{
    public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        LinkedList<double> maxValues = new LinkedList<double>();
        Queue<double> queue = new Queue<double>();
        foreach (DataPoint point in data)
        {
            queue.Enqueue(point.OriginalY);

            while (maxValues.Count > 0 && maxValues.Last.Value < point.OriginalY)
                maxValues.RemoveLast();

            maxValues.AddLast(point.OriginalY);

            if (queue.Count > windowWidth && queue.Dequeue() == maxValues.First.Value)
                maxValues.RemoveFirst();

            yield return point.WithMaxY(maxValues.First.Value);
        }
    }
}