using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseApp : ScriptableObject, IApp {

	public string m_name;
	public EventLocation m_appType = EventLocation.None;
	public Sprite m_icon;
	public Sprite m_icon_Pressed;
	public bool m_wantsSystemNavBar = true;
	public List<GameObject> m_menuBank;

	protected List<IMenu> m_menuStack = new List<IMenu>();
	protected AppIcon m_appIconInstance;
	protected BaseMenu m_homeMenu;

	public virtual void InitializeApp ()
	{
		Debug.Log ("Initializing " + m_name);
	}

	public virtual void EnterApp ()
	{
		if (m_wantsSystemNavBar) {
			
			// system nav bar slides up

			RectTransform sysNavRT = MobileUIEngine.instance.systemNavBar.GetComponent<RectTransform> ();
			sysNavRT.anchoredPosition = new Vector2 (0, sysNavRT.rect.height * -1);
			sysNavRT.gameObject.SetActive (true);
			DOTween.To (() => sysNavRT.anchoredPosition, x => sysNavRT.anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetDelay (0.35f);

		} else {

			if (MobileUIEngine.instance.systemNavBar != null) {
				MobileUIEngine.instance.systemNavBar.gameObject.SetActive (false);
			}
		}
	}

	public virtual void UpdateApp ()
	{

	}

	public virtual void ExitApp ()
	{
		while (m_menuStack.Count > 0) {

			IMenu m = m_menuStack[m_menuStack.Count-1];

			Debug.Log ("Popping Menu: " + m);

			m_menuStack.RemoveAt (m_menuStack.Count-1);
			m.OnExit (false);

			if (m_menuStack.Count > 0) {

				IMenu returningMenu = m_menuStack[m_menuStack.Count-1];

				Debug.Log ("Returning To Menu: " + returningMenu);

				returningMenu.OnReturn ();
			}
		}

		if (m_wantsSystemNavBar) {
			
			RectTransform sysNavRT = MobileUIEngine.instance.systemNavBar.GetComponent<RectTransform> ();
			DOTween.To (() => sysNavRT.anchoredPosition, x => sysNavRT.anchoredPosition = x, new Vector2 (0, sysNavRT.rect.height * -1), 0.25f).SetDelay (0.35f);
		}
	}

	public virtual void HoldApp ()
	{

	}

	public virtual void AppReturn ()
	{

	}

	public virtual void PushMenu (IMenu menu)
	{
		Debug.Log ("Pushing Menu: " + menu);

		if (m_menuStack.Count > 0) {

			m_menuStack [m_menuStack.Count - 1].OnHold ();
		} 

		m_menuStack.Add (menu);
		menu.OnEnter (true);
	}

	public virtual void PopMenu ()
	{
		if (m_menuStack.Count > 0) {

			IMenu m = m_menuStack[m_menuStack.Count-1];

			Debug.Log ("Popping Menu: " + m);

			m_menuStack.RemoveAt (m_menuStack.Count-1);
			m.OnExit (true);

			if (m_menuStack.Count > 0) {

				IMenu returningMenu = m_menuStack[m_menuStack.Count-1];

				Debug.Log ("Returning To Menu: " + returningMenu);

				returningMenu.OnReturn ();
			}
		}
	}

	public virtual void SetAlerts ()
	{

	}

	public string Name 
	{
		get
		{
			return m_name;
		}
		set
		{
			m_name = value;
		}   
	}

	public Sprite Icon 
	{
		get
		{
			return m_icon;
		}
		set
		{
			m_icon = value;
		}   
	}

	public Sprite Icon_Pressed 
	{
		get
		{
			return m_icon_Pressed;
		}
		set
		{
			m_icon_Pressed = value;
		}   
	}

	public List<GameObject> MenuBank 
	{ get { return m_menuBank; }}

	public bool WantsSystemNavBar 
	{ get{ return m_wantsSystemNavBar; } }

	public int menuStackSize {get{ return m_menuStack.Count; }}
	public AppIcon AppIconInstance {set{ m_appIconInstance = value; }}
	public BaseMenu homeMenu {get{ return m_homeMenu;}}
}
