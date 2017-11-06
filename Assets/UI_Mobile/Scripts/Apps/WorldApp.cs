using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WorldApp : BaseApp {

	private World_FilterMenu m_filterMenu;
	private World_DetailMenu m_detailMenu;

	public override void InitializeApp ()
	{
		GameObject go = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		go.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (BaseMenu)go.GetComponent<BaseMenu> ();
		m_homeMenu.Initialize (this);
		
		GameObject filterMenuGO = (GameObject)GameObject.Instantiate (m_menuBank[1], Vector3.zero, Quaternion.identity);
		filterMenuGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_filterMenu = (World_FilterMenu)filterMenuGO.GetComponent<World_FilterMenu>();
		m_filterMenu.Initialize (this);
		m_filterMenu.parentMenu = (World_HomeMenu) m_homeMenu;

		GameObject detailMenuGO = (GameObject)GameObject.Instantiate (m_menuBank[2], Vector3.zero, Quaternion.identity);
		detailMenuGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_detailMenu = (World_DetailMenu)detailMenuGO.GetComponent<World_DetailMenu>();
		m_detailMenu.Initialize (this);

		base.InitializeApp ();
	}

	public override void EnterApp ()
	{
		if (m_menuStack.Count == 0) {

			PushMenu (m_homeMenu);
		}

		base.EnterApp ();
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

	public World_FilterMenu filterMenu {get{ return m_filterMenu; }}
	public World_DetailMenu detailMenu {get{ return m_detailMenu; }}
}
