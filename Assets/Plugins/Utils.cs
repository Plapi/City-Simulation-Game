using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Utils {
	public static void SetAlpha(this Graphic graphic, float alpha) {
		graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
	}

	public static void SetAnchorPosX(this RectTransform rectTransform, float x) {
		rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
	}

	public static void SetAnchorPosY(this RectTransform rectTransform, float y) {
		rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
	}

	public static bool TryGetHitCollider(int layer, Vector2 mousePos, out Collider collider) {
		collider = null;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit hit, Mathf.Infinity, layer)) {
			collider = hit.collider;
			return true;
		}
		return false;
	}

	public static Vector2 WorldPositionToUI(Vector3 worldPos, RectTransform rectTransform) {
		Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPos);
		return new Vector2((viewportPosition.x * rectTransform.sizeDelta.x) - (rectTransform.sizeDelta.x * 0.5f),
				(viewportPosition.y * rectTransform.sizeDelta.y) - (rectTransform.sizeDelta.y * 0.5f));
	}

	public static bool TryGetHitPoint(Vector2 mousePos, int layer, out Vector3 hitPoint) {
		hitPoint = Vector3.zero;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit hit, Mathf.Infinity, layer)) {
			hitPoint = hit.point;
			return true;
		}
		return false;
	}

	public static bool IsOverUI() {
		if (EventSystem.current.IsPointerOverGameObject()) {
			return true;
		}
		if (Input.touchCount > 0) {
			return EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId);
		}
		return false;
	}

	#region Transform Extensions

	public static void SetX(this Transform transform, float x) {
		transform.position = new Vector3(x, transform.position.y, transform.position.z);
	}
	public static void SetY(this Transform transform, float y) {
		transform.position = new Vector3(transform.position.x, y, transform.position.z);
	}
	public static void SetZ(this Transform transform, float z) {
		transform.position = new Vector3(transform.position.x, transform.position.y, z);
	}

	public static void SetAPX(this RectTransform rectTransform, float x) {
		rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
	}
	public static void SetAPY(this RectTransform rectTransform, float y) {
		rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
	}

	public static void SetXY(this Transform transform, float x, float y) {
		transform.position = new Vector3(x, y, transform.position.z);
	}
	public static void SetXZ(this Transform transform, float x, float z) {
		transform.position = new Vector3(x, transform.position.y, z);
	}
	public static void SetYZ(this Transform transform, float y, float z) {
		transform.position = new Vector3(transform.position.z, y, z);
	}
	public static void SetXYZ(this Transform transform, float x, float y, float z) {
		transform.position = new Vector3(x, y, z);
	}

	public static void SetLocalX(this Transform transform, float x) {
		transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
	}
	public static void SetLocalY(this Transform transform, float y) {
		transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
	}
	public static void SetLocalZ(this Transform transform, float z) {
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
	}

	public static void SetLocalXY(this Transform transform, float x, float y) {
		transform.localPosition = new Vector3(x, y, transform.localPosition.z);
	}
	public static void SetLocalXZ(this Transform transform, float x, float z) {
		transform.localPosition = new Vector3(x, transform.localPosition.y, z);
	}
	public static void SetLocalYZ(this Transform transform, float y, float z) {
		transform.localPosition = new Vector3(transform.localPosition.z, y, z);
	}
	public static void SetLocalXYZ(this Transform transform, float x, float y, float z) {
		transform.localPosition = new Vector3(x, y, z);
	}

	#endregion
}
