using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Floor_Minimal : UICell {

	public enum FloorState
	{
		None,
		Positive,
		Negative,

	}

	public void SetFloor (Floor floor)
	{
		m_headerText.text = "Facility: " + floor.m_name;
	}

	public void SetFloor (Floor floor, FloorState state)
	{
		SetFloor (floor);

		if (state == FloorState.Positive) {

			m_headerText.color = Color.green;

		} else if (state == FloorState.Negative) {

			m_headerText.color = Color.red;
		}
	}
}
