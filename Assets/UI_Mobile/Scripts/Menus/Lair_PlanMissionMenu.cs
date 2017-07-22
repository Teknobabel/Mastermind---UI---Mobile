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
		m_startMissionCellGO;

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
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

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

		// display current state of mission selection

		GameObject selectMissionCellGO = (GameObject)Instantiate (m_selectMissionCellGO, m_contentParent);
		UICell selectMissionCell = (UICell)selectMissionCellGO.GetComponent<UICell> ();
		m_cells.Add (selectMissionCell);

		if (m_floorSlot.m_missionPlan.m_currentMission != null) {

			selectMissionCell.m_headerText.text = "Current Mission: " + m_floorSlot.m_missionPlan.m_currentMission.m_name;
		}

		Button b = selectMissionCell.m_buttons [0];
		b.onClick.AddListener (delegate {
			SelectMissionButtonPressed ();
		});



		// display current state of site selection

		if (m_floorSlot.m_missionPlan.m_currentMission == null || (m_floorSlot.m_missionPlan.m_currentMission != null
		    && m_floorSlot.m_missionPlan.m_currentMission.m_targetType != Mission.TargetType.Lair)) {

			GameObject selectSiteCellGO = (GameObject)Instantiate (m_selectSiteCellGO, m_contentParent);
			UICell selectSiteCell = (UICell)selectSiteCellGO.GetComponent<UICell> ();
			m_cells.Add (selectSiteCell);

			if (m_floorSlot.m_missionPlan.m_missionSite != null) {

				string s = "Current Site: " + m_floorSlot.m_missionPlan.m_missionSite.m_siteName;

				if (m_floorSlot.m_missionPlan.m_currentMission != null && m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Asset) {

					s += ", Asset: " + m_floorSlot.m_missionPlan.m_currentAsset.m_asset.m_name;
				}

				selectSiteCell.m_headerText.text = s;
			}

			Button b2 = selectSiteCell.m_buttons [0];
			b2.onClick.AddListener (delegate {
				SelectSiteButtonPressed ();
			});

		}



		// display current state of henchmen selection

		foreach (Player.ActorSlot aSlot in m_floorSlot.m_actorSlots) {

			GameObject selectHenchmenCellGO = (GameObject)Instantiate (m_selectHenchmenCellGO, m_contentParent);
			UICell selectHenchmenCell = (UICell)selectHenchmenCellGO.GetComponent<UICell> ();
			m_cells.Add (selectHenchmenCell);

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				selectHenchmenCell.m_headerText.text = "Current Henchmen: " + aSlot.m_actor.m_actorName;
			}

			Button b3 = selectHenchmenCell.m_buttons [0];
			b3.onClick.AddListener (delegate {
				SelectHenchmenButtonPressed (aSlot);
			});
		}

		// start mission button

		GameObject startMissionCellGO = (GameObject)Instantiate (m_startMissionCellGO, m_contentParent);
		UICell startMissionCell = (UICell)startMissionCellGO.GetComponent<UICell> ();
		m_cells.Add (startMissionCell);

		ColorBlock cb = startMissionCell.m_buttons [0].colors;
		cb.normalColor = Color.green;
		cb.disabledColor = Color.gray;
		startMissionCell.m_buttons [0].colors = cb;

		Player.CommandPool cp = GameController.instance.GetCommandPool (0);

		if (m_floorSlot.m_missionPlan.m_successChance > 0 && m_floorSlot.m_missionPlan.m_currentMission.m_cost <= cp.m_currentPool &&
			((m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Asset && m_floorSlot.m_missionPlan.m_currentAsset != null) ||  
				(m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Site && m_floorSlot.m_missionPlan.m_missionSite != null) ||
				(m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Lair))) {

			startMissionCell.m_buttons [0].interactable = true;

			startMissionCell.m_buttons [0].onClick.AddListener (delegate {
				StartMissionButtonPressed ();
			});

		} else {

			startMissionCell.m_buttons [0].interactable = false;
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

	public void SelectHenchmenButtonPressed (Player.ActorSlot slot)
	{
		((LairApp)m_parentApp).selectHenchmenMenu.currentSlot = slot;
		((LairApp)m_parentApp).selectHenchmenMenu.floorSlot = m_floorSlot;
		ParentApp.PushMenu (((LairApp)m_parentApp).selectHenchmenMenu);
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}

	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
	public bool isDirty {set{m_isDirty = value;}}
}
