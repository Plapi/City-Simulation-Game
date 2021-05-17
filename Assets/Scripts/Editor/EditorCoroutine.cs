using System.Collections;
using UnityEditor;

public class EditorCoroutine {
	private readonly IEnumerator _routine;

	public static EditorCoroutine Start(IEnumerator _routine) {
		EditorCoroutine coroutine = new EditorCoroutine(_routine);
		coroutine.Start();
		return coroutine;
	}

	private EditorCoroutine(IEnumerator _routine) {
		this._routine = _routine;
	}

	private void Start() {
		EditorApplication.update += Update;
	}

	private void Stop() {
		EditorApplication.update -= Update;
	}

	private void Update() {
		if (!_routine.MoveNext()) {
			Stop();
		}
	}
}