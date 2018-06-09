using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lair_HomeMenu : BaseMenu {

	public Text
	m_appNameText;

	public GameObject
	m_floorCellGO,
	m_buildFacilityCellGO,
	m_missionCell,
	m_newHeader,
	m_spacer;

	public Transform
		m_contentParent;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		m_appNameText.text = parentApp.Name;
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
//		int firstTimeEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.FirstTime_Lair);

//		if (helpEnabled == 1 && firstTimeEnabled == 1) {
//
//			string header = "Lair App";
//			string message = "Each Facility in your Lair gives you access to Missions. Build more Facilities to unlock new Missions, and hire Henchmen to carry them out.";
//
//			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
//			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
//			b2.onClick.AddListener (delegate {
//				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
//			});
//			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_Lair, 0);
//
//		} else if (helpEnabled == 0 && firstTimeEnabled == 1) {
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_Lair, 0);
//		}
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		// clear out new flags

		Lair l = GameController.instance.GetLair (0);

		foreach (Lair.FloorSlot fSlot in l.floorSlots) {

			if (fSlot.m_new && fSlot.m_state != Lair.FloorSlot.FloorState.Empty) {

				Action_SetFloorNewState newState = new Action_SetFloorNewState ();
				newState.m_floorSlot = fSlot;
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

		RectTransform rt = gameObject.GetComponent<RectTransform> ();
		rt.anchoredPosition = Vector2.zero;

		this.gameObject.SetActive (false);
		m_isDirty = false;
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

	public void FloorButtonClicked (int floorSlotID)
	{
		Debug.Log ("Idle floor clicked, start mission planning");

		Lair l = GameController.instance.GetLair (0);

		foreach (Lair.FloorSlot fSlot in l.floorSlots) {

			if (fSlot.m_id == floorSlotID) {

				fSlot.m_missionPlan.m_missionOptions.Clear ();
				fSlot.m_missionPlan.m_missionOptions.Add (fSlot);
				fSlot.m_missionPlan.m_displayAdvancedFloors = true;

//				fSlot.m_missionPlan.m_missionOptions.Clear ();
//
//				foreach (Mission m in fSlot.m_floor.m_missions) {
//
//					fSlot.m_missionPlan.m_missionOptions.Add (m);
//				}

				fSlot.m_missionPlan.m_maxActorSlots = fSlot.m_numActorSlots;

				((LairApp)(m_parentApp)).planMissionMenu.missionPlan = fSlot.m_missionPlan;
				m_parentApp.PushMenu (((LairApp)(m_parentApp)).planMissionMenu);
				break;
			}
		}
	}

	public void BuildNewFloorButtonClicked ()
	{
		Lair l = GameController.instance.GetLair (0);

		// make sure there are valid options before continuing

		bool validOptions = false;

		// get unlocked floors

		List<Floor> unlockedFloors = new List<Floor> ();
		List<Floor> builtFloors = new List<Floor> ();

		foreach (Mission m in l.unlockedFacilities) {

			unlockedFloors.Add (((Mission_BuildFloor)m).m_floor);
		}

		// compare against current floors and see if any remain

		foreach (Lair.FloorSlot fSlot in l.floorSlots) {

			if (fSlot.m_state != Lair.FloorSlot.FloorState.Empty) {

				builtFloors.Add (fSlot.m_floor);
			}
		}

		foreach (Floor f in builtFloors) {

			if (unlockedFloors.Contains (f)) {

				unlockedFloors.Remove (f);
			}
		}

		if (unlockedFloors.Count > 0) {

			validOptions = true;
		}

		if (!validOptions) {

			// show message

			string header = "No New Facilities";
			string message = "Upgrade your current Facilities to unlock access to new ones.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener (delegate {
				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
			});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

		}
		else if (l.builder.m_missionPlan.m_state != MissionPlan.State.Active) {
			
			l.builder.m_missionPlan.m_missionOptions.Clear ();
			l.builder.m_missionPlan.m_missionOptions.Add (l.builder);
			l.builder.m_missionPlan.m_maxActorSlots = l.builder.m_numActorSlots;

			((LairApp)(m_parentApp)).planMissionMenu.missionPlan = l.builder.m_missionPlan;
			m_parentApp.PushMenu (((LairApp)(m_parentApp)).planMissionMenu);


		} 

	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		Lair l = GameController.instance.GetLair (0);

		List<Floor> builtFloors = new List<Floor> ();

//		for (int i = 0; i < l.maxFloors; i++) {

		GameObject spacerGO = (GameObject)Instantiate (m_spacer, m_contentParent);
		Cell_Spacer spacer = (Cell_Spacer)spacerGO.GetComponent<Cell_Spacer> ();
		m_cells.Add (spacer);

		foreach (Lair.FloorSlot fSlot in l.floorSlots) {

			if (fSlot.m_new) {
				GameObject newCell = (GameObject)Instantiate (m_newHeader, m_contentParent);
				UICell nCell = (UICell)newCell.GetComponent<UICell> ();
				m_cells.Add (nCell);
			}

				GameObject floorGO = (GameObject)Instantiate (m_floorCellGO, m_contentParent);
				Cell_LairFloor floorCell = (Cell_LairFloor)floorGO.GetComponent<Cell_LairFloor> ();
				floorCell.SetFloor (fSlot);
				m_cells.Add ((UICell)floorCell);
			builtFloors.Add (fSlot.m_floor);

				floorCell.m_buttons [0].onClick.AddListener (delegate {
					FloorButtonClicked (fSlot.m_id);
				});

			GameObject spacerGO2 = (GameObject)Instantiate (m_spacer, m_contentParent);
			Cell_Spacer spacer2 = (Cell_Spacer)spacerGO2.GetComponent<Cell_Spacer> ();
			m_cells.Add (spacer2);

			} 
//		}

		List<Floor> unlockedFloors = new List<Floor> ();
		foreach (Mission m in l.unlockedFacilities) {

			Mission_BuildFloor bf = (Mission_BuildFloor)m;
			unlockedFloors.Add (bf.m_floor);
		}

		foreach (Floor f in builtFloors) {

			if (unlockedFloors.Contains (f)) {

				unlockedFloors.Remove (f);
			}
		}

//		GameObject buildFacilityGO = (GameObject)Instantiate (m_buildFacilityCellGO, m_contentParent);
//		UICell buildFacilityCell = (UICell)buildFacilityGO.GetComponent<UICell> ();
//		buildFacilityCell.m_bodyText.text = unlockedFloors.Count.ToString () + " Facilities Available";
//		m_cells.Add (buildFacilityCell);
//
//		buildFacilityCell.m_buttons [0].onClick.AddListener (delegate {
//			BuildNewFloorButtonClicked ();
//		});

		GameObject spacerGO3 = (GameObject)Instantiate (m_spacer, m_contentParent);
		Cell_Spacer spacer3 = (Cell_Spacer)spacerGO3.GetComponent<Cell_Spacer> ();
		spacer3.SetHeight (200);
		m_cells.Add (spacer3);

		LayoutRebuilder.ForceRebuildLayoutImmediate (m_contentParent.GetComponent<RectTransform>());
	}

