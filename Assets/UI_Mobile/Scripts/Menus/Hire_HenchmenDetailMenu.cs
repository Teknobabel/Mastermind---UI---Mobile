using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hire_HenchmenDetailMenu : ContactsDetailViewMenu {

	public Text m_cost;

	public override void OnEnter (bool animate)
	{
		Henchmen h = GetDummyData.instance.GetHenchmen (m_henchmenID);

		if (h != null) {

			m_cost.text = "-" + h.m_cost.ToString ();

		}

		base.OnEnter (animate);
	}


	public void HireButtonPressed ()
	{

	}
}
