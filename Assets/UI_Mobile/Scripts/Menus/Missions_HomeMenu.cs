using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missions_HomeMenu : BaseMenu {

	public Text
	m_appNameText;

	public GameObject
	m_missionCellGO,
	m_noMissionsCellGO;

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

		int helpEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.HelpEnabled);
		int firstTimeEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.FirstTime_Missions);

		if (helpEnabled == 1 && firstTimeEnabled == 1) {

			string header = "Missions App";
			string message = "See all Missions currently being carried out by your Henchmen, or start planning a new one.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener (delegate {
				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
			});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_Missions, 0);

		} else if (helpEnabled == 0 && firstTimeEnabled == 1) {

			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_Missions, 0);
		}
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
		OnExitComplete ();
		//		}

	}

	public void OnExitComplete ()
	{

		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

//		RectTransform rt = gameObject.GetComponent<RectTransform> ();
//		rt.anchoredPosition = Vector2.zero;

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

	public void MissionButtonPressed (MissionPlan mp)
	{
//		((MissionsApp)(m_parentApp)).missionOverviewMenu.missionPlan = mp;
//		ParentApp.PushMenu (((MissionsApp)(m_parentApp)).missionOverviewMenu);

		((MissionsApp)(m_parentApp)).planMissionMenu.missionPlan = mp;
		ParentApp.PushMenu (((MissionsApp)(m_parentApp)).planMissionMenu);
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

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		List<MissionPlan> missions = GameController.instance.GetMissions (0);

		if (missions.Count == 0) {

			GameObject noMissionsCellGO = (GameObject)Instantiate (m_noMissionsCellGO, m_contentParent);
			UICell noMissionsCell = (UICell)noMissionsCellGO.GetComponent<UICell> ();
			m_cells.Add (noMissionsCell);

		} else {

			foreach (MissionPlan mp in missions) {

				GameObject missionCellGO = (GameObject)Instantiate (m_missionCellGO, m_contentParent);
				Cell_Mission missionCell = (Cell_Mission)missionCellGO.GetComponent<Cell_Mission> ();
				missionCell.SetMission (mp);
				m_cells.Add (missionCell);

				Button b = missionCell.m_buttons [0];
				b.onClick.AddListener (delegate {
					MissionButtonPressed (mp);
				});

				LayoutRebuilder.ForceRebuildLayoutImmediate (m_contentParent.GetComponent<RectTransform>());
			}
		}
	}
}
