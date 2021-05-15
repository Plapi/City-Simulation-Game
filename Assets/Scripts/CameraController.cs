using UnityEngine;

public class CameraController : MonoBehaviour, IMouseDragListener, IMouseZoomListener {

	[SerializeField] private Camera cam = default;
	[SerializeField] private float zoomSpeed = default;
	[SerializeField] private float cameraMinOrtoSize = default;
	[SerializeField] private float cameraMaxOrtoSize = default;

	private Plane plane;

	protected void Awake() {
		MouseController.Instance.AddMouseDragListener(this);
		MouseController.Instance.AddMouseZoomListener(this);
	}

	public void OnMouseDrag(Vector2 mousePos, Vector2 deltaPos) {
		plane.SetNormalAndPosition(transform.up, transform.position);
		transform.Translate(PlanePositionDelta(mousePos, deltaPos), Space.World);
	}

	public void OnMouseZoom(float deltaY) {
		cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - deltaY * zoomSpeed, cameraMinOrtoSize, cameraMaxOrtoSize);
	}

	private Vector3 PlanePositionDelta(Vector2 mousePos, Vector2 deltaPos) {
		var rayBefore = cam.ScreenPointToRay(mousePos - deltaPos);
		var rayNow = cam.ScreenPointToRay(mousePos);
		if (plane.Raycast(rayBefore, out float enterBefore) && plane.Raycast(rayNow, out float enterNow)) {
			return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);
		}
		return Vector3.zero;
	}
}
