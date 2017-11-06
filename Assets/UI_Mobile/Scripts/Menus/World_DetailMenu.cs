using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World_DetailMenu : BaseMenu {

	public Text m_text;
	public Button m_button;

	private SiteTrait m_trait;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		base.OnEnter (animate);

		string s = m_trait.GetDescription();

//		if (m_assetSlot.m_state == Site.AssetSlot.State.InUse) {
//			s += " - In Use";
//		} 

		m_text.text = s;

		this.gameObject.SetActive (true);

	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		this.gameObject.SetActive (false);
	}

	public void DismissButtonClicked ()
	{
		m_parentApp.PopMenu ();
	}

	public SiteTrait trait {set{ m_trait = value; }}
}
