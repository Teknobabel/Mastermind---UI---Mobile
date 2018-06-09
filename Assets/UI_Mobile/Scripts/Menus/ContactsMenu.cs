using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContactsMenu : BaseMenu, IUIObserver {

	public enum DisplayType {
		Alpha,
		Trait,
		Mission,
	}

	public Text
	m_appNameText,
	m_capacityText;

	public GameObject
	m_henchmenCellGO,
	m_costPanel,
	m_cellDetailPanel,
	m_newHeader,
	m_spacer,
	m_henchmenCell_Compact,
	m_headerCellGO,
	m_emptyListCellGO;

	public Transform
	m_contentParent_Horizontal,
	m_contentParent_Vertical;

	public SegmentedToggle m_infoPanelToggle;

	private DisplayType m_displayType = DisplayType.Alpha;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		m_appNameText.text = parentApp.Name;
		m_infoPanelToggle.AddObserver (this);
		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		base.OnEnter (animate);


		// display first time help popup if enabled

//		int helpEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.HelpEnabled);
//		int firstTimeEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.FirstTime_Contacts);

//		if (helpEnabled == 1 && firstTimeEnabled == 1) {
//
//			string header = "Contacts App";
//			string message = "Any Henchmen you've hired will be displayed here. You can check their Traits, see which Missions they're on, or remove them from service.";
//
//			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
//			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
//			b2.onClick.AddListener (delegate {
//				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
//			});
//			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_Contacts, 0);
//
//		} else if (helpEnabled == 0 && firstTimeEnabled == 1) {
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_Contacts, 0);
//		}
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);
		// clear any remaining new flags

//		bool newStateChanged = false;

		List<Player.ActorSlot> hiringPool = GameController.instance.GetHiredHenchmen (0);

		foreach (Player.ActorSlot aSlot in hiringPool) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_new) {

				Action_SetActorNewState newState = new Action_SetActorNewState ();
				newState.m_newState = false;
				newState.m_actorSlot = aSlot;
				GameController.instance.ProcessAction (newState);
//				newStateChanged = true;
			}
		}

