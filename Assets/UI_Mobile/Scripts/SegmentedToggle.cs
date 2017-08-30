using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegmentedToggle : MonoBehaviour, IUISubject {

	public Button[] m_buttons;

	public Color 
	m_buttonActiveStateColor = Color.black,
	m_buttonTextActiveStateColor = Color.white,
	m_buttonInactiveStateColor = Color.white,
	m_buttonTextInactiveStateColor = Color.black;

	private int
	m_activeButton = -1;

	private List<IUIObserver>
	m_observers = new List<IUIObserver> ();

	// Use this for initialization
	void Start () {
		SetButtonActiveState (0);
		m_activeButton = 0;
	}

	public void ToggleButtonClicked (int buttonNum)
	{
		if (buttonNum != m_activeButton) {
			SetButtonActiveState (buttonNum);
			m_activeButton = buttonNum;
			Notify (this, UIEvent.UI_ToggleButtonPressed);
		}
	}

	private void SetButtonActiveState (int buttonNum)
	{
		// deactivate previous button
		if (m_activeButton >= 0) {

			Button b = m_buttons [m_activeButton];
			ColorBlock colors = b.colors;
			colors.normalColor = m_buttonInactiveStateColor;
			colors.highlightedColor = m_buttonInactiveStateColor;
			colors.pressedColor = m_buttonInactiveStateColor;
			b.colors = colors;

			Text t = b.GetComponentInChildren<Text> ();
			t.color = m_buttonTextInactiveStateColor;
		}

		// activate new button

		Button b1 = m_buttons [buttonNum];
		ColorBlock colors1 = b1.colors;
		colors1.normalColor = m_buttonActiveStateColor;
		colors1.highlightedColor = m_buttonActiveStateColor;
		colors1.pressedColor = m_buttonActiveStateColor;
		b1.colors = colors1;

		Text t1 = b1.GetComponentInChildren<Text> ();
		t1.color = m_buttonTextActiveStateColor;

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
