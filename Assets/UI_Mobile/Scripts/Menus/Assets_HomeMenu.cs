using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Assets_HomeMenu : BaseMenu {

	public Text
	m_appNameText,
	m_capacityText;

	public Transform
	m_contentParent,
	m_gridContentParent;

	public GameObject
	m_assetCellGO,
	m_gridAssetCellGO,
	m_headerCellGO,
	m_spacer;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		m_appNameText.text = parentApp.Name;
		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		base.OnEnter (animate);

		// display first time help popup if enabled

//		int helpEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.HelpEnabled);
//		int firstTimeEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.FirstTime_Assets);
//
//		if (helpEnabled == 1 && firstTimeEnabled == 1) {
//
//			string header = "Assets App";
//			string message = "Any Assets you've found, created, or stolen will be displayed here.";
//
//			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
//			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
//			b2.onClick.AddListener (delegate {
//				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
//			});
//			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_Assets, 0);
//
//		} else if (helpEnabled == 0 && firstTimeEnabled == 1) {
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_Assets, 0);
//		}
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		// clear out new flags

		List<Site.AssetSlot> assets = GameController.instance.GetAssets (0);

		foreach (Site.AssetSlot aSlot in assets) {
			
			if (aSlot.m_new) {

				Action_SetAssetNewState setNewState = new Action_SetAssetNewState ();
				setNewState.m_assetSlot = aSlot;
				setNewState.m_newState = false;
				GameController.instance.ProcessAction (setNewState);


			}
		}

//		this.gameObject.SetActive (false);
	}

	private void UpdateCapacity ()
	{
		Player player = GameController.instance.game.playerList [0];
		List<Site.AssetSlot> assets = GameController.instance.GetAssets (0);

		int maxAssets = player.GetMaxAssets();
		int currentAssets = 0;

		foreach (Site.AssetSlot aSlot in assets) {

			if (aSlot.m_asset != null) {

				currentAssets++;
			}
		}

		string s = "<b>Asset Capacity: " + currentAssets.ToString () + "/" + maxAssets.ToString () + "</b>";

		if (currentAssets > maxAssets) {

			int infamyPenalty = player.GetMaxAssetsPenalty ();
			s += " (+" + infamyPenalty.ToString() + " Infamy each Turn)";
			m_capacityText.color = Color.red;
		} else {
			Color c = new Color(0.2509f,0.2509f,0.2509f,1);
			m_capacityText.color = c;
		}

		m_capacityText.text = s;
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		UpdateCapacity ();

		List<Site.AssetSlot> assets = GameController.instance.GetAssets (0);

//		List<Asset> assetsNeededForOP = (GameController.instance.GetOmegaPlan (0)).m_omegaPlan.GetNeededAssets ();

//		int numAssetSlots = GameController.instance.GetNumAssetSlots (0);

		for (int i=0; i < assets.Count; i++)
		{
			Site.AssetSlot aSlot = assets [i];

//			if (i == numAssetSlots && i < assets.Count) {
//
//				GameObject headerGO = (GameObject)Instantiate (m_headerCellGO, m_contentParent);
//				Cell_Header headerCell = (Cell_Header)headerGO.GetComponent<Cell_Header> ();
//				headerCell.SetHeader("Assets over the limit");
//				m_cells.Add (headerCell);
//			}

			GameObject assetGO = (GameObject)Instantiate (m_assetCellGO, m_contentParent);
			Cell_Asset_Card assetCell = (Cell_Asset_Card)assetGO.GetComponent<Cell_Asset_Card> ();
			assetCell.SetAsset (aSlot);
			m_cells.Add (assetCell);

//			Button b = assetCell.m_buttons [0];
//			b.onClick.AddListener (delegate {
//				AssetClicked (aSlot);
//			});

			GameObject spacerGO = (GameObject)Instantiate (m_spacer, m_contentParent);
			Cell_Spacer spacer = (Cell_Spacer)spacerGO.GetComponent<Cell_Spacer> ();
			m_cells.Add (spacer);
		}

		GameObject spacerGO3 = (GameObject)Instantiate (m_spacer, m_contentParent);
		Cell_Spacer spacer3 = (Cell_Spacer)spacerGO3.GetComponent<Cell_Spacer> ();
		spacer3.SetHeight (100);
		m_cells.Add (spacer3);
	}

//	public override void DisplayContent ()
//	{
//		base.DisplayContent ();
//
//		List<Site.AssetSlot> assets = GameController.instance.GetAssets (0);
//
//		List<Asset> assetsNeededForOP = (GameController.instance.GetOmegaPlan (0)).m_omegaPlan.GetNeededAssets ();
//
//		int numAssetSlots = GameController.instance.GetNumAssetSlots (0);
//
//		for (int i=0; i < assets.Count; i++)
//		{
//			Site.AssetSlot aSlot = assets [i];
//
//			if (i == numAssetSlots && i < assets.Count) {
//
//				GameObject headerGO = (GameObject)Instantiate (m_headerCellGO, m_contentParent);
//				Cell_Header headerCell = (Cell_Header)headerGO.GetComponent<Cell_Header> ();
//				headerCell.SetHeader("Assets over the limit");
//				m_cells.Add (headerCell);
//			}
//
//			GameObject assetGO = (GameObject)Instantiate (m_assetCellGO, m_contentParent);
//			Cell_Asset assetCell = (Cell_Asset)assetGO.GetComponent<Cell_Asset> ();
//			assetCell.SetAsset (aSlot);
//			m_cells.Add (assetCell);
//
//			Button b = assetCell.m_buttons [0];
//			b.onClick.AddListener (delegate {
//				AssetClicked (aSlot);
//			});
//		}
//
////		int emptySlots = numAssetSlots - assets.Count;
//
////		if (emptySlots > 0) {
////
////			for (int i=0; i < emptySlots; i++)
////			{
////				GameObject assetGO = (GameObject)Instantiate (m_assetCellGO, m_contentParent);
////				UICell assetCell = (UICell)assetGO.GetComponent<UICell> ();
////				assetCell.m_headerText.text = "Empty";
////				assetCell.m_headerText.color = Color.gray;
////				m_cells.Add (assetCell);
////			}
////		}
//
//	}

	public void AssetClicked (Site.AssetSlot assetSlot)
	{
		((AssetsApp)m_parentApp).assetDetailMenu.assetSlot = assetSlot;
		ParentApp.PushMenu (((AssetsApp)m_parentApp).assetDetailMenu);
	}

}
