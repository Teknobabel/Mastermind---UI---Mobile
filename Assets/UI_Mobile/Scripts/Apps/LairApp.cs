using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LairApp : BaseApp {

	public override void InitializeApp ()
	{
//		GameObject go = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
//		go.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		m_homeMenu = (Hire_HomeMenu)go.GetComponent<Hire_HomeMenu> ();
//		m_homeMenu.Initialize (this);
//
//		GameObject detailScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[1], Vector3.zero, Quaternion.identity);
//		detailScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		m_henchmenDetailMenu = (Hire_HenchmenDetailMenu)detailScreenGO.GetComponent<Hire_HenchmenDetailMenu>();
//		m_henchmenDetailMenu.Initialize (this);
//		m_henchmenDetailMenu.AddObserver (this);

		base.InitializeApp ();
	}

	public override void EnterApp ()
	{
//		if (m_menuStack.Count == 0) {
//
//			PushMenu (m_homeMenu);
//		}
//
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
}
