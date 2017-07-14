using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class ActivityCenterMenu : MonoBehaviour, IObserver {

	public Transform
	m_scrollViewParent;

	public GameObject
	m_activityHeader,
	m_activityCell;

	private List<GameObject> m_cells = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize ()
	{
		GameController.instance.AddObserver (this);

		Dictionary<int, List<NotificationCenter.Notification>> feed = GameController.instance.GetPlayerNotifications(0);
		SetActivity (feed);
	}

	public void SetActivity (Dictionary<int, List<NotificationCenter.Notification>> activityList)
	{
		while (m_cells.Count > 0) {

			GameObject go = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (go);
		}

		foreach(KeyValuePair<int, List<NotificationCenter.Notification>> entry in activityList.Reverse())
		{
			GameObject header = (GameObject)Instantiate (m_activityHeader, m_scrollViewParent);
			UICell headerCell = (UICell)header.GetComponent<UICell> ();
			headerCell.m_headerText.text = "Turn " + entry.Key.ToString ();
			m_cells.Add (header);

			List<NotificationCenter.Notification> sList = entry.Value;

			for (int i = 0; i < sList.Count; i++) {
					
				NotificationCenter.Notification s = sList[i];
				GameObject cellGO = (GameObject)Instantiate (m_activityCell, m_scrollViewParent);
				UICell cell = (UICell)cellGO.GetComponent<UICell> ();
				cell.m_bodyText.text = s.m_title + "\n";
				cell.m_bodyText.text += s.m_message;
				m_cells.Add (cellGO);

				if (entry.Key == activityList.Count-1) {

					cell.m_rectTransforms [0].anchoredPosition = new Vector2(MobileUIEngine.instance.m_mainCanvas.sizeDelta.x, 0);
				}
			}
		}
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {

		case GameEvent.Turn_PlayerPhaseStarted:

			Dictionary<int, List<NotificationCenter.Notification>> feed = GameController.instance.GetPlayerNotifications(0);
			SetActivity (feed);

			break;
		}
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
