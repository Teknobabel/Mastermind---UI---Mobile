using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanMission_SelectHenchmenMenu : BaseMenu {

	public GameObject
	m_henchmenCellGO,
	m_traitCellGO,
	m_separatorCellGO,
	m_spacer,
	m_cellDetailPanel,
	m_cellCostPanel;

	public Transform
	m_contentParent;

//	private Lair.FloorSlot m_floorSlot;
	private Player.ActorSlot m_currentSlot;

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

	public  override void DisplayContent ()
	{
		base.DisplayContent ();

		List<Player.ActorSlot> henchmenPool = GameController.instance.GetHiredHenchmen (0);

		List<Player.ActorSlot> hList = new List<Player.ActorSlot> ();

		foreach (Player.ActorSlot aSlot in henchmenPool) {

			// make sure henchmen isn't already on a mission

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_state != Player.ActorSlot.ActorSlotState.OnMission) {

				// make sure henchmen isn't already added this floor

				bool present = false;

				foreach (Player.ActorSlot a in m_missionPlan.m_actorSlots)
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
			Cell_Actor c = (Cell_Actor)hCell.GetComponent<Cell_Actor> ();
			m_cells.Add ((UICell)c);
			c.SetActor (h);

//			string nameString = h.m_actor.m_actorName;
//			//			string statusString = "Status: " + h.m_actor.m_status.m_name;
//
//			string statusString = "";
//
//			switch (h.m_actor.m_rank) {
//
//			case 1:
//				statusString += "Novice ";
//				break;
//			case 2:
//				statusString += "Skilled ";
//				break;
//			case 3:
//				statusString += "Veteran ";
//				break;
//			case 4:
//				statusString += "Master ";
//				break;
//			}
//
//			if (h.m_actor.traits.Count > 0) {
//
//				Trait t = h.m_actor.traits [0];
//				statusString += t.m_name;
//			}
//
//			c.m_headerText.text = nameString;
//			c.m_bodyText.text = statusString;
//			c.m_image.texture = h.m_actor.m_portrait_Compact;

			c.m_buttons [1].gameObject.SetActive (false);
			c.m_buttons[0].onClick.AddListener (delegate {
				HenchmenSelected (h);
			});

			// set detail panel - traits

			GameObject traitPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
			Cell_DetailPanel dPanel = (Cell_DetailPanel)traitPanel.GetComponent<Cell_DetailPanel> ();
			dPanel.SetTraits (h);
			m_cells.Add (dPanel);

//			foreach (Trait t in h.m_actor.traits) {
//
//				GameObject traitCellGO = (GameObject)Instantiate (m_traitCellGO, m_contentParent);
//				UICell traitCell = (UICell)traitCellGO.GetComponent<UICell> ();
//				m_cells.Add (traitCell);
//
//				traitCell.m_headerText.text = "Trait: " + t.m_name;
//
//				if (m_missionPlan.m_requiredTraits.Contains (t)) {
//
//					traitCell.m_headerText.color = Color.green;
//				}
//			}

			GameObject spacerGO2 = (GameObject)Instantiate (m_spacer, m_contentParent);
			Cell_Spacer spacer2 = (Cell_Spacer)spacerGO2.GetComponent<Cell_Spacer> ();
			m_cells.Add (spacer2);
		}

		GameObject spacerGO3 = (GameObject)Instantiate (m_spacer, m_contentParent);
		Cell_Spacer spacer3 = (Cell_Spacer)spacerGO3.GetComponent<Cell_Spacer> ();
		spacer3.SetHeight (100);
		m_cells.Add (spacer3);
	}

	public void HenchmenSelected (Player.ActorSlot aSlot)
	{
		if (m_currentSlot != null) {

			m_missionPlan.m_actorSlots.Remove (m_currentSlot);
			m_currentSlot = null;
		}

		m_missionPlan.m_actorSlots.Add (aSlot);

		//		Actor a = GameController.instance.GetActor (id);

		//		if (m_currentSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {
		//
		//			m_currentSlot.RemoveHenchmen ();
		//		}
		//
		//		m_currentSlot.SetHenchmen (a);
		//		m_currentSlot.m_actor = a;
		//		m_currentSlot.m_state = Player.ActorSlot.ActorSlotState;

		m_parentMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

//	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
	public MissionPlan missionPlan {set{ m_missionPlan = value;}}
		public Player.ActorSlot currentSlot {set {m_currentSlot = value;}}
	public BaseMenu parentMenu {set{ m_parentMenu = value; }}
}
