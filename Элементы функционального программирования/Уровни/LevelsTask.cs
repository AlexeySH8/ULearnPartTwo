using System;
using System.Collections.Generic;

namespace func_rocket;

public class LevelsTask
{
    static readonly Physics standardPhysics = new();
    static readonly Vector startPosition = new Vector(200, 500);
    static readonly Rocket rocket = new Rocket(startPosition, Vector.Zero, -0.5 * Math.PI);
    static readonly Vector target = new Vector(600, 200);
    static readonly Vector anomaly = (target + startPosition) / 2;

    static readonly Gravity whiteHoleGravity = (size, v) =>
    {
        var d = (target - v).Length;
        return (target - v).Normalize() * (-140 * d) / (d * d + 1);
    };

    static readonly Gravity blackHoleGravity = (size, v) =>
    {
        var d = (anomaly - v).Length;
        return (anomaly - v).Normalize() * (300 * d) / (d * d + 1);
    };

    public static IEnumerable<Level> CreateLevels()
    {
        yield return new Level("Zero", rocket, target,
            (size, v) => Vector.Zero, standardPhysics);

        yield return new Level("Heavy", rocket, target,
            (size, v) => new Vector(0, 0.9), standardPhysics);

        yield return new Level("Up", rocket, new Vector(700, 500),
            (size, v) => new Vector(0, -300 / (size.Y - v.Y + 300.0)), standardPhysics);

        yield return new Level("WhiteHole", rocket,
            target, whiteHoleGravity, standardPhysics);

        yield return new Level("BlackHole", rocket,
            target, blackHoleGravity, standardPhysics);

        yield return new Level("BlackAndWhite", rocket, target,
            (size, v) => (blackHoleGravity(size, v) + whiteHoleGravity(size, v)) / 2,
            standardPhysics);
    }
}