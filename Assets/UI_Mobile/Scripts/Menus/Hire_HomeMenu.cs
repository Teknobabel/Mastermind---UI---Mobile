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
	m_appNameText;

	public GameObject
	m_henchmenCellGO,
	m_headerCellGO;

	public Transform
	m_contactsListParent;

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
		base.OnEnter (animate);

		this.gameObject.SetActive (true);

		// update seeking skills

		Player player = GameEngine.instance.game.playerList [0];
		m_currentlySeekingSkills = player.hiringPool.m_seekingSkills;

		DisplayContent ();

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
			DisplayContent ();
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

	public override void DisplayContent ()
	{
		base.DisplayContent ();

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

		// display looking to hire menu

		GameObject lookingToHireHeaderGO = (GameObject)Instantiate (m_headerCellGO, m_contactsListParent);
		UICell lookingToHireCell = (UICell)lookingToHireHeaderGO.GetComponent<UICell> ();
		lookingToHireCell.m_headerText.text = "I WANT TO HIRE:";
		m_cells.Add (lookingToHireCell);

		foreach (Trait t in m_skills) {

			GameObject skillTraitGO = (GameObject)Instantiate (m_headerCellGO, m_contactsListParent);
			UICell skillTraitCell = (UICell)skillTraitGO.GetComponent<UICell> ();
			skillTraitCell.m_headerText.text = t.m_name;
			m_cells.Add (skillTraitCell);

			if (m_currentlySeekingSkills.Contains (t)) {

				skillTraitCell.m_images [0].color = Color.blue;
				skillTraitCell.m_headerText.color = Color.white;
			}

			skillTraitCell.m_buttons[0].onClick.AddListener (delegate {
				SkillTypeTapped (t);
			});
		}
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
}
