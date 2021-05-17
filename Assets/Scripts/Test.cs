using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	[SerializeField] private Transform[] transformPoints = new Transform[0];
	[SerializeField] private float offset = 0.2f;

	public float angle;

	private void OnDrawGizmos() {
		Vector3[] points = new Vector3[transformPoints.Length];
		for (int i = 0; i < points.Length; i++) {
			points[i] = transformPoints[i].position;
		}
		DrawLinePoints(points, Color.blue);

		if (points.Length >= 3) {
			Vector3[] offsets = GetPointsOffsets(points);
			DrawLinePoints(offsets, Color.red);
			Vector3[] curvePoints = Chaikin.SmoothPath(new List<Vector3>(offsets), 2).ToArray();
			DrawLinePoints(curvePoints, Color.yellow);
		}
	}

	private Vector3[] GetPointsOffsets(Vector3[] points) {
		if (points.Length >= 3) {
			Vector3[] offsets = new Vector3[points.Length];
			offsets[0] = GetOffsetPoints(points[0], points[1], 90f);
			for (int i = 1; i < points.Length - 1; i++) {
				offsets[i] = GetOffsetPoint(points[i - 1], points[i], points[i + 1]);
			}
			offsets[offsets.Length - 1] = GetOffsetPoints(points[points.Length - 1], points[points.Length - 2], -90f);
			return offsets;
		}
		return null;
	}

	private void DrawLinePoints(Vector3[] points, Color color) {
		Gizmos.color = color;
		for (int i = 0; i < points.Length - 1; i++) {
			Gizmos.DrawLine(points[i], points[i + 1]);
		}
	}

	private Vector3 GetOffsetPoints(Vector3 p0, Vector3 p1, float angle) {
		Vector3 direction = (p1 - p0).normalized;
		Vector3 right = Quaternion.Euler(0, angle, 0) * direction;
		return p0 + right * offset;
	}

	private Vector3 GetOffsetPoint(Vector3 p0, Vector3 p1, Vector3 p2) {
		Vector3 direction0 = (p1 - p0).normalized;
		Vector3 direction1 = (p2 - p1).normalized;
		angle = 180f - Vector3.SignedAngle(direction0, direction1, Vector3.up);
		Vector3 newDirection = Quaternion.Euler(0, angle / 2f, 0) * direction1;
		return p1 + newDirection * offset;
	}
}
