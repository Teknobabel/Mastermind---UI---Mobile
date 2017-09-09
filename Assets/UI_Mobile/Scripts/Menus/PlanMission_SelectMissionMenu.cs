using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanMission_SelectMissionMenu : BaseMenu {

	public GameObject
	m_missionCell,
	m_missionStatsCell,
	m_traitCell,
	m_separatorCell;

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
		base.OnEnter (animate);

		this.gameObject.SetActive (true);

		DisplayContent ();
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		this.gameObject.SetActive (false);
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		foreach (Mission m in m_missionPlan.m_missionOptions) {

			if (m.IsValid (m_missionPlan)) {

				GameObject missionCellGO = (GameObject)Instantiate (m_missionCell, m_contentParent);
				UICell missionCell = (UICell)missionCellGO.GetComponent<UICell> ();
				m_cells.Add (missionCell);
				missionCell.m_headerText.text = m.m_name;

				missionCell.m_bodyText.text = m.m_description;

				GameObject missionStatsCellGO = (GameObject)Instantiate (m_missionStatsCell, m_contentParent);
				UICell missionStatsCell = (UICell)missionStatsCellGO.GetComponent<UICell> ();
				m_cells.Add (missionStatsCell);
				missionStatsCell.m_headerText.text = m.m_cost.ToString () + " CP";
				missionStatsCell.m_bodyText.text = m.m_duration.ToString () + " Turns";

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

				Button b = missionCell.m_buttons [0];
				b.onClick.AddListener (delegate {
					MissionSelected (m);
				});

				GameObject separatorCellGO = (GameObject)Instantiate (m_separatorCell, m_contentParent);
				UICell separatorCell = (UICell)separatorCellGO.GetComponent<UICell> ();
				m_cells.Add (separatorCell);
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
