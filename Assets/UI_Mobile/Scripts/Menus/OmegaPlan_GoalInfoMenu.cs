using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OmegaPlan_GoalInfoMenu : BaseMenu {

	public RectTransform
		m_infoPanel;

	public RawImage
		m_bgPanel;

	private float m_darkenAmount = 0;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

//		m_appNameText.text = parentApp.Name;
		m_darkenAmount = m_bgPanel.color.a;
		this.gameObject.SetActive (false);
	}

	public void DismissButtonClicked ()
	{
		m_parentApp.PopMenu ();
	}

	public override void OnEnter (bool animate)
	{
		base.OnEnter (animate);

		this.gameObject.SetActive (true);

		// slide in animation
		if (animate) {

			// fade in background
			Color c = m_bgPanel.color;
			c.a = 0;
			m_bgPanel.color = c;

			Color c2 = m_bgPanel.color;
			c2.a = m_darkenAmount;

			DOTween.To (() => m_bgPanel.color, x => m_bgPanel.color = x, c2, 0.25f);

			// info panel slides up

			m_infoPanel.anchoredPosition = new Vector2 (0, m_infoPanel.rect.height * -1);

			DOTween.To (() => m_infoPanel.anchoredPosition, x => m_infoPanel.anchoredPosition = x, new Vector2 (0, 0), 0.25f);

		} else {

			Color c = m_bgPanel.color;
			c.a = m_darkenAmount;
			m_bgPanel.color = c;

			m_infoPanel.anchoredPosition = Vector2.zero;
		}
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		if (animate) {
			// fade in background
			Color c = m_bgPanel.color;
			float fadeDest = c.a;
			c.a = 0;

			DOTween.To (() => m_bgPanel.color, x => m_bgPanel.color = x, c, 0.25f);

			// info panel slides up

//		m_infoPanel.anchoredPosition = new Vector2 (0, m_infoPanel.rect.height * -1);

			DOTween.To (() => m_infoPanel.anchoredPosition, x => m_infoPanel.anchoredPosition = x, new Vector2 (0, m_infoPanel.rect.height * -1), 0.25f).OnComplete (OnExitComplete);
		} else {

			OnExitComplete ();
		}
	}

	public virtual void OnExitComplete ()
	{
		Color c = m_bgPanel.color;
		c.a = m_darkenAmount;
		m_bgPanel.color = c;

		m_infoPanel.anchoredPosition = Vector2.zero;

		this.gameObject.SetActive (false);
	}
}
