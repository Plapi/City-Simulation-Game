using UnityEngine;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour {

	private static T instance;

	public static T Instance {
		get {
			if (instance == null) {
				instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
			}
			return instance;
		}
	}

	protected virtual void Awake() {
		if (instance != null && instance != this) {
			Debug.LogWarning("One single instance should be used.");
		}
		instance = this as T;
	}

	private void OnDestroy() {
		instance = null;
	}
}
