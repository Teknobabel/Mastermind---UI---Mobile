using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Asset_Card : UICell {

	public void SetAsset (Site.AssetSlot aSlot)
	{
		if (aSlot.m_state == Site.AssetSlot.State.Revealed) {
			
			m_headerText.text = aSlot.m_asset.m_name;
			if (aSlot.m_asset.m_portrait != null) {
				m_image.texture = aSlot.m_asset.m_portrait;
			}
//			m_bodyText.text = "Level " + aSlot.m_asset.m_rank.ToString () + " Asset";
			m_bodyText.text = aSlot.m_asset.m_description;
		} else {

			m_headerText.text = "Hidden Asset";
			m_bodyText.text = "";
		}
	}
}
