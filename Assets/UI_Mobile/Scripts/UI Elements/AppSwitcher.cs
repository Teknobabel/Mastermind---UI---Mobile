using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AppSwitcher : MonoBehaviour {

	public Transform m_appListContentParent;

	public GameObject m_AppIconGO;

	private List<UICell> m_cells = new List<UICell>();

	private IApp m_hoveredApp = null,
	m_appInFocus = null;

	private float 
		m_holdTime = 0.5f,
		m_currentHoldTime = 0.0f;

	public bool 
	m_menuActive = false,
	m_switcherPressed = false;

	private UICell m_appInFocusCell = null;

	public void Initialize ()
	{
		GameObject go = (GameObject)Instantiate (m_AppIconGO, m_appListContentParent);
		UICell appIcon = (UICell)go.GetComponent<UICell> ();
		m_appInFocusCell = appIcon;
		m_appInFocusCell.gameObject.SetActive (false);

		EventTrigger trigger = appIcon.GetComponent<EventTrigger> ();
		EventTrigger.Entry entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener((data) => { SwitcherButtonPressed();});
		trigger.triggers.Add(entry);

		EventTrigger.Entry entry2 = new EventTrigger.Entry ();
		entry2.eventID = EventTriggerType.PointerUp;
		entry2.callback.AddListener((data) => { SwitcherButtonReleased();});
		trigger.triggers.Add(entry2);

		EventTrigger.Entry entry3 = new EventTrigger.Entry ();
		entry3.eventID = EventTriggerType.PointerExit;
		entry3.callback.AddListener((data) => { EndAppHover();});
		trigger.triggers.Add(entry3);

	}

	public void CloseAppSwitcher ()
	{
//		m_appListContentParent.gameObject.SetActive (false);
		m_menuActive = false;
		if (m_hoveredApp != null) {

			MobileUIEngine.instance.SwitchApps (m_hoveredApp);
			m_hoveredApp = null;
		}

		while (m_cells.Count > 0) {

			UICell cell = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (cell.gameObject);
		}
	}

	void Update ()
	{
		if (m_switcherPressed && !m_menuActive) {

			m_currentHoldTime = Mathf.Clamp (m_currentHoldTime + Time.deltaTime, 0.0f, m_holdTime);

			if (m_currentHoldTime == m_holdTime) {

				m_currentHoldTime = 0.0f;
				ShowAppSheet ();

			}

		} else if (m_currentHoldTime > 0.0f) {

			m_currentHoldTime = 0.0f;
		}
	}

	public void SwitcherButtonReleased ()
	{
		m_switcherPressed = false;

		CloseAppSwitcher ();
	}

	private void ShowAppSheet ()
	{
		// get current app
		m_menuActive = true;
		IApp currentApp = MobileUIEngine.instance.GetCurrentApp();

		foreach (KeyValuePair<EventLocation, IApp> pair in MobileUIEngine.instance.appList) {

			if (pair.Value.WantsSystemNavBar && pair.Value != currentApp) {

				GameObject go = (GameObject)Instantiate (m_AppIconGO, m_appListContentParent);
				UICell appIcon = (UICell)go.GetComponent<UICell> ();
				appIcon.m_image.texture = pair.Value.Icon.texture;
				go.transform.SetAsFirstSibling ();
				m_cells.Add (appIcon);

				EventTrigger trigger = appIcon.GetComponent<EventTrigger> ();
				EventTrigger.Entry entry = new EventTrigger.Entry ();
				entry.eventID = EventTriggerType.PointerEnter;
				entry.callback.AddListener((data) => { BeginAppHover(pair.Value);});
				trigger.triggers.Add(entry);

//				appIcon.m_buttons[0].OnPointerEnter((data) => { BeginAppHover(pair.Value);});

				EventTrigger.Entry entry2 = new EventTrigger.Entry ();
				entry2.eventID = EventTriggerType.PointerExit;
				entry2.callback.AddListener((data) => { EndAppHover();});
				trigger.triggers.Add(entry2);

				//				Debug.Log (trigger.triggers.Count);
				//					Button b = appIcon.m_buttons [0];
				//					b.onClick.AddListener (delegate {
				//						AppButtonPressed (pair.Value);
				//					});
			}
		}
	}

	public void SwitcherButtonPressed ()
	{
		Debug.Log ("Switcher Pressed");
		m_hoveredApp = m_appInFocus;
		m_switcherPressed = true;
	}

//	public void OnPointerUpDelegate (PointerEventData data)
	public void BeginAppHover (IApp app)
	{
		Debug.Log ("Begin app hover");
		m_hoveredApp = app;
	}

	public void EndAppHover ()
	{
		m_hoveredApp = null;
	}

	public void AppEntered (IApp app)
	{
		if (app.WantsSystemNavBar && m_appInFocus == null) {

			m_appInFocus = app;
			m_appInFocusCell.m_image.texture = app.Icon.texture;
			m_appInFocusCell.gameObject.SetActive (true);
		}
	}

	public void AppExited (IApp app)
	{
		if (app.WantsSystemNavBar) {

			m_appInFocus = app;
			m_appInFocusCell.m_image.texture = app.Icon.texture;
		}
	}

//	public void AppButtonPressed (IApp app)
//	{
//		SwitcherButtonPressed ();
//		CloseAppSwitcher();
//		MobileUIEngine.instance.SwitchApps (app);
//		MobileUIEngine.instance.PopApp ();
//		MobileUIEngine.instance.PushApp (app);
//	}
}
