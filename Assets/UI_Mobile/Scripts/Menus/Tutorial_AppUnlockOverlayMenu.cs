using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_AppUnlockOverlayMenu : OmegaPlan_GoalInfoMenu {

	public GameObject m_appIconGO;

	public Transform m_appParent;

	private List<GameObject> m_cells = new List<GameObject>();

	public void InitializeOverlay (List<IApp> appList)
	{

		foreach (IApp a in appList) {

			GameObject go = (GameObject)GameObject.Instantiate (m_appIconGO, m_appParent);
			m_cells.Add (go);
			AppIcon ai = (AppIcon)go.GetComponent<AppIcon>();
			ai.Initialize(a);
			ai.DisableButton ();
		}
	}

	public override void OnExitComplete ()
	{
		while (m_cells.Count > 0) {

			GameObject g = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (g);
		}

		base.OnExitComplete ();
	}
}
