using UnityEngine;

public class UIController : MonoBehaviour {

	[SerializeField] private UIConstructionPanel constructionPanel = default;
	[SerializeField] private UIConfirmPanel confirmPanel = default;

	public UIConstructionPanel ConstructionPanel => constructionPanel;
	public UIConfirmPanel ConfirmPanel => confirmPanel;

}
