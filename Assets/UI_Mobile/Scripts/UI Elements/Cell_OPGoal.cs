using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_OPGoal : UICell {

	public void SetGoal (OmegaPlan.OPGoal goal)
	{
		m_headerText.text = goal.m_mission.m_name;
		m_bodyText.text = goal.m_state.ToString ();

		if (goal.m_new) {

			m_rectTransforms [1].gameObject.SetActive (true);
		}
	}
}
