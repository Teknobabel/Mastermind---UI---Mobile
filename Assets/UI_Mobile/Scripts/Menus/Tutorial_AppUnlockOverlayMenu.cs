using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_AppUnlockOverlayMenu : OmegaPlan_GoalInfoMenu {

	public GameObject m_appIconGO;

	public Transform m_appParent;

	private List<GameObject> m_appIcons = new List<GameObject>();

	public void InitializeOverlay (List<IApp> appList)
	{

		foreach (IApp a in appList) {

			GameObject go = (GameObject)GameObject.Instantiate (m_appIconGO, m_appParent);
			m_appIcons.Add (go);
			AppIcon ai = (AppIcon)go.GetComponent<AppIcon>();
			ai.Initialize(a);
			ai.DisableButton ();
		}
	}

	public override void OnExitComplete ()
	{
		while (m_appIcons.Count > 0) {

			GameObject g = m_appIcons [0];
			m_appIcons.RemoveAt (0);
			Destroy (g);
		}


		base.OnExitComplete ();
	}
}
