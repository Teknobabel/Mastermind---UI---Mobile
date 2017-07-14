using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TurnProcessingApp : BaseApp, IObserver {

	private GameObject m_turnProcessingMenu;

	private float
		m_durationTime = 3.0f,
		m_timer = 0.0f;

	private bool m_finishedProcessingTurn = false;

	public override void InitializeApp ()
	{
		foreach (GameObject menu in m_menuBank) {

			GameObject go = (GameObject)GameObject.Instantiate (menu, Vector3.zero, Quaternion.identity);
			go.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
			m_turnProcessingMenu = go;
			m_turnProcessingMenu.SetActive (false);
		}

		GameController.instance.AddObserver (this);

		base.InitializeApp ();
	}

	public override void EnterApp ()
	{
		m_turnProcessingMenu.gameObject.SetActive (true);

		base.EnterApp ();
	}

	public override void ExitApp ()
	{
		m_turnProcessingMenu.gameObject.SetActive (false);

		m_finishedProcessingTurn = false;
		m_timer = 0.0f;

		base.ExitApp ();
	}

	public override void UpdateApp ()
	{
		base.UpdateApp ();

		m_timer = Mathf.Clamp (m_timer += Time.deltaTime, 0, m_durationTime);

		if (m_timer == m_durationTime && m_finishedProcessingTurn) {

			MobileUIEngine.instance.PopApp ();
		}

	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {

		case GameEvent.Turn_PlayerPhaseStarted:

			m_finishedProcessingTurn = true;

			break;
		}
	}
}
