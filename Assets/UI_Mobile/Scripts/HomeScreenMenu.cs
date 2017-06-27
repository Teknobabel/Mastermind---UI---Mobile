using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreenMenu : MonoBehaviour, IObserver {

	public Transform
	m_contentParent;

	public PageIndicator
	m_pageIndicator;

	// Use this for initialization
	void Start () {
		
	}
	
	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent)
		{
		case GameEvent.UI_PageChanged:

			m_pageIndicator.SetPage (((ScrollRectSnap)subject).target);
			break;
		}

	}
}
