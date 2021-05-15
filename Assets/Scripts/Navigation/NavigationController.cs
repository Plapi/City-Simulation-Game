using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NavigationController : MonoBehaviourSingleton<NavigationController> {

	[SerializeField] private Transform car = default;

	private List<Construction> allNodes = new List<Construction>();
	private int[][] adjacents;

	public void AddNode(Construction node) {
		allNodes.Add(node);
	}

	public void CalculateAllAdjacents() {
		adjacents = BFS<Construction>.GetAdjacents(allNodes);
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.A)) {
			Construction input = allNodes.Find(node => node.NodeType == NodeType.INPUT);
			Construction output = allNodes.Find(node => node.NodeType == NodeType.OUTPUT);
			if (BFS<Construction>.FindPath(allNodes, adjacents, input, output, out List<Construction> nodes)) {
				Navigate(nodes);
			}
		}
	}

	private void Navigate(List<Construction> nodes) {
		Vector3[] points = new Vector3[nodes.Count];
		for (int i = 0; i < points.Length; i++) {
			points[i] = nodes[i].transform.position;
		}
		car.transform.position = nodes[0].transform.position;
		car.DOPath(points, 1f).SetSpeedBased().SetEase(Ease.Linear);
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
