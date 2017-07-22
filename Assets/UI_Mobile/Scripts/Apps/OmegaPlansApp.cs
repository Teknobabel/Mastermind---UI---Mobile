using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[CreateAssetMenu]
public class OmegaPlansApp : BaseApp, IObserver {

//	private OmegaPlanHomeMenu m_homeMenu;

	private Transform m_menuParent = null;

	private OmegaPlan_GoalInfoMenu m_opGoalInfoOverlay;
	private OmegaPlanHomeContainerMenu m_homeMenu;

	public override void InitializeApp ()
	{

		GameController.instance.AddObserver (this);

		// instantiate home screen base

		GameObject homeScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		m_menuParent = homeScreenGO.transform;
		homeScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (OmegaPlanHomeContainerMenu)homeScreenGO.GetComponent<OmegaPlanHomeContainerMenu> ();
		m_homeMenu.Initialize (this);

		GameObject infoScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[2], Vector3.zero, Quaternion.identity);
//		m_menuParent = homeScreenGO.transform;
		infoScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_opGoalInfoOverlay = (OmegaPlan_GoalInfoMenu) infoScreenGO.GetComponent<OmegaPlan_GoalInfoMenu> ();
		m_opGoalInfoOverlay.Initialize (this);

		m_menuParent.gameObject.SetActive (false);

		base.InitializeApp ();
	}

	public void GoalButtonClicked ()
	{
		PushMenu (m_opGoalInfoOverlay);
	}

	public override void EnterApp ()
	{
		if (m_menuStack.Count == 0) {

			PushMenu (m_homeMenu);
		}

		base.EnterApp ();


	}

	public override void ExitApp ()
	{
		base.ExitApp ();
	}

	public override void SetAlerts ()
	{
		Player.OmegaPlanSlot opSlot = GameController.instance.GetOmegaPlan (0);

		int alerts = 0;

		if (opSlot.m_state == Player.OmegaPlanSlot.State.New) {

			alerts++;
		}

		m_appIconInstance.SetAlerts (alerts);
	}

	public void OnNotify (ISubject subject, GameEvent thisEvent)
	{
		switch (thisEvent)
		{
		case GameEvent.Player_OmegaPlanChanged:

			SetAlerts ();

			break;
		}
	}
}
