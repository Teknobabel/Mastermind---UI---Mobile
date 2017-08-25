using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Assets_AssetDetailMenu : MonoBehaviour, IMenu {

	public Text m_text;
	public Button m_button;

	private IApp m_parentApp;

	private Site.AssetSlot m_assetSlot;

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
		this.gameObject.SetActive (false);
	}

	public void OnEnter (bool animate)
	{
		string s = "Asset: " + m_assetSlot.m_asset.m_name;

		if (m_assetSlot.m_state == Site.AssetSlot.State.InUse) {
			s += " - In Use";
			m_button.gameObject.SetActive (false);
		} else {
			m_button.gameObject.SetActive (true);
		}

		m_text.text = s;

		this.gameObject.SetActive (true);

	}

	public void OnExit (bool animate)
	{
		this.gameObject.SetActive (false);
	}

	public void OnHold ()
	{

	}

	public void OnReturn ()
	{

	}

	public void DiscardAssetButtonClicked ()
	{
		Action_RemoveAsset discardAsset = new Action_RemoveAsset ();
		discardAsset.m_assetSlot = m_assetSlot;
		discardAsset.m_playerID = 0;
		GameController.instance.ProcessAction (discardAsset);

		((AssetsApp)m_parentApp).homeMenu.isDirty = true;
		m_parentApp.PopMenu ();
	}

	public void DismissButtonClicked ()
	{
		m_parentApp.PopMenu ();
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}
	public Site.AssetSlot assetSlot {set{m_assetSlot = value;}}
}
