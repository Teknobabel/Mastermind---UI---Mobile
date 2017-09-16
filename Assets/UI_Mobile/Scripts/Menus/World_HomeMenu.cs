﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World_HomeMenu : BaseMenu {
	
	public enum AssetTypeFilter
	{
		AllAssets,
		RevealedAssets,
		OPAssets,
	}

	public Text
	m_appNameText;

	public GameObject
	m_regionHeaderCellGO,
	m_siteInfoCellGO,
	m_siteAlertCellGO,
	m_siteAssetCellGO,
	m_siteTraitCellGO,
	m_separatorCellGO,
	m_filterOptionsCellGO;

	public Transform
	m_worldListParent;

	private AssetTypeFilter m_assetFilterType = AssetTypeFilter.AllAssets;
	private Site.Type m_siteTypeFilter = Site.Type.None;

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
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		this.gameObject.SetActive (false);
	}

	public override void OnHold ()
	{
		base.OnHold ();

		MobileUIEngine.instance.systemNavBar.SetBackButtonState (true);
	}

	public override void OnReturn ()
	{
		base.OnReturn ();

		MobileUIEngine.instance.systemNavBar.SetBackButtonState (false);
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		// display filter options button

		GameObject filterButtonGO = (GameObject)Instantiate (m_filterOptionsCellGO, m_worldListParent);
		UICell filterButton = (UICell)filterButtonGO.GetComponent<UICell> ();
		m_cells.Add (filterButton);

		// update text with current number of enabled filters

		string filterText = "Filter Options";
		int numFilters = 0;

		if (m_assetFilterType != AssetTypeFilter.AllAssets) {
			numFilters++;
		}

		if (m_siteTypeFilter != Site.Type.None) {
			numFilters++;
		}

		switch (numFilters) {

		case 1:
			filterText += "\n(1 Filter Enabled)";
			break;
		case 2:
			filterText += "\n(2 Filters Enabled)";
			break;
		}

		filterButton.m_headerText.text = filterText;

		filterButton.m_buttons[0].onClick.AddListener (delegate {
			FilterButtonPressed ();
		});

		List<Asset> assetsNeededForOP = (GameController.instance.GetOmegaPlan (0)).m_omegaPlan.GetNeededAssets ();

		List<Region> regionList = GameController.instance.GetWorld ();

		foreach (Region r in regionList) {

			// create header

//			GameObject header = (GameObject)Instantiate (m_regionHeaderCellGO, m_worldListParent);
//			Cell_Header headerCell = (Cell_Header)header.GetComponent<Cell_Header> ();
//			headerCell.SetHeader(r.m_regionName);
//			m_cells.Add (headerCell);

			foreach (Site s in r.sites) {

				// check against site type filter

				if (m_siteTypeFilter == Site.Type.None || m_siteTypeFilter == s.m_type) {

					// check against asset type filter

					bool displaySite = true;

					if (m_assetFilterType == AssetTypeFilter.RevealedAssets) {

						displaySite = false;

						foreach (Site.AssetSlot aSlot in s.assets) {

							if (aSlot.m_state == Site.AssetSlot.State.Revealed) {

								displaySite = true;
								break;
							}
						}
					} else if (m_assetFilterType == AssetTypeFilter.OPAssets) {

						displaySite = false;

						foreach (Site.AssetSlot aSlot in s.assets) {

							if (aSlot.m_state == Site.AssetSlot.State.Revealed && assetsNeededForOP.Contains(aSlot.m_asset)) {

								displaySite = true;
								break;
							}
						}
					}

					if (displaySite) {
					
						// create site info cell

						GameObject siteInfo = (GameObject)Instantiate (m_siteInfoCellGO, m_worldListParent);
						UICell siteInfoCell = (UICell)siteInfo.GetComponent<UICell> ();
						siteInfoCell.m_headerText.text = s.m_siteName;
						siteInfoCell.m_bodyText.text = s.m_type.ToString ();

						m_cells.Add (siteInfoCell);

						// create site alert cell

						GameObject siteAlert = (GameObject)Instantiate (m_siteAlertCellGO, m_worldListParent);
						UICell siteAlertCell = (UICell)siteAlert.GetComponent<UICell> ();
						m_cells.Add (siteAlertCell);

						for (int i = 0; i < siteAlertCell.m_rawImages.Length; i++) {

							if (i >= s.m_maxAlertLevel) {

								siteAlertCell.m_rawImages [i].gameObject.SetActive (false);
							} else if (i < s.currentAlertLevel) {

								siteAlertCell.m_rawImages [i].texture = siteAlertCell.m_sprites [0].texture;
							}
						}

						// create site trait cells

						foreach (Trait t in s.traits) {

							GameObject siteTrait = (GameObject)Instantiate (m_siteTraitCellGO, m_worldListParent);
							Cell_Trait siteTraitCell = (Cell_Trait)siteTrait.GetComponent<Cell_Trait> ();
							siteTraitCell.SetTrait (t);
							m_cells.Add (siteTraitCell);
						}

						// create site asset cells

						foreach (Site.AssetSlot aSlot in s.assets) {

							GameObject siteAsset = (GameObject)Instantiate (m_siteAssetCellGO, m_worldListParent);
							Cell_Asset siteAssetCell = (Cell_Asset)siteAsset.GetComponent<Cell_Asset> ();
							siteAssetCell.SetAsset (aSlot);
							m_cells.Add (siteAssetCell);
						}

						GameObject separatorCellGO = (GameObject)Instantiate (m_separatorCellGO, m_worldListParent);
						UICell separatorCell = (UICell)separatorCellGO.GetComponent<UICell> ();
						m_cells.Add (separatorCell);
					}
				}
			}
		}
	}

	public void FilterButtonPressed ()
	{
		m_parentApp.PushMenu (((WorldApp)m_parentApp).filterMenu);
	}

	public AssetTypeFilter assetFilterType {get{ return m_assetFilterType; }set{m_assetFilterType = value; }}
	public Site.Type siteTypeFilter { get { return m_siteTypeFilter; } set { m_siteTypeFilter = value; } }
}
