using UnityEngine;

public class GameController : MonoBehaviour {

	[SerializeField] private UIController uiController = default;
	[SerializeField] private ConstructionController constructionController = default;

	private void Awake() {
		constructionController.LoadFromDataSave();
		uiController.ConstructionPanel.Init(id => {
			constructionController.StartBuild(id);
			uiController.ConstructionPanel.Hide(() => {
				uiController.ConfirmPanel.Show();
			});
		});
		uiController.ConfirmPanel.Init(() => {
			constructionController.StopBuild();
			uiController.ConfirmPanel.Hide(() => {
				uiController.ConstructionPanel.Show();
			});
		});
	}
}
