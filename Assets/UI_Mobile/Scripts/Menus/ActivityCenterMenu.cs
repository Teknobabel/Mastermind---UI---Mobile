using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class ActivityCenterMenu : BaseMenu, IObserver {

	public Transform
	m_scrollViewParent;

	public GameObject
	m_activityHeader,
	m_activityCell,
	m_activityCellWithIcon,
	m_playerActivityCell;

//	private List<GameObject> m_cells = new List<GameObject>();

	// Use this for initialization
//	void Start () {
//		
//	}

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		GameController.instance.AddObserver (this);
		Dictionary<int, List<NotificationCenter.Notification>> feed = GameController.instance.GetPlayerNotifications(0);

		GameObject playerCellGO = (GameObject)Instantiate (m_playerActivityCell, m_scrollViewParent);
		Cell_PlayerActivity playerCell = (Cell_PlayerActivity)playerCellGO.GetComponent<Cell_PlayerActivity> ();
		Player player = GameController.instance.game.playerList [0];
		playerCell.SetPlayer (player);

		SetActivity (feed);
	}

	public void SetActivity (Dictionary<int, List<NotificationCenter.Notification>> activityList)
	{
		while (m_cells.Count > 0) {

			UICell go = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (go.gameObject);
		}

		foreach(KeyValuePair<int, List<NotificationCenter.Notification>> entry in activityList.Reverse())
		{
			GameObject header = (GameObject)Instantiate (m_activityHeader, m_scrollViewParent);
			UICell headerCell = (UICell)header.GetComponent<UICell> ();
			headerCell.m_headerText.text = ("Turn " + entry.Key.ToString ()).ToUpper();
			m_cells.Add (headerCell);

			List<NotificationCenter.Notification> sList = entry.Value;

			for (int i = 0; i < sList.Count; i++) {
					
				NotificationCenter.Notification s = sList[i];

				if (s.m_location != EventLocation.None) {



					GameObject cellGO = (GameObject)Instantiate (m_activityCellWithIcon, m_scrollViewParent);
					Cell_Notification cell = (Cell_Notification)cellGO.GetComponent<Cell_Notification> ();
					cell.SetNotification (s);

					Button b = cell.m_buttons [0];
					b.onClick.AddListener (delegate {
						NotificationTapped(s.m_location);});

					m_cells.Add ((UICell)cell);

					if (entry.Key == activityList.Count-1) {

						cell.m_rectTransforms [0].anchoredPosition = new Vector2(MobileUIEngine.instance.m_mainCanvas.sizeDelta.x, 0);
					}

				} else {

					GameObject cellGO = (GameObject)Instantiate (m_activityCell, m_scrollViewParent);
					UICell cell = (UICell)cellGO.GetComponent<UICell> ();
					cell.m_bodyText.text = s.m_title + "\n";
					cell.m_bodyText.text += s.m_message;
					m_cells.Add (cell);

					if (entry.Key == activityList.Count-1) {

						cell.m_rectTransforms [0].anchoredPosition = new Vector2(MobileUIEngine.instance.m_mainCanvas.sizeDelta.x, 0);
					}
				}
			}
		}

//		LayoutRebuilder.ForceRebuildLayoutImmediate (m_scrollViewParent.GetComponent<RectTransform>());
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
