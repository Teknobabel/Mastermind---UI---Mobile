using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsApp : BaseApp, IObserver {

	private Assets_HomeMenu m_homeMenu;

	public override void InitializeApp ()
	{
		GameObject go = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		go.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (Assets_HomeMenu)go.GetComponent<Assets_HomeMenu> ();
		m_homeMenu.Initialize (this);

		GameController.instance.AddObserver (this);
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
		base.SetAlerts ();

		int alerts = 0;

		List<Site.AssetSlot> assets = GameController.instance.GetAssets (0);

		foreach (Site.AssetSlot aSlot in assets) {

			if (aSlot.m_new) {
				alerts++;
			}
		}

		m_appIconInstance.SetAlerts (alerts);
	}

	public void OnNotify (ISubject subject, GameEvent thisEvent)
	{
		switch (thisEvent)
		{
		case GameEvent.Player_AssetsChanged:

			SetAlerts ();

			break;
		}
	}
}
