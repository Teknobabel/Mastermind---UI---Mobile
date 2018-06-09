using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TutorialApp : BaseApp {
	
	public GameObject m_appUnlockOverlayGO;

	private List<IMenu> m_tutorialMenus = new List<IMenu> ();

	private Tutorial_AppUnlockOverlayMenu m_appUnlockOverlay;

	// Use this for initialization
	void Start () {
		
	}

	public override void InitializeApp ()
	{
		foreach (GameObject menu in m_menuBank) {

			GameObject go = (GameObject)GameObject.Instantiate (menu, Vector3.zero, Quaternion.identity);
			go.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
			IMenu ta = (IMenu)go.GetComponent<IMenu> ();
			ta.Initialize (this);

			m_tutorialMenus.Add (ta);

//			PushMenu (ta);
		}

//		GameObject overlayGO = (GameObject)GameObject.Instantiate (m_appUnlockOverlayGO, Vector3.zero, Quaternion.identity);
//		overlayGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
//		IMenu overlay = (IMenu)overlayGO.GetComponent<IMenu> ();
//		overlay.Initialize (this);
//		m_appUnlockOverlay = (Tutorial_AppUnlockOverlayMenu)overlay;

		NextTutorialMenu ();

		base.InitializeApp ();
	}

	public void NextTutorialMenu ()
	{
//		PopMenu ();
		Debug.Log("Next Tutorial Menu");
		if (m_tutorialMenus.Count > 0) {

			IMenu menu = m_tutorialMenus [0];
			m_tutorialMenus.RemoveAt (0);

			PushMenu (menu);
		} else {
			
			MobileUIEngine.instance.CreateLogInMenu ();

		}
	}

	public override void ExitApp ()
	{
		base.ExitApp ();

		if (m_menuStack.Count == 0) {

			MobileUIEngine.instance.CreateLogInMenu ();
		}
	}

	public Tutorial_AppUnlockOverlayMenu appOverlay {get{return m_appUnlockOverlay;}}
}
