using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanMission_SelectTargetActorMenu : BaseMenu {

	public GameObject m_henchmenCellGO;

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

		List<Player.ActorSlot> actors = GameController.instance.GetHiredHenchmen (0);

		foreach (Player.ActorSlot aSlot in actors) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				GameObject actorCellGO = (GameObject)Instantiate (m_henchmenCellGO, m_contentParent);
				UICell actorCell = (UICell)actorCellGO.GetComponent<UICell> ();

				string nameString = aSlot.m_actor.m_actorName;
				string statusString = "Status: " + aSlot.m_actor.m_status.m_name;

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

		m_missionPlan.m_targetActor = targetSlot;

		((LairApp)m_parentApp).planMissionMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

//	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
	public MissionPlan missionPlan {set{ m_missionPlan = value;}}
	public BaseMenu parentMenu {set{ m_parentMenu = value; }}
}
