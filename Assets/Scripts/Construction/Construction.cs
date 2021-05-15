using System.Collections.Generic;
using UnityEngine;

public abstract class Construction : MonoBehaviour, Node {

	[SerializeField] protected List<Construction> nextConstructions = new List<Construction>();
	protected int index;

	public int Index { get => index; set => index = value; }

	public abstract NodeType NodeType { get; }

	public BFSNode[] GetNextNodes() {
		return nextConstructions.ToArray();
	}

	public void AddNextConstruction(Construction node) {
		nextConstructions.Add(node);
	}
}
