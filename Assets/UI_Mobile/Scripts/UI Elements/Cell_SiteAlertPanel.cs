using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_SiteAlertPanel : UICell {

	public Cell_DetailPanel_ObjectRow m_row;

	public void SetAlertLevel (Site s, List<Trait> currentTraits)
	{

		for (int i = 0; i < m_row.m_objectList.Count; i++) {
			


			if (i < s.m_maxAlertLevel) {

				Cell_DetailPanel_ObjectRow.RowObject ro = m_row.m_objectList [i];
				
				ro.m_icon.gameObject.SetActive (true);

				if (i < s.currentAlertLevel) {

					foreach (Director.AlertData aData in GameEngine.instance.game.director.m_alertData) {

						Cell_DetailPanel_ObjectRow.TraitState tState = Cell_DetailPanel_ObjectRow.TraitState.Normal;

						if (aData.m_siteType == s.m_type && i < aData.m_traitList.Length) {

							Trait t = aData.m_traitList [i];

							if (currentTraits.Contains (t)) {

								tState = Cell_DetailPanel_ObjectRow.TraitState.HasTrait;
							}

							m_row.AddSiteAlert (t,tState);

							break;
						}
					}
				}
			} else {

				m_row.m_objectList [i].m_bg.gameObject.SetActive (false);

			}

		}
	}
}
