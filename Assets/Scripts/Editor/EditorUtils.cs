using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.SceneManagement;

public static class EditorUtils {

	private static IEnumerator WaitToApplicationClose(Action _onComplete) {
		while (Application.isPlaying) {
			yield return null;
		}
		_onComplete();
	}

	private static void CheckSaveScene(Action _onComplete) {
		Scene scene = EditorSceneManager.GetActiveScene();
		EditorPrefs.SetString("LAST_SCENE", scene.path);
		if (scene.isDirty) {
			if (EditorUtility.DisplayDialog("Save Scene", "Do you want to save " + scene.name + " before playing?", "Yes", "No")) {
				EditorSceneManager.SaveScene(scene, "", false);
				_onComplete();
			} else {
				_onComplete();
			}
		} else {
			_onComplete();
		}
	}

	private static void SaveLastGameObjectSelected(GameObject _selectedGameObject) {
		EditorPrefs.SetString("LAST_GAMEOBJECT_SELECTED_NAME", _selectedGameObject != null ? _selectedGameObject.name : null);
	}

	private static void SelectLastGameObjectSelected() {
		Selection.activeGameObject = GameObject.Find(EditorPrefs.GetString("LAST_GAMEOBJECT_SELECTED_NAME"));
	}

	[MenuItem("Editor Utils/Take Screenshot %k")]
	private static void TakeScreenshot() {
		EditorCoroutine.Start(TakeScreenshotIEnumerator());
	}

	private static IEnumerator TakeScreenshotIEnumerator() {
		string screenCaptureName = "ScreenCapture " + DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss") + ".png";

		ScreenCapture.CaptureScreenshot(screenCaptureName);
		while (!File.Exists(Application.dataPath.Replace("Assets", screenCaptureName))) {
			yield return null;
		}

		string screenshotPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + screenCaptureName;

		File.WriteAllBytes(screenshotPath, File.ReadAllBytes(Application.dataPath.Replace("Assets", screenCaptureName)));
		File.Delete(Application.dataPath.Replace("Assets", screenCaptureName));

		System.Diagnostics.Process m_process = new System.Diagnostics.Process {
			StartInfo = new System.Diagnostics.ProcessStartInfo(screenshotPath)
		};

		m_process.Start();
	}

	[MenuItem("Editor Utils/Reload Current Scene Or Prefab %t")]
	public static void ReloadCurrentSceneOrPrefab() {
		if (!Application.isPlaying) {
			PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			if (prefabStage != null) {
				AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(prefabStage.assetPath));
			} else {
				CheckSaveScene(() => {
					SaveLastGameObjectSelected(Selection.activeGameObject);
					EditorSceneManager.OpenScene(SceneManager.GetActiveScene().path);
					SelectLastGameObjectSelected();
				});
			}
		}
	}
}
