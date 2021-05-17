using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;

public class Car : MonoBehaviour {

	[SerializeField] private Renderer rend = default;
	[SerializeField] private BoxCollider boxCollider = default;

	private DG.Tweening.Core.TweenerCore<Vector3, Path, PathOptions> path;
	private Car inFrontCar;
	private Vector3[] points;
	private int currentIndexPoint;
	private float pathTargetTimeScale;

	public bool IsNavigating => path != null;
	public bool IsStoppedByOtherCar => inFrontCar != null;

	public Car Init() {
		rend.material.color = Utils.RandomColor();
		return this;
	}

	public void Navigate(Vector3[] points, Action onComplete) {
		this.points = points;

		transform.position = points[0];
		transform.rotation = Quaternion.LookRotation(points[1] - points[0]);
		gameObject.SetActive(true);

		currentIndexPoint = 0;
		pathTargetTimeScale = 1f;

		path = transform.DOPath(points, 2f).SetSpeedBased().OnComplete(() => {
			gameObject.SetActive(false);
			path = null;
			onComplete?.Invoke();
		}).OnWaypointChange(index => {
			currentIndexPoint = index;
		}).OnUpdate(() => {
			Quaternion futureRotation = Quaternion.LookRotation(transform.position - points[currentIndexPoint]);
			transform.rotation = Quaternion.Slerp(transform.rotation, futureRotation, Time.deltaTime * 10f);
		}).SetDelay(Utils.Random(1f, 4f));
	}

	public void CheckCarIsInFront(Car otherCar) {
		if (otherCar.inFrontCar != this && CanCollideWithOtherCar(otherCar)) {
			inFrontCar = otherCar;
			pathTargetTimeScale = 0f;
		}
	}

	public void CheckInFrontCarIsStillInFront() {
		if (inFrontCar == null || !CanCollideWithOtherCar(inFrontCar)) {
			inFrontCar = null;
			pathTargetTimeScale = 1f;
		}
	}

	private bool CanCollideWithOtherCar(Car otherCar) {
		List<Line> frontLines = GetFrontLines();
		for (int i = 0; i < frontLines.Count; i++) {
			if (frontLines[i].IntersectBounds(otherCar.boxCollider.bounds)) {
				return true;
			}
		}
		return false;
	}

	private void Update() {
		if (path != null) {
			float p = pathTargetTimeScale > path.timeScale ? 0.1f : 0.2f;
			path.timeScale += (pathTargetTimeScale - path.timeScale) * p;
		}
	}

	private List<Line> GetFrontLines() {
		float distance = 0.5f;

		List<Line> frontLines = new List<Line>();
		Vector3 currentPos = transform.position;

		for (int i = currentIndexPoint + 1; i < points.Length; i++) {
			float distToNextPoint = Vector3.Distance(currentPos, points[i]);
			if (distance > distToNextPoint) {
				frontLines.Add(new Line(currentPos, points[i]));
				currentPos = points[i];
			} else {
				frontLines.Add(new Line(currentPos, currentPos + (points[i] - currentPos).normalized * distance));
				break;
			}
			distance -= distToNextPoint;
		}

		return frontLines;
	}

	private class Line {

		private Vector3 point0;
		private Vector3 point1;
		private float distance;

		public Line(Vector3 p0, Vector3 p1) {
			point0 = p0;
			point1 = p1;
			distance = Vector3.Distance(point0, point1);
		}

		public bool IntersectBounds(Bounds bounds) {
			if (Vector3.Distance(point0, bounds.ClosestPoint(point0)) > distance) {
				return false;
			}
			Ray ray = new Ray(point0, (point1 - point0).normalized);
			return bounds.IntersectRay(ray);
		}

		public bool Intersect(Line line) {
			Vector2 p0 = new Vector2(point0.x, point0.z);
			Vector2 p1 = new Vector2(point1.x, point1.z);
			Vector2 p2 = new Vector2(line.point0.x, line.point0.z);
			Vector2 p3 = new Vector2(line.point1.x, line.point1.z);
			return Utils.LineSegmentsIntersection(p0, p1, p2, p3, out _);
		}
	}
}
