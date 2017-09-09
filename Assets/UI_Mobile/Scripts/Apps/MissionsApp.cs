using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MissionsApp : BaseApp, IObserver {

//	private Missions_HomeMenu m_homeMenu;
//	private Missions_MissionOverviewMenu m_missionOverviewMenu;
	private PlanMissionMenu m_planMissionMenu;
//	private Missions_PlanMissionMenu_SelectMissionMenu m_selectMissionMenu;

//	private Missions_PlanMissionMenu_SelectHenchmenMenu m_selectHenchmenMenu;
//	private Missions_PlanMissionMenu_SelectSiteMenu m_selectSiteMenu;
//	private Missions_PlanMissionMenu_SelectTargetActorMenu m_selectTargetActorMenu;

	public override void InitializeApp ()
	{
		GameObject go = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		go.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (BaseMenu)go.GetComponent<BaseMenu> ();
		m_homeMenu.Initialize (this);
	
		GameObject planMissionGO = (GameObject)GameObject.Instantiate (m_menuBank[1], Vector3.zero, Quaternion.identity);
		planMissionGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_planMissionMenu = (PlanMissionMenu)planMissionGO.GetComponent<PlanMissionMenu>();
		m_planMissionMenu.Initialize (this);

//		GameObject planMissionGO = (GameObject)GameObject.Instantiate (m_menuBank[1], Vector3.zero, Quaternion.identity);
//		planMissionGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		m_planMissionMenu = (Missions_PlanMissionMenu)planMissionGO.GetComponent<Missions_PlanMissionMenu>();
//		m_planMissionMenu.Initialize (this);

//		GameObject selectMissionGO = (GameObject)GameObject.Instantiate (m_menuBank[2], Vector3.zero, Quaternion.identity);
//		selectMissionGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		m_selectMissionMenu = (Missions_PlanMissionMenu_SelectMissionMenu)selectMissionGO.GetComponent<Missions_PlanMissionMenu_SelectMissionMenu>();
//		m_selectMissionMenu.Initialize (this);
//
//		GameObject selectHenchmenGO = (GameObject)GameObject.Instantiate (m_menuBank[3], Vector3.zero, Quaternion.identity);
//		selectHenchmenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		m_selectHenchmenMenu = (Missions_PlanMissionMenu_SelectHenchmenMenu)selectHenchmenGO.GetComponent<Missions_PlanMissionMenu_SelectHenchmenMenu>();
//		m_selectHenchmenMenu.Initialize (this);
//
//		GameObject selectSiteGO = (GameObject)GameObject.Instantiate (m_menuBank[4], Vector3.zero, Quaternion.identity);
//		selectSiteGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		m_selectSiteMenu = (Missions_PlanMissionMenu_SelectSiteMenu)selectSiteGO.GetComponent<Missions_PlanMissionMenu_SelectSiteMenu>();
//		m_selectSiteMenu.Initialize (this);
//
//		GameObject selectTargetActorGO = (GameObject)GameObject.Instantiate (m_menuBank[5], Vector3.zero, Quaternion.identity);
//		selectTargetActorGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		m_selectTargetActorMenu = (Missions_PlanMissionMenu_SelectTargetActorMenu)selectTargetActorGO.GetComponent<Missions_PlanMissionMenu_SelectTargetActorMenu>();
//		m_selectTargetActorMenu.Initialize (this);

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
		case GameEvent.Player_MissionCancelled:
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

//	public Missions_HomeMenu homeMenu {get{ return m_homeMenu; }}
//	public Missions_MissionOverviewMenu missionOverviewMenu {get{ return m_missionOverviewMenu; }}
	public PlanMissionMenu planMissionMenu {get{ return m_planMissionMenu; }}
//	public Missions_PlanMissionMenu_SelectMissionMenu selectMissionMenu {get{ return m_selectMissionMenu; }}
//	public Missions_PlanMissionMenu_SelectSiteMenu selectSiteMenu {get{ return m_selectSiteMenu; }}
//	public Missions_PlanMissionMenu_SelectHenchmenMenu selectHenchmenMenu {get{ return m_selectHenchmenMenu; }}
//	public Missions_PlanMissionMenu_SelectTargetActorMenu selectTargetActorMenu {get{ return m_selectTargetActorMenu; }}
}
