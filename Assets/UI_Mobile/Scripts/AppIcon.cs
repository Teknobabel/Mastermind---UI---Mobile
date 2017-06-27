using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppIcon : MonoBehaviour {

	public Image m_appIcon;
	public Text m_appName;

	private IApp m_app;

	public void Initialize (IApp app)
	{
		m_appName.text = app.Name;
		m_appIcon.sprite = app.Icon;
		Button b = m_appIcon.GetComponent<Button> ();
		SpriteState ss = b.spriteState;
		ss.pressedSprite = app.Icon_Pressed;
		b.spriteState = ss;

		m_app = app;
	}

	public void DisableButton ()
	{
		Button b = m_appIcon.GetComponent<Button> ();
		b.interactable = false;
	}

	public void AppTapped ()
	{
		if (m_app != null) {

			MobileUIEngine.instance.PushApp (m_app);
		}
	}
}
