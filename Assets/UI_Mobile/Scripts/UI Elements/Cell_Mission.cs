using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Mission : UICell {

	public enum MissionState
	{
		None,
		Enabled,
		Disabled,
	}

	public void SetMission (Mission mission)
	{
		m_headerText.text = mission.m_name;
		m_bodyText.text = mission.m_description;
		m_text[0].text = mission.m_cost.ToString () + " CP";
		m_text[1].text = mission.m_duration.ToString () + " Turns";
		m_text [2].gameObject.SetActive (false);
	}

	public void SetMission (MissionPlan plan)
	{
		m_headerText.text = plan.m_currentMission.m_name;
		m_bodyText.text = plan.m_currentMission.m_description;

		m_text[0].text = plan.m_currentMission.m_cost.ToString () + " CP";
		m_text[1].text = plan.m_currentMission.m_duration.ToString () + " Turns";
		m_text [2].text = plan.m_successChance.ToString () + "%";

		if (plan.m_new) {
			m_rectTransforms [0].gameObject.SetActive (true);
		}			
	}

	public void SetMission (MissionPlan plan, bool showFlag)
	{
		m_headerText.text = plan.m_currentMission.m_name;
		m_bodyText.text = plan.m_currentMission.m_description;

		m_text[0].text = plan.m_currentMission.m_cost.ToString () + " CP";
		m_text[1].text = plan.m_currentMission.m_duration.ToString () + " Turns";
		m_text [2].text = plan.m_successChance.ToString () + "%";

		if (plan.m_new && showFlag) {
			m_rectTransforms [0].gameObject.SetActive (true);
		}			
	}

	public void SetMission (Mission mission, MissionState state)
	{
		SetMission (mission);

		if (state == MissionState.Disabled) {

			m_headerText.color = Color.gray;
			m_bodyText.color = Color.gray;
			m_text[0].color = Color.gray;
			m_text[1].color = Color.gray;
		}
	}
}
