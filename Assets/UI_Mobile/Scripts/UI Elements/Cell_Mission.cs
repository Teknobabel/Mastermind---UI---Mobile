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

		if (mission.m_portrait != null) {

			m_image.texture = mission.m_portrait;
		}

//		m_text[0].text = mission.m_cost.ToString ();
//		m_text [1].text = mission.m_duration.ToString ();


	}

	public void DisplaySelectButton ()
	{
		m_rectTransforms [1].gameObject.SetActive (true);
	}

	public void SetMission (MissionPlan plan)
	{
		m_headerText.text = plan.m_currentMission.m_name;
		m_bodyText.text = plan.m_currentMission.m_description;
//		m_text[0].text = plan.m_currentMission.m_cost.ToString ();
//		m_text [1].text = plan.m_currentMission.m_duration.ToString ();

		if (plan.m_currentMission.m_portrait != null) {

			m_image.texture = plan.m_currentMission.m_portrait;
		}

//		if (plan.m_state == MissionPlan.State.Planning) {
//
//			m_text [0].text = plan.m_currentMission.m_cost.ToString ();
//			m_text [1].text = plan.m_currentMission.m_duration.ToString () + " Turns";
//			m_text [2].text = plan.m_successChance.ToString () + "%";

//		} else if (plan.m_state == MissionPlan.State.Active) {

//			m_text [0].text = "";
//			m_text [1].text = plan.m_turnNumber.ToString () + " / " + plan.m_currentMission.m_duration.ToString () + " Turns";
//			m_text [2].text = plan.m_successChance.ToString () + "%";
//		}

//		if (plan.m_new) {
//			m_rectTransforms [0].gameObject.SetActive (true);
//		}			
	}

	public void SetMission (MissionPlan plan, bool showFlag)
	{
		m_headerText.text = plan.m_currentMission.m_name;
		m_bodyText.text = plan.m_currentMission.m_description;
//		m_text[0].text = plan.m_currentMission.m_cost.ToString ();
//		m_text [1].text = plan.m_currentMission.m_duration.ToString ();

		if (plan.m_currentMission.m_portrait != null) {

			m_image.texture = plan.m_currentMission.m_portrait;
		}

		if (plan.m_state == MissionPlan.State.Planning) {
			
//			m_text [0].text = plan.m_currentMission.m_cost.ToString () + " CP";
//			m_text [1].text = plan.m_currentMission.m_duration.ToString () + " Turns";
//			m_text [2].text = plan.m_successChance.ToString () + "%";

		} else if (plan.m_state == MissionPlan.State.Active) {
			
//			m_text [0].text = "";
//			m_text [1].text = plan.m_turnNumber.ToString () + " / " + plan.m_currentMission.m_duration.ToString () + " Turns";
//			m_text [2].text = plan.m_successChance.ToString () + "%";
		}

		if (plan.m_new && showFlag) {
			m_rectTransforms [0].gameObject.SetActive (true);
		}			
	}

	public void SetMission (Mission mission, MissionState state)
	{
		SetMission (mission);

//		if (state == MissionState.Disabled) {
//
//			m_headerText.color = Color.gray;
//			m_bodyText.color = Color.gray;
////			m_text[0].color = Color.gray;
////			m_text[1].color = Color.gray;
//		}
	}
}
