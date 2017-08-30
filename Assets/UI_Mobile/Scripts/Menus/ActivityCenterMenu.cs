using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class ActivityCenterMenu : MonoBehaviour, IObserver {

	public Transform
	m_scrollViewParent;

	public GameObject
	m_activityHeader,
	m_activityCell,
	m_activityCellWithIcon;

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

				if (s.m_location != EventLocation.None) {



					GameObject cellGO = (GameObject)Instantiate (m_activityCellWithIcon, m_scrollViewParent);
					UICell cell = (UICell)cellGO.GetComponent<UICell> ();
					cell.m_bodyText.text = s.m_title + "\n";
					cell.m_bodyText.text += s.m_message;

					IApp app = MobileUIEngine.instance.GetApp (s.m_location);
					Debug.Log (app);
					if (app != null && app.Icon != null) {
						cell.m_image.texture = app.Icon.texture;
					}

					Button b = cell.m_buttons [0];
					b.onClick.AddListener (delegate {
						NotificationTapped(s.m_location);});

					m_cells.Add (cellGO);

					if (entry.Key == activityList.Count-1) {

						cell.m_rectTransforms [0].anchoredPosition = new Vector2(MobileUIEngine.instance.m_mainCanvas.sizeDelta.x, 0);
					}

				} else {

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

	public void NotificationTapped (EventLocation location)
	{
		IApp app = MobileUIEngine.instance.GetApp (location);

		if (app != null) {

			MobileUIEngine.instance.PushApp (app);
		}
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
