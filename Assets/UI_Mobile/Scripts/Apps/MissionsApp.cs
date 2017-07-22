using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MissionsApp : BaseApp, IObserver {

	private Missions_HomeMenu m_homeMenu;

	public override void InitializeApp ()
	{
		GameObject go = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		go.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (Missions_HomeMenu)go.GetComponent<Missions_HomeMenu> ();
		m_homeMenu.Initialize (this);
	
		//		GameObject detailScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[1], Vector3.zero, Quaternion.identity);
		//		detailScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		//		m_henchmenDetailMenu = (Hire_HenchmenDetailMenu)detailScreenGO.GetComponent<Hire_HenchmenDetailMenu>();
		//		m_henchmenDetailMenu.Initialize (this);
		//		m_henchmenDetailMenu.AddObserver (this);

		GameController.instance.AddObserver (this);

		base.InitializeApp ();
	}

	public override void EnterApp ()
	{
		if (m_menuStack.Count == 0) {

			PushMenu (m_homeMenu);
		}

		base.EnterApp ();
	}

	public override void SetAlerts ()
	{
		List<MissionPlan> mp = GameController.instance.GetMissions (0);

		int alerts = 0;

		foreach (MissionPlan m in mp) {

			if (m.m_new) {

				alerts++;
			}
		}

		m_appIconInstance.SetAlerts (alerts);
	}

	public void OnNotify (ISubject subject, GameEvent thisEvent)
	{
		switch (thisEvent)
		{
		case GameEvent.Player_MissionCompleted:
		case GameEvent.Player_NewMissionStarted:

			SetAlerts ();

			break;
		}
	}

	//	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	//	{
	//		switch (thisUIEvent)
	//		{
	//		case UIEvent.UI_BackButtonPressed:
	//
	//			PopMenu ();
	//			break;
	//		}
	//	}

	public Missions_HomeMenu homeMenu {get{ return m_homeMenu; }}
}
