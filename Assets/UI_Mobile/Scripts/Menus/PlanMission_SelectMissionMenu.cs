using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanMission_SelectMissionMenu : BaseMenu, IUIObserver  {


	public GameObject
	m_missionCell,
	m_missionStatsCell,
	m_traitCell,
	m_assetCell,
	m_separatorCell,
	m_headerCell,
	m_floorCell,
	m_spacer,
	m_cellDetailPanel,
	m_cellCostPanel;

	public Transform
	m_contentParent;

	public SegmentedToggle m_infoPanelToggle;

//	private Lair.FloorSlot m_floorSlot;
	private MissionPlan m_missionPlan;

	private int m_currentLevel = 1;
	private Mission.MissionSkillType m_missionType = Mission.MissionSkillType.Hacker;

	private BaseMenu m_parentMenu;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);
		m_infoPanelToggle.AddObserver (this);
		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		base.OnEnter (animate);

		// update floor level toggle
		if (m_missionPlan.m_floorSlot != null) {

			int maxLevel = m_missionPlan.m_floorSlot.m_floor.m_missions [m_missionPlan.m_floorSlot.m_floor.m_missions.Count - 1].m_minFloorLevel;

			for (int i = 0; i < m_infoPanelToggle.m_buttons.Length; i++) {
				
				Button b = m_infoPanelToggle.m_buttons [i];

				if (i < maxLevel) {

					b.gameObject.SetActive (true);
				} else {
					b.gameObject.SetActive (false);
				}
			}
		} 
//		else {
//
//			m_infoPanelToggle.gameObject.SetActive (false);
//		}
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		m_currentLevel = 1;

