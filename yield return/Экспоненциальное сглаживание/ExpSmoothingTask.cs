using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> points, double alpha)
    {
        DataPoint previousPoint = null;
        foreach (var point in points)
        {
            if (previousPoint == null)
            {
                var expSmoothedY = point.OriginalY;
                previousPoint = point.WithExpSmoothedY(expSmoothedY);
                yield return previousPoint;
            }
            else
            {
                var expSmoothedY = (alpha * point.OriginalY) + ((1 - alpha) * previousPoint.ExpSmoothedY);
                var currentPoint = point.WithExpSmoothedY(expSmoothedY);
                previousPoint = new DataPoint(currentPoint);
                yield return currentPoint;
            }
        }
    }
}
