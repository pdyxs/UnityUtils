using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Bezier {

	public static Vector2 bezierAt(this List<Vector2> points, float t)
    {
        var nPoints = new List<Vector2>();
        for (int i = 0; i < points.Count - 1; ++i) {
            var p1 = points[i];
            var p2 = points[i + 1];
            nPoints.Add(p1 + (p2 - p1) * t);
        }

        if (nPoints.Count == 1) {
            return nPoints[0];
        } else {
            return nPoints.bezierAt(t);
        }
    }

    private static List<Vector2> getBezierLines(this List<Vector2> points, int lines, float max) {
        var ret = new List<Vector2>();

        float spacing = 1f / lines;
        for (int i = 0; i <= lines; ++i) {
            if (i * spacing > max) {
                ret.Add(points.bezierAt(max));
                break;
            }
            ret.Add(points.bezierAt(i * spacing));
        }

        return ret;
    }

    public static Vector2[] bezierLines2D(this List<Vector2> points, int lines, float max = 1) {
        return points.getBezierLines(lines, max).ToArray();
    }

    public static Vector3[] bezierLines3D(this List<Vector2> points, int lines, float max = 1)
    {
        return points.getBezierLines(lines, max).ConvertAll((i) => new Vector3(i.x, i.y)).ToArray();
    }

	public static float bezierClosestTo(this List<Vector2> points, Vector2 target, int slices, int iterations = 1, float start = 0, float end = 1)
	{
		if (iterations <= 0) 
        {
            return (start + end) / 2;
        }

		float tick = (end - start) / (float)slices;
		float best = 0;
        float bestDistance = float.PositiveInfinity;
		float currentDistance;
		float t = start;
		while (t <= end)
		{
            var dp = points.bezierAt(t) - target;

            currentDistance = dp.sqrMagnitude;
			if (currentDistance < bestDistance)
			{
				bestDistance = currentDistance;
				best = t;
			}
			t += tick;
		}
		return points.bezierClosestTo(target, slices, iterations - 1, Mathf.Max(best - tick, 0f), Mathf.Min(best + tick, 1f));
	}
}