//		if (newStateChanged) {
//
//			m_parentApp.SetAlerts ();
//		}

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

	public override void OnHold ()
	{
		base.OnHold ();

		MobileUIEngine.instance.systemNavBar.SetBackButtonState (true);
	}

	public override void OnReturn ()
	{
		base.OnReturn ();

		MobileUIEngine.instance.systemNavBar.SetBackButtonState (false);
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
			DisplayContent ();
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

	public void FireButtonPressed (Player.ActorSlot aSlot)
	{
		Debug.Log ("Fire Henchmen Button Pressed");

		string header = "Fire Henchmen?";
		string message = "Fired Henchmen may return for hire in the future.";

		MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
		Button b = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
		b.onClick.AddListener(delegate { FireHenchmen(aSlot);});
		Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Cancel");
		b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
		m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

	}

	public void FireHenchmen (Player.ActorSlot aSlot)
	{
		Action_FireAgent fireAgent = new Action_FireAgent ();
		fireAgent.m_playerNumber = (0);
		fireAgent.m_henchmenID = aSlot.m_actor.id;
		GameController.instance.ProcessAction (fireAgent);

		DisplayContent ();

		if (MobileUIEngine.instance.alertDialogue.alertActive) {
			MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
		}
	}

	private void UpdateCapacity ()
	{
		Player player = GameController.instance.game.playerList [0];
		List<Player.ActorSlot> hiringPool = GameController.instance.GetHiredHenchmen (0);

		int maxContacts = player.GetMaxHenchmen();
		int currentlyHired = 0;

		foreach (Player.ActorSlot aSlot in hiringPool) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				currentlyHired++;
			}
		}

		string s = "<b>Contacts Capacity: " + currentlyHired.ToString () + "/" + maxContacts.ToString () + "</b>";

		if (currentlyHired > maxContacts) {

			int infamyPenalty = player.GetMaxHenchmenPenalty ();
			s += " (+" + infamyPenalty.ToString() + " Infamy each Turn)";
			m_capacityText.color = Color.red;
		} else {
			Color c = new Color(0.2509f,0.2509f,0.2509f,1);
			m_capacityText.color = c;
		}

		m_capacityText.text = s;
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		UpdateCapacity ();

		List<Player.ActorSlot> hiringPool = GameController.instance.GetHiredHenchmen (0);

		List<Player.ActorSlot> hList = new List<Player.ActorSlot> ();

		foreach (Player.ActorSlot aSlot in hiringPool) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				hList.Add (aSlot);
			}
		}

		int emptySlots = hiringPool.Count - hList.Count;

		switch (m_displayType)
		{
		case DisplayType.Alpha:

//			m_contentParent_Horizontal.parent.gameObject.SetActive (true);
//			m_contentParent_Vertical.parent.gameObject.SetActive (false);

			if (hList.Count == 0) {

				// display empty list message

				GameObject emptyCellGO = (GameObject)Instantiate (m_emptyListCellGO, m_contentParent_Vertical);
				UICell c = (UICell)emptyCellGO.GetComponent<UICell> ();
				m_cells.Add (c);

			}

			hList.Sort (delegate(Player.ActorSlot a, Player.ActorSlot b) {
				return a.m_actor.m_actorName.CompareTo (b.m_actor.m_actorName);
			});

			GameObject spacerGO = (GameObject)Instantiate (m_spacer, m_contentParent_Vertical);
			Cell_Spacer spacer = (Cell_Spacer)spacerGO.GetComponent<Cell_Spacer> ();
			m_cells.Add (spacer);

			foreach (Player.ActorSlot h in hList) {

				if (h.m_new) {
					GameObject newCell = (GameObject)Instantiate (m_newHeader, m_contentParent_Vertical);
					UICell nCell = (UICell)newCell.GetComponent<UICell> ();
					m_cells.Add (nCell);
				}

//				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contentParent_Horizontal);
				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contentParent_Vertical);
				Cell_Actor c = (Cell_Actor)hCell.GetComponent<Cell_Actor> ();
				m_cells.Add ((UICell)c);
				c.SetActor (h);

				// set detail panel - traits

				GameObject traitPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent_Vertical);
				Cell_DetailPanel dPanel = (Cell_DetailPanel)traitPanel.GetComponent<Cell_DetailPanel> ();
				dPanel.SetTraits (h);
				m_cells.Add (dPanel);

				// set cost panel

				GameObject costPanel = (GameObject)Instantiate (m_costPanel, m_contentParent_Vertical);
				Cell_CostPanel cPanel = (Cell_CostPanel)costPanel.GetComponent<Cell_CostPanel> ();
				cPanel.SetCostPanel (h);
				m_cells.Add (cPanel);

				c.m_buttons [0].gameObject.SetActive (false);
//				c.m_buttons[0].onClick.AddListener (delegate {
//					HenchmenCellClicked (h);
//				});

				c.m_buttons[1].onClick.AddListener (delegate {
					FireButtonPressed (h);
				});

				GameObject spacerGO2 = (GameObject)Instantiate (m_spacer, m_contentParent_Vertical);
				Cell_Spacer spacer2 = (Cell_Spacer)spacerGO2.GetComponent<Cell_Spacer> ();
				m_cells.Add (spacer2);
			}

			GameObject spacerGO3 = (GameObject)Instantiate (m_spacer, m_contentParent_Vertical);
			Cell_Spacer spacer3 = (Cell_Spacer)spacerGO3.GetComponent<Cell_Spacer> ();
			spacer3.SetHeight (100);
			m_cells.Add (spacer3);

