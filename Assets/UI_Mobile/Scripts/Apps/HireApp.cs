using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HireApp : BaseApp, IUIObserver, IObserver {

	private Hire_HomeMenu m_homeMenu;
	private Hire_HenchmenDetailMenu m_henchmenDetailMenu;

	public override void InitializeApp ()
	{
		GameObject go = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		go.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (Hire_HomeMenu)go.GetComponent<Hire_HomeMenu> ();
		m_homeMenu.Initialize (this);

		GameObject detailScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[1], Vector3.zero, Quaternion.identity);
		detailScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_henchmenDetailMenu = (Hire_HenchmenDetailMenu)detailScreenGO.GetComponent<Hire_HenchmenDetailMenu>();
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

	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent)
		{
		case UIEvent.UI_BackButtonPressed:

			PopMenu ();
			break;
		}
	}

	public override void SetAlerts ()
	{
		List<Player.ActorSlot> hp = GameController.instance.GetHiringPool (0);

		int alerts = 0;

		foreach (Player.ActorSlot aSlot in hp) {

			if (aSlot.m_new && aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				alerts++;
			}
		}

		m_appIconInstance.SetAlerts (alerts);
	}

	public void OnNotify (ISubject subject, GameEvent thisEvent)
	{
		switch (thisEvent)
		{
		case GameEvent.Player_HiringPoolChanged:

			SetAlerts ();

			break;
		}
	}

	public Hire_HomeMenu homeMenu {get{ return m_homeMenu; }}
	public Hire_HenchmenDetailMenu detailMenu{get{ return m_henchmenDetailMenu; }}
}
