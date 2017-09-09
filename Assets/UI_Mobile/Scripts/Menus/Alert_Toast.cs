using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Alert_Toast : BaseMenu {

	public UICell m_toastCell;

	public override void OnEnter (bool animate)
	{
		gameObject.SetActive (true);

		LayoutRebuilder.ForceRebuildLayoutImmediate (m_toastCell.GetComponent<RectTransform>());

		RectTransform rt = gameObject.GetComponent<RectTransform> ();
		Rect r = rt.rect;
		rt.anchoredPosition = new Vector2 (0, rt.rect.height * -1);

		DOTween.Kill (0, false);

		Sequence newSequence = DOTween.Sequence ();
		newSequence.Append (DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, rt.rect.height), 0.35f).SetDelay (0.35f));
		newSequence.Append (DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, rt.rect.height * -1), 0.25f).SetDelay (3.0f));
		newSequence.SetId (0);
		newSequence.Play ();

//		DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, rt.rect.height), 0.35f).SetDelay (0.35f);

//		DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, rt.rect.height * -1), 0.25f).SetDelay (3.0f);
	}
}
