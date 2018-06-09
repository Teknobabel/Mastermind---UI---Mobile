using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_CostPanel : UICell {

	public void SetCostPanel (Player.ActorSlot aSlot)
	{
//		m_text [0].text = aSlot.m_actor.m_startingCost.ToString ();
//		m_text[1].text = aSlot.m_actor.m_turnCost.ToString ();
//		m_text[2].text = aSlot.m_actor.m_infamyGain.ToString ();

		m_headerText.text = "+" + aSlot.m_actor.m_turnCost.ToString ();
		m_bodyText.gameObject.SetActive (false);
	}

	public void SetCostPanel (OmegaPlan.OPGoal opGoal)
	{
		SetCostPanel (opGoal.m_mission);
	}

	public void SetCostPanel (MissionPlan mp)
	{
		m_headerText.text = "+" + mp.m_currentMission.m_cost.ToString ();
		m_bodyText.text = mp.m_successChance.ToString () + "%";
	}

	public void SetCostPanel (Mission m)
	{
		m_headerText.text = "+" + m.m_cost.ToString ();
		m_bodyText.gameObject.SetActive (false);

//		m_text [0].text = m.m_cost.ToString ();
//		m_text [1].text = m.m_duration.ToString ();
//
//		m_text [4].text = "Turns";
//		m_images [1].sprite = m_sprites [0];
//
//		switch (m.m_infamy) {
//
//		case Mission.InfamyLevel.Low:
//			m_text[2].text = "1";
//			break;
//		case Mission.InfamyLevel.Medium:
//			m_text[2].text = "3";
//			break;
//		case Mission.InfamyLevel.High:
//			m_text[2].text = "5";
//			break;
//		}
	}
}
