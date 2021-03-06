﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Asset : UICell {

	public enum AssetState
	{
		None,
		Positive,
		Negative,
		Disabled
	}

	public void SetAsset (Asset asset)
	{
		m_headerText.text = "Asset: " + asset.m_name;
	}

	public void SetAsset(Site.AssetSlot assetSlot)
	{
		if (assetSlot.m_state == Site.AssetSlot.State.Hidden) {

			m_headerText.text = "Asset: ?????";

		} else {

//			string s = "Asset: " + assetSlot.m_asset.m_name;
			string s = assetSlot.m_asset.m_name;
			if (assetSlot.m_state == Site.AssetSlot.State.InUse) {
				s += " - In Use";
			}

			m_headerText.text = s;

			if (assetSlot.m_new) {

				m_rectTransforms [1].gameObject.SetActive (true);
			}

			List<Asset> assetsNeededForOP = (GameController.instance.GetOmegaPlan (0)).m_omegaPlan.GetNeededAssets ();

//			if (assetsNeededForOP.Contains (assetSlot.m_asset)) {
//
//				m_rectTransforms [2].gameObject.SetActive (true);
//			}

			if (assetSlot.m_asset.m_portrait != null) {

				m_rawImages [0].texture = assetSlot.m_asset.m_portrait;
			}
					
		}
	}

	public void SetAsset (Site.AssetSlot assetSlot, AssetState state)
	{
		SetAsset (assetSlot);

		if (state == AssetState.Positive) {

			m_headerText.color = Color.green;

		} else if (state == AssetState.Negative) {

			m_headerText.color = Color.red;

		} else if (state == AssetState.Disabled) {

			m_headerText.color = Color.grey;
		}
	}

	public void SetAsset (Asset asset, AssetState state)
	{
		SetAsset (asset);

		if (state == AssetState.Positive) {

			m_headerText.color = Color.green;

		} else if (state == AssetState.Negative) {

			m_headerText.color = Color.red;

		} else if (state == AssetState.Disabled) {

			m_headerText.color = Color.grey;
		}
	}
}
