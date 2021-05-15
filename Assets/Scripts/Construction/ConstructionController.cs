using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour, IMousePositonUpdate, IMouseTapListener {

	[SerializeField] private Construction[] constructions = default;

	private int currentConstructionId;
	private Dictionary<(int, int), Construction> createdConstructions = new Dictionary<(int, int), Construction>();

	private Construction currentConstruction => constructions[currentConstructionId];

	public void LoadFromDataSave() {
		CityData.Instance.Constructions.ForEach(construction => {
			PlaceConstruction(construction.Id, construction.X, construction.Z);
		});
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
		}
	}

	private void PlaceConstruction(int id, int x, int z) {
		Construction newConstruction = Instantiate(constructions[id], transform);
		newConstruction.gameObject.SetActive(true);
		newConstruction.transform.position = new Vector3(x, 0f, z);
		createdConstructions.Add((x, z), newConstruction);
	}

	private bool CanPlaceCurrentConstruction(int x, int z) {
		if (createdConstructions.ContainsKey((x, z))) {
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
}

