using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemNavBar : MonoBehaviour, IObserver {

	public Text m_currentCommandPoolText;

	public GameObject m_backButtonParent;

	private bool m_isActive = false;

	public void Initialize ()
	{
		GameController.instance.AddObserver (this);

		Player.CommandPool cp = GameController.instance.GetCommandPool (0);
		m_currentCommandPoolText.text = cp.m_currentPool.ToString ();
		SetBackButtonState (false);
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
}
