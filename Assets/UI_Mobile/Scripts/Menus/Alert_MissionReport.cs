using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alert_MissionReport : BaseMenu {

	public GameObject
	m_notificationCellGO,
	m_henchmenCellGO;

	public Transform
	m_contentParent;

	public Text
	m_headerText;

	private MissionSummary m_missionSummary;
//	private List<NotificationCenter.Notification> m_notifications = new List<NotificationCenter.Notification> ();

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		gameObject.SetActive (false);
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		gameObject.SetActive (true);

		m_headerText.text = "Mission Report:\n";
		m_headerText.text += m_missionSummary.m_mission.m_name;

		// create leader cell

		if (m_missionSummary.m_participatingActors.Count > 0) {

			Actor leader = m_missionSummary.m_participatingActors [0];

			GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contentParent);
			UICell c = (UICell)hCell.GetComponent<UICell> ();
			m_cells.Add (c);

			string nameString = leader.m_actorName;

			string statusString = "";

			switch (leader.m_rank) {

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

			if (leader.traits.Count > 0) {

				Trait t = leader.traits [0];
				statusString += t.m_name;
			}

			c.m_headerText.text = nameString;
			c.m_bodyText.text = statusString;
		}


		// create notification cells

		List<NotificationCenter.Notification> notifications = GameController.instance.GetMissionNotifications (0, m_missionSummary.m_missionID);

		foreach (NotificationCenter.Notification n in notifications) {

			GameObject cellGO = (GameObject)Instantiate (m_notificationCellGO, m_contentParent);
			UICell cell = (UICell)cellGO.GetComponent<UICell> ();
			cell.m_bodyText.text = n.m_title + "\n";
			cell.m_bodyText.text += n.m_message;
			m_cells.Add (cell);
		}

		LayoutRebuilder.ForceRebuildLayoutImmediate (m_contentParent.GetComponent<RectTransform>());
	}

	public override void OnEnter (bool animate)
	{
		base.OnEnter (animate);

		DisplayContent ();

	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

//		m_notifications.Clear ();

		gameObject.SetActive (false);
	}

	public void ContinueButtonPressed ()
	{
		m_parentApp.PopMenu ();
	}

//	public List<NotificationCenter.Notification> notifications {set{ m_notifications = value; }}
	public MissionSummary missionSummary {set{ m_missionSummary = value;}}
}
