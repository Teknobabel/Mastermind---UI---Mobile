using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegmentedToggle : MonoBehaviour, IUISubject {

	public Button[] m_buttons;

	public UIEvent m_sentEvent = UIEvent.UI_ToggleButtonPressed;

	public Color 
	m_buttonTextActiveStateColor = Color.black,
	m_buttonTextInactiveStateColor = Color.white;

	private int
	m_activeButton = 0;

	private List<IUIObserver>
	m_observers = new List<IUIObserver> ();

	// Use this for initialization
	void Start () {

		foreach (Button b in m_buttons) {
			Text t = b.GetComponentInChildren<Text> ();
			Image ri = b.GetComponentInChildren<Image> ();
			t.color = m_buttonTextInactiveStateColor;
			Color c = ri.color;
			c.a = 0;
			ri.color = c;
		}

		SetButtonActiveState (0);
		m_activeButton = 0;
	}

	public void ToggleButtonClicked (int buttonNum)
	{
		if (buttonNum != m_activeButton) {
			SetButtonActiveState (buttonNum);
			m_activeButton = buttonNum;
			Notify (this, m_sentEvent);
		}
	}

	private void SetButtonActiveState (int buttonNum)
	{
		// deactivate previous button
//		if (m_activeButton >= 0) {

			Button b = m_buttons [m_activeButton];
			Image ri = b.GetComponentInChildren<Image> ();
//		ri.gameObject.SetActive (false);
			if (ri != null) {
				Color c = ri.color;
				c.a = 0;
				ri.color = c;
			}

//			ColorBlock colors = b.colors;
//			colors.normalColor = m_buttonInactiveStateColor;
//			colors.highlightedColor = m_buttonInactiveStateColor;
//			colors.pressedColor = m_buttonInactiveStateColor;
//			b.colors = colors;
//
			Text t2 = b.GetComponentInChildren<Text> ();
			t2.color = m_buttonTextInactiveStateColor;
//		}

		// activate new button

		Button b1 = m_buttons [buttonNum];
//		ColorBlock colors1 = b1.colors;
//		colors1.normalColor = m_buttonActiveStateColor;
//		colors1.highlightedColor = m_buttonActiveStateColor;
//		colors1.pressedColor = m_buttonActiveStateColor;
//		b1.colors = colors1;
//
		Text t3 = b1.GetComponentInChildren<Text> ();
		t3.color = m_buttonTextActiveStateColor;

		Image ri2 = b1.GetComponentInChildren<Image> ();
//		ri2.gameObject.SetActive (true);
		if (ri2 != null) {
			Color c1 = ri2.color;
			c1.a = 1.0f;
			ri2.color = c1;
		}
	}

	public void AddObserver (IUIObserver observer)	
	{
		m_observers.Add (observer);
	}

	public void RemoveObserver (IUIObserver observer)
	{
		if (m_observers.Contains(observer))
		{
			m_observers.Remove(observer);
		}
	}

	public void Notify (IUISubject subject, UIEvent thisUIEvent)
	{
		for (int i=0; i < m_observers.Count; i++)
		{
			m_observers[i].OnNotify(subject, thisUIEvent);
		}
	}

	public int activeButton {get{return m_activeButton; }}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
