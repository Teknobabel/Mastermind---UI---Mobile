using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OmegaPlan_SelectHenchmenMenu : BaseMenu {

	public GameObject
	m_henchmenCellGO;

	public Transform
	m_contentParent;

	private OmegaPlan.OPGoal m_goal;

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

		List<Player.ActorSlot> hList = new List<Player.ActorSlot> ();

		foreach (Player.ActorSlot aSlot in henchmenPool) {

			// make sure henchmen isn't already on a mission

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_state != Player.ActorSlot.ActorSlotState.OnMission) {

				// make sure henchmen isn't already added this floor

				bool present = false;

				foreach (Player.ActorSlot a in m_goal.plan.m_actorSlots)
				{
					if (a.m_state != Player.ActorSlot.ActorSlotState.Empty && a.m_actor.id == aSlot.m_actor.id) {

						present = true;
					}
				}

				if (!present) {
					hList.Add (aSlot);
				}
			}
		}

		if (hList.Count > 1) {
			hList.Sort (delegate(Player.ActorSlot a, Player.ActorSlot b) {
				return a.m_actor.m_actorName.CompareTo (b.m_actor.m_actorName);
			});
		}

		foreach (Player.ActorSlot h in hList) {

			GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contentParent);
			UICell c = (UICell)hCell.GetComponent<UICell> ();
			m_cells.Add (c);

			string nameString = h.m_actor.m_actorName;
			string statusString = "Active";

			c.m_headerText.text = nameString;
			c.m_bodyText.text = statusString;
			c.m_image.texture = h.m_actor.m_portrait_Compact;

			hCell.GetComponent<Button> ().onClick.AddListener (delegate {
				HenchmenSelected (h);
			});
		}
	}

	public void HenchmenSelected (Player.ActorSlot aSlot)
	{
		m_goal.plan.m_actorSlots.Add (aSlot);

//		Actor a = GameController.instance.GetActor (id);
//
//		if (m_currentSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {
//
//			m_currentSlot.RemoveHenchmen ();
//		}
//
//		m_currentSlot.SetHenchmen (a);
		//		m_currentSlot.m_actor = a;
		//		m_currentSlot.m_state = Player.ActorSlot.ActorSlotState;

		((OmegaPlansApp)ParentApp).missionPlanningMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

	public OmegaPlan.OPGoal goal {set {m_goal = value;}}
}
