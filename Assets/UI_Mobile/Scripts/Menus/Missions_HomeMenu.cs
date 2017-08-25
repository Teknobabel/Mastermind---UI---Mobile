using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missions_HomeMenu : MonoBehaviour, IMenu {

	public Text
	m_appNameText;

	public GameObject
	m_missionCellGO,
	m_noMissionsCellGO;

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

		DisplayMissions ();

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
		m_isDirty = false;
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
			DisplayMissions ();
		}
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
		ParentApp.PushMenu (((MissionsApp)(m_parentApp)).planMissionMenu);
	}

	private void DisplayMissions ()
	{
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		List<MissionPlan> missions = GameController.instance.GetMissions (0);

		if (missions.Count == 0) {

			GameObject noMissionsCellGO = (GameObject)Instantiate (m_noMissionsCellGO, m_contentParent);
			UICell noMissionsCell = (UICell)noMissionsCellGO.GetComponent<UICell> ();
			m_cells.Add (noMissionsCell);

		} else {

			foreach (MissionPlan mp in missions) {

				GameObject missionCellGO = (GameObject)Instantiate (m_missionCellGO, m_contentParent);
				UICell missionCell = (UICell)missionCellGO.GetComponent<UICell> ();
				missionCell.m_headerText.text = mp.m_currentMission.m_name;
				missionCell.m_bodyText.text = mp.m_turnNumber.ToString () + " / " + mp.m_currentMission.m_duration.ToString () + " Turns";
				m_cells.Add (missionCell);

				if (mp.m_new) {

					missionCell.m_rectTransforms [1].gameObject.SetActive (true);
				}

				Button b = missionCell.m_buttons [0];
				b.onClick.AddListener (delegate {
					MissionButtonPressed (mp);
				});
			}
		}
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}
	public bool isDirty {get{return m_isDirty;} set{ m_isDirty = value; }}
}
