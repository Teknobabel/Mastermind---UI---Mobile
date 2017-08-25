using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missions_PlanMissionMenu : MonoBehaviour, IMenu {

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

//	private Lair.FloorSlot m_floorSlot;

	private bool m_isDirty = false;

	private MissionPlan m_missionPlan = new MissionPlan ();

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
		this.gameObject.SetActive (false);
	}

	public void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		if (m_missionPlan == null) {
			m_missionPlan = new MissionPlan ();
		}
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

			if (m_missionPlan.m_floorSlot != null) {
				m_missionPlan.m_actorSlots = m_missionPlan.m_floorSlot.m_actorSlots;
			}

			GameController.instance.CompileMission (m_missionPlan);

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
		missionOverviewCell.m_bodyText.text = "Success Chance: " + m_missionPlan.m_successChance.ToString () + "%";

		// if mission has been compiled, display traits

		foreach (Trait t in m_missionPlan.m_requiredTraits) {

			GameObject traitCellGO = (GameObject)Instantiate (m_traitCellGO, m_contentParent);
			UICell traitCell = (UICell)traitCellGO.GetComponent<UICell> ();
			m_cells.Add (traitCell);
			traitCell.m_headerText.text = t.m_name;

			if (m_missionPlan.m_matchingTraits.Contains (t)) {

				traitCell.m_headerText.color = Color.green;
			}
		}

		// if mission has been compiled, display assets

		if (m_missionPlan.m_requiredAssets.Count > 0) {

			Player player = GameEngine.instance.game.playerList [0];
			List<Asset> assets = new List<Asset> ();

			foreach (Site.AssetSlot aSlot in player.assets) {

				assets.Add (aSlot.m_asset);
			}

			foreach (Asset a in m_missionPlan.m_requiredAssets) {

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

		if (m_missionPlan.m_currentMission != null) {

			selectMissionCell.m_headerText.text = "Current Mission: " + m_missionPlan.m_currentMission.m_name;
		}

		Button b = selectMissionCell.m_buttons [0];

		if (m_missionPlan.m_state == MissionPlan.State.Planning) {

			b.onClick.AddListener (delegate {
				SelectMissionButtonPressed ();
			});

		} else {

			b.gameObject.SetActive (false);
		}




		// display current state of site selection

		if (m_missionPlan.m_currentMission != null && m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Actor) {

			GameObject selectHenchmenCellGO = (GameObject)Instantiate (m_selectHenchmenCellGO, m_contentParent);
			UICell selectHenchmenCell = (UICell)selectHenchmenCellGO.GetComponent<UICell> ();
			m_cells.Add (selectHenchmenCell);

			if (m_missionPlan.m_targetActor != null) {

				selectHenchmenCell.m_headerText.text = "Target Henchmen: " + m_missionPlan.m_targetActor.m_actor.m_actorName;
			} else {
				selectHenchmenCell.m_headerText.text = "Select Target Henchmen: ";
			}

			Button b2 = selectHenchmenCell.m_buttons [0];

			if (m_missionPlan.m_state == MissionPlan.State.Planning) {

				b2.onClick.AddListener (delegate {
					SelectTargetActorButtonPressed ();
				});
			} else {

				b2.gameObject.SetActive (false);
			}

		} else if (m_missionPlan.m_currentMission != null && m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Region) {

			GameObject selectSiteCellGO = (GameObject)Instantiate (m_selectSiteCellGO, m_contentParent);
			UICell selectSiteCell = (UICell)selectSiteCellGO.GetComponent<UICell> ();
			m_cells.Add (selectSiteCell);

			string s = "Current Region: ";

			if (m_missionPlan.m_targetRegion != null) {

				s += m_missionPlan.m_targetRegion.m_regionName;
			} else {
				s += "None";
			}

			selectSiteCell.m_headerText.text = s;

			Button b2 = selectSiteCell.m_buttons [0];

			if (m_missionPlan.m_state == MissionPlan.State.Planning) {

				Text t = b2.GetComponentInChildren<Text> ();
				t.text = "Select Region";

				b2.onClick.AddListener (delegate {
					SelectSiteButtonPressed ();
				});

			} else {

				b2.gameObject.SetActive (false);
			}

		}
		else if (m_missionPlan.m_currentMission != null && m_missionPlan.m_currentMission.m_targetType != Mission.TargetType.Lair) {

			GameObject selectSiteCellGO = (GameObject)Instantiate (m_selectSiteCellGO, m_contentParent);
			UICell selectSiteCell = (UICell)selectSiteCellGO.GetComponent<UICell> ();
			m_cells.Add (selectSiteCell);

			if (m_missionPlan.m_missionSite != null) {

				string s = "Current Site: " + m_missionPlan.m_missionSite.m_siteName;

				if (m_missionPlan.m_currentMission != null && m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Asset) {

					s += ", Asset: " + m_missionPlan.m_currentAsset.m_asset.m_name;
				} else if (m_missionPlan.m_currentMission != null && m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.SiteTrait) {

					s += ", Trait: " + m_missionPlan.m_targetSiteTrait.m_name;
				}

				selectSiteCell.m_headerText.text = s;
			}

			Button b2 = selectSiteCell.m_buttons [0];

			if (m_missionPlan.m_state == MissionPlan.State.Planning) {

				b2.onClick.AddListener (delegate {
					SelectSiteButtonPressed ();
				});

			} else {

				b2.gameObject.SetActive (false);
			}

		}



		// display current state of henchmen selection

		int numHenchmenPresent = 0;

		if (m_missionPlan.m_floorSlot != null)
		{
			foreach (Player.ActorSlot aSlot in m_missionPlan.m_floorSlot.m_actorSlots) {

				GameObject selectHenchmenCellGO = (GameObject)Instantiate (m_selectHenchmenCellGO, m_contentParent);
				UICell selectHenchmenCell = (UICell)selectHenchmenCellGO.GetComponent<UICell> ();
				m_cells.Add (selectHenchmenCell);

				if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

					selectHenchmenCell.m_headerText.text = "Current Henchmen: " + aSlot.m_actor.m_actorName;
					numHenchmenPresent++;
				}

				Button b3 = selectHenchmenCell.m_buttons [0];

				if (m_missionPlan.m_state == MissionPlan.State.Planning) {
					b3.onClick.AddListener (delegate {
						SelectHenchmenButtonPressed (aSlot);
					});
				} else {

					b3.gameObject.SetActive (false);
				}
			}
		}

		// start mission button

		if (m_missionPlan.m_state == MissionPlan.State.Planning) {

			GameObject startMissionCellGO = (GameObject)Instantiate (m_startMissionCellGO, m_contentParent);
			UICell startMissionCell = (UICell)startMissionCellGO.GetComponent<UICell> ();
			m_cells.Add (startMissionCell);

			ColorBlock cb = startMissionCell.m_buttons [0].colors;
			cb.normalColor = Color.green;
			cb.disabledColor = Color.gray;
			startMissionCell.m_buttons [0].colors = cb;

			Player.CommandPool cp = GameController.instance.GetCommandPool (0);

			if (m_missionPlan.m_successChance > 0 && m_missionPlan.m_currentMission.m_cost <= cp.m_currentPool && numHenchmenPresent > 0 &&
				((m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Actor && m_missionPlan.m_targetActor != null) ||
					(m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Asset && m_missionPlan.m_currentAsset != null) ||
					(m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Site && m_missionPlan.m_missionSite != null) ||
					(m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Lair) || 
					(m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.SiteTrait && m_missionPlan.m_targetSiteTrait != null) ||
					(m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Region && m_missionPlan.m_targetRegion != null))) {

				startMissionCell.m_buttons [0].interactable = true;

				startMissionCell.m_buttons [0].onClick.AddListener (delegate {
					StartMissionButtonPressed ();
				});

			} else {

				startMissionCell.m_buttons [0].interactable = false;
			}
		} else if (m_missionPlan.m_state == MissionPlan.State.Active) {

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
				CancelMissionButtonPressed (m_missionPlan);
			});
		}
	}

	public void StartMissionButtonPressed ()
	{
		Debug.Log ("Starting new mission: " + m_missionPlan.m_currentMission.m_name);

		Action_SpendCommandPoints payForMission = new Action_SpendCommandPoints ();
		payForMission.m_playerID = 0;
		payForMission.m_amount = m_missionPlan.m_currentMission.m_cost;
		GameController.instance.ProcessAction (payForMission);

		Action_StartNewMission newMission = new Action_StartNewMission ();
		newMission.m_missionPlan = m_missionPlan;
		newMission.m_playerID = 0;
		GameController.instance.ProcessAction (newMission);

		m_missionPlan = null;

		((MissionsApp)ParentApp).homeMenu.isDirty = true;
		ParentApp.PopMenu ();
	}

	public void CancelMissionButtonPressed (MissionPlan plan)
	{
		Action_CancelMission cancelMission = new Action_CancelMission ();
		cancelMission.m_missionPlan = plan;
		cancelMission.m_playerID = 0;
		GameController.instance.ProcessAction (cancelMission);

		((MissionsApp)ParentApp).homeMenu.isDirty = true;
		m_parentApp.PopMenu ();
	}

	public void SelectMissionButtonPressed ()
	{
//		((LairApp)m_parentApp).selectMissionMenu.floorSlot = m_floorSlot;
		ParentApp.PushMenu (((MissionsApp)m_parentApp).selectMissionMenu);
	}

	public void SelectSiteButtonPressed ()
	{
//		((MissionsApp)m_parentApp).selectSiteMenu.floorSlot = m_missionPlan.m_floorSlot;
		ParentApp.PushMenu (((MissionsApp)m_parentApp).selectSiteMenu);
	}

	public void SelectTargetActorButtonPressed ()
	{
//		((MissionsApp)m_parentApp).selectTargetActorMenu.floorSlot = m_missionPlan.m_floorSlot;
		ParentApp.PushMenu (((MissionsApp)m_parentApp).selectTargetActorMenu);
	}

	public void SelectHenchmenButtonPressed (Player.ActorSlot slot)
	{
		((MissionsApp)m_parentApp).selectHenchmenMenu.currentSlot = slot;
		((MissionsApp)m_parentApp).selectHenchmenMenu.floorSlot = m_missionPlan.m_floorSlot;
		ParentApp.PushMenu (((MissionsApp)m_parentApp).selectHenchmenMenu);
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}

//	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
	public MissionPlan missionPlan {get{return m_missionPlan;} set{m_missionPlan = value;}}
	public bool isDirty {set{m_isDirty = value;}}
}
