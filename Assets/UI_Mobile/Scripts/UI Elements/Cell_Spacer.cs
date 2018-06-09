using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell_Spacer : UICell {

	public LayoutElement m_layout;

	public void SetHeight (int newHeight)
	{
		m_layout.minHeight = newHeight;
	}
}
