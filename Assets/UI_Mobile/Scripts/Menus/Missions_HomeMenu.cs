using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missions_HomeMenu : BaseMenu {

	public Text
	m_appNameText,
	m_capacityText;

	public GameObject
	m_missionCellGO,
	m_noMissionsCellGO,
	m_cellDetailPanel,
	m_cellCostPanel,
	m_newHeader,
	m_spacer,
	m_cellActorSmall,
	m_cellSiteSmall,
	m_cellAsset;

	public Transform
	m_contentParent;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		m_appNameText.text = parentApp.Name;
		//		m_infoPanelToggle.AddObserver (this);
		//		m_infoPanelToggle.ToggleButtonClicked (0);
		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		base.OnEnter (animate);

		//		// slide in animation
		//		if (animate) {
		//
		//			RectTransform rt = gameObject.GetComponent<RectTransform> ();
		//			Rect r = rt.rect;
		//			rt.anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);
		//
		//			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, 0), 0.5f);
		//
		//			for (int i = 0; i < m_cells.Count; i++) {
		//
		//				UICell c = m_cells [i];
		//				c.m_rectTransforms[0].anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);
		//				DOTween.To (() => c.m_rectTransforms [0].anchoredPosition, x => c.m_rectTransforms [0].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetEase (Ease.OutCirc).SetDelay (0.25f + (i * 0.07f));
		//
		//				if (c.m_image != null) {
		//					c.m_image.transform.localScale = Vector3.zero;
		//					DOTween.To (() => c.m_image.transform.localScale, x => c.m_image.transform.localScale = x, new Vector3 (1, 1, 1), 0.5f).SetEase (Ease.OutCirc).SetDelay (0.75f + (i * 0.09f));
		//				}
		//			}
		//		} 
		//		else {
		//
		//			RectTransform rt = gameObject.GetComponent<RectTransform> ();
		//			rt.anchoredPosition = Vector2.zero;
		//
		//		}

		// display first time help popup if enabled

//		int helpEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.HelpEnabled);
//		int firstTimeEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.FirstTime_Missions);
//
//		if (helpEnabled == 1 && firstTimeEnabled == 1) {
//
//			string header = "Missions App";
//			string message = "See all Missions currently being carried out by your Henchmen, or start planning a new one.";
//
//			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
//			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
//			b2.onClick.AddListener (delegate {
//				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
//			});
//			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_Missions, 0);
//
//		} else if (helpEnabled == 0 && firstTimeEnabled == 1) {
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_Missions, 0);
//		}
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		// clear out new flags

		List<MissionPlan> missions = GameController.instance.GetMissions (0);

		foreach (MissionPlan mp in missions) {

			if (mp.m_new) {

				Action_SetMissionNewState newState = new Action_SetMissionNewState ();
				newState.m_plan = mp;
				newState.m_newState = false;
				GameController.instance.ProcessAction (newState);
			}
		}

		//		if (animate) {
		//			// slide out animation
		//			RectTransform rt = gameObject.GetComponent<RectTransform> ();
		//			Rect r = rt.rect;
		//
		//			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0), 0.5f);
		//
		//		} else {
		//
//		OnExitComplete ();
		//		}

	}

