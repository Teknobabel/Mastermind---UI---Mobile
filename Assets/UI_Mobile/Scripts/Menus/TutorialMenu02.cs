using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialMenu02 : Tutorial_BaseMenu {

	public RectTransform m_parentMenu;

	public List<ScriptableObject> m_unlockedApps;

	public RawImage m_backgroundImage;

	public Tutorial_AppUnlockOverlayMenu m_alert;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		if (m_alert != null) {
			m_alert.Initialize (parentApp);
		}
	}

	public override void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);
		m_skipButton.interactable = true;

		m_parentMenu.anchoredPosition += new Vector2(MobileUIEngine.instance.m_mainCanvas.rect.width, 0);
		Color c = m_backgroundImage.color;
		c.a = 0.5f;

		DOTween.To (() => m_backgroundImage.color, x => m_backgroundImage.color = x, c, 0.5f).SetEase (Ease.OutCirc).SetDelay (0.5f);
		DOTween.To (() => m_parentMenu.anchoredPosition, x => m_parentMenu.anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetEase (Ease.OutCirc).SetDelay (0.5f);
	}

	public override void OnExit (bool animate)
	{
		this.gameObject.SetActive (false);
	}

//	public override void OnHold ()
//	{
//		base.OnHold ();
//	}

	public override void OnReturn ()
	{
		base.OnReturn ();

		DismissButtonClicked ();
	}

	void Update ()
	{
		//		Debug.Log (m_buttonHeld);
		if (m_buttonHeld && m_currentHoldTime < m_holdTime) {

			m_currentHoldTime = Mathf.Clamp (m_currentHoldTime + Time.deltaTime, 0.0f, m_holdTime);

			float fillAmt = m_currentHoldTime / m_holdTime;

			m_progressBarImage.fillAmount = fillAmt;

			if (m_currentHoldTime == m_holdTime) {

				List<IApp> unlockedApps = new List<IApp> ();

				foreach (ScriptableObject so in m_unlockedApps) {

					unlockedApps.Add ((IApp)so);
				}

				if (m_alert != null) {
					
					m_parentApp.PushMenu (m_alert);

				} else {

					DismissButtonClicked ();
				}

			} else if (m_currentProgressText.Count > 0) {

				ProgressText p = m_currentProgressText [0];

				if (fillAmt >= p.m_time) {

					m_currentProgressText.RemoveAt (0);

					m_progressTextField.text = p.m_text;
				}
			}
		}
	}
}
