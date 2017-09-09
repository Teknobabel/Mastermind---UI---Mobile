using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TurnProcessingApp : BaseApp, IObserver {

//	private TurnProcessing_HomeMenu m_turnProcessingMenu;
	private Alert_MissionReport m_missionReport;

	private float
		m_durationTime = 3.0f,
		m_timer = 0.0f;

	private bool m_finishedProcessingTurn = false;

//	private List<MissionSummary> m_completedMissions = new List<MissionSummary> ();

	public override void InitializeApp ()
	{
		
		GameObject go = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		go.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (BaseMenu)go.GetComponent<BaseMenu>();
		m_homeMenu.Initialize (this);

		GameObject missionReportGO = (GameObject)GameObject.Instantiate (m_menuBank[1], Vector3.zero, Quaternion.identity);
		missionReportGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_missionReport = (Alert_MissionReport)missionReportGO.GetComponent<Alert_MissionReport>();
		m_missionReport.Initialize (this);

		GameController.instance.AddObserver (this);

		base.InitializeApp ();
	}

	public override void EnterApp ()
	{
		if (m_menuStack.Count == 0) {

			PushMenu (m_homeMenu);
		}

//		m_turnProcessingMenu.gameObject.SetActive (true);

//		Action_EndPhase endTurn = new Action_EndPhase ();
//		GameController.instance.ProcessAction (endTurn);
//
//		base.EnterApp ();
//
//		Player player = GameController.instance.game.playerList [0];
//
//		m_completedMissions = player.missionsCompletedThisTurn;
	}

//	public override void ExitApp ()
//	{
////		m_turnProcessingMenu.gameObject.SetActive (false);
//
//		m_finishedProcessingTurn = false;
//		m_timer = 0.0f;
//
//		base.ExitApp ();
//	}

//	public override void UpdateApp ()
//	{
//		base.UpdateApp ();
//
//		m_timer = Mathf.Clamp (m_timer += Time.deltaTime, 0, m_durationTime);
//
//		if (m_timer == m_durationTime && m_finishedProcessingTurn) {
//
//
//			if (m_completedMissions.Count > 0) {
//				
//				MissionSummary ms = m_completedMissions [0];
//				m_completedMissions.RemoveAt (0);
//
//				m_missionReport.missionSummary = ms;
//				PushMenu (m_missionReport);
//				return;
//			} else {
//
//				Player player = GameController.instance.game.playerList [0];
//				player.missionsCompletedThisTurn.Clear ();
//				((HomeScreenApp)MobileUIEngine.instance.homeScreenApp).NewTurnStarted ();
//				MobileUIEngine.instance.PopApp ();
//			}
//		}
//
//	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {

		case GameEvent.Turn_PlayerPhaseStarted:

//			Player player = GameController.instance.game.playerList [0];
//
//			m_completedMissions = player.missionsCompletedThisTurn;

			m_finishedProcessingTurn = true;

			break;
		}
	}

	public Alert_MissionReport missionReport {get{ return m_missionReport; }}
}
