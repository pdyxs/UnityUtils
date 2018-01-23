using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils {

    public static bool isCloseTo(this Vector2 v1, Vector2 v2, float tolerance) {
        return (v1 - v2).sqrMagnitude < tolerance * tolerance;
    }

    public static Vector3 toWorld(this Vector2 v, Vector3 anchor, float scaleFactor) {
        return anchor + (Vector3)v * scaleFactor;
    }

	public static Vector3 toWorld(this Vector2 v, Transform anchor, float scaleFactor)
	{
        return v.toWorld(anchor.position, scaleFactor);
	}

	public static Vector3 toWorld(this Vector2 v, Transform anchor, Canvas canvas)
	{
        return v.toWorld(anchor.position, canvas.scaleFactor);
	}

	public static Vector3 toWorld(this Vector2 v, Vector3 anchor, Canvas canvas)
	{
		return v.toWorld(anchor, canvas.scaleFactor);
	}

    public static Vector2 toCanvas(this Vector3 v, Vector3 anchor, float scaleFactor) {
        return (v - anchor) / scaleFactor;
	}

	public static Vector2 toCanvas(this Vector3 v, Transform anchor, float scaleFactor)
	{
        return v.toCanvas(anchor.position, scaleFactor);
	}

	public static Vector2 toCanvas(this Vector3 v, Transform anchor, Canvas canvas)
	{
		return v.toCanvas(anchor.position, canvas.scaleFactor);
	}

	public static Vector2 toCanvas(this Vector3 v, Vector3 anchor, Canvas canvas)
	{
		return v.toCanvas(anchor, canvas.scaleFactor);
	}

    public static Vector2 to2D(this Vector3 v) {
        return (Vector2)v;
	}

	public static Vector3 to3D(this Vector2 v)
	{
		return (Vector3)v;
	}

    public static Vector2 swap(this Vector2 v) {
        return new Vector2(v.y, v.x);
    }

    public static Vector3 withZ(this Vector3 v, float z) {
        return new Vector3(v.x, v.y, z);
    }
}
