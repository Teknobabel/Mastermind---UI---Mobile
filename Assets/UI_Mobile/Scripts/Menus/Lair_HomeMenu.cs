using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lair_HomeMenu : MonoBehaviour, IMenu {

	public Text
	m_appNameText;

	public GameObject
	m_floorCellGO;

	public Transform
		m_contentParent;

	private IApp m_parentApp;

	private List<UICell> m_cells = new List<UICell>();

	private bool m_isDirty = false;

	// Use this for initialization
	void Start () {

	}

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
		m_appNameText.text = parentApp.Name;
//		m_infoPanelToggle.AddObserver (this);
//		m_infoPanelToggle.ToggleButtonClicked (0);
		this.gameObject.SetActive (false);
	}

	public void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		DisplayFloors ();

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

	public void OnExit (bool animate)
	{
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

	public void OnHold ()
	{

	}

	public void OnReturn ()
	{
		if (m_isDirty) {

			m_isDirty = false;

			DisplayFloors ();
		}
	}

	public void IdleFloorButtonClicked (int floorSlotID)
	{
		Debug.Log ("Idle floor clicked, start mission planning");

		Lair l = GameController.instance.GetLair (0);

		foreach (Lair.FloorSlot fSlot in l.floorSlots) {

			if (fSlot.m_id == floorSlotID) {

				((LairApp)(m_parentApp)).planMissionMenu.floorSlot = fSlot;
				m_parentApp.PushMenu (((LairApp)(m_parentApp)).planMissionMenu);
				break;
			}
		}
	}

	private void DisplayFloors ()
	{
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		Lair l = GameController.instance.GetLair (0);

		foreach (Lair.FloorSlot fSlot in l.floorSlots) {

			GameObject floorGO = (GameObject)Instantiate (m_floorCellGO, m_contentParent);
			UICell floorCell = (UICell)floorGO.GetComponent<UICell> ();
			floorCell.m_headerText.text = fSlot.m_floor.m_name;
			m_cells.Add (floorCell);

			Button b = floorCell.m_buttons [0];

			if (fSlot.m_floor.m_missions.Count == 0) {

				b.interactable = false;
				b.gameObject.SetActive (false);

			} else {

				if (fSlot.m_state == Lair.FloorSlot.FloorState.MissionInProgress) {

					Text t = b.GetComponentInChildren<Text> ();
					t.text = "Mission In Progress";
				}

				b.onClick.AddListener (delegate {
					IdleFloorButtonClicked (fSlot.m_id);
				});
			}

			if (fSlot.m_new) {
				floorCell.m_rectTransforms [1].gameObject.SetActive (true);
			}
		}
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}

	public bool isDirty {set{ m_isDirty = value;}}
}
