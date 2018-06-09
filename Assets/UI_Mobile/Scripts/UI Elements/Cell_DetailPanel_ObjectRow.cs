using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell_DetailPanel_ObjectRow : MonoBehaviour {

	[System.Serializable]
	public class RowObject
	{
		public Text m_text;
		public Image m_icon;
		public Image m_bg;

		private bool m_inUse = false;
		public bool inUse {get{ return m_inUse; }set{m_inUse = value; }}
	}

	public enum TraitState
	{
		Normal,
		HasTrait,
		MissingTrait,
		DisabledTrait,
	}

	public enum AssetState
	{
		Normal,
		HasAsset,
		MissingAsset,
	}

	public List<RowObject> m_objectList;
	public Color 
	m_normalStateColor,
	m_matchingStateColor,
	m_disabledStateColor;

	public Sprite[] m_sprites;

	// Use this for initialization
	void Start () {
		
	}

	public void AddFacility (Floor f)
	{
		foreach (RowObject ro in m_objectList) {

			if (!ro.inUse) {

				ro.m_text.gameObject.SetActive (true);
				ro.m_text.text = f.m_name;
				ro.m_icon.gameObject.SetActive (true);
				ro.m_bg.color = m_normalStateColor;

				ro.inUse = true;
				return;
			}

		}
	}

	public void AddSiteTrait (Site.SiteTraitSlot sts, TraitState tState)
	{
		
		foreach (RowObject ro in m_objectList) {

			if (!ro.inUse) {

				ro.m_text.gameObject.SetActive (true);
				ro.m_text.text = sts.m_trait.m_name;
				ro.m_icon.gameObject.SetActive (true);

				switch (tState) {

				case TraitState.Normal:

					ro.m_bg.color = m_normalStateColor;
					ro.m_icon.gameObject.SetActive (false);

					break;

				case TraitState.HasTrait:

					ro.m_bg.color = m_matchingStateColor;
					ro.m_icon.sprite = m_sprites [0];

					break;

				}

				ro.inUse = true;
				return;
			}

		}
	}

	public void AddSiteAlert (Trait t, TraitState tState)
	{
		foreach (RowObject ro in m_objectList) {

			if (!ro.inUse) {

				ro.m_text.gameObject.SetActive (true);
				ro.m_text.text = t.m_name;
				ro.m_icon.gameObject.SetActive (true);

				switch (tState) {

				case TraitState.Normal:

					ro.m_bg.color = m_normalStateColor;
					ro.m_icon.gameObject.SetActive (false);

					break;

				case TraitState.HasTrait:

					ro.m_bg.color = m_matchingStateColor;
//					ro.m_icon.sprite = m_sprites [0];

					break;

				}

				ro.inUse = true;
				return;
			}

		}
	}

	public void AddTrait (Trait t, TraitState tState)
	{

		foreach (RowObject ro in m_objectList) {

			if (!ro.inUse) {

				ro.m_text.gameObject.SetActive (true);
				ro.m_icon.gameObject.SetActive (true);
				ro.m_text.text = t.m_name;

				switch (tState) {

				case TraitState.Normal:

					ro.m_bg.color = m_normalStateColor;
					ro.m_icon.gameObject.SetActive (false);

					break;

				case TraitState.HasTrait:

					ro.m_bg.color = m_matchingStateColor;
					ro.m_icon.sprite = m_sprites [0];

					break;

				case TraitState.DisabledTrait:

					ro.m_bg.color = m_disabledStateColor;
					ro.m_icon.gameObject.SetActive (false);

					break;
				}

				ro.inUse = true;
				return;
			}
		}
	}
	
	public void AddAsset (Asset a, AssetState aState)
	{

		foreach (RowObject ro in m_objectList) {

			if (!ro.inUse) {

				ro.m_text.gameObject.SetActive (true);
				ro.m_icon.gameObject.SetActive (true);
				ro.m_text.text = a.m_name;

				switch (aState) {

				case AssetState.Normal:

					ro.m_bg.color = m_normalStateColor;
					ro.m_icon.gameObject.SetActive (false);

					break;

				case AssetState.HasAsset:

					ro.m_bg.color = m_matchingStateColor;
					ro.m_icon.sprite = m_sprites [0];

					break;

				}

				ro.inUse = true;
				return;
			}
		}
	}
}
