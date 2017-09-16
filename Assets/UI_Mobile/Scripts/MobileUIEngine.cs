using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class MobileUIEngine : MonoBehaviour, IObserver {
	public static MobileUIEngine instance;

	public RectTransform 
	m_mainCanvas;

	public ScriptableObject
	m_homeScreen;

	public GameObject
	m_systemNavBarGO,
	m_toastGO,
	m_alertDialogueGO;

	public ScriptableObject m_tutorial;

	public ScriptableObject m_turnProcessing;

	public ScriptableObject[] m_apps;

	private List<IApp> m_appStack = new List<IApp>();

	private Dictionary<EventLocation, IApp> m_appList = new Dictionary<EventLocation, IApp>();

	private SystemNavBar m_systemNavBar;

	private Alert_Toast m_toast;

	private Alert_Generic m_alertDialogue;

	private IApp 
	m_turnProcessingApp,
	m_homeScreenApp;

	public bool m_doTutorial = false;

	void Awake() {
		Application.targetFrameRate = 60;

		// initialize DoTween
		DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

		if(!instance) {
			instance = this;
		} else {
			Destroy(gameObject);
		}

		// initialize player prefs

		if (!PlayerPrefs.HasKey ("PlayTutorial")) {

			PlayerPrefs.SetInt ("PlayTutorial", 0);
			PlayerPrefs.Save ();
		}
	}

	void Start () {
		// initialize dummy data

		GetDummyData.instance.Initialize ();

		IApp startApp;

		int playTutorial = PlayerPrefs.GetInt ("PlayTutorial");

//		if (m_doTutorial) {
		if (playTutorial == 1) {

			startApp = (IApp)ScriptableObject.Instantiate(m_tutorial);
			startApp.InitializeApp ();

		} else {

			Action_StartNewGame newGameAction = new Action_StartNewGame ();
			GameController.instance.ProcessAction (newGameAction);

			Action_EndPhase progressPhaseAction = new Action_EndPhase ();
			GameController.instance.ProcessAction (progressPhaseAction);
			
			// instantiate home screen
			startApp = (IApp)ScriptableObject.Instantiate(m_homeScreen);
			startApp.InitializeApp ();

			if (!m_appList.ContainsKey (((BaseApp)startApp).m_appType)) {
				m_appList.Add (((BaseApp)startApp).m_appType, startApp);
			}
			m_homeScreenApp = startApp;

			// instantitae turn processing screen

			m_turnProcessingApp = (IApp)ScriptableObject.Instantiate(m_turnProcessing);
			m_turnProcessingApp.InitializeApp ();

		}

		PushApp (startApp);

		GameController.instance.AddObserver (this);
	}

	public void PushApp (IApp newApp)
	{
		Debug.Log ("Pushing App: " + newApp.Name);

		if (m_appStack.Count > 0) {

			IApp oldApp = m_appStack[m_appStack.Count-1];

			oldApp.HoldApp ();
		}

		m_appStack.Add (newApp);

		newApp.EnterApp ();

	}

	public void PopApp ()
	{
		if (m_appStack.Count > 0) {

			IApp oldApp = m_appStack[m_appStack.Count-1];

			Debug.Log ("Popping App: " + oldApp.Name);

			m_appStack.RemoveAt (m_appStack.Count-1);
			oldApp.ExitApp ();

			if (m_appStack.Count > 0) {

				IApp returningApp = m_appStack[m_appStack.Count-1];

				Debug.Log ("Returning To App: " + returningApp.Name);

				returningApp.AppReturn ();
			}
		}
	}

	void Update ()
	{
		if (m_appStack.Count > 0) {

			IApp currentApp = m_appStack [m_appStack.Count - 1];
			currentApp.UpdateApp ();
		}
	}

	public IApp GetCurrentApp ()
	{
		if (m_appStack.Count > 0) {

			return m_appStack[m_appStack.Count-1];

		} else {

			Debug.Log ("No app found");

			return null;
		}
	}

	public IApp GetApp (EventLocation type)
	{
		foreach (KeyValuePair<EventLocation, IApp> pair in m_appList) {

			if (pair.Key == type) {

				return pair.Value;
			}
		}

		Debug.Log ("App of type: " + type + " not found");

		return null;
	}

	public void DisplayToast (NotificationCenter.Notification notification)
	{
		Debug.Log ("Displaying Toast");

		toast.m_toastCell.m_bodyText.text = notification.m_title + "\n";
		toast.m_toastCell.m_bodyText.text += notification.m_message;

		IApp app = MobileUIEngine.instance.GetApp (notification.m_location);
		if (app != null && app.Icon != null) {
			toast.m_toastCell.m_image.texture = app.Icon.texture;
		}

		toast.OnEnter (true);
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) 
		{
		case GameEvent.Player_NotificationReceived:

			NotificationCenter.Notification n = (NotificationCenter.Notification)subject;

			DisplayToast (n);

			break;
		}
	}

	public SystemNavBar systemNavBar {get{ return m_systemNavBar; } set { m_systemNavBar = value; }}
	public Alert_Toast toast {get{ return m_toast; } set { m_toast = value; }}
	public Alert_Generic alertDialogue {get{ return m_alertDialogue; } set { m_alertDialogue = value;}}
	public IApp turnProcessingApp {get{ return m_turnProcessingApp; }}
	public IApp homeScreenApp {get{ return m_homeScreenApp; }}
	public Dictionary<EventLocation, IApp> appList {get{ return m_appList; } set{ m_appList = value; }}
}
