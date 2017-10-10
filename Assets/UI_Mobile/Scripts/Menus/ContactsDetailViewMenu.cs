using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class ContactsDetailViewMenu : BaseMenu, IUISubject, IUIObserver {

	public enum InfoPanelType
	{
		None,
		Traits,
		History,
		Info,
	}

	public RawImage
	m_portrait;

	public Text
	m_name,
	m_upkeepCost;

	public GameObject
	m_traitCellGO,
	m_activityHeader,
	m_activityCell;

	public SegmentedToggle m_infoPanelToggle;

	public Transform[] m_infoPanels;

	public Transform[] m_panels;

	private List<IUIObserver>
	m_observers = new List<IUIObserver> ();

	private InfoPanelType 
	m_currentPanel = InfoPanelType.Traits;

	protected int m_henchmenID = -1;

	private ContactsDetailViewMenu m_menuObject;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		m_infoPanelToggle.AddObserver (this);
		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		base.OnEnter (animate);

		Actor h = GameController.instance.GetActor (m_henchmenID);

		if (h != null) {

//			ContactsDetailViewMenu cMenu = (ContactsDetailViewMenu)m_menuParent.GetComponent<ContactsDetailViewMenu> ();
			m_name.text = h.m_actorName;
			if (m_upkeepCost != null) {
				m_upkeepCost.text = "-" + h.m_turnCost.ToString ();
			}
			m_portrait.texture = h.m_portrait_Large;
		}
		this.gameObject.SetActive (true);

		if (m_currentPanel == InfoPanelType.Traits) {
			SetInfoPanel (InfoPanelType.Traits);
		}

		if (animate) {
			// slide in animation
			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			Rect r = rt.rect;
			rt.anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);

			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, 0), 0.25f);

			// portrait image slides slightly to the right
			RectTransform picRT = m_portrait.GetComponent<RectTransform> ();
			picRT.anchoredPosition = new Vector2 (-200, 0);
			DOTween.To (() => picRT.anchoredPosition, x => picRT.anchoredPosition = x, new Vector2 (0, 0), 0.25f);

			// info panel slides up slightly
			RectTransform infoPanelRT = m_panels [0].GetComponent<RectTransform> ();
			infoPanelRT.anchoredPosition = new Vector2 (0, -200);
			RectTransform infoPanel2RT = m_panels [1].GetComponent<RectTransform> ();
			infoPanel2RT.anchoredPosition = new Vector2 (0, -200);
			DOTween.To (() => infoPanelRT.anchoredPosition, x => infoPanelRT.anchoredPosition = x, new Vector2 (0, 0), 0.25f);
			DOTween.To (() => infoPanel2RT.anchoredPosition, x => infoPanel2RT.anchoredPosition = x, new Vector2 (0, 0), 0.25f);


			// back button slides down 
