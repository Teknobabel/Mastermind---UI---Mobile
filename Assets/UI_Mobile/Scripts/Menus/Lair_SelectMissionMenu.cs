using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lair_SelectMissionMenu : MonoBehaviour, IMenu {

	public GameObject
		m_missionCell;

	public Transform
	m_contentParent;

	private IApp m_parentApp;

	private List<UICell> m_cells = new List<UICell>();

	private Lair.FloorSlot m_floorSlot;

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

		DisplayMissions ();
	}

	public void OnExit (bool animate)
	{
		this.gameObject.SetActive (false);
	}

	public void OnHold ()
	{

	}

	public void OnReturn ()
	{

	}

	private void DisplayMissions ()
	{
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		foreach (Mission m in m_floorSlot.m_floor.m_missions) {

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

	public void MissionSelected (Mission m)
	{
		Debug.Log( "Mission: " + m.m_name + " selected");

		m_floorSlot.m_missionPlan.m_currentMission = m;

		((LairApp)m_parentApp).planMissionMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}

	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
}
