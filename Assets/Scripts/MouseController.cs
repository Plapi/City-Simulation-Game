using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviourSingleton<MouseController> {

	private Vector2 firstMouseDownPosition;
	private float fristMouseDownTime;
	private State state;

	private Vector2 prevMousePosition;

	public List<IMouseTapListener> mouseTapListeners = new List<IMouseTapListener>();
	public List<IMouseDragListener> mouseDragListeners = new List<IMouseDragListener>();
	public List<IMouseRotateListener> mouseRotateListeners = new List<IMouseRotateListener>();
	public List<IMouseZoomListener> mouseZoomListeners = new List<IMouseZoomListener>();
	public List<IMousePositonUpdate> mousePositionUpdateListeners = new List<IMousePositonUpdate>();

	public void AddMouseTapListener(IMouseTapListener listener) {
		mouseTapListeners.Add(listener);
	}

	public void RemoveMouseTapListener(IMouseTapListener listener) {
		mouseTapListeners.Remove(listener);
	}

	public void AddMouseDragListener(IMouseDragListener listener) {
		mouseDragListeners.Add(listener);
	}

	public void RemoveMouseDragListener(IMouseDragListener listener) {
		mouseDragListeners.Remove(listener);
	}

	public void AddMouseRotateListener(IMouseRotateListener listener) {
		mouseRotateListeners.Add(listener);
	}

	public void RemoveMouseRotateListener(IMouseRotateListener listener) {
		mouseRotateListeners.Remove(listener);
	}

	public void AddMouseZoomListener(IMouseZoomListener listener) {
		mouseZoomListeners.Add(listener);
	}

	public void RemoveMouseZoomListener(IMouseZoomListener listener) {
		mouseZoomListeners.Remove(listener);
	}

	public void AddMousePositionUpdateListener(IMousePositonUpdate listener) {
		mousePositionUpdateListeners.Add(listener);
	}

	public void RemoveMousePositionUpdateListener(IMousePositonUpdate listener) {
		mousePositionUpdateListeners.Remove(listener);
	}

	private void Update() {
		bool mouseIsOverUI = Utils.IsOverUI();

		if (!mouseIsOverUI) {
			OnMousePositionUpdate(Input.mousePosition);
		}

		if (Input.GetMouseButtonDown(0)) {
			state = mouseIsOverUI ? State.CANCEL : State.START;
			prevMousePosition = firstMouseDownPosition = Input.mousePosition;
			fristMouseDownTime = Time.time;
		} else if (Input.GetMouseButton(0)) {
			if (state == State.START && !IsTap()) {
				state = State.DRAG;
			}
			if (state == State.DRAG) {
				OnMouseDrag(Input.mousePosition, (Vector2)Input.mousePosition - prevMousePosition);
			}
			prevMousePosition = Input.mousePosition;
		} else if (Input.GetMouseButtonUp(0)) {
			if (state == State.START && IsTap()) {
				OnMouseTap(Input.mousePosition);
			}
			state = State.END;
		}

		if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {
			prevMousePosition = Input.mousePosition;
		} else if (Input.GetMouseButton(1) || Input.GetMouseButtonDown(2)) {
			OnMouseRotate(prevMousePosition - (Vector2)Input.mousePosition);
			prevMousePosition = Input.mousePosition;
		}

		float deltaY = Input.mouseScrollDelta.y;
		if (Mathf.Abs(deltaY) > Mathf.Epsilon) {
			OnMouseZoom(deltaY);
		}
	}

	private bool IsTap() {
		return Time.time - fristMouseDownTime < 0.4f && Vector2.Distance(Input.mousePosition, firstMouseDownPosition) < 10;
	}

	private void OnMouseTap(Vector2 mousePos) {
		mouseTapListeners.ForEach(listener => {
			if (listener != null) {
				listener.OnMouseTap(mousePos);
			}
		});
	}

	private void OnMouseDrag(Vector2 mousePos, Vector2 deltaPos) {
		mouseDragListeners.ForEach(listener => {
			if (listener != null) {
				listener.OnMouseDrag(mousePos, deltaPos);
			}
		});
	}

	private void OnMouseRotate(Vector2 deltaPos) {
		mouseRotateListeners.ForEach(listener => {
			if (listener != null) {
				listener.OnMouseRotate(deltaPos);
			}
		});
	}

	private void OnMouseZoom(float deltaY) {
		mouseZoomListeners.ForEach(listener => {
			if (listener != null) {
				listener.OnMouseZoom(deltaY);
			}
		});
	}

	private void OnMousePositionUpdate(Vector2 mousePos) {
		mousePositionUpdateListeners.ForEach(listener => {
			if (listener != null) {
				listener.OnMousePositionUpdate(mousePos);
			}
		});
	}

	private enum State {
		NONE,
		START,
		DRAG,
		CANCEL,
		END
	}
}

public interface IMouseTapListener {
	void OnMouseTap(Vector2 mousePos);
}

public interface IMouseDragListener {
	void OnMouseDrag(Vector2 mousePos, Vector2 deltaPos);
}

public interface IMouseRotateListener {
	void OnMouseRotate(Vector2 deltaPos);
}

public interface IMouseZoomListener {
	void OnMouseZoom(float deltaY);
}

public interface IMousePositonUpdate {
	void OnMousePositionUpdate(Vector2 mousePos);
}
