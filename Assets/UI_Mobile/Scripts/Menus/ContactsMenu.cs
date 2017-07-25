using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContactsMenu : MonoBehaviour, IMenu, IUIObserver {

	public enum DisplayType {
		Alpha,
		Trait,
		Mission,
	}

	public Text
	m_appNameText;

	public GameObject
	m_henchmenCellGO,
	m_headerCellGO;

	public Transform
	m_contactsListParent;

	public SegmentedToggle m_infoPanelToggle;

	private IApp m_parentApp;

	private List<UICell> m_cells = new List<UICell>();

	private DisplayType m_displayType = DisplayType.Alpha;

	private bool m_isDirty = false;

//	private Transform m_menuParent;

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
		m_appNameText.text = parentApp.Name;
		m_infoPanelToggle.AddObserver (this);
//		m_infoPanelToggle.ToggleButtonClicked (0);
		this.gameObject.SetActive (false);
	}

	public void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		DisplayHenchmen ();

		// slide in animation
		if (animate) {
			
			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			Rect r = rt.rect;
			rt.anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);

			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, 0), 0.5f);

			for (int i = 0; i < m_cells.Count; i++) {

				UICell c = m_cells [i];
				c.m_rectTransforms[0].anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);
				DOTween.To (() => c.m_rectTransforms [0].anchoredPosition, x => c.m_rectTransforms [0].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetEase (Ease.OutCirc).SetDelay (0.25f + (i * 0.07f));

				if (c.m_image != null) {
					c.m_image.transform.localScale = Vector3.zero;
					DOTween.To (() => c.m_image.transform.localScale, x => c.m_image.transform.localScale = x, new Vector3 (1, 1, 1), 0.5f).SetEase (Ease.OutCirc).SetDelay (0.75f + (i * 0.09f));
				}
			}
		} 
		else {

			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			rt.anchoredPosition = Vector2.zero;

		}
	}

	public void OnExit (bool animate)
	{
		// clear any remaining new flags

		bool newStateChanged = false;

		List<Player.ActorSlot> hiringPool = GameController.instance.GetHiredHenchmen (0);

		foreach (Player.ActorSlot aSlot in hiringPool) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_new) {

				Action_SetActorNewState newState = new Action_SetActorNewState ();
				newState.m_newState = false;
				newState.m_actorSlot = aSlot;
				GameController.instance.ProcessAction (newState);
				newStateChanged = true;
			}
		}

		if (newStateChanged) {

			m_parentApp.SetAlerts ();
		}

		if (animate) {
			// slide out animation
			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			Rect r = rt.rect;

			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0), 0.5f);

		} else {

			OnExitComplete ();
		}

	}

	public void OnExitComplete ()
	{

		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		RectTransform rt = gameObject.GetComponent<RectTransform> ();
		rt.anchoredPosition = Vector2.zero;

		this.gameObject.SetActive (false);
	}

	public void OnHold ()
	{

	}

	public void OnReturn ()
	{
		if (m_isDirty) {

			m_isDirty = false;
			DisplayHenchmen ();
		}
	}

	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent)
		{
		case UIEvent.UI_ToggleButtonPressed:
			

			switch (m_infoPanelToggle.activeButton) {
			case 0:
				m_displayType = DisplayType.Alpha;
				break;
			case 1:
				m_displayType = DisplayType.Trait;
				break;
			case 2:
				m_displayType = DisplayType.Mission;
				break;
				
			}
			DisplayHenchmen ();
			break;
		}
	}

	public void HenchmenCellClicked (Player.ActorSlot aSlot)
	{
//		Debug.Log("Henchmen Cell w id: " + henchmenID + " clicked");
//
//		m_henchmenDetailMenu.SetHenchmen (henchmenID);
//
		((HenchmenApp)(m_parentApp)).detailMenu.SetHenchmen(aSlot.m_actor.id);
		m_parentApp.PushMenu (((HenchmenApp)(m_parentApp)).detailMenu);
	}

	private void DisplayHenchmen ()
	{
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		List<Player.ActorSlot> hiringPool = GameController.instance.GetHiredHenchmen (0);

//		bool newStateChanged = false;

		List<Player.ActorSlot> hList = new List<Player.ActorSlot> ();

		foreach (Player.ActorSlot aSlot in hiringPool) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				hList.Add (aSlot);

//				if (aSlot.m_new) {
//
//					Action_SetActorNewState newState = new Action_SetActorNewState ();
//					newState.m_actorSlot = aSlot;
//					GameController.instance.ProcessAction (newState);
//					newStateChanged = true;
//				}
			}
		}

		int emptySlots = hiringPool.Count - hList.Count;

