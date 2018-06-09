using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hire_HomeMenu : BaseMenu, IUIObserver {

	public enum DisplayType {
		New,
		Alpha,
		Trait,
	}

	public Text
	m_appNameText,
	m_capacityText;

	public GameObject
	m_henchmenCellGO,
	m_costPanel,
	m_cellDetailPanel,
	m_henchmenCell_CompactGO,
	m_headerCellGO,
	m_assetCellGO,
	m_emptyListCellGO,
	m_newHeader,
	m_spacer;

	public Transform
	m_contentParent_Horizontal,
	m_contentParent_Vertical;

	public Trait[] m_skills;

	public SegmentedToggle m_infoPanelToggle;

	private DisplayType m_displayType = DisplayType.Alpha;

	private List<Trait> m_currentlySeekingSkills = new List<Trait>();
	private bool m_currentlySeekingChanged = false;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		m_appNameText.text = parentApp.Name;
		m_infoPanelToggle.AddObserver (this);
		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
//		animate = false; //debug
		this.gameObject.SetActive (true);

		base.OnEnter (animate);

		// update seeking skills

		Player player = GameEngine.instance.game.playerList [0];
		m_currentlySeekingSkills = player.hiringPool.m_seekingSkills;

		// display first time help popup if enabled

//		int helpEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.HelpEnabled);
//		int firstTimeEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.FirstTime_Hire);
//
//		if (helpEnabled == 1 && firstTimeEnabled == 1) {
//
//			string header = "Hire App";
//			string message = "Hire Henchmen to carry out your Missions. You can also specify the type of Henchmen you'd like to recruit. Each Henchmen you have employed will reduce your available Command Pool.";
//
//			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
//			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
//			b2.onClick.AddListener (delegate {
//				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
//			});
//			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_Hire, 0);
//
//		} else if (helpEnabled == 0 && firstTimeEnabled == 1) {
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_Hire, 0);
//		}
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		// update currently seeking skills if necessary

		if (m_currentlySeekingChanged) {

			Action_UpdateSeekingSkills updateSeeking = new Action_UpdateSeekingSkills ();
			updateSeeking.m_playerID = 0;
			updateSeeking.m_seekingSkills = m_currentlySeekingSkills;
			GameController.instance.ProcessAction (updateSeeking);
		}

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
//			case 0:
//				m_displayType = DisplayType.New;
//				break;
			case 0:
				m_displayType = DisplayType.Alpha;
				break;
			case 1:
				m_displayType = DisplayType.Trait;
				break;

			}
			DisplayContent ();
			break;
		}
	}

	public void HenchmenCellClicked (Player.ActorSlot aSlot)
	{
		((HireApp)(m_parentApp)).detailMenu.SetHenchmen(aSlot.m_actor.id);

		m_parentApp.PushMenu (((HireApp)(m_parentApp)).detailMenu);
	}

	private void UpdateCapacity ()
	{
		Player player = GameController.instance.game.playerList [0];
		List<Player.ActorSlot> hiringPool = GameController.instance.GetHiringPool (0);

		int maxHireable = player.GetMaxHireableHenchmen();
		int currentlyHireable = 0;

		foreach (Player.ActorSlot aSlot in hiringPool) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				currentlyHireable++;
			}
		}

		string s = "<b>Hire Capacity: " + currentlyHireable.ToString () + "/" + maxHireable.ToString () + "</b>";
		m_capacityText.text = s;
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		UpdateCapacity ();

		List<Player.ActorSlot> hiringPool = GameController.instance.GetHiringPool (0);

		List<Player.ActorSlot> hList = new List<Player.ActorSlot> ();

		foreach (Player.ActorSlot aSlot in hiringPool) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				hList.Add (aSlot);
			}
		}

		if (hList.Count == 0) {

//			GameObject emptyCellGO = (GameObject)Instantiate (m_emptyListCellGO, m_contentParent_Horizontal);
			GameObject emptyCellGO = (GameObject)Instantiate (m_emptyListCellGO, m_contentParent_Vertical);
			UICell c = (UICell)emptyCellGO.GetComponent<UICell> ();
			m_cells.Add (c);
		}

		int emptySlots = hiringPool.Count - hList.Count;

		switch (m_displayType)
		{
		case DisplayType.New:

//			List<Player.ActorSlot> newHenchmenList = new List<Player.ActorSlot> ();
//
//			foreach (Player.ActorSlot aSlot in hiringPool) {
//
//				if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_new) {
//
//					newHenchmenList.Add (aSlot);
//				}
//			}
//
//			if (newHenchmenList.Count > 1) {
//
//				newHenchmenList.Sort (delegate(Player.ActorSlot a, Player.ActorSlot b) {
//					return a.m_actor.m_actorName.CompareTo (b.m_actor.m_actorName);
//				});
//			}
//
//			foreach (Player.ActorSlot aSlot in newHenchmenList) {
//
//				Actor h = aSlot.m_actor;
//
//				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
//				Cell_Actor c = (Cell_Actor)hCell.GetComponent<Cell_Actor> ();
//				c.SetActor (aSlot);
//				m_cells.Add (c);
//
//				hCell.GetComponent<Button> ().onClick.AddListener (delegate {
//					HenchmenCellClicked (aSlot);
//				});
//			}

//			for (int i = 0; i < emptySlots; i++) {
//
//				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
//				UICell c = (UICell)hCell.GetComponent<UICell> ();
//				m_cells.Add (c);
//
//				string nameString = "EMPTY";
//				string statusString = "";
//
//				c.m_headerText.text = nameString;
//				c.m_headerText.color = Color.gray;
//				c.m_bodyText.text = statusString;
//			}

			break;
		case DisplayType.Alpha:

//			m_contentParent_Horizontal.parent.gameObject.SetActive (true);
//			m_contentParent_Vertical.parent.gameObject.SetActive (false);

			GameObject spacerGO = (GameObject)Instantiate (m_spacer, m_contentParent_Vertical);
			Cell_Spacer spacer = (Cell_Spacer)spacerGO.GetComponent<Cell_Spacer> ();
			m_cells.Add (spacer);

			hList.Sort (delegate(Player.ActorSlot a, Player.ActorSlot b) {
				return a.m_actor.m_actorName.CompareTo (b.m_actor.m_actorName);
			});

			foreach (Player.ActorSlot aSlot in hList) {

				Actor h = aSlot.m_actor;

				if (aSlot.m_new) {
					GameObject newCell = (GameObject)Instantiate (m_newHeader, m_contentParent_Vertical);
					UICell nCell = (UICell)newCell.GetComponent<UICell> ();
					m_cells.Add (nCell);
				}

//				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contentParent_Horizontal);
				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contentParent_Vertical);
				Cell_Actor c = (Cell_Actor)hCell.GetComponent<Cell_Actor> ();
				c.SetActor (aSlot);
				c.DisplayPrice (h.m_turnCost);
				m_cells.Add (c);

				// set detail panel - traits

				GameObject traitPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent_Vertical);
				Cell_DetailPanel dPanel = (Cell_DetailPanel)traitPanel.GetComponent<Cell_DetailPanel> ();
				dPanel.SetTraits (aSlot);
				m_cells.Add (dPanel);

				// set cost panel

				GameObject costPanel = (GameObject)Instantiate (m_costPanel, m_contentParent_Vertical);
				Cell_CostPanel cPanel = (Cell_CostPanel)costPanel.GetComponent<Cell_CostPanel> ();
				cPanel.SetCostPanel (aSlot);
				m_cells.Add (cPanel);

				// button for actor detail

