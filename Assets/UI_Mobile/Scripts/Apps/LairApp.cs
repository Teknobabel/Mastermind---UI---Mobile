using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LairApp : BaseApp {

	private Lair_HomeMenu m_homeMenu;
	private Lair_PlanMissionMenu m_planMissionMenu;
	private Lair_SelectMissionMenu m_selectMissionMenu;
	private Lair_SelectSiteMenu m_selectSiteMenu;
	private Lair_SelectHenchmenMenu m_selectHenchmenMenu;

	public override void InitializeApp ()
	{
		GameObject go = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		go.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (Lair_HomeMenu)go.GetComponent<Lair_HomeMenu> ();
		m_homeMenu.Initialize (this);

		GameObject planMissionGO = (GameObject)GameObject.Instantiate (m_menuBank[1], Vector3.zero, Quaternion.identity);
		planMissionGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_planMissionMenu = (Lair_PlanMissionMenu)planMissionGO.GetComponent<Lair_PlanMissionMenu>();
		m_planMissionMenu.Initialize (this);

		GameObject selectHenchmenGO = (GameObject)GameObject.Instantiate (m_menuBank[2], Vector3.zero, Quaternion.identity);
		selectHenchmenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_selectHenchmenMenu = (Lair_SelectHenchmenMenu)selectHenchmenGO.GetComponent<Lair_SelectHenchmenMenu>();
		m_selectHenchmenMenu.Initialize (this);

		GameObject selectMissionGO = (GameObject)GameObject.Instantiate (m_menuBank[3], Vector3.zero, Quaternion.identity);
		selectMissionGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_selectMissionMenu = (Lair_SelectMissionMenu)selectMissionGO.GetComponent<Lair_SelectMissionMenu>();
		m_selectMissionMenu.Initialize (this);

		GameObject selectSiteGO = (GameObject)GameObject.Instantiate (m_menuBank[4], Vector3.zero, Quaternion.identity);
		selectSiteGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_selectSiteMenu = (Lair_SelectSiteMenu)selectSiteGO.GetComponent<Lair_SelectSiteMenu>();
		m_selectSiteMenu.Initialize (this);

		base.InitializeApp ();
	}

	public override void EnterApp ()
	{
		if (m_menuStack.Count == 0) {

			PushMenu (m_homeMenu);
		}

		base.EnterApp ();
	}

	public void IdleFloorButtonClicked (int floorSlotID)
	{
		Debug.Log ("Idle floor clicked, start mission planning");

		Lair l = GameController.instance.GetLair (0);

		foreach (Lair.FloorSlot fSlot in l.floorSlots) {

			if (fSlot.m_id == floorSlotID) {

				m_planMissionMenu.floorSlot = fSlot;
				PushMenu (m_planMissionMenu);
				break;
			}
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
	public Lair_HomeMenu homeMenu {get{ return m_homeMenu;}}
	public Lair_PlanMissionMenu planMissionMenu {get{ return m_planMissionMenu; }}
	public Lair_SelectMissionMenu selectMissionMenu {get{ return m_selectMissionMenu; }}
	public Lair_SelectSiteMenu selectSiteMenu {get{ return m_selectSiteMenu; }}
	public Lair_SelectHenchmenMenu selectHenchmenMenu {get{ return m_selectHenchmenMenu; }}
}
