using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class HomeScreenApp : BaseApp {

	public GameObject 
	m_homeScreen,
	m_activityCenter,
	m_appScreen,
	m_appIcon;

	private Transform m_menuParent = null;

	private HomeScreenMenu m_homeScreenMenu = null;
	private bool m_isDirty = false;
	private HomeScreen_CPBreakdownMenu m_cpBreakdownMenu;

	public override void InitializeApp ()
	{
		// set canvas size

		float screenWidth = MobileUIEngine.instance.m_mainCanvas.sizeDelta.x;
		float screenHeight = MobileUIEngine.instance.m_mainCanvas.sizeDelta.y;

		// instantiate home screen base

		GameObject homeScreenGO = (GameObject)GameObject.Instantiate (m_homeScreen, Vector3.zero, Quaternion.identity);
		m_menuParent = homeScreenGO.transform;
		homeScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);

		HomeScreenMenu homeScreenMenu = (HomeScreenMenu)homeScreenGO.GetComponent<HomeScreenMenu> ();
		m_homeScreenMenu = homeScreenMenu;

		int numPages = 0;

		// determine # of app screens needed based on m_apps array length

		List<ScriptableObject> tempAppList = new List<ScriptableObject> (MobileUIEngine.instance.m_apps);
		List<List<ScriptableObject>> appsByPage = new List<List<ScriptableObject>> ();
		int maxAppsPerPage = 6;

		while (tempAppList.Count > 0) {

			ScriptableObject a = tempAppList [0];
			tempAppList.RemoveAt (0);

			if (appsByPage.Count > 0) {

				List<ScriptableObject> list = appsByPage[appsByPage.Count-1];

				if (list.Count < maxAppsPerPage) {

					list.Add (a);
					appsByPage [appsByPage.Count - 1] = list;
				} else {

					List<ScriptableObject> newList = new List<ScriptableObject> ();
					newList.Add (a);
					appsByPage.Add (newList);
				}

			} else {

				List<ScriptableObject> newList = new List<ScriptableObject> ();
				newList.Add (a);
				appsByPage.Add (newList);
			}
		}

		LayoutElement hsle = homeScreenMenu.m_contentParent.GetComponent<LayoutElement> ();
		hsle.preferredWidth = screenWidth * (appsByPage.Count + 1);
		hsle.preferredHeight = screenHeight;
//		hsle.minWidth = screenWidth * 3;
//		hsle.minHeight = screenHeight;

		Vector2 size = homeScreenMenu.m_contentParent.GetComponent<RectTransform> ().sizeDelta;
		size.x = hsle.preferredWidth;
		size.y = hsle.preferredHeight;
		homeScreenMenu.m_contentParent.GetComponent<RectTransform> ().sizeDelta = size;



		// add app screens

		for (int i = 0; i < appsByPage.Count; i++) {

			List<ScriptableObject> appSOList = appsByPage [i];
			List<IApp> appList = new List<IApp> ();

			foreach (ScriptableObject a in appSOList) {

				// instantiate app
				IApp newApp = (IApp)ScriptableObject.Instantiate(a);
				newApp.InitializeApp ();

				if (!MobileUIEngine.instance.appList.ContainsKey (((BaseApp)newApp).m_appType)) {
					
					MobileUIEngine.instance.appList.Add (((BaseApp)newApp).m_appType, newApp);
				}

				appList.Add (newApp);
			}

			GameObject go = (GameObject)GameObject.Instantiate (m_appScreen, homeScreenMenu.m_contentParent);
			LayoutElement le = go.GetComponent<LayoutElement> ();
			le.preferredWidth = screenWidth;
			le.preferredHeight = screenHeight;

			AppScreen ascreen = go.GetComponent<AppScreen> ();
			ascreen.Initialize (appList, m_appIcon, this);

			numPages++;
		}

		// add activity center menu

		GameObject ac = (GameObject)GameObject.Instantiate (m_activityCenter, homeScreenMenu.m_contentParent);
		ActivityCenterMenu acMenu = (ActivityCenterMenu)ac.GetComponent<ActivityCenterMenu> ();

		LayoutElement acle = ac.GetComponent<LayoutElement> ();
		acle.minWidth = screenWidth;
		acle.minHeight = screenHeight;

		numPages++;

		acMenu.Initialize (this);

		ScrollRectSnap srt = (ScrollRectSnap) homeScreenGO.GetComponent<ScrollRectSnap> ();
		srt.screens = numPages;
		srt.Initialize ();
		srt.AddObserver (homeScreenMenu);

		homeScreenMenu.m_pageIndicator.SetNumPages (numPages);
		homeScreenMenu.m_pageIndicator.SetPage (0);

		GameObject cpBreakdownMenuGO = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		cpBreakdownMenuGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_cpBreakdownMenu = (HomeScreen_CPBreakdownMenu)cpBreakdownMenuGO.GetComponent<HomeScreen_CPBreakdownMenu> ();
		m_cpBreakdownMenu.Initialize (this);

		m_menuParent.gameObject.SetActive (false);

		base.InitializeApp ();

	}

	public void NewTurnStarted ()
	{
		m_isDirty = true;
	}

	public override void AppReturn ()
	{
		if (m_isDirty) {
			
			m_isDirty = false;

			// reset to activity center page
			ScrollRectSnap srt = (ScrollRectSnap) m_homeScreenMenu.GetComponent<ScrollRectSnap> ();
			srt.GoToPage (0);
		} 

		base.AppReturn ();

//		ScrollRectSnap srt = (ScrollRectSnap) m_homeScreenMenu.GetComponent<ScrollRectSnap> ();
//		srt.GoToPage (0);
	}

	public override void EnterApp ()
	{
		m_menuParent.gameObject.SetActive (true);

		base.EnterApp ();

	}

	public HomeScreen_CPBreakdownMenu cpBreakdownMenu {get{ return m_cpBreakdownMenu; }}
	public bool isDirty {get{ return m_isDirty; } set{ m_isDirty = value;}}
}