//	public override void DisplayContent ()
//	{
//		base.DisplayContent ();
//
//		Lair l = GameController.instance.GetLair (0);
//
//		for (int i = 0; i < l.maxFloors; i++) {
//
//			if (i < l.floorSlots.Count) {
//
//				Lair.FloorSlot fSlot = l.floorSlots [i];
//
//				GameObject floorGO = (GameObject)Instantiate (m_floorCellGO, m_contentParent);
//				Cell_LairFloor floorCell = (Cell_LairFloor)floorGO.GetComponent<Cell_LairFloor> ();
//				floorCell.SetFloor (fSlot);
//				m_cells.Add ((UICell)floorCell);
//
//				floorCell.m_buttons [0].onClick.AddListener (delegate {
//					FloorButtonClicked (fSlot.m_id);
//				});
//
//			} else if (i == l.floorSlots.Count && i < l.maxFloors) {
//				
//				if (l.builder.m_missionPlan.m_state != MissionPlan.State.Active) {
//					
//					GameObject buildFacilityGO = (GameObject)Instantiate (m_floorCellGO, m_contentParent);
//					Cell_LairFloor buildFacilityCell = (Cell_LairFloor)buildFacilityGO.GetComponent<Cell_LairFloor> ();
//					buildFacilityCell.SetFloor (l.builder);
//					m_cells.Add (buildFacilityCell);
//
//					buildFacilityCell.m_buttons [0].onClick.AddListener (delegate {
//						BuildNewFloorButtonClicked ();
//					});
//
//				} else {
//
//					GameObject buildMissionGO = (GameObject)Instantiate (m_missionCell, m_contentParent);
//					Cell_Mission buildMissionCell = (Cell_Mission)buildMissionGO.GetComponent<Cell_Mission> ();
//					buildMissionCell.SetMission (l.builder.m_missionPlan);
//					m_cells.Add ((UICell)buildMissionCell);
//
//					buildMissionCell.m_buttons [0].onClick.AddListener (delegate {
//						BuildNewFloorButtonClicked ();
//					});
//				}
//
//			} else {
//
////				GameObject floorGO = (GameObject)Instantiate (m_floorCellGO, m_contentParent);
////				Cell_LairFloor floorCell = (Cell_LairFloor)floorGO.GetComponent<Cell_LairFloor> ();
////				floorCell.m_headerText.text = "Empty";
////				floorCell.m_headerText.color = Color.gray;
////				floorCell.m_bodyText.text = "No Facility Present";
////				m_cells.Add ((UICell)floorCell);
//			}
//		}
//
//		LayoutRebuilder.ForceRebuildLayoutImmediate (m_contentParent.GetComponent<RectTransform>());
//	}
}
