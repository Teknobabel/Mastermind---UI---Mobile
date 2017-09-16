using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Assets_AssetDetailMenu : BaseMenu {

	public Text m_text;
	public Button m_button;

	private Site.AssetSlot m_assetSlot;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		base.OnEnter (animate);

		string s = "Asset: " + m_assetSlot.m_asset.m_name;

		if (m_assetSlot.m_state == Site.AssetSlot.State.InUse) {
			s += " - In Use";
//			m_button.gameObject.SetActive (false);
		} 
//		else {
//			m_button.gameObject.SetActive (true);
//		}

		m_text.text = s;

		this.gameObject.SetActive (true);

	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		this.gameObject.SetActive (false);
	}

	public void DiscardAssetButtonClicked ()
	{
		if (m_assetSlot.m_state == Site.AssetSlot.State.InUse) {

			string header = "Asset In Use";
			string message = "This Asset is being used for a Mission and cannot be discarded.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener (delegate {
				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
			});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

		} else {

			string header = "Discard Asset";
			string message = "Are you sure you want to permanently remove this Asset?";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b1 = MobileUIEngine.instance.alertDialogue.AddButton ("Discard Asset");
			b1.onClick.AddListener (delegate {
				DiscardAsset ();
			});
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Cancel");
			b2.onClick.AddListener (delegate {
				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
			});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
		}
	}

	public void DiscardAsset ()
	{

		MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();

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

	public Site.AssetSlot assetSlot {set{m_assetSlot = value;}}
}
