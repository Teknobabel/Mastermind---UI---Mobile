using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanMission_SelectMissionMenu : BaseMenu {

	public GameObject
	m_missionCell,
	m_missionStatsCell,
	m_traitCell,
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

		foreach (Lair.FloorSlot fSlot in m_missionPlan.m_missionOptions) {

			if (m_missionPlan.m_missionOptions.Count > 0) {
				
				GameObject headerCellGO = (GameObject)Instantiate (m_headerCell, m_contentParent);
				Cell_Header headerCell = (Cell_Header)headerCellGO.GetComponent<Cell_Header> ();
				m_cells.Add ((UICell)headerCell);
				headerCell.SetHeader (fSlot.m_floor.m_name);

			}

			foreach (Mission m in fSlot.m_floor.m_missions) {
				
				if (m.IsValid (m_missionPlan)) {

					GameObject missionCellGO = (GameObject)Instantiate (m_missionCell, m_contentParent);
					Cell_Mission missionCell = (Cell_Mission)missionCellGO.GetComponent<Cell_Mission> ();
					missionCell.SetMission (m);
					m_cells.Add ((UICell)missionCell);

					foreach (Trait t in m.m_requiredTraits) {

						GameObject traitCellGO = (GameObject)Instantiate (m_traitCell, m_contentParent);
						UICell traitCell = (UICell)traitCellGO.GetComponent<UICell> ();
						m_cells.Add (traitCell);
						traitCell.m_headerText.text = "Trait: " + t.m_name;
					}

					foreach (Asset a in m.m_requiredAssets) {

						GameObject assetCellGO = (GameObject)Instantiate (m_traitCell, m_contentParent);
						UICell assetCell = (UICell)assetCellGO.GetComponent<UICell> ();
						m_cells.Add (assetCell);
						assetCell.m_headerText.text = "Asset: " + a.m_name;
					}

					foreach (Floor floor in m.m_requiredFloors) {

						GameObject floorCellGO = (GameObject)Instantiate (m_floorCell, m_contentParent);
						Cell_Floor_Minimal floorCell = (Cell_Floor_Minimal)floorCellGO.GetComponent<Cell_Floor_Minimal> ();
						m_cells.Add ((UICell)floorCell);

						floorCell.SetFloor (floor);
					}

					Button b = missionCell.m_buttons [0];
					b.onClick.AddListener (delegate {
						MissionSelected (m);
					});
					b = missionCell.m_buttons [1];
					b.onClick.AddListener (delegate {
						MissionSelected (m);
					});

					GameObject separatorCellGO = (GameObject)Instantiate (m_separatorCell, m_contentParent);
					UICell separatorCell = (UICell)separatorCellGO.GetComponent<UICell> ();
					m_cells.Add (separatorCell);
				}
			}
		}

		LayoutRebuilder.ForceRebuildLayoutImmediate (m_contentParent.GetComponent<RectTransform>());
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
