using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_OPGoal : UICell {

	public void SetGoal (OmegaPlan.OPGoal goal)
	{
		m_headerText.text = goal.m_mission.m_name;
//		m_bodyText.text = goal.m_state.ToString ();
		m_bodyText.text = goal.m_mission.m_description;
//		m_text [0].text = goal.m_mission.m_cost.ToString ();
//		m_text [1].text = goal.m_mission.m_duration.ToString ();

		if (goal.m_state == OmegaPlan.OPGoal.State.Locked) {

			m_rawImages [0].gameObject.SetActive (false);
			m_rawImages [1].gameObject.SetActive (true);

		} else if (goal.m_state == OmegaPlan.OPGoal.State.Complete) {

			m_rawImages [0].gameObject.SetActive (false);
			m_rawImages [2].gameObject.SetActive (true);

		} else if (goal.m_state == OmegaPlan.OPGoal.State.InProgress) {

			m_rawImages [0].gameObject.SetActive (false);
			m_rawImages [3].gameObject.SetActive (true);
		}

		if (goal.m_mission.m_portrait != null) {

			m_image.texture = goal.m_mission.m_portrait;
		}

//		if (goal.m_new) {
//
//			m_rectTransforms [1].gameObject.SetActive (true);
//		}
	}
}
