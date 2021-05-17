using System;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;

public class Car : MonoBehaviour {

	[SerializeField] private Renderer rend = default;
	[SerializeField] private BoxCollider frontCollision = default;

	private DG.Tweening.Core.TweenerCore<Vector3, Path, PathOptions> path;
	//private Car inFrontCar;

	//public bool IsNavigating => path != null;
	//public bool IsStoppedByOtherCar => inFrontCar != null;

	public Car Init() {
		rend.material.color = Utils.RandomColor();
		return this;
	}

	public void Navigate(Vector3[] points, Action onComplete) {
		transform.position = points[0];
		transform.rotation = Quaternion.LookRotation(points[1] - points[0]);
		gameObject.SetActive(true);

		int currentIndexPoint = 0;

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

	/*private bool IntersectBounds(Bounds bounds) {
		return bounds.Intersects(rend.bounds);
	}

	public void CheckCarIsInFront(Car otherCar) {
		if (otherCar.IntersectBounds(rend.bounds) && otherCar.inFrontCar != this) {
			inFrontCar = otherCar;
			path?.Pause();
		}
	}

	public void CheckInFrontCarIsStillInFront() {
		if (inFrontCar == null || !IntersectBounds(inFrontCar.rend.bounds)) {
			inFrontCar = null;
			path?.Play();
		}
	}*/
}
