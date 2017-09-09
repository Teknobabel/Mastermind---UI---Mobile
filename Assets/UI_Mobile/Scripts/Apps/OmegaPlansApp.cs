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
//	private OmegaPlanHomeContainerMenu m_homeMenu;
	private PlanMissionMenu m_missionPlanningMenu;
//	private OmegaPlan_SelectHenchmenMenu m_selectHenchmenMenu;

	public override void InitializeApp ()
	{

		GameController.instance.AddObserver (this);

		// instantiate home screen base

		GameObject homeScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		m_menuParent = homeScreenGO.transform;
		homeScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_homeMenu = (BaseMenu)homeScreenGO.GetComponent<BaseMenu> ();
		m_homeMenu.Initialize (this);

		GameObject infoScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[2], Vector3.zero, Quaternion.identity);
//		m_menuParent = homeScreenGO.transform;
		infoScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_opGoalInfoOverlay = (OmegaPlan_GoalInfoMenu) infoScreenGO.GetComponent<OmegaPlan_GoalInfoMenu> ();
		m_opGoalInfoOverlay.Initialize (this);

		GameObject missionPlanningGO = (GameObject)GameObject.Instantiate (m_menuBank[3], Vector3.zero, Quaternion.identity);
		missionPlanningGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_missionPlanningMenu = (PlanMissionMenu)missionPlanningGO.GetComponent<PlanMissionMenu> ();
		m_missionPlanningMenu.Initialize (this);

//		GameObject selectHenchmenGO = (GameObject)GameObject.Instantiate (m_menuBank[4], Vector3.zero, Quaternion.identity);
//		selectHenchmenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		m_selectHenchmenMenu = (OmegaPlan_SelectHenchmenMenu)selectHenchmenGO.GetComponent<OmegaPlan_SelectHenchmenMenu> ();
//		m_selectHenchmenMenu.Initialize (this);

		m_menuParent.gameObject.SetActive (false);

		base.InitializeApp ();
	}

//	public void GoalButtonClicked ()
//	{
//		PushMenu (m_opGoalInfoOverlay);
//	}

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

//		if (opSlot.m_state == Player.OmegaPlanSlot.State.New) {
//
//			alerts++;
//		}
		for (int i = 0; i < opSlot.m_omegaPlan.phases.Count; i++) {
			
			OmegaPlan.Phase phase = opSlot.m_omegaPlan.phases [i];

			foreach (OmegaPlan.OPGoal goal in phase.m_goals) {

				if (i == opSlot.m_omegaPlan.currentPhase && goal.m_new) {

					alerts++;
				}
			}
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

	public PlanMissionMenu missionPlanningMenu {get{ return m_missionPlanningMenu;}}
//	public OmegaPlan_SelectHenchmenMenu selectHenchmenMenu {get{ return m_selectHenchmenMenu;}}
//	public OmegaPlanHomeContainerMenu homeMenu {get{ return m_homeMenu;}}
}
