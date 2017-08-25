﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hire_HomeMenu : MonoBehaviour, IMenu, IUIObserver {

	public enum DisplayType {
		New,
		Alpha,
		Trait,
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
//		m_infoPanelToggle.ToggleButtonClicked (0); // find some other way to set initial toggle state
		this.gameObject.SetActive (false);
	}

	public void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

//		MobileUIEngine.instance.systemNavBar.SetBackButtonState (false);

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

		List<Player.ActorSlot> hiringPool = GameController.instance.GetHiringPool (0);

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

		m_isDirty = false;

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
		MobileUIEngine.instance.systemNavBar.SetBackButtonState (true);
	}

	public void OnReturn ()
	{
		MobileUIEngine.instance.systemNavBar.SetBackButtonState (false);

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
				m_displayType = DisplayType.New;
				break;
			case 1:
				m_displayType = DisplayType.Alpha;
				break;
			case 2:
				m_displayType = DisplayType.Trait;
				break;

			}
			DisplayHenchmen ();
			break;
		}
	}

	public void HenchmenCellClicked (Player.ActorSlot aSlot)
	{
//		Debug.Log("Henchmen Cell w id: " + henchmenID + " clicked");

		// clear new state
//
//		if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_new) {
//
//			Action_SetActorNewState newState = new Action_SetActorNewState ();
//			newState.m_newState = false;
//			newState.m_actorSlot = aSlot;
//			GameController.instance.ProcessAction (newState);
//			m_parentApp.SetAlerts ();
//			m_isDirty = true;
//
//		}

		((HireApp)(m_parentApp)).detailMenu.SetHenchmen(aSlot.m_actor.id);

		m_parentApp.PushMenu (((HireApp)(m_parentApp)).detailMenu);
	}

	private void DisplayHenchmen ()
	{
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		List<Player.ActorSlot> hiringPool = GameController.instance.GetHiringPool (0);

		List<Player.ActorSlot> hList = new List<Player.ActorSlot> ();

		foreach (Player.ActorSlot aSlot in hiringPool) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				hList.Add (aSlot);
			}
		}

		int emptySlots = hiringPool.Count - hList.Count;

		switch (m_displayType)
		{
		case DisplayType.New:

			List<Player.ActorSlot> newHenchmenList = new List<Player.ActorSlot> ();

			foreach (Player.ActorSlot aSlot in hiringPool) {

				if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_new) {

					newHenchmenList.Add (aSlot);
				}
			}

			if (newHenchmenList.Count > 1) {

				newHenchmenList.Sort (delegate(Player.ActorSlot a, Player.ActorSlot b) {
					return a.m_actor.m_actorName.CompareTo (b.m_actor.m_actorName);
				});
			}

			foreach (Player.ActorSlot aSlot in newHenchmenList) {

				Actor h = aSlot.m_actor;

				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
				UICell c = (UICell)hCell.GetComponent<UICell> ();
				m_cells.Add (c);

				string nameString = h.m_actorName;
//				string statusString = "Status: " + h.m_status.m_name;

				string statusString = "";

				switch (aSlot.m_actor.m_rank) {

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

				if (aSlot.m_actor.traits.Count > 0) {

					Trait t = aSlot.m_actor.traits [0];
					statusString += t.m_name;
				}

				c.m_headerText.text = nameString;
				c.m_bodyText.text = statusString;
				c.m_image.texture = h.m_portrait_Compact;

				if (aSlot.m_new) {
					c.m_rectTransforms [1].gameObject.SetActive (true);
				}


				hCell.GetComponent<Button> ().onClick.AddListener (delegate {
					HenchmenCellClicked (aSlot);
				});
			}

			for (int i = 0; i < emptySlots; i++) {

				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
				UICell c = (UICell)hCell.GetComponent<UICell> ();
				m_cells.Add (c);

				string nameString = "EMPTY";
				string statusString = "";

				c.m_headerText.text = nameString;
				c.m_headerText.color = Color.gray;
				c.m_bodyText.text = statusString;
			}

			break;
		case DisplayType.Alpha:

			hList.Sort (delegate(Player.ActorSlot a, Player.ActorSlot b) {
				return a.m_actor.m_actorName.CompareTo (b.m_actor.m_actorName);
			});

			foreach (Player.ActorSlot aSlot in hList) {

				Actor h = aSlot.m_actor;

				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
				UICell c = (UICell)hCell.GetComponent<UICell> ();
				m_cells.Add (c);

				string nameString = h.m_actorName;
//				string statusString = "Status: " + h.m_status.m_name;

				string statusString = "";

				switch (aSlot.m_actor.m_rank) {

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

				if (aSlot.m_actor.traits.Count > 0) {

					Trait t = aSlot.m_actor.traits [0];
					statusString += t.m_name;
				}

				c.m_headerText.text = nameString;
				c.m_bodyText.text = statusString;
				c.m_image.texture = h.m_portrait_Compact;

				if (aSlot.m_new) {
					c.m_rectTransforms [1].gameObject.SetActive (true);
				}

				hCell.GetComponent<Button> ().onClick.AddListener (delegate {
					HenchmenCellClicked (aSlot);
				});
			}

			for (int i = 0; i < emptySlots; i++) {

				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
				UICell c = (UICell)hCell.GetComponent<UICell> ();
				m_cells.Add (c);

				string nameString = "EMPTY";
				string statusString = "";

				c.m_headerText.text = nameString;
				c.m_headerText.color = Color.gray;
				c.m_bodyText.text = statusString;
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

					foreach (Player.ActorSlot aSlot in sortedList) {

						Actor h = aSlot.m_actor;
							
						GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
						UICell c = (UICell)hCell.GetComponent<UICell> ();
						m_cells.Add (c);

						string nameString = h.m_actorName;
//						string statusString = "Status: " + h.m_status.m_name;

						string statusString = "";

						switch (aSlot.m_actor.m_rank) {

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

						if (aSlot.m_actor.traits.Count > 0) {

							Trait t = aSlot.m_actor.traits [0];
							statusString += t.m_name;
						}

						c.m_headerText.text = nameString;
						c.m_bodyText.text = statusString;
						c.m_image.texture = h.m_portrait_Compact;

						if (aSlot.m_new) {
							c.m_rectTransforms [1].gameObject.SetActive (true);
						}

						hCell.GetComponent<Button> ().onClick.AddListener (delegate {
							HenchmenCellClicked (aSlot);
						});
					}
				}
			}

			break;
		}
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}

	public bool isDirty {get{ return m_isDirty; }set{ m_isDirty = value; }}
}
