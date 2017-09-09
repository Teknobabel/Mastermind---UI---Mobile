using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LairApp : BaseApp, IObserver {

//	private Lair_HomeMenu m_homeMenu;
	private PlanMissionMenu m_planMissionMenu;
//	private Lair_SelectMissionMenu m_selectMissionMenu;
//	private Lair_SelectSiteMenu m_selectSiteMenu;
//	private Lair_SelectHenchmenMenu m_selectHenchmenMenu;
//	private Lair_SelectTargetActorMenu m_selectTargetActorMenu;

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

//		GameObject selectHenchmenGO = (GameObject)GameObject.Instantiate (m_menuBank[2], Vector3.zero, Quaternion.identity);
//		selectHenchmenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		m_selectHenchmenMenu = (Lair_SelectHenchmenMenu)selectHenchmenGO.GetComponent<Lair_SelectHenchmenMenu>();
//		m_selectHenchmenMenu.Initialize (this);
//
//		GameObject selectMissionGO = (GameObject)GameObject.Instantiate (m_menuBank[3], Vector3.zero, Quaternion.identity);
//		selectMissionGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		m_selectMissionMenu = (Lair_SelectMissionMenu)selectMissionGO.GetComponent<Lair_SelectMissionMenu>();
//		m_selectMissionMenu.Initialize (this);
//
//		GameObject selectSiteGO = (GameObject)GameObject.Instantiate (m_menuBank[4], Vector3.zero, Quaternion.identity);
//		selectSiteGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		m_selectSiteMenu = (Lair_SelectSiteMenu)selectSiteGO.GetComponent<Lair_SelectSiteMenu>();
//		m_selectSiteMenu.Initialize (this);
//
//		GameObject selectTargetActorGO = (GameObject)GameObject.Instantiate (m_menuBank[5], Vector3.zero, Quaternion.identity);
//		selectTargetActorGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		m_selectTargetActorMenu = (Lair_SelectTargetActorMenu)selectTargetActorGO.GetComponent<Lair_SelectTargetActorMenu>();
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
		Lair l = GameController.instance.GetLair (0);

		int alerts = 0;

		foreach (Lair.FloorSlot fSlot in l.floorSlots) {

			if (fSlot.m_new && fSlot.m_state != Lair.FloorSlot.FloorState.Empty) {

				alerts++;
			}
		}

		m_appIconInstance.SetAlerts (alerts);
	}

	public void OnNotify (ISubject subject, GameEvent thisEvent)
	{
		switch (thisEvent)
		{
		case GameEvent.Player_LairChanged:

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
//	public Lair_HomeMenu homeMenu {get{ return m_homeMenu;}}
	public PlanMissionMenu planMissionMenu {get{ return m_planMissionMenu; }}
//	public Lair_SelectMissionMenu selectMissionMenu {get{ return m_selectMissionMenu; }}
//	public Lair_SelectSiteMenu selectSiteMenu {get{ return m_selectSiteMenu; }}
//	public Lair_SelectHenchmenMenu selectHenchmenMenu {get{ return m_selectHenchmenMenu; }}
//	public Lair_SelectTargetActorMenu selectTargetActorMenu {get{ return m_selectTargetActorMenu;}}
}
