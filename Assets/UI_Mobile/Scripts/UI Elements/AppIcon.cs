using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppIcon : MonoBehaviour {

	public Image m_appIcon;
	public Text m_appName;
	public Transform m_alertParent;
	public Text m_alertCountText;
	public Button m_button;

	private IApp m_app;

	public void Initialize (IApp app)
	{
		m_appName.text = app.Name;
		m_appIcon.sprite = app.Icon;

		SpriteState ss = m_button.spriteState;
		ss.pressedSprite = app.Icon_Pressed;
		m_button.spriteState = ss;

		m_app = app;
	}

	public void DisableButton ()
	{
		Button b = m_appIcon.GetComponent<Button> ();
		b.interactable = false;
	}

	public void SetAlerts (int num)
	{
		if (num == 0) {

			m_alertParent.gameObject.SetActive (false);

		} else {

			m_alertParent.gameObject.SetActive (true);
			m_alertCountText.text = num.ToString ();
		}
	}

	public void AppTapped ()
	{
		if (m_app != null) {

			MobileUIEngine.instance.PushApp (m_app);
		}
	}
}
