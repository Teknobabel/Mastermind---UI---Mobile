using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreenMenu : MonoBehaviour, IUIObserver {

	public Transform
	m_contentParent;

	public PageIndicator
	m_pageIndicator;

	// Use this for initialization
	void Start () {
		
	}
	
	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent)
		{
		case UIEvent.UI_PageChanged:

			m_pageIndicator.SetPage (((ScrollRectSnap)subject).target);
			break;
		}

	}
}
