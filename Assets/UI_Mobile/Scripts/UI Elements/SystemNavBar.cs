using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SystemNavBar : MonoBehaviour, IObserver, IUIObserver {

	public Text m_currentCommandPoolText;

	public GameObject m_backButtonParent;

	public AppSwitcher m_appSwitcher;

	private bool m_isActive = false;

	public void Initialize ()
	{
		GameController.instance.AddObserver (this);

		m_appSwitcher.Initialize ();

		Player.CommandPool cp = GameController.instance.GetCommandPool (0);
		m_currentCommandPoolText.text = cp.m_currentPool.ToString ();
		SetBackButtonState (false);

		foreach (KeyValuePair<EventLocation, IApp> pair in MobileUIEngine.instance.appList) {

			BaseApp baseApp = (BaseApp)pair.Value;
			baseApp.AddObserver (this);
		}
	}

	public void SetActiveState (bool isActive)
	{
		if (!isActive) {

			m_isActive = false;
			gameObject.SetActive (false);
		} else {

			m_isActive = true;
			gameObject.SetActive (true);
		}
	}

	public void SetBackButtonState (bool isActive)
	{
		if (isActive) {
			m_backButtonParent.SetActive (true);
		} else {
			m_backButtonParent.SetActive (false);
		}
	}

	public void CloseAppButtonClicked ()
	{
		MobileUIEngine.instance.PopApp ();
	}

	public void BackButtonClicked ()
	{
		MobileUIEngine.instance.GetCurrentApp ().PopMenu ();	
	}
		
	public bool isActive {get{ return m_isActive; }}
	
	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {

		case GameEvent.Player_CommandPoolChanged:

			Player.CommandPool cp = GameController.instance.GetCommandPool (0);
			m_currentCommandPoolText.text = cp.m_currentPool.ToString ();

			break;
		}
	}

	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent) {

		case UIEvent.App_Return:
		case UIEvent.App_Enter:
			
			BaseApp baseApp = (BaseApp)subject;
			Debug.Log ("System nav bar detects app enter " + baseApp.m_name);
			if (baseApp.WantsSystemNavBar) {

				if (!m_isActive) {

					// system nav bar slides up

					SetActiveState (true);
					RectTransform sysNavRT = GetComponent<RectTransform> ();
					sysNavRT.anchoredPosition = new Vector2 (0, sysNavRT.rect.height * -1);
					sysNavRT.gameObject.SetActive (true);
					DOTween.To (() => sysNavRT.anchoredPosition, x => sysNavRT.anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetDelay (0.35f);
				}

			} else {

				if (m_isActive) {
					
					RectTransform sysNavRT = GetComponent<RectTransform> ();
					DOTween.To (() => sysNavRT.anchoredPosition, x => sysNavRT.anchoredPosition = x, new Vector2 (0, sysNavRT.rect.height * -1), 0.25f).SetDelay (0.35f).OnComplete (OnExitComplete);
				}
			}

			m_appSwitcher.AppEntered ((IApp)subject);

			break;
		case UIEvent.App_Exit:
//			Debug.Log ("System nav bar detects app exit");
			m_appSwitcher.AppExited ((IApp)subject);
			break;
		}
	}

	public void OnExitComplete ()
	{
		SetActiveState (false);
	}
}
