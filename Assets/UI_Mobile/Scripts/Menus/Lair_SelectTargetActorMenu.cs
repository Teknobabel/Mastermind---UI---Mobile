using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lair_SelectTargetActorMenu : MonoBehaviour, IMenu {

	public GameObject m_henchmenCellGO;

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

		DisplayActors ();
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

	private void DisplayActors ()
	{
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		List<Player.ActorSlot> actors = GameController.instance.GetHiredHenchmen (0);

		foreach (Player.ActorSlot aSlot in actors) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				GameObject actorCellGO = (GameObject)Instantiate (m_henchmenCellGO, m_contentParent);
				UICell actorCell = (UICell)actorCellGO.GetComponent<UICell> ();

				string nameString = aSlot.m_actor.m_actorName;
				string statusString = "Active";

				actorCell.m_headerText.text = nameString;
				actorCell.m_bodyText.text = statusString;
				actorCell.m_image.texture = aSlot.m_actor.m_portrait_Compact;

				m_cells.Add (actorCell);

				actorCell.GetComponent<Button> ().onClick.AddListener (delegate {
					TargetSelected (aSlot);
				});
			}
			
		}
	}

	public void TargetSelected (Player.ActorSlot targetSlot)
	{
//		Debug.Log( "Site: " + s.m_siteName + " selected");

		m_floorSlot.m_missionPlan.m_targetActor = targetSlot;

		((LairApp)m_parentApp).planMissionMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}

	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
}
