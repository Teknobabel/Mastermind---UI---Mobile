using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemNavBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	private bool m_isActive = false;

	public void SetActiveState (bool isActive)
	{
		if (!isActive) {

			m_isActive = false;
			gameObject.SetActive (false);
		} else {

			m_isActive = true;
			gameObject.SetActive (true);
		}
	}

	public void CloseAppButtonClicked ()
	{
		MobileUIEngine.instance.PopApp ();
	}
		
	public bool isActive {get{ return m_isActive; }}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
