using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu]
public class SettingsApp : BaseApp {

//	private Settings_HomeMenu m_homeMenu;

	public override void InitializeApp ()
	{
		GameObject settingsScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		settingsScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (BaseMenu)settingsScreenGO.GetComponent<BaseMenu> ();
		m_homeMenu.Initialize (this);

		base.InitializeApp ();
	}

	public override void EnterApp ()
	{
		if (m_menuStack.Count == 0) {

			PushMenu (m_homeMenu);
		}

		base.EnterApp ();
	}
}