//		if (newStateChanged) {
//
//			m_parentApp.SetAlerts ();
//		}

		switch (m_displayType)
		{
		case DisplayType.Alpha:

				hList.Sort (delegate(Player.ActorSlot a, Player.ActorSlot b) {
					return a.m_actor.m_actorName.CompareTo (b.m_actor.m_actorName);
			});

				foreach (Player.ActorSlot h in hList) {

				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
				UICell c = (UICell)hCell.GetComponent<UICell> ();
				m_cells.Add (c);

					string nameString = h.m_actor.m_actorName;
				string statusString = "Active";

				c.m_headerText.text = nameString;
				c.m_bodyText.text = statusString;
					c.m_image.texture = h.m_actor.m_portrait_Compact;

				if (h.m_new) {
					c.m_rectTransforms [1].gameObject.SetActive (true);
				}

				hCell.GetComponent<Button> ().onClick.AddListener (delegate {
						HenchmenCellClicked (h);
				});
			}

			for (int i = 0; i < emptySlots; i++) {

				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
				UICell c = (UICell)hCell.GetComponent<UICell> ();
				m_cells.Add (c);

				string nameString = "EMPTY";
				string statusString = "Recruit henchmen in the Hire app";

				c.m_headerText.text = nameString;
				c.m_headerText.color = Color.gray;
				c.m_bodyText.text = statusString;
				c.m_bodyText.color = Color.gray;
			}

			break;

		case DisplayType.Trait:

			Dictionary<string, List<Player.ActorSlot>> hListByTrait = new Dictionary<string, List<Player.ActorSlot>> ();

			foreach (Player.ActorSlot h in hList) {

				foreach (Trait t in h.m_actor.traits) {

					if (hListByTrait.ContainsKey (t.m_name)) {

						List<Player.ActorSlot> l = hListByTrait [t.m_name];
						l.Add (h);
						hListByTrait [t.m_name] = l;

					} else {

						List<Player.ActorSlot> newList = new List<Player.ActorSlot> ();
						newList.Add (h);
						hListByTrait.Add (t.m_name, newList);
					}
				}
			}

			if (hListByTrait.Count > 0) {

				foreach(KeyValuePair<string, List<Player.ActorSlot>> entry in hListByTrait)
				{
					GameObject header = (GameObject)Instantiate (m_headerCellGO, m_contactsListParent);
					UICell headerCell = (UICell)header.GetComponent<UICell> ();
					headerCell.m_headerText.text = entry.Key.ToString ();
					m_cells.Add (headerCell);

					List<Player.ActorSlot> sortedList = entry.Value;

					sortedList.Sort (delegate(Player.ActorSlot a, Player.ActorSlot b) {
						return a.m_actor.m_actorName.CompareTo (b.m_actor.m_actorName);
					});

					foreach (Player.ActorSlot h in sortedList) {

						GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
						UICell c = (UICell)hCell.GetComponent<UICell> ();
						m_cells.Add (c);

						string nameString = h.m_actor.m_actorName;
						string statusString = "Active";

						c.m_headerText.text = nameString;
						c.m_bodyText.text = statusString;
						c.m_image.texture = h.m_actor.m_portrait_Compact;

						if (h.m_new) {
							c.m_rectTransforms [1].gameObject.SetActive (true);
						}

						hCell.GetComponent<Button> ().onClick.AddListener (delegate {
							HenchmenCellClicked (h);
						});
					}
				}
			}
			
			break;
		}
	}
	
	public IApp ParentApp 
	{ get{ return m_parentApp; }}

	public bool isDirty {set{ m_isDirty = value; }}
}