//				c.m_buttons[0].onClick.AddListener (delegate {
//					HenchmenCellClicked (aSlot);
//				});

				// button to hire henchmen
				c.DisplaySelectButton ();
				c.m_buttons [0].onClick.AddListener (delegate {
					HireButtonPressed (aSlot);
				});

				// button to dismiss henchmen
				c.m_buttons [1].onClick.AddListener (delegate {
					DismissButtonPressed (aSlot);
				});

				// spacer

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
//				string statusString = "";
//
//				c.m_headerText.text = nameString;
//				c.m_headerText.color = Color.gray;
//				c.m_bodyText.text = statusString;
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

						Actor h = aSlot.m_actor;
							
						GameObject hCell = (GameObject)Instantiate (m_henchmenCell_CompactGO, m_contentParent_Vertical);
						Cell_Actor c = (Cell_Actor)hCell.GetComponent<Cell_Actor> ();
						c.SetActor (aSlot);
						m_cells.Add (c);

						hCell.GetComponent<Button> ().onClick.AddListener (delegate {
							HenchmenCellClicked (aSlot);
						});
					}
				}
			}

			break;
		}

		// display looking to hire menu

//		GameObject lookingToHireHeaderGO = (GameObject)Instantiate (m_headerCellGO, m_contactsListParent);
//		Cell_Header lookingToHireCell = (Cell_Header)lookingToHireHeaderGO.GetComponent<Cell_Header> ();
//		lookingToHireCell.SetHeader("I WANT TO HIRE:");
//		m_cells.Add (lookingToHireCell);
//
//		foreach (Trait t in m_skills) {
//
//			GameObject skillTraitGO = (GameObject)Instantiate (m_assetCellGO, m_contactsListParent);
//			UICell skillTraitCell = (UICell)skillTraitGO.GetComponent<UICell> ();
//			skillTraitCell.m_headerText.text = t.m_name;
//			m_cells.Add (skillTraitCell);
//
//			if (m_currentlySeekingSkills.Contains (t)) {
//
//				skillTraitCell.m_headerText.color = Color.green;
////				skillTraitCell.m_images [0].color = Color.blue;
////				skillTraitCell.m_headerText.color = Color.white;
//			}
//
//			skillTraitCell.m_buttons[0].onClick.AddListener (delegate {
//				SkillTypeTapped (t);
//			});
//		}
	}

	private void SkillTypeTapped (Trait t)
	{
		if (!m_currentlySeekingSkills.Contains (t)) {

			m_currentlySeekingSkills.Add (t);
		} else {
			m_currentlySeekingSkills.Remove (t);
		}

		m_currentlySeekingChanged = true;
		DisplayContent ();
	}

	public void HireButtonPressed (Player.ActorSlot aSlot)
	{
		Player.CommandPool cp = GameController.instance.GetCommandPool (0);
		Actor h = aSlot.m_actor;

		// check for open henchmen slot

		bool vacancy = false;

		List<Player.ActorSlot> pool = GameController.instance.GetHiredHenchmen (0);

		foreach (Player.ActorSlot eSlot in pool) {

			if (eSlot.m_state == Player.ActorSlot.ActorSlotState.Empty) {

				vacancy = true;
				break;
			}
		}


		if (!vacancy) {

			string header = "No Space Available";
			string message = "There is no room for an additional Henchmen. Fire a Henchmen or expand your Lair and try again.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
		}

		// can player afford the actor

		if (cp.m_currentPool >= h.m_startingCost && vacancy) {

			string header = "Hire Henchmen";
			string message = "This Henchmen will generate " + h.m_turnCost.ToString() + " Infamy each turn they are employed.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b.onClick.AddListener(delegate { HireHenchmen(aSlot);});
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Cancel");
			b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

//			Action_HireAgent hireAction = new Action_HireAgent ();
//			hireAction.m_playerNumber = 0;
//			hireAction.m_henchmenID = h.id;
//			GameController.instance.ProcessAction (hireAction);
//
//			Action_SpendCommandPoints payForHenchmen = new Action_SpendCommandPoints ();
//			payForHenchmen.m_amount = h.m_startingCost;
//			payForHenchmen.m_playerID = 0;
//			GameController.instance.ProcessAction (payForHenchmen);
//
//			DisplayContent ();

		} else if (h.m_startingCost > cp.m_currentPool) {

			string header = "Can't Afford Henchmen";
			string message = "There aren't enough points in your Command Pool to hire this Henchmen.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
		}
	}

	public void HireHenchmen (Player.ActorSlot aSlot)
	{
		Action_SpendCommandPoints payForHenchmen = new Action_SpendCommandPoints ();
		payForHenchmen.m_amount = aSlot.m_actor.m_startingCost;
		payForHenchmen.m_playerID = 0;
		GameController.instance.ProcessAction (payForHenchmen);

		Action_HireAgent hireAction = new Action_HireAgent ();
		hireAction.m_playerNumber = 0;
		hireAction.m_henchmenID = aSlot.m_actor.id;
		GameController.instance.ProcessAction (hireAction);

		DisplayContent ();

		if (MobileUIEngine.instance.alertDialogue.alertActive) {
			MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
		}
	}

	public void DismissButtonPressed (Player.ActorSlot aSlot)
	{
		Debug.Log ("Dismiss Henchmen Button Pressed");

		string header = "Dismiss Henchmen?";
		string message = "Dismissed Henchmen may return for hire in the future.";

		MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
		Button b = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
		b.onClick.AddListener(delegate { DismissHenchmen(aSlot);});
		Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Cancel");
		b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
		m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

	}

	public void DismissHenchmen (Player.ActorSlot aSlot)
	{
		Action_RemoveHireable dismissHenchmen = new Action_RemoveHireable ();
		dismissHenchmen.m_playerID = 0;
		dismissHenchmen.m_actorID = aSlot.m_actor.id;
		dismissHenchmen.m_wasDismissed = true;
		GameController.instance.ProcessAction (dismissHenchmen);

		DisplayContent ();

		if (MobileUIEngine.instance.alertDialogue.alertActive) {
			MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
		}
	}
}
