using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OmegaPlanHomeMenu : BaseMenu, IUIObserver  {

	public Text
	m_appNameText,
	m_phaseText;

	public GameObject
	m_opGoalCell,
	m_cellDetailPanel,
	m_cellCostPanel,
	m_newHeader,
	m_spacer;

	public Transform
	m_opGoalListParent;

	public SegmentedToggle m_infoPanelToggle;

//	private OmegaPlan.Phase m_phaseGoals;

	private int m_phaseNumber = 0;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		Player.OmegaPlanSlot omegaPlan = GameController.instance.GetOmegaPlan (0);

//		string s = parentApp.Name + ":\n";
//		s += omegaPlan.m_omegaPlan.m_name;
		m_appNameText.text = omegaPlan.m_omegaPlan.m_name;
		m_infoPanelToggle.AddObserver (this);

//		DummyOmegaPlan op = GetDummyData.instance.GetDummyOmegaPlan ();

		m_phaseText.text = omegaPlan.m_omegaPlan.m_description;


	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		Player player = GameController.instance.game.playerList [0];
		List<Player.ActorSlot> henchmenPool = GameController.instance.GetHiredHenchmen (0);

		// gather traits from currently assigned henchmen

		List<Trait> currentHenchmenTraits = new List<Trait> ();

		foreach (Player.ActorSlot aSlot in henchmenPool) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_actor != null)
			{
				foreach (Trait t in aSlot.m_actor.traits) {

					if (!currentHenchmenTraits.Contains (t)) {

						currentHenchmenTraits.Add (t);
					}
				}
			}
		}

		// gather assets

//		List<Asset> assets = new List<Asset> ();
//
//		foreach (Site.AssetSlot aSlot in player.assets) {
//
//			if (aSlot.m_state != Site.AssetSlot.State.InUse) {
//				assets.Add (aSlot.m_asset);
//			}
//		}


		Player.OmegaPlanSlot omegaPlan = GameController.instance.GetOmegaPlan (0);

		// populate with goals
//		for (int i = 0; i < omegaPlan.m_omegaPlan.phases.Count; i++)
//		{
//			if (i == m_phaseNumber) {

		OmegaPlan.Phase p = omegaPlan.m_omegaPlan.phases [m_phaseNumber];

		GameObject spacerGO = (GameObject)Instantiate (m_spacer, m_opGoalListParent);
		Cell_Spacer spacer = (Cell_Spacer)spacerGO.GetComponent<Cell_Spacer> ();
		spacer.SetHeight (40);
		m_cells.Add (spacer);

		foreach (OmegaPlan.OPGoal g in p.m_goals) {

			g.plan.m_actorSlots.Clear ();

			foreach (Player.ActorSlot aSlot in henchmenPool) {

				if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_actor != null)
				{
					g.plan.m_actorSlots.Add (aSlot);
				}
			}

			GameController.instance.CompileMission (g.plan);
//			Debug.Log (g.plan.m_successChance);

			if (g.m_new) {
				GameObject newCell = (GameObject)Instantiate (m_newHeader, m_opGoalListParent);
				UICell nCell = (UICell)newCell.GetComponent<UICell> ();
				m_cells.Add (nCell);
			}

			GameObject gCell = (GameObject)Instantiate (m_opGoalCell, m_opGoalListParent);
			Cell_OPGoal c = (Cell_OPGoal)gCell.GetComponent<Cell_OPGoal> ();
			c.SetGoal (g);
			m_cells.Add ((UICell)c);

			// set detail panel - traits

			if (g.m_mission.m_requiredTraits.Length > 0) {
				
				GameObject traitPanel = (GameObject)Instantiate (m_cellDetailPanel, m_opGoalListParent);
				Cell_DetailPanel dPanel = (Cell_DetailPanel)traitPanel.GetComponent<Cell_DetailPanel> ();
				dPanel.SetTraits (g.m_mission, currentHenchmenTraits);
				m_cells.Add (dPanel);
			}

			// set detail panel - assets

			if (g.m_mission.m_requiredAssets.Length > 0) {
				
				GameObject assetPanel = (GameObject)Instantiate (m_cellDetailPanel, m_opGoalListParent);
				Cell_DetailPanel aPanel = (Cell_DetailPanel)assetPanel.GetComponent<Cell_DetailPanel> ();
				aPanel.SetAssets (g.m_mission, Cell_DetailPanel.MissionState.Planning);
				m_cells.Add (aPanel);
			}

			// set detail panel - facilities

			if (g.m_mission.m_requiredFloors.Length > 0) {

				GameObject facilityPanel = (GameObject)Instantiate (m_cellDetailPanel, m_opGoalListParent);
				Cell_DetailPanel fPanel = (Cell_DetailPanel)facilityPanel.GetComponent<Cell_DetailPanel> ();
				fPanel.SetFacilities (g.m_mission, Cell_DetailPanel.MissionState.Normal);
				m_cells.Add (fPanel);
			}

			// set cost panel

			GameObject costPanel = (GameObject)Instantiate (m_cellCostPanel, m_opGoalListParent);
			Cell_CostPanel cPanel = (Cell_CostPanel)costPanel.GetComponent<Cell_CostPanel> ();
			cPanel.SetCostPanel (g.plan);
			m_cells.Add (cPanel);

			if (g.m_state == OmegaPlan.OPGoal.State.InProgress) {

				// enable cancel button

				c.m_buttons [0].gameObject.SetActive (false);
				c.m_buttons [1].gameObject.SetActive (true);

				c.m_buttons [1].onClick.AddListener (delegate {
					CancelMissionButtonPressed(g);
				});
			}
			else if (g.m_state != OmegaPlan.OPGoal.State.Complete) {
				c.m_buttons [0].onClick.AddListener (delegate {
					GoalButtonClicked (g);
				});
			}

			GameObject spacerGO2 = (GameObject)Instantiate (m_spacer, m_opGoalListParent);
			Cell_Spacer spacer2 = (Cell_Spacer)spacerGO2.GetComponent<Cell_Spacer> ();
			m_cells.Add (spacer2);
		}
