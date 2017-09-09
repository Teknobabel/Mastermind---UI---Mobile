using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lair_HomeMenu : BaseMenu {

	public Text
	m_appNameText;

	public GameObject
	m_floorCellGO;

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
		base.OnEnter (animate);

		this.gameObject.SetActive (true);

		DisplayContent ();

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

	public void IdleFloorButtonClicked (int floorSlotID)
	{
		Debug.Log ("Idle floor clicked, start mission planning");

		Lair l = GameController.instance.GetLair (0);

		foreach (Lair.FloorSlot fSlot in l.floorSlots) {

			if (fSlot.m_id == floorSlotID) {

				fSlot.m_missionPlan.m_missionOptions.Clear ();

				foreach (Mission m in fSlot.m_floor.m_missions) {

					fSlot.m_missionPlan.m_missionOptions.Add (m);
				}

				fSlot.m_missionPlan.m_maxActorSlots = fSlot.m_numActorSlots;

				((LairApp)(m_parentApp)).planMissionMenu.missionPlan = fSlot.m_missionPlan;
				m_parentApp.PushMenu (((LairApp)(m_parentApp)).planMissionMenu);
				break;
			}
		}
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		Lair l = GameController.instance.GetLair (0);

		foreach (Lair.FloorSlot fSlot in l.floorSlots) {

			GameObject floorGO = (GameObject)Instantiate (m_floorCellGO, m_contentParent);
			UICell floorCell = (UICell)floorGO.GetComponent<UICell> ();
			floorCell.m_headerText.text = fSlot.m_floor.m_name;
			floorCell.m_bodyText.text = "Level " + fSlot.m_floor.level.ToString() + "\n";
			floorCell.m_bodyText.text += "No Active Missions";
			m_cells.Add (floorCell);

			Button b = floorCell.m_buttons [0];

			if (fSlot.m_floor.m_missions.Count == 0) {

				b.interactable = false;
				b.gameObject.SetActive (false);

			} else {

				if (fSlot.m_state == Lair.FloorSlot.FloorState.MissionInProgress) {

//					Text t = b.GetComponentInChildren<Text> ();
//					t.text = "Mission In Progress";
					floorCell.m_bodyText.color = Color.black;
					floorCell.m_bodyText.text = "Level " + fSlot.m_floor.level.ToString() + "\n";
					floorCell.m_bodyText.text = "Mission In Progress:\n";
					floorCell.m_bodyText.text += fSlot.m_missionPlan.m_currentMission.m_name;
					floorCell.m_bodyText.text += "(" + fSlot.m_missionPlan.m_turnNumber.ToString () + "/" + fSlot.m_missionPlan.m_currentMission.m_duration.ToString () + ")";
				}

				b.onClick.AddListener (delegate {
					IdleFloorButtonClicked (fSlot.m_id);
				});
			}

			if (fSlot.m_new) {
				floorCell.m_rectTransforms [0].gameObject.SetActive (true);
			}
		}

		LayoutRebuilder.ForceRebuildLayoutImmediate (m_contentParent.GetComponent<RectTransform>());
	}
}
