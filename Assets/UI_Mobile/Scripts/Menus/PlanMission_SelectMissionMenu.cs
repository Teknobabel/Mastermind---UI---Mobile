using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanMission_SelectMissionMenu : BaseMenu {

	public GameObject
	m_missionCell,
	m_missionStatsCell,
	m_traitCell,
	m_assetCell,
	m_separatorCell,
	m_headerCell,
	m_floorCell;

	public Transform
	m_contentParent;

//	private Lair.FloorSlot m_floorSlot;
	private MissionPlan m_missionPlan;

	private BaseMenu m_parentMenu;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);
		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		base.OnEnter (animate);
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		this.gameObject.SetActive (false);
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		Player player = GameController.instance.game.playerList [0];

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

			if (m_missionPlan.m_missionOptions.Count > 0) {
				
				GameObject headerCellGO = (GameObject)Instantiate (m_headerCell, m_contentParent);
				Cell_Header headerCell = (Cell_Header)headerCellGO.GetComponent<Cell_Header> ();
				m_cells.Add ((UICell)headerCell);
				headerCell.SetHeader (fSlot.m_floor.m_name);

			}

			foreach (Mission m in fSlot.m_floor.m_missions) {

				Cell_Mission.MissionState state = Cell_Mission.MissionState.None;
				bool validMission = m.IsValid (m_missionPlan);

				if (validMission) {

					state = Cell_Mission.MissionState.Enabled;

				} else if (!validMission && m_missionPlan.m_displayAdvancedFloors && m.m_minFloorLevel > fSlot.m_floor.level)
				{
					state = Cell_Mission.MissionState.Disabled;
				}

				if (state != Cell_Mission.MissionState.None) {

					GameObject missionCellGO = (GameObject)Instantiate (m_missionCell, m_contentParent);
					Cell_Mission missionCell = (Cell_Mission)missionCellGO.GetComponent<Cell_Mission> ();
					missionCell.SetMission (m, state);
					m_cells.Add ((UICell)missionCell);

					foreach (Trait t in m.m_requiredTraits) {

						GameObject traitCellGO = (GameObject)Instantiate (m_traitCell, m_contentParent);
						Cell_Trait traitCell = (Cell_Trait)traitCellGO.GetComponent<Cell_Trait> ();
						m_cells.Add ((UICell)traitCell);

						if (state == Cell_Mission.MissionState.Disabled) {
							traitCell.SetTrait (t, Cell_Trait.TraitState.Disabled);
						}
						else if (traits.Contains (t)) {
							traitCell.SetTrait (t, Cell_Trait.TraitState.Positive);
						} else {
							traitCell.SetTrait (t);
						}
					}

					foreach (Asset a in m.m_requiredAssets) {

						GameObject assetCellGO = (GameObject)Instantiate (m_assetCell, m_contentParent);
						Cell_Asset assetCell = (Cell_Asset)assetCellGO.GetComponent<Cell_Asset> ();
						m_cells.Add ((UICell)assetCell);

						if (state == Cell_Mission.MissionState.Disabled) {
							assetCell.SetAsset (a, Cell_Asset.AssetState.Disabled);
						}
						else if (assets.Contains (a)) {

							assetCell.SetAsset (a, Cell_Asset.AssetState.Positive);
							assets.Remove (a);

						} else {
							assetCell.SetAsset (a);
						}
					}

					foreach (Floor floor in m.m_requiredFloors) {

						GameObject floorCellGO = (GameObject)Instantiate (m_floorCell, m_contentParent);
						Cell_Floor_Minimal floorCell = (Cell_Floor_Minimal)floorCellGO.GetComponent<Cell_Floor_Minimal> ();
						m_cells.Add ((UICell)floorCell);

						if (state == Cell_Mission.MissionState.Disabled) {
							floorCell.SetFloor (floor, Cell_Floor_Minimal.FloorState.Disabled);
						}
						else if (floors.Contains (floor.m_name)) {
							floorCell.SetFloor (floor, Cell_Floor_Minimal.FloorState.Positive);
						} else {
							floorCell.SetFloor (floor);
						}
					}

					if (state == Cell_Mission.MissionState.Enabled) {

						Button b = missionCell.m_buttons [0];
						b.onClick.AddListener (delegate {
							MissionSelected (m);
						});

						b = missionCell.m_buttons [1];
						b.onClick.AddListener (delegate {
							MissionSelected (m);
						});

					} else if (state == Cell_Mission.MissionState.Disabled) {
						
						Button b = missionCell.m_buttons [0];
						b.onClick.AddListener (delegate {
							InvalidMissionSelected ();
						});

						b = missionCell.m_buttons [1];
						b.onClick.AddListener (delegate {
							InvalidMissionSelected ();
						});
					}
						

					GameObject separatorCellGO = (GameObject)Instantiate (m_separatorCell, m_contentParent);
					UICell separatorCell = (UICell)separatorCellGO.GetComponent<UICell> ();
					m_cells.Add (separatorCell);
				}
			}
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

		m_parentMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

//	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
	public MissionPlan missionPlan {set{ m_missionPlan = value;}}
	public BaseMenu parentMenu {set{ m_parentMenu = value; }}
}
