using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MessagesApp : BaseApp, IUIObserver {

	private Messages_HomeMenu m_homeMenu;
	private Messages_DetailMenu m_detailMenu;
	private Messages_NewMessageOverlay m_newMessageOverlay;

	public override void InitializeApp ()
	{
		GameObject go = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		go.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (Messages_HomeMenu)go.GetComponent<Messages_HomeMenu> ();
		m_homeMenu.Initialize (this);

		GameObject detailScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[1], Vector3.zero, Quaternion.identity);
		detailScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_detailMenu = (Messages_DetailMenu)detailScreenGO.GetComponent<Messages_DetailMenu>();
		m_detailMenu.Initialize (this);
		m_detailMenu.AddObserver (this);

		GameObject overlayGO = (GameObject)GameObject.Instantiate (m_menuBank[2], Vector3.zero, Quaternion.identity);
		overlayGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		IMenu overlay = (IMenu)overlayGO.GetComponent<IMenu> ();
		overlay.Initialize (this);
		m_newMessageOverlay = (Messages_NewMessageOverlay)overlay;

		base.InitializeApp ();
	}
		
	public void HenchmenCellClicked (int henchmenID)
	{
		Debug.Log("Henchmen Cell w id: " + henchmenID + " clicked");

		m_detailMenu.SetHenchmen (henchmenID);
//
		PushMenu (m_detailMenu);
	}

	public override void EnterApp ()
	{
		if (m_menuStack.Count == 0) {

			PushMenu (m_homeMenu);
		}

		base.EnterApp ();
	}

	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent)
		{
		case UIEvent.UI_BackButtonPressed:

			PopMenu ();
			break;
		}
	}

	public Messages_NewMessageOverlay newMessageOverlay {get{ return m_newMessageOverlay; }}
}