//			for (int i = 0; i < emptySlots; i++) {
//
//				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
//				UICell c = (UICell)hCell.GetComponent<UICell> ();
//				m_cells.Add (c);
//
//				string nameString = "EMPTY";
//				string statusString = "Recruit henchmen in the Hire app";
//
//				c.m_headerText.text = nameString;
//				c.m_headerText.color = Color.gray;
//				c.m_bodyText.text = statusString;
//				c.m_bodyText.color = Color.gray;
//			}



			break;

		case DisplayType.Trait:

//			m_contentParent_Horizontal.parent.gameObject.SetActive (false);
//			m_contentParent_Vertical.parent.gameObject.SetActive (true);

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
					GameObject header = (GameObject)Instantiate (m_headerCellGO, m_contentParent_Vertical);
					UICell headerCell = (UICell)header.GetComponent<UICell> ();
					headerCell.m_headerText.text = entry.Key.ToString ();
					m_cells.Add (headerCell);

					List<Player.ActorSlot> sortedList = entry.Value;

					sortedList.Sort (delegate(Player.ActorSlot a, Player.ActorSlot b) {
						return a.m_actor.m_actorName.CompareTo (b.m_actor.m_actorName);
					});

					foreach (Player.ActorSlot aSlot in sortedList) {

						GameObject hCell = (GameObject)Instantiate (m_henchmenCell_Compact, m_contentParent_Vertical);
						Cell_Actor c = (Cell_Actor)hCell.GetComponent<Cell_Actor> ();
						m_cells.Add ((UICell)c);
						c.SetActor (aSlot);

						hCell.GetComponent<Button> ().onClick.AddListener (delegate {
							HenchmenCellClicked (aSlot);
						});
					}
				}
			}
			
			break;

		case DisplayType.Mission:

//			m_contentParent_Horizontal.parent.gameObject.SetActive (false);
//			m_contentParent_Vertical.parent.gameObject.SetActive (true);

			List<MissionPlan> missions = GameController.instance.GetMissions (0);

			foreach (MissionPlan mp in missions) {

				GameObject header = (GameObject)Instantiate (m_headerCellGO, m_contentParent_Vertical);
				UICell headerCell = (UICell)header.GetComponent<UICell> ();
				headerCell.m_headerText.text = mp.m_currentMission.m_name;
				m_cells.Add (headerCell);

				foreach (Player.ActorSlot aSlot in mp.m_actorSlots) {

					if (hList.Contains (aSlot)) {

						hList.Remove (aSlot);
					}

					GameObject hCell = (GameObject)Instantiate (m_henchmenCell_Compact, m_contentParent_Vertical);
					Cell_Actor c = (Cell_Actor)hCell.GetComponent<Cell_Actor> ();
					m_cells.Add ((UICell)c);
					c.SetActor (aSlot);

					hCell.GetComponent<Button> ().onClick.AddListener (delegate {
						HenchmenCellClicked (aSlot);
					});

				}
			}

			if (hList.Count > 0) {

				hList.Sort (delegate(Player.ActorSlot a, Player.ActorSlot b) {
					return a.m_actor.m_actorName.CompareTo (b.m_actor.m_actorName);
				});

				GameObject header = (GameObject)Instantiate (m_headerCellGO, m_contentParent_Vertical);
				UICell headerCell = (UICell)header.GetComponent<UICell> ();
				headerCell.m_headerText.text = "Unassigned";
				m_cells.Add (headerCell);

				foreach (Player.ActorSlot aSlot in hList) {

					GameObject hCell = (GameObject)Instantiate (m_henchmenCell_Compact, m_contentParent_Vertical);
					Cell_Actor c = (Cell_Actor)hCell.GetComponent<Cell_Actor> ();
					m_cells.Add ((UICell)c);
					c.SetActor (aSlot);

					hCell.GetComponent<Button> ().onClick.AddListener (delegate {
						HenchmenCellClicked (aSlot);
					});
				}
			}

			break;
		}
	}
}
