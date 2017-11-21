using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_AppUnlockOverlayMenu : BaseMenu {


	public void DismissButtonClicked ()
	{
		m_parentApp.PopMenu ();
		((TutorialApp)m_parentApp).NextTutorialMenu ();
	}

	public override void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);
	}

	public override void OnExit (bool animate)
	{
		this.gameObject.SetActive (false);
	}

//	public void InitializeOverlay (List<IApp> appList)
//	{
//
//		foreach (IApp a in appList) {
//
//			GameObject go = (GameObject)GameObject.Instantiate (m_appIconGO, m_appParent);
//			m_appIcons.Add (go);
//			AppIcon ai = (AppIcon)go.GetComponent<AppIcon>();
//			ai.Initialize(a);
//			ai.DisableButton ();
//		}
//	}
//
//	public override void OnExitComplete ()
//	{
//		while (m_appIcons.Count > 0) {
//
//			GameObject g = m_appIcons [0];
//			m_appIcons.RemoveAt (0);
//			Destroy (g);
//		}
//
//
//		base.OnExitComplete ();
//	}
}
