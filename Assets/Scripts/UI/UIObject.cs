using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIObject : MonoBehaviour {

	private RectTransform rectTransform;

	public RectTransform RectTransform {
		get {
			if (rectTransform == null) {
				rectTransform = GetComponent<RectTransform>();
			}
			return rectTransform;
		}
	}

	public Vector2 AnchoredPos {
		get => RectTransform.anchoredPosition;
		set => RectTransform.anchoredPosition = value;
	}

	public float AnchoredPosX {
		get => AnchoredPos.x;
		set => AnchoredPos = new Vector2(value, AnchoredPos.y);
	}

	public float AnchoredPosY {
		get => AnchoredPos.x;
		set => AnchoredPos = new Vector2(AnchoredPos.x, value);
	}

	public Vector2 Size {
		get => RectTransform.sizeDelta;
		set => RectTransform.sizeDelta = value;
	}

	public float SizeX {
		get => Size.x;
		set => Size = new Vector2(value, Size.y);
	}

	public float SizeY {
		get => Size.y;
		set => Size = new Vector2(Size.x, value);
	}
}
