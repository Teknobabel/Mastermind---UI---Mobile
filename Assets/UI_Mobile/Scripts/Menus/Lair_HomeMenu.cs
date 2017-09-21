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

		for (int i = 0; i < l.maxFloors; i++) {

			if (i < l.floorSlots.Count) {

				Lair.FloorSlot fSlot = l.floorSlots [i];

				GameObject floorGO = (GameObject)Instantiate (m_floorCellGO, m_contentParent);
				Cell_LairFloor floorCell = (Cell_LairFloor)floorGO.GetComponent<Cell_LairFloor> ();
				floorCell.SetFloor (fSlot);
				m_cells.Add ((UICell)floorCell);

				floorCell.m_buttons [0].onClick.AddListener (delegate {
					FloorButtonClicked (fSlot.m_id);
				});
			} else {

				GameObject floorGO = (GameObject)Instantiate (m_floorCellGO, m_contentParent);
				Cell_LairFloor floorCell = (Cell_LairFloor)floorGO.GetComponent<Cell_LairFloor> ();
				floorCell.m_headerText.text = "Empty";
				floorCell.m_headerText.color = Color.gray;
				floorCell.m_bodyText.text = "No Facility Present";
//				floorCell.SetFloor (fSlot);
				m_cells.Add ((UICell)floorCell);
			}
		}

		LayoutRebuilder.ForceRebuildLayoutImmediate (m_contentParent.GetComponent<RectTransform>());
	}
}
