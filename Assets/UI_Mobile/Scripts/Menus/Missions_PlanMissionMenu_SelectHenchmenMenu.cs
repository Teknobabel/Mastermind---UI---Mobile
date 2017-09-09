using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missions_PlanMissionMenu_SelectHenchmenMenu : BaseMenu {

	public GameObject
	m_henchmenCellGO;

	public Transform
	m_contentParent;

	private Lair.FloorSlot m_floorSlot;
	private Player.ActorSlot m_currentSlot;

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
//			string statusString = "Status: " + h.m_status.m_name;

			string statusString = "";

			switch (h.m_rank) {

			case 1:
				statusString += "Novice ";
				break;
			case 2:
				statusString += "Skilled ";
				break;
			case 3:
				statusString += "Veteran ";
				break;
			case 4:
				statusString += "Master ";
				break;
			}

			if (h.traits.Count > 0) {

				Trait t = h.traits [0];
				statusString += t.m_name;
			}

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

		((MissionsApp)ParentApp).planMissionMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
	public Player.ActorSlot currentSlot {set {m_currentSlot = value;}}
}