//			}
//		}

		GameObject spacerGO3 = (GameObject)Instantiate (m_spacer, m_opGoalListParent);
		Cell_Spacer spacer3 = (Cell_Spacer)spacerGO3.GetComponent<Cell_Spacer> ();
		spacer3.SetHeight (150);
		m_cells.Add (spacer3);
	}

	public void CancelMission (OmegaPlan.OPGoal goal)
	{
		Player player = GameController.instance.game.playerList [0];

		// display toast

		string title = "Mission Aborted";
		string message = "Mission: " + goal.plan.m_currentMission.m_name + " is has been aborted.";

		goal.m_state = OmegaPlan.OPGoal.State.Incomplete;
		player.notifications.AddNotification (GameController.instance.GetTurnNumber(), title, message, EventLocation.Missions, true, goal.plan.m_missionID);
		player.RemoveMission (goal.plan);

		ParentApp.PopMenu ();

		DisplayContent ();
	}

	public void CancelMissionButtonPressed (OmegaPlan.OPGoal goal)
	{
		string header = "Abort Mission";
		string message = "Are you sure you want to abort this Mission?";

		MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
		Button b3 = MobileUIEngine.instance.alertDialogue.AddButton ("Abort");
		b3.onClick.AddListener(delegate { CancelMission(goal);});
		Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Cancel");
		b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
		m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
	}

	private void GoalButtonClicked (OmegaPlan.OPGoal goal)
	{
		// push mission planning menu

//		((OmegaPlansApp)(m_parentApp)).missionPlanningMenu.missionPlan = goal.plan;
//		ParentApp.PushMenu (((OmegaPlansApp)(m_parentApp)).missionPlanningMenu);

		// put all henchmen on mission    

		List<Player.ActorSlot> henchmenPool = GameController.instance.GetHiredHenchmen (0);

		foreach (Player.ActorSlot aSlot in henchmenPool) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {
				goal.plan.m_actorSlots.Add (aSlot);
			}
		}

		GameController.instance.CompileMission (goal.plan);

		Player.CommandPool cp = GameController.instance.GetCommandPool (0);

		if (goal.plan.m_currentMission.m_cost > cp.m_currentPool) {

			string header = "Can't Afford Mission";
			string message = "There aren't enough points in your Command Pool to start this Mission.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

			return;
		}

		if (goal.plan.m_successChance <= 0) {

			string header = "Can't Start Mission";
			string message = "You can't start a Mission with a 0% Success Chance.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

			return;
		}

		string header2 = "Start Mission";
		string message2 = "This mission will generate " + goal.plan.m_currentMission.m_cost.ToString () + " Infamy and has a " +
			goal.plan.m_successChance.ToString() + "% chance of success.";

		MobileUIEngine.instance.alertDialogue.SetAlert (header2, message2, m_parentApp);
		Button b3 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
		b3.onClick.AddListener(delegate { StartMission(goal);});
		Button b4 = MobileUIEngine.instance.alertDialogue.AddButton ("Cancel");
		b4.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
		m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);



	}

	public void StartMission (OmegaPlan.OPGoal goal)
	{
		Action_SpendCommandPoints payForMission = new Action_SpendCommandPoints ();
		payForMission.m_playerID = 0;
		payForMission.m_amount = goal.plan.m_currentMission.m_cost;
		GameController.instance.ProcessAction (payForMission);

		Action_StartNewMission newMission = new Action_StartNewMission ();
		newMission.m_missionPlan = goal.plan;
		newMission.m_playerID = 0;
		GameController.instance.ProcessAction (newMission);

		ParentApp.PopMenu ();
	}

	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent)
		{
		case UIEvent.UI_ToggleButtonPressed:

			m_phaseNumber = m_infoPanelToggle.activeButton;
//			switch (m_infoPanelToggle.activeButton) {
//			case 0:
//				m_displayType = DisplayType.Alpha;
//				break;
//			case 1:
//				m_displayType = DisplayType.Trait;
//				break;
//			case 2:
//				m_displayType = DisplayType.Mission;
//				break;
//
//			}
			DisplayContent ();
			break;
		}
	}

	public override void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		base.OnEnter (animate);

