using System;
using UnityEngine;
using UnityEngine.UI;

public class UIConstructionPanel : UIPanel {

	[SerializeField] private Button[] constructionButtons = default;

	public void Init(Action<int> onButton) {
		for (int i = 0; i < constructionButtons.Length; i++) {
			int id = i;
			constructionButtons[i].onClick.AddListener(() => {
				onButton?.Invoke(id);
			});
		}
	}
}