//	public void OnExitComplete ()
//	{
//
//		while (m_cells.Count > 0) {
//
//			UICell c = m_cells [0];
//			m_cells.RemoveAt (0);
//			Destroy (c.gameObject);
//		}
//
////		RectTransform rt = gameObject.GetComponent<RectTransform> ();
////		rt.anchoredPosition = Vector2.zero;
//
//		this.gameObject.SetActive (false);
//	}

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

	public void MissionButtonPressed (MissionPlan mp)
	{
//		((MissionsApp)(m_parentApp)).missionOverviewMenu.missionPlan = mp;
//		ParentApp.PushMenu (((MissionsApp)(m_parentApp)).missionOverviewMenu);

		((MissionsApp)(m_parentApp)).planMissionMenu.missionPlan = mp;
		ParentApp.PushMenu (((MissionsApp)(m_parentApp)).planMissionMenu);
	}

	public void CancelMission (MissionPlan mp)
	{
		Player player = GameController.instance.game.playerList [0];

		// display toast

		string title = "Mission Aborted";
		string message = "Mission: " + mp.m_currentMission.m_name + " is has been aborted.";

		player.notifications.AddNotification (GameController.instance.GetTurnNumber(), title, message, EventLocation.Missions, true, mp.m_missionID);
		player.RemoveMission (mp);

		ParentApp.PopMenu ();

		DisplayContent ();
	}

	public void CancelMissionButtonPressed (MissionPlan mp)
	{
		string header = "Abort Mission";
		string message = "Are you sure you want to abort this Mission?";

		MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
		Button b3 = MobileUIEngine.instance.alertDialogue.AddButton ("Abort");
		b3.onClick.AddListener(delegate { CancelMission(mp);});
		Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Cancel");
		b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
		m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
	}

	public void NewMissionButtonPressed ()
	{
		MissionPlan newPlan = new MissionPlan ();

		Lair lair = GameController.instance.GetLair (0);

		foreach (Lair.FloorSlot fSlot in lair.floorSlots) {

			newPlan.m_missionOptions.Add (fSlot);

//			if (fSlot.m_state != Lair.FloorSlot.FloorState.Empty && fSlot.m_state != Lair.FloorSlot.FloorState.MissionInProgress) {
//
//				foreach (Mission m in fSlot.m_floor.m_missions) {
//
//					newPlan.m_missionOptions.Add (m);
//				}
//			}
		}

		((MissionsApp)(m_parentApp)).planMissionMenu.missionPlan = newPlan;
		ParentApp.PushMenu (((MissionsApp)(m_parentApp)).planMissionMenu);
	}

	private void UpdateCapacity ()
	{
		Player player = GameController.instance.game.playerList [0];
		List<MissionPlan> missions = GameController.instance.GetMissions (0);

		int maxMissions = player.GetMaxMissions();
		int currentMissions = missions.Count;

		string s = "<b>Activity Capacity: " + currentMissions.ToString () + "/" + maxMissions.ToString () + "</b>";

		if (currentMissions > maxMissions) {

			int infamyPenalty = player.GetMaxMissionPenalty ();
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

		List<MissionPlan> missions = GameController.instance.GetMissions (0);

		if (missions.Count == 0) {

			GameObject noMissionsCellGO = (GameObject)Instantiate (m_noMissionsCellGO, m_contentParent);
			UICell noMissionsCell = (UICell)noMissionsCellGO.GetComponent<UICell> ();
			m_cells.Add (noMissionsCell);

		} else {

			foreach (MissionPlan mp in missions) {

				if (mp.m_new) {
					GameObject newCell = (GameObject)Instantiate (m_newHeader, m_contentParent);
					UICell nCell = (UICell)newCell.GetComponent<UICell> ();
					m_cells.Add (nCell);
				}

				GameObject missionCellGO = (GameObject)Instantiate (m_missionCellGO, m_contentParent);
				Cell_Mission missionCell = (Cell_Mission)missionCellGO.GetComponent<Cell_Mission> ();
				missionCell.SetMission (mp);
				m_cells.Add (missionCell);


				missionCell.m_buttons[0].gameObject.SetActive(false);
				missionCell.m_buttons[2].gameObject.SetActive(true);

				Button b = missionCell.m_buttons [2];
				b.onClick.AddListener (delegate {
					CancelMissionButtonPressed(mp);
				});

				// set detail panel - traits

				if (mp.m_currentMission.m_requiredTraits.Length > 0) {

					GameObject traitPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
					Cell_DetailPanel dPanel = (Cell_DetailPanel)traitPanel.GetComponent<Cell_DetailPanel> ();
					//						dPanel.SetTraits (m, currentHenchmenTraits);
					dPanel.SetTraits (mp, Cell_DetailPanel.MissionState.Planning);
					m_cells.Add (dPanel);
				}

				// set detail panel - assets

				if (mp.m_currentMission.m_requiredAssets.Length > 0) {

					GameObject assetPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
					Cell_DetailPanel aPanel = (Cell_DetailPanel)assetPanel.GetComponent<Cell_DetailPanel> ();
					aPanel.SetAssets (mp, Cell_DetailPanel.MissionState.Planning);
					m_cells.Add (aPanel);
				}

				// set cost panel

				GameObject costPanel = (GameObject)Instantiate (m_cellCostPanel, m_contentParent);
				Cell_CostPanel cPanel = (Cell_CostPanel)costPanel.GetComponent<Cell_CostPanel> ();
				cPanel.SetCostPanel (mp);
				m_cells.Add (cPanel);

				if (mp.m_missionSite != null && mp.m_currentAsset == null) {

					GameObject siteCell = (GameObject)Instantiate (m_cellSiteSmall, m_contentParent);
					Cell_Site sCell = (Cell_Site)siteCell.GetComponent<Cell_Site> ();
					sCell.SetSite (mp.m_missionSite);
					m_cells.Add (sCell);
				}

				if (mp.m_currentAsset != null) {

					GameObject assetCell = (GameObject)Instantiate (m_cellAsset, m_contentParent);
					Cell_Asset_Card aCell = (Cell_Asset_Card)assetCell.GetComponent<Cell_Asset_Card> ();
					aCell.SetAsset (mp.m_currentAsset);
					m_cells.Add (aCell);
				}

				// set participating henchmen

				foreach (Player.ActorSlot aSlot in mp.m_actorSlots) {

					GameObject henchmenPanel = (GameObject)Instantiate (m_cellActorSmall, m_contentParent);
					Cell_Actor_Small hPanel = (Cell_Actor_Small)henchmenPanel.GetComponent<Cell_Actor_Small> ();
					hPanel.SetActor (aSlot);
					m_cells.Add (hPanel);
				}

				GameObject spacerGO = (GameObject)Instantiate (m_spacer, m_contentParent);
				Cell_Spacer spacer = (Cell_Spacer)spacerGO.GetComponent<Cell_Spacer> ();
				m_cells.Add (spacer);

			}

			GameObject spacerGO3 = (GameObject)Instantiate (m_spacer, m_contentParent);
			Cell_Spacer spacer3 = (Cell_Spacer)spacerGO3.GetComponent<Cell_Spacer> ();
			spacer3.SetHeight (100);
			m_cells.Add (spacer3);

			LayoutRebuilder.ForceRebuildLayoutImmediate (m_contentParent.GetComponent<RectTransform>());
		}
	}
}
