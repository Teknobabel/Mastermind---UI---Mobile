using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missions_PlanMissionMenu_SelectMissionMenu : BaseMenu, IUIObserver {

	public enum DisplayType {
		ByLairFloor,
		BySkillTrait,
	}

	public GameObject
	m_missionCell,
	m_headerCell;

	public Transform
	m_contentParent;

	public SegmentedToggle m_infoPanelToggle;

	private Lair.FloorSlot m_floorSlot;

	private DisplayType m_displayType = DisplayType.ByLairFloor;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);
		m_infoPanelToggle.AddObserver (this);
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

		Lair lair = GameController.instance.GetLair (0);

		if (m_displayType == DisplayType.ByLairFloor) {
			
			// display all missions from all floors not actively on a mission

			foreach (Lair.FloorSlot fSlot in lair.floorSlots) {

				if (fSlot.m_state != Lair.FloorSlot.FloorState.Empty && fSlot.m_state != Lair.FloorSlot.FloorState.MissionInProgress) {

					GameObject headerCellGO = (GameObject)Instantiate (m_headerCell, m_contentParent);
					UICell headerCell = (UICell)headerCellGO.GetComponent<UICell> ();
					m_cells.Add (headerCell);
					headerCell.m_headerText.text = fSlot.m_floor.m_name;

					foreach (Mission m in fSlot.m_floor.m_missions) {

						if (m.IsValid (fSlot.m_missionPlan)) {

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
			}

		} else if (m_displayType == DisplayType.BySkillTrait) {

			Dictionary<string, List<Mission>> missionsBySkill = new Dictionary<string, List<Mission>> ();

			foreach (Lair.FloorSlot fSlot in lair.floorSlots) {

				if (fSlot.m_state != Lair.FloorSlot.FloorState.Empty && fSlot.m_state != Lair.FloorSlot.FloorState.MissionInProgress) {

					foreach (Mission m in fSlot.m_floor.m_missions) {

						if (m.IsValid(fSlot.m_missionPlan) && m.m_requiredTraits.Length > 0) {
							
							Trait skillTrait = m.m_requiredTraits [0];

							if (!missionsBySkill.ContainsKey (skillTrait.m_name)) {

								List<Mission> newMissionList = new List<Mission> ();
								newMissionList.Add (m);
								missionsBySkill.Add (skillTrait.m_name, newMissionList);

							} else {

								List<Mission> missionList = missionsBySkill [skillTrait.m_name];
								missionList.Add (m);
								missionsBySkill [skillTrait.m_name] = missionList;

							}
						}
					}
				}
			}

			if (missionsBySkill.Count > 0) {

				foreach (KeyValuePair<string, List<Mission>> pair in missionsBySkill) {

					GameObject headerCellGO = (GameObject)Instantiate (m_headerCell, m_contentParent);
					UICell headerCell = (UICell)headerCellGO.GetComponent<UICell> ();
					m_cells.Add (headerCell);
					headerCell.m_headerText.text = pair.Key;

					foreach (Mission m in pair.Value) {

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

		}

	}

	public void MissionSelected (Mission m)
	{
		Debug.Log( "Mission: " + m.m_name + " selected");

		((MissionsApp)m_parentApp).planMissionMenu.missionPlan.m_currentMission = m;

		Lair lair = GameController.instance.GetLair (0);

		foreach (Lair.FloorSlot fSlot in lair.floorSlots) {

			if (fSlot.m_floor.m_missions.Contains (m)) {

				((MissionsApp)m_parentApp).planMissionMenu.missionPlan.m_floorSlot = fSlot;
				break;
			}
		}
			
		((MissionsApp)m_parentApp).planMissionMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent)
		{
		case UIEvent.UI_ToggleButtonPressed:


			switch (m_infoPanelToggle.activeButton) {
			case 0:
				m_displayType = DisplayType.ByLairFloor;
				break;
			case 1:
				m_displayType = DisplayType.BySkillTrait;
				break;

			}
			DisplayContent ();
			break;
		}
	}

	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
}
