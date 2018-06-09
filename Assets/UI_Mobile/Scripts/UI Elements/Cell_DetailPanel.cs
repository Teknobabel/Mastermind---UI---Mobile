using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_DetailPanel : UICell {

	public enum MissionState
	{
		Normal,
		Planning,
		Active,
	}

	public Cell_DetailPanel_ObjectRow m_row;

	public List<Cell_DetailPanel_ObjectRow> m_rows;

	public void SetTraits (Player.ActorSlot aSlot, MissionPlan mp, MissionState mState)
	{
		m_headerText.text = "TRAITS";

		foreach (Trait t in aSlot.m_actor.traits) {

			Cell_DetailPanel_ObjectRow.TraitState tState = Cell_DetailPanel_ObjectRow.TraitState.Normal;

			if (mState == MissionState.Planning && mp.m_matchingTraits.Contains (t)) {

				tState = Cell_DetailPanel_ObjectRow.TraitState.HasTrait;
			}

			AddTrait (t, tState);
		}
	}

//	public void SetTraits (Player.ActorSlot aSlot)
//	{
//		m_headerText.text = "TRAITS";
//
//		foreach (Trait t in aSlot.m_actor.traits) {
//
//			AddTrait (t, Cell_DetailPanel_ObjectRow.TraitState.Normal);
//		}
//	}

	public void SetAgentTraits (Player.ActorSlot aSlot, List<Trait> currentTraits)
	{
		m_headerText.text = "TRAITS";

		foreach (Trait t in aSlot.m_actor.traits) {

			Cell_DetailPanel_ObjectRow.TraitState tState = Cell_DetailPanel_ObjectRow.TraitState.Normal;

			if (currentTraits.Contains (t)) {

				tState = Cell_DetailPanel_ObjectRow.TraitState.HasTrait;
			}

			AddTrait (t, tState);
		}
	}

	public void SetTraits (Player.ActorSlot aSlot)
	{
		m_headerText.text = "TRAITS";

//		List<Trait> tList = new List<Trait> ();

		if (aSlot.m_actor.traits [0].m_name != "Agent") {
			for (int i = 0; i < aSlot.m_actor.m_upgradeTraits.Length + 1; i++) {

				if (i < aSlot.m_actor.level) {

//				tList.Add(aSlot.m_actor.traits[0]);
					AddTrait (aSlot.m_actor.traits [i], Cell_DetailPanel_ObjectRow.TraitState.Normal);

				} else {


//				tList.Add(aSlot.m_actor.m_upgradeTraits[i-1]);
					AddTrait (aSlot.m_actor.m_upgradeTraits [i - 1], Cell_DetailPanel_ObjectRow.TraitState.DisabledTrait);


				}
			}
		} else {

			foreach (Trait t in aSlot.m_actor.traits) {

				AddTrait (t, Cell_DetailPanel_ObjectRow.TraitState.Normal);
			}
		}

//		foreach (Trait t in tList) {
//
//			AddTrait (t, Cell_DetailPanel_ObjectRow.TraitState.Normal);
//		}
	}

	public void SetTraits (Mission m, MissionState mState)
	{
		m_headerText.text = "REQUIRED TRAITS";

		foreach (Trait t in m.m_requiredTraits) {

			AddTrait (t, Cell_DetailPanel_ObjectRow.TraitState.Normal);
		}
	}

	public void SetTraits (Mission m, List<Trait> currentTraits)
	{
		m_headerText.text = "REQUIRED TRAITS";

		foreach (Trait t in m.m_requiredTraits) {

			Cell_DetailPanel_ObjectRow.TraitState tState = Cell_DetailPanel_ObjectRow.TraitState.Normal;

			if (currentTraits.Contains (t)) {

				tState = Cell_DetailPanel_ObjectRow.TraitState.HasTrait;
			}

			AddTrait (t, tState);
		}
	}

	public void SetTraits (MissionPlan mp, MissionState mState)
	{
		m_headerText.text = "REQUIRED TRAITS";

		foreach (Trait t in mp.m_requiredTraits) {

			Cell_DetailPanel_ObjectRow.TraitState tState = Cell_DetailPanel_ObjectRow.TraitState.Normal;

			if (mState == MissionState.Planning && mp.m_matchingTraits.Contains (t)) {

				tState = Cell_DetailPanel_ObjectRow.TraitState.HasTrait;
			}

			AddTrait (t, tState);
		}
	}

	// traits in response to asset

	public void SetTraits (Asset a)
	{
		m_headerText.text = "REQUIRED TRAITS";

		foreach (Trait t in a.m_requiredTraits) {

			AddTrait (t, Cell_DetailPanel_ObjectRow.TraitState.Normal);
		}
	}

	public void SetAssets (Mission m, MissionState mState)
	{
		m_headerText.text = "REQUIRED ASSETS";

		List<Asset> playerAssets = new List<Asset> ();

		if (mState == MissionState.Planning) {

			List<Site.AssetSlot> aSlot = GameController.instance.GetAssets (0);

			foreach (Site.AssetSlot a in aSlot) {

				if (a.m_asset != null) {
					playerAssets.Add (a.m_asset);
				}
			}
		}

		foreach (Asset a in m.m_requiredAssets) {

			Cell_DetailPanel_ObjectRow.AssetState aState = Cell_DetailPanel_ObjectRow.AssetState.Normal;

			if (mState == MissionState.Planning) {

				for (int i = 0; i < playerAssets.Count; i++) {

					Asset thisAsset = playerAssets [i];

					if (thisAsset.m_name == a.m_name) {

						aState = Cell_DetailPanel_ObjectRow.AssetState.HasAsset;
						playerAssets.RemoveAt(i);
						break;
					}
				}
			}

			AddAsset (a, aState);
		}

//		foreach (Asset a in m.m_requiredAssets) {
//
//			AddAsset (a, Cell_DetailPanel_ObjectRow.AssetState.Normal);
//		}
	}

	public void SetAssets (MissionPlan mp, MissionState mState)
	{
		m_headerText.text = "REQUIRED ASSETS";

		List<Asset> playerAssets = new List<Asset> ();

		if (mState == MissionState.Planning) {
			
			List<Site.AssetSlot> aSlot = GameController.instance.GetAssets (0);

			foreach (Site.AssetSlot a in aSlot) {

				if (a.m_asset != null) {
					playerAssets.Add (a.m_asset);
				}
			}
		}

		foreach (Asset a in mp.m_requiredAssets) {

			Cell_DetailPanel_ObjectRow.AssetState aState = Cell_DetailPanel_ObjectRow.AssetState.Normal;

			if (mState == MissionState.Planning) {

				for (int i = 0; i < playerAssets.Count; i++) {

					Asset thisAsset = playerAssets [i];

					if (thisAsset.m_name == a.m_name) {

						aState = Cell_DetailPanel_ObjectRow.AssetState.HasAsset;
						playerAssets.RemoveAt(i);
						break;
					}
				}
			}

			AddAsset (a, aState);
		}
	}

	public void SetSiteTraits (Site s)
	{
		m_headerText.text = "REQUIRED TRAITS";

		foreach (Site.SiteTraitSlot sts in s.traits) {

			AddSiteTrait (sts, Cell_DetailPanel_ObjectRow.TraitState.Normal);
		}
	}

	public void SetSiteTraits (Site s, List<Trait> currentTraits)
	{
		m_headerText.text = "REQUIRED TRAITS";

		foreach (Site.SiteTraitSlot sts in s.traits) {

			Cell_DetailPanel_ObjectRow.TraitState tState = Cell_DetailPanel_ObjectRow.TraitState.Normal;

			if (currentTraits.Contains (sts.m_trait.m_requiredTrait)) {

				tState = Cell_DetailPanel_ObjectRow.TraitState.HasTrait;
			}

			AddSiteTrait (sts, tState);
		}
	}

	public void SetFacilities (Mission m, MissionState mState)
	{
		m_headerText.text = "REQUIRED FACILITIES";

		foreach (Floor f in m.m_requiredFloors) {

			AddFacility (f);
		}
	}

	public void SetFacilities (MissionPlan mp, MissionState mState)
	{
		m_headerText.text = "REQUIRED FACILITIES";

		foreach (Floor f in mp.m_requiredFloors) {

			AddFacility (f);
		}
	}

	private void AddSiteTrait (Site.SiteTraitSlot sts, Cell_DetailPanel_ObjectRow.TraitState tState)
	{
		foreach (Cell_DetailPanel_ObjectRow r in m_rows) {

			if (!r.gameObject.activeSelf) {

				r.gameObject.SetActive (true);
			}

			foreach (Cell_DetailPanel_ObjectRow.RowObject ro in r.m_objectList) {

				if (!ro.inUse) {

					r.AddSiteTrait (sts, tState);
					return;
				}
			}
		}
	}

	private void AddFacility (Floor f)
	{
		foreach (Cell_DetailPanel_ObjectRow r in m_rows) {

			if (!r.gameObject.activeSelf) {

				r.gameObject.SetActive (true);
			}

			foreach (Cell_DetailPanel_ObjectRow.RowObject ro in r.m_objectList) {

				if (!ro.inUse) {

					r.AddFacility (f);
					return;
				}
			}
		}
	}

	private void AddTrait (Trait t, Cell_DetailPanel_ObjectRow.TraitState tState)
	{
		foreach (Cell_DetailPanel_ObjectRow r in m_rows) {

			if (!r.gameObject.activeSelf) {

				r.gameObject.SetActive (true);
			}

			foreach (Cell_DetailPanel_ObjectRow.RowObject ro in r.m_objectList) {

				if (!ro.inUse) {

					r.AddTrait (t, tState);
					return;
				}
			}
		}
	}

	private void AddAsset (Asset a, Cell_DetailPanel_ObjectRow.AssetState aState)
	{
		foreach (Cell_DetailPanel_ObjectRow r in m_rows) {

			if (!r.gameObject.activeSelf) {

				r.gameObject.SetActive (true);
			}

			foreach (Cell_DetailPanel_ObjectRow.RowObject ro in r.m_objectList) {

				if (!ro.inUse) {

					r.AddAsset (a, aState);
					return;
				}
			}
		}
	}
}
