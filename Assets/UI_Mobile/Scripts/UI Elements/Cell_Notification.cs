using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Notification : UICell {

	public void SetNotification (NotificationCenter.Notification notification)
	{
		m_bodyText.text = notification.m_title + "\n";
		m_bodyText.text += notification.m_message;

		IApp app = MobileUIEngine.instance.GetApp (notification.m_location);
		if (app != null && app.Icon != null) {
			m_image.texture = app.Icon.texture;
		}
	}
}
