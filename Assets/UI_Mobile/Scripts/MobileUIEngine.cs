using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class MobileUIEngine : MonoBehaviour {
	public static MobileUIEngine instance;

	public RectTransform 
	m_mainCanvas;

	public ScriptableObject
	m_homeScreen;

	public GameObject
	m_systemNavBarGO;

	public ScriptableObject m_tutorial;

	public ScriptableObject[] m_apps;

	private List<IApp> m_appStack = new List<IApp>();

	private SystemNavBar m_systemNavBar;

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

		} else {
			
			// instantiate home screen
			startApp = (IApp)ScriptableObject.Instantiate(m_homeScreen);

		}
			
		startApp.InitializeApp ();

		PushApp (startApp);
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

	public SystemNavBar systemNavBar {get{ return m_systemNavBar; } set { m_systemNavBar = value; }}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
