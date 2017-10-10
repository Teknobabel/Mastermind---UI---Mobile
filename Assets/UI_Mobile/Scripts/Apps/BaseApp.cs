using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseApp : ScriptableObject, IApp, IUISubject {

	public string m_name;
	public EventLocation m_appType = EventLocation.None;
	public Sprite m_icon;
	public Sprite m_icon_Pressed;
	public bool m_wantsSystemNavBar = true;
	public List<GameObject> m_menuBank;

	protected List<IMenu> m_menuStack = new List<IMenu>();
	protected AppIcon m_appIconInstance;
	protected BaseMenu m_homeMenu;

	private List<IUIObserver>
	m_observers = new List<IUIObserver> ();

	public virtual void InitializeApp ()
	{
		Debug.Log ("Initializing " + m_name);
	}

	public virtual void EnterApp ()
	{
//		if (m_wantsSystemNavBar) {
//			
//			if (!MobileUIEngine.instance.systemNavBar.isActive) {
//				
//				// system nav bar slides up
//
//				MobileUIEngine.instance.systemNavBar.SetActiveState (true);
//				RectTransform sysNavRT = MobileUIEngine.instance.systemNavBar.GetComponent<RectTransform> ();
//				sysNavRT.anchoredPosition = new Vector2 (0, sysNavRT.rect.height * -1);
//				sysNavRT.gameObject.SetActive (true);
//				DOTween.To (() => sysNavRT.anchoredPosition, x => sysNavRT.anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetDelay (0.35f);
//			}
//
//		} else {
//
////			if (MobileUIEngine.instance.systemNavBar.isActive) {
//
//				RectTransform sysNavRT = MobileUIEngine.instance.systemNavBar.GetComponent<RectTransform> ();
//			DOTween.To (() => sysNavRT.anchoredPosition, x => sysNavRT.anchoredPosition = x, new Vector2 (0, sysNavRT.rect.height * -1), 0.25f).SetDelay (0.35f).OnComplete (OnExitComplete);
////			}
//
////			if (MobileUIEngine.instance.systemNavBar != null) {
////				MobileUIEngine.instance.systemNavBar.gameObject.SetActive (false);
////			}
//		}

		Notify ((IUISubject)this, UIEvent.App_Enter);
	}

//	public void OnExitComplete ()
//	{
//		MobileUIEngine.instance.systemNavBar.SetActiveState (false);
//	}

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

		Notify ((IUISubject)this, UIEvent.App_Exit);
	}

	public virtual void HoldApp ()
	{

	}

	public virtual void AppReturn ()
	{
		Notify ((IUISubject)this, UIEvent.App_Return);
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

	public void AddObserver (IUIObserver observer)	
	{
		m_observers.Add (observer);
	}

	public void RemoveObserver (IUIObserver observer)
	{
		if (m_observers.Contains(observer))
		{
			m_observers.Remove(observer);
		}
	}

	public void Notify (IUISubject subject, UIEvent thisUIEvent)
	{
		for (int i=0; i < m_observers.Count; i++)
		{
			m_observers[i].OnNotify(subject, thisUIEvent);
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
