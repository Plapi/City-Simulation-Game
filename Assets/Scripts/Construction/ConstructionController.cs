using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour, IMousePositonUpdate, IMouseTapListener {

	[SerializeField] private Construction[] constructions = default;

	private int currentConstructionId;
	private Dictionary<(int, int), Construction> createdConstructionsDict = new Dictionary<(int, int), Construction>();

	private Construction currentConstruction => constructions[currentConstructionId];

	public void LoadFromDataSave() {
		CityData.Instance.Constructions.ForEach(construction => {
			PlaceConstruction(construction.Id, construction.X, construction.Z);
		});
		NavigationController.Instance.UpdateGraph();
	}

	public void StartBuild(int id) {
		MouseController.Instance.AddMousePositionUpdateListener(this);
		MouseController.Instance.AddMouseTapListener(this);
		currentConstructionId = id;
		currentConstruction.gameObject.SetActive(true);
	}

	public void StopBuild() {
		MouseController.Instance.RemoveMousePositionUpdateListener(this);
		MouseController.Instance.RemoveMouseTapListener(this);
		currentConstruction.gameObject.SetActive(false);
	}

	public void OnMousePositionUpdate(Vector2 mousePos) {
		if (TryGetSnappedPos(mousePos, out int x, out int z)) {
			currentConstruction.transform.position += (new Vector3(x, 0f, z) - currentConstruction.transform.position) * 0.5f;
		}
	}

	public void OnMouseTap(Vector2 mousePos) {
		if (TryGetSnappedPos(mousePos, out int x, out int z) && CanPlaceCurrentConstruction(x, z)) {
			PlaceConstruction(currentConstructionId, x, z);
			CityData.Instance.AddConstruction(currentConstructionId, x, z);
			NavigationController.Instance.UpdateGraph();
		}
	}

	private void PlaceConstruction(int id, int x, int z) {
		Construction newConstruction = Instantiate(constructions[id], transform);
		newConstruction.gameObject.SetActive(true);
		newConstruction.transform.position = new Vector3(x, 0f, z);
		createdConstructionsDict.Add((x, z), newConstruction);
		CreateConnections(newConstruction, x, z);
		NavigationController.Instance.AddNode(newConstruction);
	}

	private bool CanPlaceCurrentConstruction(int x, int z) {
		if (createdConstructionsDict.ContainsKey((x, z))) {
			return false;
		}
		return true;
	}

	private bool TryGetSnappedPos(Vector2 mousePos, out int x, out int z) {
		x = z = default;
		if (Utils.TryGetHitPoint(mousePos, 1 << 8, out Vector3 hitPoint)) {
			x = Mathf.RoundToInt(hitPoint.x);
			z = Mathf.RoundToInt(hitPoint.z);
			return true;
		}
		return false;
	}

	// create connexions between (INTERMEDIAR <--> INTERMEDIAR; INPUT <--> INTERMEDIAR; OUTPUT <--> INTERMEDIAR)
	private void CreateConnections(Construction newConstruction, int x, int z) {
		GetAdjConstructions(x, z).ForEach(adjConstruction => {
			if (newConstruction.NodeType == NodeType.INTERMEDIAR || adjConstruction.NodeType == NodeType.INTERMEDIAR) {
				newConstruction.AddNextConstruction(adjConstruction);
				adjConstruction.AddNextConstruction(newConstruction);
			}
		});
	}

	private List<Construction> GetAdjConstructions(int x, int z) {
		List<Construction> adjConstructions = new List<Construction>();
		(int, int)[] checks = new (int, int)[4] {
			(x + 1, z), (x, z + 1), (x - 1, z), (x, z - 1)
		};
		for (int i = 0; i < checks.Length; i++) {
			if (createdConstructionsDict.ContainsKey(checks[i])) {
				adjConstructions.Add(createdConstructionsDict[checks[i]]);
			}
		}
		return adjConstructions;
	}
}

