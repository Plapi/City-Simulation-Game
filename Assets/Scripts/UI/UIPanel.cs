using System;
using UnityEngine;
using DG.Tweening;

public abstract class UIPanel : UIObject {

	public virtual void Show(Action onComplete = null) {
		gameObject.SetActive(true);
		AnchoredPosY = -SizeY;
		RectTransform.DOAnchorPosY(0f, 0.25f).SetEase(Ease.OutExpo).OnComplete(() => onComplete?.Invoke());
	}

	public virtual void Hide(Action onComplete = null) {
		RectTransform.DOAnchorPosY(-SizeY, 0.25f).SetEase(Ease.OutExpo).OnComplete(() => {
			gameObject.SetActive(false);
			onComplete?.Invoke();
		});
	}
}