//		this.gameObject.SetActive (false);
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		Player player = GameController.instance.game.playerList [0];
		List<Player.ActorSlot> henchmenPool = GameController.instance.GetHiredHenchmen (0);

		// gather traits from currently assigned henchmen

		List<Trait> currentHenchmenTraits = new List<Trait> ();
		m_missionPlan.m_actorSlots.Clear ();

		foreach (Player.ActorSlot aSlot in henchmenPool) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_actor != null)
			{
				foreach (Trait t in aSlot.m_actor.traits) {

					if (!currentHenchmenTraits.Contains (t)) {

						currentHenchmenTraits.Add (t);
						m_missionPlan.m_actorSlots.Add (aSlot);
					}
				}
			}
		}

		// gather assets

		List<Asset> assets = new List<Asset> ();

		foreach (Site.AssetSlot aSlot in player.assets) {

			if (aSlot.m_state != Site.AssetSlot.State.InUse) {
				assets.Add (aSlot.m_asset);
			}
		}

		// gather floors

		List<string> floors = new List<string> ();

		foreach (Lair.FloorSlot fSlot in player.lair.floorSlots) {

			floors.Add (fSlot.m_floor.m_name);
		}

		// if henchmen are assigned, gather traits

		List<Trait> traits = new List<Trait> ();

		foreach (Player.ActorSlot aSlot in m_missionPlan.m_actorSlots) {

			foreach (Trait t in aSlot.m_actor.traits) {

				traits.Add (t);
			}
		}

		foreach (Lair.FloorSlot fSlot in m_missionPlan.m_missionOptions) {

//			if (m_missionPlan.m_missionOptions.Count > 0) {
//				
//				GameObject headerCellGO = (GameObject)Instantiate (m_headerCell, m_contentParent);
//				Cell_Header headerCell = (Cell_Header)headerCellGO.GetComponent<Cell_Header> ();
//				m_cells.Add ((UICell)headerCell);
//				headerCell.SetHeader (fSlot.m_floor.m_name);
//
//			}

			foreach (Mission m in fSlot.m_floor.m_missions) 
			{
				m_missionPlan.m_currentMission = m;
				GameController.instance.CompileMission (m_missionPlan);
				int maxLevel = 1;

				switch (m.m_skillType) {

				case Mission.MissionSkillType.Hacker:

					maxLevel = player.GetMaxHackerMissionLevel ();
					break;
				case Mission.MissionSkillType.Spy:

					maxLevel = player.GetMaxSpyMissionLevel ();
					break;
				case Mission.MissionSkillType.Scientist:

					maxLevel = player.GetMaxScientistMissionLevel ();
					break;

				}

				Cell_Mission.MissionState state = Cell_Mission.MissionState.Enabled;
				bool validMission = m.IsValid (m_missionPlan);

//				if (m.m_skillType != m_missionType) {
//
//					validMission = false;
//				}

//				if (validMission && m.m_targetType == m_missionPlan.m_selectedTargetType) {
//
//					state = Cell_Mission.MissionState.Enabled;
//
//				} 
//				else if (!validMission && m_missionPlan.m_displayAdvancedFloors && m.m_minFloorLevel > fSlot.m_floor.level)
//				{
//					state = Cell_Mission.MissionState.Disabled;
//				}

				if (m.m_targetType == m_missionPlan.m_selectedTargetType && m.m_skillType == m_missionType && m.m_minFloorLevel <= maxLevel) {

					GameObject missionCellGO = (GameObject)Instantiate (m_missionCell, m_contentParent);
					Cell_Mission missionCell = (Cell_Mission)missionCellGO.GetComponent<Cell_Mission> ();
					missionCell.SetMission (m, state);
					m_cells.Add ((UICell)missionCell);

					// set detail panel - traits

					if (m.m_requiredTraits.Length > 0) {

						GameObject traitPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
						Cell_DetailPanel dPanel = (Cell_DetailPanel)traitPanel.GetComponent<Cell_DetailPanel> ();
//						dPanel.SetTraits (m, currentHenchmenTraits);
						dPanel.SetTraits (m_missionPlan, Cell_DetailPanel.MissionState.Planning);
						m_cells.Add (dPanel);
					}

					// set detail panel - assets

					if (m.m_requiredAssets.Length > 0) {

						GameObject assetPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
						Cell_DetailPanel aPanel = (Cell_DetailPanel)assetPanel.GetComponent<Cell_DetailPanel> ();
						aPanel.SetAssets (m, Cell_DetailPanel.MissionState.Planning);
						m_cells.Add (aPanel);
					}

					// set detail panel - facilities

					if (m.m_requiredFloors.Length > 0) {

						GameObject facilityPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
						Cell_DetailPanel fPanel = (Cell_DetailPanel)facilityPanel.GetComponent<Cell_DetailPanel> ();
						fPanel.SetFacilities (m, Cell_DetailPanel.MissionState.Normal);
						m_cells.Add (fPanel);
					}

					// set cost panel

					GameObject costPanel = (GameObject)Instantiate (m_cellCostPanel, m_contentParent);
					Cell_CostPanel cPanel = (Cell_CostPanel)costPanel.GetComponent<Cell_CostPanel> ();
					cPanel.SetCostPanel (m_missionPlan);
					m_cells.Add (cPanel);


//					foreach (Trait t in m.m_requiredTraits) {
//
//						GameObject traitCellGO = (GameObject)Instantiate (m_traitCell, m_contentParent);
//						Cell_Trait traitCell = (Cell_Trait)traitCellGO.GetComponent<Cell_Trait> ();
//						m_cells.Add ((UICell)traitCell);
//
//						if (state == Cell_Mission.MissionState.Disabled) {
//							traitCell.SetTrait (t, Cell_Trait.TraitState.Disabled);
//						}
//						else if (traits.Contains (t)) {
//							traitCell.SetTrait (t, Cell_Trait.TraitState.Positive);
//						} else {
//							traitCell.SetTrait (t);
//						}
//					}

//					foreach (Asset a in m.m_requiredAssets) {
//
//						GameObject assetCellGO = (GameObject)Instantiate (m_assetCell, m_contentParent);
//						Cell_Asset assetCell = (Cell_Asset)assetCellGO.GetComponent<Cell_Asset> ();
//						m_cells.Add ((UICell)assetCell);
//
//						if (state == Cell_Mission.MissionState.Disabled) {
//							assetCell.SetAsset (a, Cell_Asset.AssetState.Disabled);
//						}
//						else if (assets.Contains (a)) {
//
//							assetCell.SetAsset (a, Cell_Asset.AssetState.Positive);
//							assets.Remove (a);
//
//						} else {
//							assetCell.SetAsset (a);
//						}
//					}

//					foreach (Floor floor in m.m_requiredFloors) {
//
//						GameObject floorCellGO = (GameObject)Instantiate (m_floorCell, m_contentParent);
//						Cell_Floor_Minimal floorCell = (Cell_Floor_Minimal)floorCellGO.GetComponent<Cell_Floor_Minimal> ();
//						m_cells.Add ((UICell)floorCell);
//
//						if (state == Cell_Mission.MissionState.Disabled) {
//							floorCell.SetFloor (floor, Cell_Floor_Minimal.FloorState.Disabled);
//						}
//						else if (floors.Contains (floor.m_name)) {
//							floorCell.SetFloor (floor, Cell_Floor_Minimal.FloorState.Positive);
//						} else {
//							floorCell.SetFloor (floor);
//						}
//					}

					if (state == Cell_Mission.MissionState.Enabled) {

						missionCell.DisplaySelectButton ();

						Button b = missionCell.m_buttons [0];
						b.onClick.AddListener (delegate {
							MissionSelected (m);
						});

//						b = missionCell.m_buttons [1];
//						b.onClick.AddListener (delegate {
//							MissionSelected (m);
//						});

					} else if (state == Cell_Mission.MissionState.Disabled) {

						// turn on locked icon
						missionCell.m_rawImages[0].gameObject.SetActive(false);
						missionCell.m_rawImages[1].gameObject.SetActive(true);

						// enable error message
						Button b = missionCell.m_buttons [0];
						b.onClick.AddListener (delegate {
							InvalidMissionSelected ();
						});

//						b = missionCell.m_buttons [1];
//						b.onClick.AddListener (delegate {
//							InvalidMissionSelected ();
//						});
					}
						
					GameObject spacerGO2 = (GameObject)Instantiate (m_spacer, m_contentParent);
					Cell_Spacer spacer2 = (Cell_Spacer)spacerGO2.GetComponent<Cell_Spacer> ();
					m_cells.Add (spacer2);
//
//					GameObject separatorCellGO = (GameObject)Instantiate (m_separatorCell, m_contentParent);
//					UICell separatorCell = (UICell)separatorCellGO.GetComponent<UICell> ();
//					m_cells.Add (separatorCell);
				}
			}

			GameObject spacerGO3 = (GameObject)Instantiate (m_spacer, m_contentParent);
			Cell_Spacer spacer3 = (Cell_Spacer)spacerGO3.GetComponent<Cell_Spacer> ();
			spacer3.SetHeight (120);
			m_cells.Add (spacer3);
		}

		LayoutRebuilder.ForceRebuildLayoutImmediate (m_contentParent.GetComponent<RectTransform>());
	}

	public void InvalidMissionSelected ()
	{
		string header = "Invalid Mission";
		string message = "You must increase the Facility Level to gain access to this Mission.";

		MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
		Button b1 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
		b1.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
		m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
	}

	public void MissionSelected (Mission m)
	{
		Debug.Log( "Mission: " + m.m_name + " selected");

		m_missionPlan.m_currentMission = m;

		// put all henchmen on mission    

		List<Player.ActorSlot> henchmenPool = GameController.instance.GetHiredHenchmen (0);

//		foreach (Player.ActorSlot aSlot in henchmenPool) {
//
//			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {
//				m_missionPlan.m_actorSlots.Add (aSlot);
//			}
//		}

		GameController.instance.CompileMission (m_missionPlan);

		Player.CommandPool cp = GameController.instance.GetCommandPool (0);

		if (m_missionPlan.m_currentMission.m_cost > cp.m_currentPool) {

			string header = "Can't Afford Mission";
			string message = "There aren't enough points in your Command Pool to start this Mission.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

			return;
		}

		if (m_missionPlan.m_successChance <= 0) {

			string header = "Can't Start Mission";
			string message = "You can't start a Mission with a 0% Success Chance.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

			return;
		}

		string header2 = "Start Mission";
		string message2 = "This mission will generate " + m_missionPlan.m_currentMission.m_cost.ToString () + " Infamy and has a " +
			m_missionPlan.m_successChance.ToString() + "% chance of success.";

		MobileUIEngine.instance.alertDialogue.SetAlert (header2, message2, m_parentApp);
		Button b3 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
		b3.onClick.AddListener(delegate { StartMission();});
		Button b4 = MobileUIEngine.instance.alertDialogue.AddButton ("Cancel");
		b4.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
		m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

//		((WorldApp)(m_parentApp)).planMissionMenu.missionPlan = m_missionPlan;
//		m_parentApp.PushMenu (((WorldApp)(m_parentApp)).planMissionMenu);
	}

	public void StartMission ()
	{
//		Action_SpendCommandPoints payForMission = new Action_SpendCommandPoints ();
//		payForMission.m_playerID = 0;
//		payForMission.m_amount = m_missionPlan.m_currentMission.m_cost;
//		GameController.instance.ProcessAction (payForMission);

		Action_StartNewMission newMission = new Action_StartNewMission ();
		newMission.m_missionPlan = m_missionPlan;
		newMission.m_playerID = 0;
		GameController.instance.ProcessAction (newMission);

		m_parentMenu.isDirty = true;

		ParentApp.PopMenu ();
		ParentApp.PopMenu ();
	}

	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent)
		{
		case UIEvent.UI_ToggleButtonPressed:


			switch (m_infoPanelToggle.activeButton) {
			case 0:
//				m_currentLevel = 1;
				m_missionType = Mission.MissionSkillType.Hacker;
				break;
			case 1:
//				m_currentLevel = 2;
				m_missionType = Mission.MissionSkillType.Spy;
				break;
			case 2:
//				m_currentLevel = 3;
				m_missionType = Mission.MissionSkillType.Scientist;
				break;

			}
			DisplayContent ();
			break;
		}
	}

//	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
	public MissionPlan missionPlan {set{ m_missionPlan = value;}}
	public BaseMenu parentMenu {set{ m_parentMenu = value; }}
}
