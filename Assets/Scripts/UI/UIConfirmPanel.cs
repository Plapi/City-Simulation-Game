using System;
using UnityEngine;
using UnityEngine.UI;

public class UIConfirmPanel : UIPanel {

	[SerializeField] private Button backButton = default;

	public void Init(Action onBack) {
		backButton.onClick.AddListener(() => onBack?.Invoke());
	}
}