//			RectTransform appNavRT = m_panels [2].GetComponent<RectTransform> ();
//			appNavRT.anchoredPosition = new Vector2 (0, appNavRT.rect.height);
//			DOTween.To (() => appNavRT.anchoredPosition, x => appNavRT.anchoredPosition = x, new Vector2 (0, 0), 0.25f).SetDelay (0.15f);

		} else {

//			RectTransform appNavRT = m_panels[2].GetComponent<RectTransform> ();
//			appNavRT.anchoredPosition = Vector2.zero;

			RectTransform infoPanelRT = m_panels[0].GetComponent<RectTransform> ();
			infoPanelRT.anchoredPosition = Vector2.zero;
			RectTransform infoPanel2RT = m_panels[1].GetComponent<RectTransform> ();
			infoPanel2RT.anchoredPosition = Vector2.zero;

			RectTransform picRT = m_portrait.GetComponent<RectTransform> ();
			picRT.anchoredPosition = Vector2.zero;

			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			rt.anchoredPosition = Vector2.zero;

		}

	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		if (animate) {
			// slide out animation
			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			Rect r = rt.rect;

			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0), 0.25f).OnComplete(OnExitComplete);

		} else {
			OnExitComplete ();
		}
	}

	public void OnExitComplete ()
	{
//		RectTransform appNavRT = m_panels[2].GetComponent<RectTransform> ();
//		appNavRT.anchoredPosition = Vector2.zero;

		RectTransform infoPanelRT = m_panels[0].GetComponent<RectTransform> ();
		infoPanelRT.anchoredPosition = Vector2.zero;
		RectTransform infoPanel2RT = m_panels[1].GetComponent<RectTransform> ();
		infoPanel2RT.anchoredPosition = Vector2.zero;

		RectTransform picRT = m_portrait.GetComponent<RectTransform> ();
		picRT.anchoredPosition = Vector2.zero;

		RectTransform rt = gameObject.GetComponent<RectTransform> ();
		rt.anchoredPosition = Vector2.zero;

		while (m_cells.Count > 0) {
			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		this.gameObject.SetActive (false);
	}

	public void SetHenchmen (int id)
	{
		m_henchmenID = id;
	}

	public void FireButtonPressed()
	{
		// check if henchmen is on a mission, block if so

		bool onMission = false;

		List<Player.ActorSlot> hiredHenchmen = GameController.instance.GetHiredHenchmen (0);

		foreach (Player.ActorSlot aSlot in hiredHenchmen) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_actor.id == m_henchmenID) {
//				Debug.Log (aSlot.m_actor.m_actorName);
//				Debug.Log (aSlot.m_state);
				if (aSlot.m_state == Player.ActorSlot.ActorSlotState.OnMission) {

					onMission = true;
					break;
				}
			}
		}

		if (onMission) {

			string header = "Can't Fire Henchmen";
			string message = "This Henchmen is currently on a Mission. Cancel the Mission if you want to fire this Henchmen.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener (delegate {
				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
			});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

		} else {
			
			string header = "Fire Henchmen";
			string message = "Are you sure you want to fire this henchmen? Fired henchmen will be available for hire again in the future.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b1 = MobileUIEngine.instance.alertDialogue.AddButton ("Fire Henchmen");
			b1.onClick.AddListener (delegate {
				FireHenchmen ();
			});
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Cancel");
			b2.onClick.AddListener (delegate {
				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
			});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
		}
	}

	public void FireHenchmen ()
	{
		MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();

		Action_FireAgent fireAgent = new Action_FireAgent ();
		fireAgent.m_playerNumber = (0);
		fireAgent.m_henchmenID = m_henchmenID;
		GameController.instance.ProcessAction (fireAgent);

		((HenchmenApp)ParentApp).homeMenu.isDirty = true;
		ParentApp.PopMenu ();
	}

	public void AddObserver (IUIObserver observer)	
	{
		m_observers.Add (observer);
	}

	public void RemoveObserver (IUIObserver observer)
	{
		if (m_observers.Contains(observer))
		{
			m_observers.Remove(observer);
		}
	}

	public void Notify (IUISubject subject, UIEvent thisUIEvent)
	{
		for (int i=0; i < m_observers.Count; i++)
		{
			m_observers[i].OnNotify(subject, thisUIEvent);
		}
	}

	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent)
		{
		case UIEvent.UI_ToggleButtonPressed:

			switch (m_infoPanelToggle.activeButton) {
			case 0:
				SetInfoPanel (InfoPanelType.Traits);
				break;
			case 1:
				SetInfoPanel (InfoPanelType.History);
				break;
			case 2:
				SetInfoPanel (InfoPanelType.Info);
				break;
			}

			break;
		}
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		switch (m_currentPanel) {

		case InfoPanelType.Traits:
			m_infoPanels [0].gameObject.SetActive (true);

			// create new trait cells

			//			Henchmen h = GetDummyData.instance.GetHenchmen (m_henchmenID);
			Actor h = GameController.instance.GetActor (m_henchmenID);

			if (h != null) {

				foreach (Trait t in h.traits) {

					GameObject traitCellGO = (GameObject)Instantiate (m_traitCellGO, m_panels[3]);
					Cell_Trait tCell = (Cell_Trait)traitCellGO.GetComponent<Cell_Trait> ();
					tCell.SetTrait(t);
					m_cells.Add (tCell);
				}
			}

			break;
		case InfoPanelType.History:

			m_infoPanels [1].gameObject.SetActive (true);

			// get history for this henchmen

			Dictionary<int, List<NotificationCenter.Notification>> notifications = GameController.instance.GetHenchmenNotifications (m_henchmenID);

			// display history

			foreach(KeyValuePair<int, List<NotificationCenter.Notification>> entry in notifications.Reverse())
			{
				GameObject header = (GameObject)Instantiate (m_activityHeader, m_panels[4]);
				Cell_Header headerCell = (Cell_Header)header.GetComponent<Cell_Header> ();
				headerCell.SetHeader("Turn " + entry.Key.ToString ());
				m_cells.Add (headerCell);

				List<NotificationCenter.Notification> sList = entry.Value;

				for (int i = 0; i < sList.Count; i++) {

					NotificationCenter.Notification s = sList[i];
					GameObject cellGO = (GameObject)Instantiate (m_activityCell, m_panels[4]);
					UICell cell = (UICell)cellGO.GetComponent<UICell> ();
					cell.m_bodyText.text = s.m_title + "\n";
					cell.m_bodyText.text += s.m_message;
					m_cells.Add (cell);

					if (entry.Key == notifications.Count-1) {

						cell.m_rectTransforms [0].anchoredPosition = new Vector2(MobileUIEngine.instance.m_mainCanvas.sizeDelta.x, 0);
					}
				}
			}

			break;

		case InfoPanelType.Info:
			m_infoPanels [2].gameObject.SetActive (true);
			break;

		}
	}

	private void SetInfoPanel (InfoPanelType pType)
	{
		
		switch (m_currentPanel) {
		case InfoPanelType.Traits:
			m_infoPanels [0].gameObject.SetActive (false);
			break;
		case InfoPanelType.History:
			m_infoPanels [1].gameObject.SetActive (false);
			break;
		case InfoPanelType.Info:
			m_infoPanels [2].gameObject.SetActive (false);
			break;

		}

		m_currentPanel = pType;

		DisplayContent ();

	}
}
