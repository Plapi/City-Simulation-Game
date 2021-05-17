using UnityEditor;
using UnityEngine;

public class DebugWindow : EditorWindow {

	private static Vector2 s_scrollPos {
		get => new Vector2(0f, EditorPrefs.GetFloat("EDITOR_WINDOW_SCROLL_POS_Y", 0f));
		set => EditorPrefs.SetFloat("EDITOR_WINDOW_SCROLL_POS_Y", value.y);
	}

	[MenuItem("Window/Debug Window")]
	private static void Init() {
		((DebugWindow)GetWindow(typeof(DebugWindow))).Show();
	}

	private void OnGUI() {
		EditorGUILayout.BeginVertical();
		s_scrollPos = EditorGUILayout.BeginScrollView(s_scrollPos);
		Time.timeScale = EditorGUILayout.Slider("Time Scale", Time.timeScale, 0f, 1f);

		if (GUILayout.Button("Delete City Data")) {
			CityData.Delete();
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}
