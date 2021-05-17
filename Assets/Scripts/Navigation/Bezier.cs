using System;
using System.Collections.Generic;
using UnityEngine;

public static class Bezier {

	public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float OneMinusT = 1f - t;
		return
			OneMinusT * OneMinusT * OneMinusT * p0 +
			3f * OneMinusT * OneMinusT * t * p1 +
			3f * OneMinusT * t * t * p2 +
			t * t * t * p3;
	}

	public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			oneMinusT * oneMinusT * p0 +
			2f * oneMinusT * t * p1 +
			t * t * p2;
	}

	public static List<Vector3> GetPoints(List<Vector3> points, int passes) {
		if (points.Count == 3 || points.Count == 4) {
			List<Vector3> bPoints = new List<Vector3>();
			int add = 100 / passes;
			for (int i = 0; i <= 100; i += add) {
				bPoints.Add(points.Count == 3 ? GetPoint(points[0], points[1], points[2], i / 100f) :
					GetPoint(points[0], points[1], points[2], points[3], i / 100f));
			}
			return bPoints;
		}
		return GetBezierPoints(points);
	}

	public static List<Vector3> GetBezierPoints(List<Vector3> points, uint pointsCount = 10) {
		List<Vector3> bezierPoints = new List<Vector3>();

		if (points.Count == 0) {
			return bezierPoints;
		}

		bezierPoints.Add(points[0]);
		float time = 1f / pointsCount;
		for (int i = 1; i < pointsCount; i++) {
			bezierPoints.Add(GetBezierPoint(time * i, points));
		}
		bezierPoints.Add(points[points.Count - 1]);

		return bezierPoints;
	}

	private static Vector3 GetBezierPoint(float time, List<Vector3> points) {
		int n = points.Count - 1;

		if (time < 0.0f || time > 1.0f) {
			Debug.LogError("Bezier Time is not set right");
			return Vector2.zero;
		}

		if (n < 0) {
			Debug.LogError("n can't be less than 0");
			return Vector2.zero;
		}

		if (Math.Abs(time) < Mathf.Epsilon) {
			return points[0];
		} else if (Math.Abs(time - 1.0f) < Mathf.Epsilon) {
			return points[n];
		}

		float pointX = 0.0f;
		float pointY = 0.0f;
		float pointZ = 0.0f;
		int varGoBack = n;

		for (int i = 0; i <= n; i++) {
			pointX += BinomialCoefficient(n, i) * Mathf.Pow(time, i) * Mathf.Pow((1 - time), varGoBack) * points[i].x;
			pointY += BinomialCoefficient(n, i) * Mathf.Pow(time, i) * Mathf.Pow((1 - time), varGoBack) * points[i].y;
			pointZ += BinomialCoefficient(n, i) * Mathf.Pow(time, i) * Mathf.Pow((1 - time), varGoBack) * points[i].z;
			varGoBack--;
		}

		return new Vector3(pointX, pointY, pointZ);
	}

	private static float BinomialCoefficient(int n, int k) {
		return Factorial(n) / (Factorial(k) * Factorial(n - k));
	}

	private static float Factorial(int x) {
		float factorial = 1;
		for (int i = 1; i <= x; i++) {
			factorial *= i;
		}
		return factorial;
	}
}