using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lair_SelectHenchmenMenu : MonoBehaviour, IMenu {


	public GameObject
		m_henchmenCellGO;

	public Transform
	m_contentParent;

	private IApp m_parentApp;

	private List<UICell> m_cells = new List<UICell>();

	private Lair.FloorSlot m_floorSlot;
	private Player.ActorSlot m_currentSlot;

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
		DisplayHenchmen ();

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

	private void DisplayHenchmen ()
	{
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		List<Player.ActorSlot> henchmenPool = GameController.instance.GetHiredHenchmen (0);

		List<Actor> hList = new List<Actor> ();

		foreach (Player.ActorSlot aSlot in henchmenPool) {

			// make sure henchmen isn't already on a mission

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_state != Player.ActorSlot.ActorSlotState.OnMission) {

				// make sure henchmen isn't already added this floor

				bool present = false;

				foreach (Player.ActorSlot a in m_floorSlot.m_actorSlots)
				{
					if (a.m_state != Player.ActorSlot.ActorSlotState.Empty && a.m_actor.id == aSlot.m_actor.id) {

						present = true;
					}
				}

				if (!present) {
					hList.Add (aSlot.m_actor);
				}
			}
		}

		if (hList.Count > 1) {
			hList.Sort (delegate(Actor a, Actor b) {
				return a.m_actorName.CompareTo (b.m_actorName);
			});
		}

		foreach (Actor h in hList) {

			GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contentParent);
			UICell c = (UICell)hCell.GetComponent<UICell> ();
			m_cells.Add (c);

			string nameString = h.m_actorName;
			string statusString = "Status: " + h.m_status.m_name;

			c.m_headerText.text = nameString;
			c.m_bodyText.text = statusString;
			c.m_image.texture = h.m_portrait_Compact;

			hCell.GetComponent<Button> ().onClick.AddListener (delegate {
				HenchmenSelected (h.id);
			});
		}
	}

	public void HenchmenSelected (int id)
	{
		Actor a = GameController.instance.GetActor (id);

		if (m_currentSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

			m_currentSlot.RemoveHenchmen ();
		}

		m_currentSlot.SetHenchmen (a);
//		m_currentSlot.m_actor = a;
//		m_currentSlot.m_state = Player.ActorSlot.ActorSlotState;

		((LairApp)ParentApp).planMissionMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}

	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
	public Player.ActorSlot currentSlot {set {m_currentSlot = value;}}
}
