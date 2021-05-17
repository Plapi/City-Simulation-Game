using System;
using System.Collections.Generic;
using UnityEngine;

public class NavigationController : MonoBehaviourSingleton<NavigationController> {

	[SerializeField] private Car car = default;
	[SerializeField] private float laneOffset = default;

	private List<Construction> allNodes = new List<Construction>();
	private List<Construction> inputWaitingNodes = new List<Construction>();
	private List<Construction> outputWaitingNodes = new List<Construction>();

	private List<Car> cars = new List<Car>();

	private List<Path> paths = new List<Path>();

	private int[][] graph;

	public void AddNode(Construction node) {
		allNodes.Add(node);

		if (node.NodeType != NodeType.INTERMEDIAR) {
			if (node.NodeType == NodeType.INPUT) {
				inputWaitingNodes.Add(node);
			} else if (node.NodeType == NodeType.OUTPUT) {
				outputWaitingNodes.Add(node);
			}
		}
	}

	public void UpdateGraph() {
		graph = BFS<Construction>.GetAdjacents(allNodes);
		CheckNavigation();
	}

	/*private void LateUpdate() {
		cars.ForEach(car0 => {
			if (car0.IsNavigating) {
				if (!car0.IsStoppedByOtherCar) {
					cars.ForEach(car1 => {
						if (car1.IsNavigating && car0 != car1) {
							car0.CheckCarIsInFront(car1);
						}
					});
				} else {
					car0.CheckInFrontCarIsStillInFront();
				}
			}
		});
	}*/

	private void CheckNavigation() {
		for (int i = 0; i < inputWaitingNodes.Count && i < outputWaitingNodes.Count; i++) {
			if (HasBidirectionalPath(inputWaitingNodes[i], outputWaitingNodes[i], out List<Vector3> goPoints, out List<Vector3> returnPoints)) {

				Car newCar = Instantiate(car, transform).Init();
				Navigate(newCar, goPoints.ToArray(), returnPoints.ToArray());
				cars.Add(newCar);

				inputWaitingNodes.RemoveAt(0);
				outputWaitingNodes.RemoveAt(0);

				i--;
			}
		}
	}

	private bool HasBidirectionalPath(Construction inputConstruction, Construction outputConstruction,
		out List<Vector3> goPoints, out List<Vector3> returnPoints) {
		goPoints = returnPoints = null;
		if (BFS<Construction>.FindPath(allNodes, graph, inputConstruction, outputConstruction, out List<Construction> goNodes) &&
			BFS<Construction>.FindPath(allNodes, graph, outputConstruction, inputConstruction, out List<Construction> returnNodes)) {
			goPoints = GetPointsFromNodes(goNodes);
			returnPoints = GetPointsFromNodes(returnNodes);
			return true;
		}
		return false;
	}

	private void Navigate(Car car, Vector3[] goPoints, Vector3[] returnPoints) {
		Navigate(car, goPoints, () => {
			Navigate(car, returnPoints, () => {
				Navigate(car, goPoints, returnPoints);
			});
		});
	}

	private void Navigate(Car car, Vector3[] points, Action onComplete) {
		Path path = new Path { points = new List<Vector3>(points) };
		AddNewPath(path);
		car.Navigate(points, () => {
			paths.Remove(path);
			onComplete?.Invoke();
		});
	}

	private void AddNewPath(Path path) {
		paths.ForEach(p => {
			path.points.ForEach(point0 => {
				p.points.ForEach(point1 => {
					if (Vector3.Distance(point0, point1) < 0.1f) {
						path.intersectionPoints.Add(Vector3.Lerp(point0, point1, 0.5f));
					}
				});
			});
		});
		paths.Add(path);
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		paths.ForEach(path => {
			path.intersectionPoints.ForEach(point => {
				Gizmos.DrawCube(point, Vector3.one * 0.1f);
			});
		});
	}

	private List<Vector3> GetPointsFromNodes(List<Construction> nodes) {
		List<Vector3> points = new List<Vector3>();
		nodes.ForEach(node => points.Add(node.transform.position));
		points = GetPointsOffsets(points);
		points = Chaikin.SmoothPath(points, 2);
		return points;
	}

	private List<Vector3> GetPointsOffsets(List<Vector3> points) {
		if (points.Count >= 3) {
			List<Vector3> offsets = new List<Vector3>(points.Count);
			offsets.Add(GetOffsetPoints(points[0], points[1], 90f));
			for (int i = 1; i < points.Count - 1; i++) {
				offsets.Add(GetOffsetPoint(points[i - 1], points[i], points[i + 1]));
			}
			offsets.Add(GetOffsetPoints(points[points.Count - 1], points[points.Count - 2], -90f));
			return offsets;
		}
		return null;
	}

	private Vector3 GetOffsetPoints(Vector3 p0, Vector3 p1, float angle) {
		Vector3 direction = (p1 - p0).normalized;
		Vector3 right = Quaternion.Euler(0, angle, 0) * direction;
		return p0 + right * laneOffset;
	}

	private Vector3 GetOffsetPoint(Vector3 p0, Vector3 p1, Vector3 p2) {
		Vector3 direction0 = (p1 - p0).normalized;
		Vector3 direction1 = (p2 - p1).normalized;
		float angle = 180f - Vector3.SignedAngle(direction0, direction1, Vector3.up);
		Vector3 newDirection = Quaternion.Euler(0, angle / 2f, 0) * direction1;
		return p1 + newDirection * laneOffset;
	}

	private class Path {
		public List<Vector3> points;
		public List<Vector3> intersectionPoints = new List<Vector3>();
	}
}

public interface Node : BFSNode {
	NodeType NodeType { get; }
}

public enum NodeType {
	INPUT,
	OUTPUT,
	INTERMEDIAR
}
