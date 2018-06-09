using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell_Site : UICell {

	public void SetSite (Site site)
	{
		m_headerText.text = site.m_siteName;
//		m_bodyText.text = site.m_type.ToString ();

		string description = "Level " + site.m_maxAlertLevel + " ";

		switch (site.m_type) {

		case Site.Type.Economy:
			description += "Economic Site";
			break;
		case Site.Type.Politics:
			description += "Political Site";
			break;
		case Site.Type.Military:
			description += "Military Site";
			break;
		}

		m_bodyText.text = description;
		m_image.texture = site.m_portrait;

//		if (site.isNew) {
//
//			m_rectTransforms [1].gameObject.SetActive (true);
//		}

		// create site alert cell

		for (int i = 0; i < m_rawImages.Length; i++) {

			if (i >= site.m_maxAlertLevel) {

				m_rawImages [i].gameObject.SetActive (false);
			} else if (i < site.currentAlertLevel) {

				m_rawImages [i].texture = m_sprites [0].texture;
			}
		}
	}

}
