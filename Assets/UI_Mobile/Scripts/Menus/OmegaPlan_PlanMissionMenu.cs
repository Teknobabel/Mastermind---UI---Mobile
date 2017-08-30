using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OmegaPlan_PlanMissionMenu : BaseMenu {

	public GameObject
	m_missionOverviewCellGO,
	m_selectMissionCellGO,
	m_selectSiteCellGO,
	m_selectHenchmenCellGO,
	m_traitCellGO,
	m_startMissionCellGO,
	m_cancelMissionCellGO;

	public Transform
	m_contentParent;

	private OmegaPlan.OPGoal m_goal;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		//		m_appNameText.text = parentApp.Name;
		//		m_infoPanelToggle.AddObserver (this);
		//		m_infoPanelToggle.ToggleButtonClicked (0);
		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		base.OnEnter (animate);

		this.gameObject.SetActive (true);

		// recompile to account for any changes since last visit

		GameController.instance.CompileMission (m_goal.plan);

		DisplayContent ();

	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		this.gameObject.SetActive (false);
	}


	public override void OnReturn ()
	{
		if (m_isDirty) {

			GameController.instance.CompileMission (m_goal.plan);

			DisplayContent ();
		}

		base.OnReturn ();
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		// display mission overview cell
		Debug.Log(m_goal.plan.m_currentMission);
		GameObject missionOverviewCellGO = (GameObject)Instantiate (m_missionOverviewCellGO, m_contentParent);
		UICell missionOverviewCell = (UICell)missionOverviewCellGO.GetComponent<UICell> ();
		m_cells.Add (missionOverviewCell);
		missionOverviewCell.m_bodyText.text = "Success Chance: " + m_goal.plan.m_successChance.ToString () + "%";

		// if mission has been compiled, display traits

		foreach (Trait t in m_goal.plan.m_requiredTraits) {

			GameObject traitCellGO = (GameObject)Instantiate (m_traitCellGO, m_contentParent);
			UICell traitCell = (UICell)traitCellGO.GetComponent<UICell> ();
			m_cells.Add (traitCell);
			traitCell.m_headerText.text = t.m_name;

			if (m_goal.plan.m_matchingTraits.Contains (t)) {

				traitCell.m_headerText.color = Color.green;
			}
		}

		// if mission has been compiled, display assets

		if (goal.plan.m_requiredAssets.Count > 0) {

			Player player = GameEngine.instance.game.playerList [0];
			List<Asset> assets = new List<Asset> ();

			foreach (Site.AssetSlot aSlot in m_goal.plan.m_linkedPlayerAssets) {

				assets.Add (aSlot.m_asset);
			}

			foreach (Asset a in goal.plan.m_requiredAssets) {

				GameObject assetCellGO = (GameObject)Instantiate (m_traitCellGO, m_contentParent);
				UICell assetCell = (UICell)assetCellGO.GetComponent<UICell> ();
				m_cells.Add (assetCell);
				assetCell.m_headerText.text = "Asset: " + a.m_name;

				if (assets.Contains(a))
				{
					assets.Remove (a);
					assetCell.m_headerText.color = Color.green;
				}
			}
		}

		// display current state of mission selection

		GameObject selectMissionCellGO = (GameObject)Instantiate (m_selectMissionCellGO, m_contentParent);
		UICell selectMissionCell = (UICell)selectMissionCellGO.GetComponent<UICell> ();
		m_cells.Add (selectMissionCell);

		if (m_goal.plan.m_currentMission != null) {

			selectMissionCell.m_headerText.text = "Current Mission: " + m_goal.plan.m_currentMission.m_name;
		}

		Button b = selectMissionCell.m_buttons [0];
		b.gameObject.SetActive (false);
//		b.onClick.AddListener (delegate {
//			SelectMissionButtonPressed ();
//		});



		// display current state of site selection

		if (m_goal.plan.m_currentMission != null && m_goal.plan.m_currentMission.m_targetType == Mission.TargetType.Actor) {

			GameObject selectHenchmenCellGO = (GameObject)Instantiate (m_selectHenchmenCellGO, m_contentParent);
			UICell selectHenchmenCell = (UICell)selectHenchmenCellGO.GetComponent<UICell> ();
			m_cells.Add (selectHenchmenCell);

			if (m_goal.plan.m_targetActor != null) {

				selectHenchmenCell.m_headerText.text = "Target Henchmen: " + m_goal.plan.m_targetActor.m_actor.m_actorName;
			} else {
				selectHenchmenCell.m_headerText.text = "Select Target Henchmen: ";
			}

			Button b2 = selectHenchmenCell.m_buttons [0];

			if (m_goal.m_state == OmegaPlan.OPGoal.State.Incomplete) {
				
				b2.onClick.AddListener (delegate {
					SelectTargetActorButtonPressed ();
				});

			} else {

				b2.gameObject.SetActive (false);
			}
		}
		else if (m_goal.plan.m_currentMission == null || (m_goal.plan.m_currentMission != null
			&& m_goal.plan.m_currentMission.m_targetType != Mission.TargetType.Lair)) {

			GameObject selectSiteCellGO = (GameObject)Instantiate (m_selectSiteCellGO, m_contentParent);
			UICell selectSiteCell = (UICell)selectSiteCellGO.GetComponent<UICell> ();
			m_cells.Add (selectSiteCell);

			if (m_goal.plan.m_missionSite != null) {

				string s = "Current Site: " + m_goal.plan.m_missionSite.m_siteName;

				if (m_goal.plan.m_currentMission != null && m_goal.plan.m_currentMission.m_targetType == Mission.TargetType.Asset) {

					s += ", Asset: " + m_goal.plan.m_currentAsset.m_asset.m_name;
				}

				selectSiteCell.m_headerText.text = s;
			}

			Button b2 = selectSiteCell.m_buttons [0];

			if (m_goal.m_state == OmegaPlan.OPGoal.State.Incomplete) {

				b2.onClick.AddListener (delegate {
					SelectSiteButtonPressed ();
				});
			} else {

				b2.gameObject.SetActive (false);

			}

		}



		// display current state of henchmen selection

		int numHenchmenPresent = 0;

//		foreach (Player.ActorSlot aSlot in goal.plan.m_actorSlots) {
		for (int i = 0; i < m_goal.m_numActorSlots; i++) {

			GameObject selectHenchmenCellGO = (GameObject)Instantiate (m_selectHenchmenCellGO, m_contentParent);
			UICell selectHenchmenCell = (UICell)selectHenchmenCellGO.GetComponent<UICell> ();
			m_cells.Add (selectHenchmenCell);

			if (i < m_goal.plan.m_actorSlots.Count) {
//			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				Player.ActorSlot aSlot = m_goal.plan.m_actorSlots [i];

				selectHenchmenCell.m_headerText.text = "Current Henchmen: " + aSlot.m_actor.m_actorName;
				numHenchmenPresent++;
			}

			Button b3 = selectHenchmenCell.m_buttons [0];

			if (m_goal.m_state == OmegaPlan.OPGoal.State.Incomplete) {
				
				b3.onClick.AddListener (delegate {
					SelectHenchmenButtonPressed ();
				});
			} else {

				b3.gameObject.SetActive (false);
			}
		}

		// start mission button

		if (m_goal.m_state == OmegaPlan.OPGoal.State.Incomplete) {
			
			GameObject startMissionCellGO = (GameObject)Instantiate (m_startMissionCellGO, m_contentParent);
			UICell startMissionCell = (UICell)startMissionCellGO.GetComponent<UICell> ();
			m_cells.Add (startMissionCell);

			ColorBlock cb = startMissionCell.m_buttons [0].colors;
			cb.normalColor = Color.green;
			cb.disabledColor = Color.gray;
			startMissionCell.m_buttons [0].colors = cb;

			Player.CommandPool cp = GameController.instance.GetCommandPool (0);

			if (m_goal.plan.m_successChance > 0 && m_goal.plan.m_currentMission.m_cost <= cp.m_currentPool && numHenchmenPresent > 0 &&
				((m_goal.plan.m_currentMission.m_targetType == Mission.TargetType.Actor && m_goal.plan.m_targetActor != null) ||
					(m_goal.plan.m_currentMission.m_targetType == Mission.TargetType.Asset && m_goal.plan.m_currentAsset != null) ||
					(m_goal.plan.m_currentMission.m_targetType == Mission.TargetType.Site && m_goal.plan.m_missionSite != null) ||
					(m_goal.plan.m_currentMission.m_targetType == Mission.TargetType.Lair) || 
					(m_goal.plan.m_currentMission.m_targetType == Mission.TargetType.SiteTrait && m_goal.plan.m_targetSiteTrait != null) ||
					(m_goal.plan.m_currentMission.m_targetType == Mission.TargetType.Region && m_goal.plan.m_targetRegion != null))) {
				
//			if (m_goal.plan.m_successChance > 0 && m_goal.plan.m_currentMission.m_cost <= cp.m_currentPool &&
//			    ((m_goal.plan.m_currentMission.m_targetType == Mission.TargetType.Actor && m_goal.plan.m_targetActor != null) ||
//			    (m_goal.plan.m_currentMission.m_targetType == Mission.TargetType.Asset && m_goal.plan.m_currentAsset != null) ||
//			    (m_goal.plan.m_currentMission.m_targetType == Mission.TargetType.Site && m_goal.plan.m_missionSite != null) ||
//			    (m_goal.plan.m_currentMission.m_targetType == Mission.TargetType.Lair))) {

				startMissionCell.m_buttons [0].interactable = true;

				startMissionCell.m_buttons [0].onClick.AddListener (delegate {
					StartMissionButtonPressed ();
				});

			} else {

				startMissionCell.m_buttons [0].interactable = false;
			}

		} else if (m_goal.m_state == OmegaPlan.OPGoal.State.InProgress) {

			// create cancel mission button

			GameObject cancelMissionCellGO = (GameObject)Instantiate (m_cancelMissionCellGO, m_contentParent);
			UICell cancelMissionCell = (UICell)cancelMissionCellGO.GetComponent<UICell> ();
			m_cells.Add (cancelMissionCell);

			ColorBlock cb = cancelMissionCell.m_buttons [0].colors;
			cb.normalColor = Color.red;
			cb.disabledColor = Color.gray;
			cancelMissionCell.m_buttons [0].colors = cb;
		}
	}

	public void StartMissionButtonPressed ()
	{
		Debug.Log ("Starting new mission: " + m_goal.plan.m_currentMission.m_name);

		Action_SpendCommandPoints payForMission = new Action_SpendCommandPoints ();
		payForMission.m_playerID = 0;
		payForMission.m_amount = m_goal.plan.m_currentMission.m_cost;
		GameController.instance.ProcessAction (payForMission);

		Action_StartNewMission newMission = new Action_StartNewMission ();
		newMission.m_missionPlan = m_goal.plan;
		newMission.m_playerID = 0;
		GameController.instance.ProcessAction (newMission);

		((OmegaPlansApp)m_parentApp).homeMenu.isDirty = true;
		ParentApp.PopMenu ();
	}

	public void SelectMissionButtonPressed ()
	{
//		((LairApp)m_parentApp).selectMissionMenu.floorSlot = m_floorSlot;
//		ParentApp.PushMenu (((LairApp)m_parentApp).selectMissionMenu);
	}

	public void SelectSiteButtonPressed ()
	{
//		((LairApp)m_parentApp).selectSiteMenu.floorSlot = m_floorSlot;
//		ParentApp.PushMenu (((LairApp)m_parentApp).selectSiteMenu);
	}

	public void SelectTargetActorButtonPressed ()
	{
//		((LairApp)m_parentApp).selectTargetActorMenu.floorSlot = m_floorSlot;
//		ParentApp.PushMenu (((LairApp)m_parentApp).selectTargetActorMenu);
	}

	public void SelectHenchmenButtonPressed ()
	{
//		((OmegaPlansApp)m_parentApp).selectHenchmenMenu.currentSlot = slot;
		((OmegaPlansApp)m_parentApp).selectHenchmenMenu.goal = m_goal;
		ParentApp.PushMenu (((OmegaPlansApp)m_parentApp).selectHenchmenMenu);
	}

	public OmegaPlan.OPGoal goal {get{ return m_goal; } set{ m_goal = value; }}
}
