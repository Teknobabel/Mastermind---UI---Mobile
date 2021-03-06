﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialMenu : Tutorial_BaseMenu {





	public Text 
	m_headerText,
	m_bodyText,
	m_skipText;



	public RectTransform[]
		m_panels;

	public RawImage[]
		m_rawImages;

	public Image[]
		m_images;

	public Text[]
		m_text;

	public Tutorial_AppUnlockOverlayMenu m_alert;


	// Use this for initialization
	void Start () {

	}

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

//		m_alert.Initialize (parentApp);
	}

	void Update ()
	{
		//		Debug.Log (m_buttonHeld);
		if (m_buttonHeld && m_currentHoldTime < m_holdTime) {

			m_currentHoldTime = Mathf.Clamp (m_currentHoldTime + Time.deltaTime, 0.0f, m_holdTime);

			float fillAmt = m_currentHoldTime / m_holdTime;

			m_progressBarImage.fillAmount = fillAmt;

			if (m_currentHoldTime == m_holdTime) {

//				m_parentApp.PushMenu (m_alert);

				((TutorialApp)m_parentApp).NextTutorialMenu ();



			} else if (m_currentProgressText.Count > 0) {

				ProgressText p = m_currentProgressText [0];

				if (fillAmt >= p.m_time) {

					m_currentProgressText.RemoveAt (0);

					m_progressTextField.text = p.m_text;
				}
			}
		}
	}



	public override void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);
//
//		// slide in animation
		if (animate) {
//
//			// fade in background
//			Color headerColor = m_headerText.color;
//			headerColor.a = 0;
//			m_headerText.color = headerColor;
//			headerColor.a = 1;
//
//			Color bodyColor = m_bodyText.color;
//			bodyColor.a = 0;
//			m_bodyText.color = bodyColor;
//			bodyColor.a = 1;
//
//			Color thumbColor = m_rawImages [0].color;
//			thumbColor.a = 0;
//			m_rawImages [0].color = thumbColor;
//			thumbColor.a = 1;

			Color startColor = m_text [0].color;
			Color startSkipColor = m_text [3].color;

			foreach (Text t in m_text) {
				
				Color c = t.color;
				c.a = 0;
				t.color = c;
			}

			foreach (Image i in m_images) {

				Color c = i.color;
				c.a = 0;
				i.color = c;
			}

			foreach (RawImage ri in m_rawImages) {

				Color c = ri.color;
				c.a = 0;
				ri.color = c;
			}

//			float startColor;
//			Color progressColor = m_images [0].color;
//			startColor = progressColor.a;
//			progressColor.a = 0;
//			m_images [0].color = progressColor;
//			progressColor.a = startColor;

//			Color skipColor = m_skipText.color;
//			skipColor.a = 0;
//			m_skipText.color = skipColor;
//			skipColor.a = 1;
			m_skipButton.interactable = false;


//			m_panels[0].anchoredPosition = new Vector2 (0, -300);
//			m_panels[1].anchoredPosition = new Vector2 (0, -300);
//			m_panels[2].anchoredPosition = new Vector2 (0, -300);

			foreach (RectTransform rt in m_panels) {

				rt.anchoredPosition = new Vector2 (0, -300);
			}

			DOTween.To (() => m_panels[0].anchoredPosition, x => m_panels[0].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetDelay(1.0f);
			DOTween.To (() => m_panels [1].anchoredPosition, x => m_panels [1].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetDelay (2.5f);
			DOTween.To (() => m_panels [2].anchoredPosition, x => m_panels [2].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetDelay (4.5f);
			DOTween.To (() => m_panels [3].anchoredPosition, x => m_panels [3].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetDelay (6.5f);
//			DOTween.To (() => m_panels [4].anchoredPosition, x => m_panels [4].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetDelay (9.5f);

			DOTween.To (() => m_text[0].color, x => m_text[0].color = x, startColor, 0.25f).SetDelay(1.0f);
			DOTween.To (() => m_text[1].color, x => m_text[1].color = x, startColor, 0.25f).SetDelay(2.5f);
			DOTween.To (() => m_text[2].color, x => m_text[2].color = x, startColor, 0.25f).SetDelay(4.5f);
			DOTween.To (() => m_text [3].color, x => m_text [3].color = x, startSkipColor, 0.25f).SetDelay (1.0f).OnComplete(EnableSkipButton);
			DOTween.To (() => m_rawImages [0].color, x => m_rawImages [0].color = x, startColor, 0.5f).SetDelay(7.5f);
			DOTween.To (() => m_images [0].color, x => m_images [0].color = x, Color.black, 0.5f).SetDelay(7.5f);
			DOTween.To (() => m_images [1].color, x => m_images [1].color = x, Color.grey, 0.5f).SetDelay(7.5f);

//			DOTween.To (() =>  m_text[4].color, x =>  m_text[4].color = x, Color.grey, 0.5f).SetDelay(7.5f).OnComplete(EnableSkipButton);

		} else {
//
//			Color c = m_bgPanel.color;
//			c.a = m_darkenAmount;
//			m_bgPanel.color = c;
//
//			m_infoPanel.anchoredPosition = Vector2.zero;
		}
	}

	public void EnableSkipButton ()
	{
		Debug.Log ("Enabling Skip Button");
		m_skipButton.interactable = true;
	}

	public override void OnExit (bool animate)
	{
		this.gameObject.SetActive (false);

//		if (animate) {

//			Color headerColor = m_headerText.color;
//			headerColor.a = 0;
//
//			Color bodyColor = m_bodyText.color;
//			bodyColor.a = 0;
//
//			Color thumbColor = m_rawImages [0].color;
//			thumbColor.a = 0;
//
//			Color progressBGColor = m_images [0].color;
//			progressBGColor.a = 0;
//
//			Color progressColor = m_images [1].color;
//			progressColor.a = 0;
//
//			Color progressTextColor = m_progressTextField.color;
//			progressTextColor.a = 0;
//
//			DOTween.To (() => m_headerText.color, x => m_headerText.color = x, headerColor, 0.25f).SetDelay(0.25f);
//			DOTween.To (() => m_bodyText.color, x => m_bodyText.color = x, bodyColor, 0.25f).SetDelay(0.5f);
//			DOTween.To (() => m_progressTextField.color, x => m_progressTextField.color = x, progressTextColor, 0.25f).SetDelay(0.75f);
//			DOTween.To (() => m_rawImages [0].color, x => m_rawImages [0].color = x, thumbColor, 0.25f).SetDelay(1.0f);
//			DOTween.To (() => m_images [0].color, x => m_images [0].color = x, progressBGColor, 0.25f).SetDelay(1.0f);
//			DOTween.To (() => m_images [1].color, x => m_images [1].color = x, progressColor, 0.25f).SetDelay(1.0f).OnComplete (OnExitComplete);

//			// fade in background
//			Color c = m_bgPanel.color;
//			float fadeDest = c.a;
//			c.a = 0;
//
//			DOTween.To (() => m_bgPanel.color, x => m_bgPanel.color = x, c, 0.25f);
//
//			// info panel slides up
//
//			//		m_infoPanel.anchoredPosition = new Vector2 (0, m_infoPanel.rect.height * -1);
//
//			DOTween.To (() => m_infoPanel.anchoredPosition, x => m_infoPanel.anchoredPosition = x, new Vector2 (0, m_infoPanel.rect.height * -1), 0.25f).OnComplete (OnExitComplete);
//		} else {
//
//			OnExitComplete ();
//		}
	}

	
//	// Update is called once per frame
//	void Update () {
//		
//	}
}
