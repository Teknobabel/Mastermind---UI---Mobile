using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_LairFloor : UICell {

//	public void SetFloor (Lair.FloorSlot floorSlot)
//	{
//		m_headerText.text = floorSlot.m_floor.m_name;
//		m_bodyText.text = "Level " + floorSlot.m_floor.level.ToString() + "\n";
//		m_bodyText.text += "No Active Missions";
//
//		if (floorSlot.m_floor.m_missions.Count == 0) {
//
//			m_buttons[0].interactable = false;
////			b.gameObject.SetActive (false);
//
//		} else {
//
//			if (floorSlot.m_state == Lair.FloorSlot.FloorState.MissionInProgress) {
//
//				m_bodyText.color = Color.black;
//				m_bodyText.text = "Level " + floorSlot.m_floor.level.ToString() + "\n";
//				m_bodyText.text = "Mission In Progress:\n";
//				m_bodyText.text += floorSlot.m_missionPlan.m_currentMission.m_name;
//				m_bodyText.text += "(" + floorSlot.m_missionPlan.m_turnNumber.ToString () + "/" + floorSlot.m_missionPlan.m_currentMission.m_duration.ToString () + ")";
//			}
//		}
//
//		if (floorSlot.m_new) {
//			m_rectTransforms [0].gameObject.SetActive (true);
//		}
//	}

	public void SetFloor (Lair.FloorSlot floorSlot)
	{
		m_headerText.text = floorSlot.m_floor.m_name;
		m_bodyText.text = "Level " + floorSlot.m_floor.level.ToString() + " Facility";

		if (floorSlot.m_floor.m_portrait != null) {

			m_rawImages [0].texture = floorSlot.m_floor.m_portrait;
		}

//		m_bodyText.text += "No Active Missions";

		if (floorSlot.m_floor.m_missions.Count == 0) {

			m_buttons[0].interactable = false;
			//			b.gameObject.SetActive (false);

		} else {

//			if (floorSlot.m_state == Lair.FloorSlot.FloorState.MissionInProgress) {
//
//				m_bodyText.color = Color.black;
//				m_bodyText.text = "Level " + floorSlot.m_floor.level.ToString() + "\n";
//				m_bodyText.text = "Mission In Progress:\n";
//				m_bodyText.text += floorSlot.m_missionPlan.m_currentMission.m_name;
//				m_bodyText.text += "(" + floorSlot.m_missionPlan.m_turnNumber.ToString () + "/" + floorSlot.m_missionPlan.m_currentMission.m_duration.ToString () + ")";
//			}
		}

//		if (floorSlot.m_new) {
//			m_rectTransforms [1].gameObject.SetActive (true);
//		}
	}
}
