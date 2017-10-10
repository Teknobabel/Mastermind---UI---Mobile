using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OmegaPlanHomeContainerMenu : BaseMenu, IUIObserver {

	public Transform
	m_contentParent;

	public PageIndicator
	m_pageIndicator;

	private List<IMenu> m_childScreens = new List<IMenu> ();

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);
//		m_appNameText.text = parentApp.Name;
	//		m_infoPanelToggle.AddObserver (this);
	//		m_infoPanelToggle.ToggleButtonClicked (0);

		float screenWidth = MobileUIEngine.instance.m_mainCanvas.sizeDelta.x;
		float screenHeight = MobileUIEngine.instance.m_mainCanvas.sizeDelta.y;

		LayoutElement hsle = m_contentParent.GetComponent<LayoutElement> ();
		hsle.preferredWidth = screenWidth;
		hsle.preferredHeight = screenHeight;

		Player.OmegaPlanSlot op = GameController.instance.GetOmegaPlan (0);

		int numPages = op.m_omegaPlan.phases.Count;

		for (int i = 0; i < numPages; i++) {

			GameObject go = (GameObject)GameObject.Instantiate (m_parentApp.MenuBank[1], m_contentParent);
			LayoutElement le = go.GetComponent<LayoutElement> ();
			le.preferredWidth = screenWidth;
			le.preferredHeight = screenHeight;

			OmegaPlanHomeMenu screen = go.GetComponent<OmegaPlanHomeMenu> ();
			screen.phaseGoals = op.m_omegaPlan.phases [i];
			screen.phaseNumber = i + 1;
			screen.Initialize (m_parentApp);
			m_childScreens.Add (screen);

		}

		ScrollRectSnap srt = (ScrollRectSnap) this.GetComponent<ScrollRectSnap> ();
		srt.screens = numPages;
		srt.Initialize ();
		srt.AddObserver (this);

		m_pageIndicator.SetNumPages (numPages);
		m_pageIndicator.SetPage (0);

		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		base.OnEnter (animate);

		foreach (IMenu menu in m_childScreens) {

			menu.OnEnter (animate);
		}

		this.gameObject.SetActive (true);

		// display first time help popup if enabled

		int helpEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.HelpEnabled);
		int firstTimeEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.FirstTime_OmegaPlan);

		if (helpEnabled == 1 && firstTimeEnabled == 1) {

			string header = "Omega Plan App";
			string message = "Your Omega Plan has 3 Phases, which each contain 3 Missions. Complete all 3 Phases to ensure world domination. You'll need Henchmen to carry out your orders and do" +
				" the dirty work.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener (delegate {
				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
			});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_OmegaPlan, 0);

		} else if (helpEnabled == 0 && firstTimeEnabled == 1) {

			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_OmegaPlan, 0);
		}
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		foreach (IMenu menu in m_childScreens) {

			menu.OnExit (animate);
		}

		Player.OmegaPlanSlot op = GameController.instance.GetOmegaPlan (0);

		for (int i = 0; i < op.m_omegaPlan.phases.Count; i++) {
			
			OmegaPlan.Phase phase = op.m_omegaPlan.phases [i];

			if (i == op.m_omegaPlan.currentPhase) {

				foreach (OmegaPlan.OPGoal goal in phase.m_goals) {

					if (goal.m_new) {

						Action_SetOmegaPlanNewState newState = new Action_SetOmegaPlanNewState ();
						newState.m_newState = false;
						newState.m_goal = goal;
						GameController.instance.ProcessAction (newState);
					}
				}
			}
		}

//		if (op.m_state == Player.OmegaPlanSlot.State.New) {
//
//			Action_SetOmegaPlanNewState opState = new Action_SetOmegaPlanNewState ();
//			opState.m_newState = false;
//			opState.m_omegaPlanSlot = op;
//			GameController.instance.ProcessAction (opState);
//		}

		this.gameObject.SetActive (false);
	}

	public override void OnHold ()
	{
		base.OnHold ();

		MobileUIEngine.instance.systemNavBar.SetBackButtonState (true);
	}

	public override void OnReturn ()
	{
		MobileUIEngine.instance.systemNavBar.SetBackButtonState (false);

		if (m_isDirty) {

			// refresh goals screens

			foreach (IMenu menu in m_childScreens) {

				menu.OnReturn ();
			}
		}

		base.OnReturn ();
	}

	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent)
		{
		case UIEvent.UI_PageChanged:

			m_pageIndicator.SetPage (((ScrollRectSnap)subject).target);
			break;
		}
	}
}
