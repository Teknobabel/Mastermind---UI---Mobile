using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lair_PlanMissionMenu : MonoBehaviour, IMenu {

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

	private IApp m_parentApp;

	private List<UICell> m_cells = new List<UICell>();

	private Lair.FloorSlot m_floorSlot;

	private bool m_isDirty = false;

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
//		m_appNameText.text = parentApp.Name;
		//		m_infoPanelToggle.AddObserver (this);
		//		m_infoPanelToggle.ToggleButtonClicked (0);
		this.gameObject.SetActive (false);
	}

	public void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		// recompile to account for any changes since last visit

		GameController.instance.CompileMission (m_floorSlot.m_missionPlan);

		DisplayMissionPlan ();

	}

	public void OnExit (bool animate)
	{
		m_isDirty = false;

		this.gameObject.SetActive (false);
	}

	public void OnHold ()
	{

	}

	public void OnReturn ()
	{
		if (m_isDirty) {

			m_isDirty = false;

//			foreach (Player.ActorSlot aSlot in m_floorSlot.m_missionPlan.m_actorSlots) {
//
//				if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {
//
//					aSlot.RemoveHenchmen ();
//				}
//			}

			m_floorSlot.m_missionPlan.m_actorSlots = m_floorSlot.m_actorSlots;

			GameController.instance.CompileMission (m_floorSlot.m_missionPlan);

			DisplayMissionPlan ();
		}
	}

	private void DisplayMissionPlan ()
	{
//		Debug.Log (m_floorSlot.m_missionPlan.m_state);
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		// check if the mission needs to be recompiled
		// if the state of any mission element changed since last visit to this menu

//		bool recompile = false;

		// check if any assets in the plan are now in use

//		foreach (Site.AssetSlot aSlot in m_floorSlot.m_missionPlan.m_linkedPlayerAssets) {
//
//			if (aSlot.m_state == Site.AssetSlot.State.InUse) {
//
//				recompile = true;
//				break;
//			}
//		}

		// check if any henchmen in the plan are now in use

//		List<Player.ActorSlot> henchmen = new List<Player.ActorSlot> ();
//		foreach (Player.ActorSlot aSlot in m_floorSlot.m_missionPlan.m_actorSlots) {
//			henchmen.Add (aSlot);
//		}
//
//		while (henchmen.Count > 0) {
//
//			Player.ActorSlot aSlot = henchmen [0];
//			henchmen.RemoveAt (0);
//
//			if (aSlot.m_state == Player.ActorSlot.ActorSlotState.OnMission) {
//
//				recompile = true;
//				aSlot.RemoveHenchmen ();
//			}
//		}

//		if (recompile) {

//			GameController.instance.CompileMission (m_floorSlot.m_missionPlan);
//		}

		// display mission overview cell

		GameObject missionOverviewCellGO = (GameObject)Instantiate (m_missionOverviewCellGO, m_contentParent);
		UICell missionOverviewCell = (UICell)missionOverviewCellGO.GetComponent<UICell> ();
		m_cells.Add (missionOverviewCell);
		missionOverviewCell.m_bodyText.text = "Success Chance: " + m_floorSlot.m_missionPlan.m_successChance.ToString () + "%";

		// if mission has been compiled, display traits

		foreach (Trait t in m_floorSlot.m_missionPlan.m_requiredTraits) {

			GameObject traitCellGO = (GameObject)Instantiate (m_traitCellGO, m_contentParent);
			UICell traitCell = (UICell)traitCellGO.GetComponent<UICell> ();
			m_cells.Add (traitCell);
			traitCell.m_headerText.text = t.m_name;

			if (m_floorSlot.m_missionPlan.m_matchingTraits.Contains (t)) {

				traitCell.m_headerText.color = Color.green;
			}
		}

		// if mission has been compiled, display assets

		if (m_floorSlot.m_missionPlan.m_requiredAssets.Count > 0) {

			Player player = GameEngine.instance.game.playerList [0];
			List<Asset> assets = new List<Asset> ();

			foreach (Site.AssetSlot aSlot in m_floorSlot.m_missionPlan.m_linkedPlayerAssets) {

				assets.Add (aSlot.m_asset);
			}

			foreach (Asset a in m_floorSlot.m_missionPlan.m_requiredAssets) {

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

		if (m_floorSlot.m_missionPlan.m_currentMission != null) {

			selectMissionCell.m_headerText.text = "Current Mission: " + m_floorSlot.m_missionPlan.m_currentMission.m_name;
		}

		Button b = selectMissionCell.m_buttons [0];

		if (m_floorSlot.m_missionPlan.m_state == MissionPlan.State.Planning) {

			b.onClick.AddListener (delegate {
				SelectMissionButtonPressed ();
			});

		} else {

			b.gameObject.SetActive (false);
		}




		// display current state of site selection

		if (m_floorSlot.m_missionPlan.m_currentMission != null && m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Actor) {

			GameObject selectHenchmenCellGO = (GameObject)Instantiate (m_selectHenchmenCellGO, m_contentParent);
			UICell selectHenchmenCell = (UICell)selectHenchmenCellGO.GetComponent<UICell> ();
			m_cells.Add (selectHenchmenCell);

			if (m_floorSlot.m_missionPlan.m_targetActor != null) {

				selectHenchmenCell.m_headerText.text = "Target Henchmen: " + m_floorSlot.m_missionPlan.m_targetActor.m_actor.m_actorName;
			} else {
				selectHenchmenCell.m_headerText.text = "Select Target Henchmen: ";
			}

			Button b2 = selectHenchmenCell.m_buttons [0];

			if (m_floorSlot.m_missionPlan.m_state == MissionPlan.State.Planning) {
				
				b2.onClick.AddListener (delegate {
					SelectTargetActorButtonPressed ();
				});
			} else {

				b2.gameObject.SetActive (false);
			}

		} else if (m_floorSlot.m_missionPlan.m_currentMission != null && m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Region) {

			GameObject selectSiteCellGO = (GameObject)Instantiate (m_selectSiteCellGO, m_contentParent);
			UICell selectSiteCell = (UICell)selectSiteCellGO.GetComponent<UICell> ();
			m_cells.Add (selectSiteCell);

			string s = "Current Region: ";

			if (m_floorSlot.m_missionPlan.m_targetRegion != null) {

				s += m_floorSlot.m_missionPlan.m_targetRegion.m_regionName;
			} else {
				s += "None";
			}

			selectSiteCell.m_headerText.text = s;

			Button b2 = selectSiteCell.m_buttons [0];

			if (m_floorSlot.m_missionPlan.m_state == MissionPlan.State.Planning) {

				Text t = b2.GetComponentInChildren<Text> ();
				t.text = "Select Region";

				b2.onClick.AddListener (delegate {
					SelectSiteButtonPressed ();
				});

			} else {

				b2.gameObject.SetActive (false);
			}

		}
		else if (m_floorSlot.m_missionPlan.m_currentMission != null && m_floorSlot.m_missionPlan.m_currentMission.m_targetType != Mission.TargetType.Lair) {

			GameObject selectSiteCellGO = (GameObject)Instantiate (m_selectSiteCellGO, m_contentParent);
			UICell selectSiteCell = (UICell)selectSiteCellGO.GetComponent<UICell> ();
			m_cells.Add (selectSiteCell);

			if (m_floorSlot.m_missionPlan.m_missionSite != null) {

				string s = "Current Site: " + m_floorSlot.m_missionPlan.m_missionSite.m_siteName;

				if (m_floorSlot.m_missionPlan.m_currentMission != null && m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Asset) {

					s += ", Asset: " + m_floorSlot.m_missionPlan.m_currentAsset.m_asset.m_name;
				} else if (m_floorSlot.m_missionPlan.m_currentMission != null && m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.SiteTrait) {

					s += ", Trait: " + m_floorSlot.m_missionPlan.m_targetSiteTrait.m_name;
				}

				selectSiteCell.m_headerText.text = s;
			}

			Button b2 = selectSiteCell.m_buttons [0];

			if (m_floorSlot.m_missionPlan.m_state == MissionPlan.State.Planning) {
				
				b2.onClick.AddListener (delegate {
					SelectSiteButtonPressed ();
				});

			} else {

				b2.gameObject.SetActive (false);
			}

		}



		// display current state of henchmen selection

		int numHenchmenPresent = 0;

		for (int i = 0; i < m_floorSlot.m_numActorSlots; i++) {

			GameObject selectHenchmenCellGO = (GameObject)Instantiate (m_selectHenchmenCellGO, m_contentParent);
			UICell selectHenchmenCell = (UICell)selectHenchmenCellGO.GetComponent<UICell> ();
			m_cells.Add (selectHenchmenCell);

			if (i < m_floorSlot.m_actorSlots.Count) {

				Player.ActorSlot aSlot = m_floorSlot.m_actorSlots [i];

				selectHenchmenCell.m_headerText.text = "Current Henchmen: " + aSlot.m_actor.m_actorName;
				numHenchmenPresent++;
			}

			Button b3 = selectHenchmenCell.m_buttons [0];

			if (m_floorSlot.m_missionPlan.m_state == MissionPlan.State.Planning) {
				b3.onClick.AddListener (delegate {
					SelectHenchmenButtonPressed ();
				});
			} else {

				b3.gameObject.SetActive (false);
			}
		}

//		foreach (Player.ActorSlot aSlot in m_floorSlot.m_actorSlots) {
//
//			GameObject selectHenchmenCellGO = (GameObject)Instantiate (m_selectHenchmenCellGO, m_contentParent);
//			UICell selectHenchmenCell = (UICell)selectHenchmenCellGO.GetComponent<UICell> ();
//			m_cells.Add (selectHenchmenCell);
//
//			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {
//
//				selectHenchmenCell.m_headerText.text = "Current Henchmen: " + aSlot.m_actor.m_actorName;
//				numHenchmenPresent++;
//			}
//
//			Button b3 = selectHenchmenCell.m_buttons [0];
//
//			if (m_floorSlot.m_missionPlan.m_state == MissionPlan.State.Planning) {
//				b3.onClick.AddListener (delegate {
//					SelectHenchmenButtonPressed (aSlot);
//				});
//			} else {
//
//				b3.gameObject.SetActive (false);
//			}
//		}

		// start mission button

		if (m_floorSlot.m_missionPlan.m_state == MissionPlan.State.Planning) {

			GameObject startMissionCellGO = (GameObject)Instantiate (m_startMissionCellGO, m_contentParent);
			UICell startMissionCell = (UICell)startMissionCellGO.GetComponent<UICell> ();
			m_cells.Add (startMissionCell);

			ColorBlock cb = startMissionCell.m_buttons [0].colors;
			cb.normalColor = Color.green;
			cb.disabledColor = Color.gray;
			startMissionCell.m_buttons [0].colors = cb;

			Player.CommandPool cp = GameController.instance.GetCommandPool (0);

			if (m_floorSlot.m_missionPlan.m_successChance > 0 && m_floorSlot.m_missionPlan.m_currentMission.m_cost <= cp.m_currentPool && numHenchmenPresent > 0 &&
			    ((m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Actor && m_floorSlot.m_missionPlan.m_targetActor != null) ||
			    (m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Asset && m_floorSlot.m_missionPlan.m_currentAsset != null) ||
			    (m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Site && m_floorSlot.m_missionPlan.m_missionSite != null) ||
			    (m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Lair) || 
				(m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.SiteTrait && m_floorSlot.m_missionPlan.m_targetSiteTrait != null) ||
				(m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Region && m_floorSlot.m_missionPlan.m_targetRegion != null))) {

				startMissionCell.m_buttons [0].interactable = true;

				startMissionCell.m_buttons [0].onClick.AddListener (delegate {
					StartMissionButtonPressed ();
				});

			} else {

				startMissionCell.m_buttons [0].interactable = false;
			}
		} else if (m_floorSlot.m_missionPlan.m_state == MissionPlan.State.Active) {

			// spawn cancel mission cell

			GameObject cancelMissionCellGO = (GameObject)Instantiate (m_cancelMissionCellGO, m_contentParent);
			UICell cancelMissionCell = (UICell)cancelMissionCellGO.GetComponent<UICell> ();
			m_cells.Add (cancelMissionCell);

			ColorBlock cb = cancelMissionCell.m_buttons [0].colors;
			cb.normalColor = Color.red;
			cb.disabledColor = Color.gray;
			cancelMissionCell.m_buttons [0].colors = cb;

			Button b3 = cancelMissionCell.m_buttons [0];
			b3.onClick.AddListener (delegate {
				CancelMissionButtonPressed (m_floorSlot.m_missionPlan);
			});
		}
	}

	public void StartMissionButtonPressed ()
	{
		Debug.Log ("Starting new mission: " + m_floorSlot.m_missionPlan.m_currentMission.m_name);

		Action_SpendCommandPoints payForMission = new Action_SpendCommandPoints ();
		payForMission.m_playerID = 0;
		payForMission.m_amount = m_floorSlot.m_missionPlan.m_currentMission.m_cost;
		GameController.instance.ProcessAction (payForMission);

		Action_StartNewMission newMission = new Action_StartNewMission ();
		newMission.m_missionPlan = m_floorSlot.m_missionPlan;
		newMission.m_playerID = 0;
		GameController.instance.ProcessAction (newMission);

		((LairApp)m_parentApp).homeMenu.isDirty = true;
		ParentApp.PopMenu ();
	}

	public void SelectMissionButtonPressed ()
	{
		((LairApp)m_parentApp).selectMissionMenu.floorSlot = m_floorSlot;
		ParentApp.PushMenu (((LairApp)m_parentApp).selectMissionMenu);
	}

	public void SelectSiteButtonPressed ()
	{
		((LairApp)m_parentApp).selectSiteMenu.floorSlot = m_floorSlot;
		ParentApp.PushMenu (((LairApp)m_parentApp).selectSiteMenu);
	}

	public void SelectTargetActorButtonPressed ()
	{
		((LairApp)m_parentApp).selectTargetActorMenu.floorSlot = m_floorSlot;
		ParentApp.PushMenu (((LairApp)m_parentApp).selectTargetActorMenu);
	}

	public void SelectHenchmenButtonPressed ()
	{
//		((LairApp)m_parentApp).selectHenchmenMenu.currentSlot = slot;
		((LairApp)m_parentApp).selectHenchmenMenu.floorSlot = m_floorSlot;
		ParentApp.PushMenu (((LairApp)m_parentApp).selectHenchmenMenu);
	}

	public void CancelMissionButtonPressed (MissionPlan plan)
	{
		Action_CancelMission cancelMission = new Action_CancelMission ();
		cancelMission.m_missionPlan = plan;
		cancelMission.m_playerID = 0;
		GameController.instance.ProcessAction (cancelMission);

		((LairApp)ParentApp).homeMenu.isDirty = true;
		m_parentApp.PopMenu ();
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}

	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
	public bool isDirty {set{m_isDirty = value;}}
}