//		List<Henchmen> hList = GetDummyData.instance.GetHenchmenList ();
//
//		foreach (Henchmen h in hList) {
//
//			GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
//			UICell c = (UICell)hCell.GetComponent<UICell> ();
//			m_cells.Add (c);
//
//			string nameString = h.m_name;
//			string statusString = "Active";
//			//			s += "\nStatus: " + h.m_status;
//			//			s += "\nLocation: " + h.m_location;
//
//			c.m_headerText.text = nameString;
//			c.m_bodyText.text = statusString;
//			c.m_image.texture = h.m_portrait_Small;
//
//			hCell.GetComponent<Button>().onClick.AddListener(delegate { ((HenchmenApp)m_parentApp).HenchmenCellClicked(h.m_id); });
//			c.m_rectTransforms[0].anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);
//
//		}
//
//		// slide in animation
//		if (animate) {
//
//			RectTransform rt = gameObject.GetComponent<RectTransform> ();
//			Rect r = rt.rect;
//			rt.anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);
//
//			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, 0), 0.5f);
//
//
//			// system nav bar slides up
//
//			RectTransform sysNavRT = MobileUIEngine.instance.systemNavBar.GetComponent<RectTransform> ();
//			sysNavRT.anchoredPosition = new Vector2 (0, sysNavRT.rect.height * -1);
//			DOTween.To (() => sysNavRT.anchoredPosition, x => sysNavRT.anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetDelay (0.35f);
//
//			for (int i = 0; i < m_cells.Count; i++) {
//
//				UICell c = m_cells [i];
//				DOTween.To (() => c.m_rectTransforms [0].anchoredPosition, x => c.m_rectTransforms [0].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetEase(Ease.OutCirc).SetDelay (0.25f + (i * 0.07f));
//
//				c.m_image.transform.localScale = Vector3.zero;
//				DOTween.To (() => c.m_image.transform.localScale, x => c.m_image.transform.localScale = x, new Vector3 (1, 1, 1), 0.5f).SetEase(Ease.OutCirc).SetDelay (0.75f + (i * 0.09f));
//			}
//		}

	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		bool newStateChanged = false;

		Player.OmegaPlanSlot omegaPlan = GameController.instance.GetOmegaPlan (0);
		OmegaPlan.Phase p = omegaPlan.m_omegaPlan.phases [m_phaseNumber];

		foreach (OmegaPlan.OPGoal g in p.m_goals) {

			if (g.m_new) {

				Action_SetOmegaPlanNewState newState = new Action_SetOmegaPlanNewState ();
				newState.m_newState = false;
				newState.m_goal = g;
				GameController.instance.ProcessAction (newState);
				newStateChanged = true;
			}
		}

		if (newStateChanged) {

			m_parentApp.SetAlerts ();
		}

//		this.gameObject.SetActive (false);
	}

//	public override void OnExit (bool animate)
//	{
//		// slide out animation
//		RectTransform rt = gameObject.GetComponent<RectTransform>();
//		Rect r = rt.rect;
//
//		DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0), 0.5f);
//
//		RectTransform sysNavRT = MobileUIEngine.instance.systemNavBar.GetComponent<RectTransform> ();
//		DOTween.To (() => sysNavRT.anchoredPosition, x => sysNavRT.anchoredPosition = x, new Vector2 (0, sysNavRT.rect.height * -1), 0.25f).SetDelay (0.35f);
//
//		while (m_cells.Count > 0) {
//
//			UICell c = m_cells [0];
//			m_cells.RemoveAt (0);
//			Destroy (c.gameObject);
//		}
//
//	}

	public void OnExitComplete ()
	{

	}

	public override void OnHold ()
	{
		base.OnHold ();

		MobileUIEngine.instance.systemNavBar.SetBackButtonState (true);
	}

	public override void OnReturn ()
	{
		MobileUIEngine.instance.systemNavBar.SetBackButtonState (false);

		base.OnReturn ();
	}

//	public OmegaPlan.Phase phaseGoals {get{ return m_phaseGoals;}set{m_phaseGoals = value; }}
	public int phaseNumber {set{ m_phaseNumber = value; }}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
