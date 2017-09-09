﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Assets_HomeMenu : BaseMenu {

	public Text
	m_appNameText;

	public Transform
	m_contentParent;

	public GameObject
	m_assetCellGO,
	m_headerCellGO;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		m_appNameText.text = parentApp.Name;
		//		m_infoPanelToggle.AddObserver (this);
		//		m_infoPanelToggle.ToggleButtonClicked (0);
		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		base.OnEnter (animate);

		this.gameObject.SetActive (true);

		DisplayContent ();
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

		this.gameObject.SetActive (false);
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		List<Site.AssetSlot> assets = GameController.instance.GetAssets (0);

		List<Asset> assetsNeededForOP = (GameController.instance.GetOmegaPlan (0)).m_omegaPlan.GetNeededAssets ();

		int numAssetSlots = GameController.instance.GetNumAssetSlots (0);

		for (int i=0; i < assets.Count; i++)
		{
			Site.AssetSlot aSlot = assets [i];

			if (i == numAssetSlots && i < assets.Count) {

				GameObject headerGO = (GameObject)Instantiate (m_headerCellGO, m_contentParent);
				UICell headerCell = (UICell)headerGO.GetComponent<UICell> ();
				headerCell.m_headerText.text = "Assets over the limit";
				m_cells.Add (headerCell);
			}

			GameObject assetGO = (GameObject)Instantiate (m_assetCellGO, m_contentParent);
			UICell assetCell = (UICell)assetGO.GetComponent<UICell> ();

			string s = aSlot.m_asset.m_name;
			if (aSlot.m_state == Site.AssetSlot.State.InUse) {
				s += " - In Use";
			}

			assetCell.m_headerText.text = s;
			m_cells.Add (assetCell);

			if (aSlot.m_new) {

				assetCell.m_rectTransforms [1].gameObject.SetActive (true);
			}

			if (assetsNeededForOP.Contains (aSlot.m_asset)) {

				assetCell.m_rectTransforms [2].gameObject.SetActive (true);
			}

			Button b = assetCell.m_buttons [0];
			b.onClick.AddListener (delegate {
				AssetClicked (aSlot);
			});
		}

		int emptySlots = numAssetSlots - assets.Count;

		if (emptySlots > 0) {

			for (int i=0; i < emptySlots; i++)
			{
				GameObject assetGO = (GameObject)Instantiate (m_assetCellGO, m_contentParent);
				UICell assetCell = (UICell)assetGO.GetComponent<UICell> ();
				assetCell.m_headerText.text = "Empty";
				assetCell.m_headerText.color = Color.gray;
				m_cells.Add (assetCell);
			}
		}

	}

	public void AssetClicked (Site.AssetSlot assetSlot)
	{
		((AssetsApp)m_parentApp).assetDetailMenu.assetSlot = assetSlot;
		ParentApp.PushMenu (((AssetsApp)m_parentApp).assetDetailMenu);
	}

}
