using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Effect : UICell {

	public void SetEffect (Effect effect)
	{
		m_headerText.text = "Effect: " + effect.m_effectName;
	}
}
