using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu]
public class HenchmenApp : BaseApp, IUIObserver, IObserver {

//	private ContactsMenu m_homeMenu;
	private ContactsDetailViewMenu m_henchmenDetailMenu;

	public override void InitializeApp ()
	{

		GameObject contactsScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		contactsScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (BaseMenu)contactsScreenGO.GetComponent<BaseMenu> ();
		m_homeMenu.Initialize (this);


		GameObject detailScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[1], Vector3.zero, Quaternion.identity);
		detailScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_henchmenDetailMenu = (ContactsDetailViewMenu)detailScreenGO.GetComponent<ContactsDetailViewMenu>();
		m_henchmenDetailMenu.Initialize (this);
		m_henchmenDetailMenu.AddObserver (this);

		GameController.instance.AddObserver (this);

		base.InitializeApp ();
	}

//	public void HenchmenCellClicked (int henchmenID)
//	{
//		Debug.Log("Henchmen Cell w id: " + henchmenID + " clicked");
//
//		m_henchmenDetailMenu.SetHenchmen (henchmenID);
//
//		PushMenu (m_henchmenDetailMenu);
//	}

	public override void EnterApp ()
	{
		if (m_menuStack.Count == 0) {

			PushMenu (m_homeMenu);
		}

		base.EnterApp ();
	}

	public override void SetAlerts ()
	{
		List<Player.ActorSlot> hp = GameController.instance.GetHiredHenchmen (0);

		int alerts = 0;

		foreach (Player.ActorSlot aSlot in hp) {

			if (aSlot.m_new && aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				alerts++;
			}
		}

		m_appIconInstance.SetAlerts (alerts);
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

	public void OnNotify (ISubject subject, GameEvent thisEvent)
	{
		switch (thisEvent)
		{
		case GameEvent.Henchmen_NewStateChanged:
		case GameEvent.Player_HenchmenPoolChanged:

			SetAlerts ();

			break;
		}
	}

//	public ContactsMenu homeMenu {get{ return m_homeMenu; }}
	public ContactsDetailViewMenu detailMenu {get{return m_henchmenDetailMenu; }}
}
