using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu]
public class HenchmenApp : BaseApp, IUIObserver {

	private ContactsMenu m_homeMenu;
	private ContactsDetailViewMenu m_henchmenDetailMenu;

	public override void InitializeApp ()
	{

		GameObject contactsScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		contactsScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (ContactsMenu)contactsScreenGO.GetComponent<ContactsMenu> ();
		m_homeMenu.Initialize (this);


		GameObject detailScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[1], Vector3.zero, Quaternion.identity);
		detailScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_henchmenDetailMenu = (ContactsDetailViewMenu)detailScreenGO.GetComponent<ContactsDetailViewMenu>();
		m_henchmenDetailMenu.Initialize (this);
		m_henchmenDetailMenu.AddObserver (this);

		base.InitializeApp ();
	}

	public void HenchmenCellClicked (int henchmenID)
	{
		Debug.Log("Henchmen Cell w id: " + henchmenID + " clicked");

		m_henchmenDetailMenu.SetHenchmen (henchmenID);

		PushMenu (m_henchmenDetailMenu);
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
}
