using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lair_SelectMissionMenu : BaseMenu {

	public GameObject
		m_missionCell;

	public Transform
	m_contentParent;

	private Lair.FloorSlot m_floorSlot;

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

		foreach (Mission m in m_floorSlot.m_floor.m_missions) {

			if (m.IsValid (m_floorSlot.m_missionPlan)) {
				
				GameObject missionCellGO = (GameObject)Instantiate (m_missionCell, m_contentParent);
				UICell missionCell = (UICell)missionCellGO.GetComponent<UICell> ();
				m_cells.Add (missionCell);
				missionCell.m_headerText.text = m.m_name;
				missionCell.m_bodyText.text = m.m_cost.ToString () + " CP, " + m.m_duration.ToString () + " Turns";

				Button b = missionCell.m_buttons [0];
				b.onClick.AddListener (delegate {
					MissionSelected (m);
				});
			}
		}
	}

	public void MissionSelected (Mission m)
	{
		Debug.Log( "Mission: " + m.m_name + " selected");

		m_floorSlot.m_missionPlan.m_currentMission = m;

		((LairApp)m_parentApp).planMissionMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
}
