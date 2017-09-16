using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Trait : UICell {

	public enum TraitState
	{
		None,
		Positive,
		Negative,

	}

	public void SetTrait (Trait trait)
	{
		m_headerText.text = "Trait: " + trait.m_name;
	}

	public void SetTrait (Trait trait, TraitState state)
	{
		SetTrait (trait);

		if (state == TraitState.Positive) {

			m_headerText.color = Color.green;

		} else if (state == TraitState.Negative) {

			m_headerText.color = Color.red;
		}
	}

	public void SetSiteTrait (SiteTrait st)
	{
		m_headerText.text = "Trait: " + st.m_name;
	}


}
