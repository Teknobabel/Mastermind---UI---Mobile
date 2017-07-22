using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemNavBar : MonoBehaviour, IObserver {

	public Text m_currentCommandPoolText;

	private bool m_isActive = false;

	public void Initialize ()
	{
		GameController.instance.AddObserver (this);

		Player.CommandPool cp = GameController.instance.GetCommandPool (0);
		m_currentCommandPoolText.text = cp.m_currentPool.ToString ();
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

	public void CloseAppButtonClicked ()
	{
		MobileUIEngine.instance.PopApp ();
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
