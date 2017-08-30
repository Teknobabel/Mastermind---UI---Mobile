using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lair_SelectSiteMenu : BaseMenu {

	public GameObject
	m_regionHeaderCellGO,
	m_siteInfoCellGO,
	m_siteAlertCellGO,
	m_siteAssetCellGO,
	m_siteTraitCellGO;

	public Transform
	m_contentParent;

	private Lair.FloorSlot m_floorSlot;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);
		//		m_appNameText.text = parentApp.Name;
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

		this.gameObject.SetActive (false);
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		List<Region> regionList = GameController.instance.GetWorld ();

		foreach (Region r in regionList) {

			// create header

			GameObject header = (GameObject)Instantiate (m_regionHeaderCellGO, m_contentParent);
			UICell headerCell = (UICell)header.GetComponent<UICell> ();
			headerCell.m_headerText.text = r.m_regionName;
			headerCell.m_headerText.color = Color.black;
			m_cells.Add (headerCell);

			if (m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Region) {

				Button b = headerCell.m_buttons [0];
				b.interactable = true;
				b.onClick.AddListener (delegate {
					RegionSelected(r);
				});
			}

			foreach (Site s in r.sites) {

				// create site info cell

				GameObject siteInfo = (GameObject)Instantiate (m_siteInfoCellGO, m_contentParent);
				UICell siteInfoCell = (UICell)siteInfo.GetComponent<UICell> ();
				siteInfoCell.m_headerText.text = s.m_siteName;
				siteInfoCell.m_bodyText.text = s.m_type.ToString ();
				m_cells.Add (siteInfoCell);

				if (m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Site) {
					
					Button b = siteInfoCell.m_buttons [0];
					b.onClick.AddListener (delegate {
						SiteSelected (s);
					});
				}

				// create site alert cell

				GameObject siteAlert = (GameObject)Instantiate (m_siteAlertCellGO, m_contentParent);
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

				foreach (SiteTrait t in s.traits) {

					GameObject siteTrait = (GameObject)Instantiate (m_siteAssetCellGO, m_contentParent);
					UICell siteTraitCell = (UICell)siteTrait.GetComponent<UICell> ();
					siteTraitCell.m_headerText.text = "Trait: " + t.m_name;
					m_cells.Add (siteTraitCell);

					if (m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.SiteTrait) {

						Button b = siteTraitCell.m_buttons [0];
						b.interactable = true;
						b.onClick.AddListener (delegate {
							SiteTraitSelected(t, s);
						});
					}
				}

				// create site asset cells

				foreach (Site.AssetSlot aSlot in s.assets) {

					GameObject siteAsset = (GameObject)Instantiate (m_siteAssetCellGO, m_contentParent);
					UICell siteAssetCell = (UICell)siteAsset.GetComponent<UICell> ();

					if (aSlot.m_state == Site.AssetSlot.State.Hidden) {

						siteAssetCell.m_headerText.text = "Asset: ?????";
					} else {
						siteAssetCell.m_headerText.text = "Asset: " + aSlot.m_asset.m_name;

					}

					if (m_floorSlot.m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Asset) {

						Button b = siteAssetCell.m_buttons [0];
						b.interactable = true;
						b.onClick.AddListener (delegate {
							AssetSelected (aSlot,s);
						});
					}

					m_cells.Add (siteAssetCell);
				}
			}
		}
	}

	public void SiteSelected (Site s)
	{
		Debug.Log( "Site: " + s.m_siteName + " selected");

		m_floorSlot.m_missionPlan.m_missionSite = s;

		((LairApp)m_parentApp).planMissionMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

	public void SiteTraitSelected (SiteTrait trait, Site s)
	{
		Debug.Log("Site Trait: " + trait.m_name + " Selected");

		m_floorSlot.m_missionPlan.m_missionSite = s;
		m_floorSlot.m_missionPlan.m_targetSiteTrait = trait;

		((LairApp)m_parentApp).planMissionMenu.isDirty = true;

		ParentApp.PopMenu ();

	}

	public void RegionSelected (Region r)
	{
		m_floorSlot.m_missionPlan.m_targetRegion = r;

		((LairApp)m_parentApp).planMissionMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

	public void AssetSelected (Site.AssetSlot aSlot, Site s)
	{
		Debug.Log (aSlot + " / " + s);

		if (aSlot.m_state == Site.AssetSlot.State.Revealed) {
			
			m_floorSlot.m_missionPlan.m_missionSite = s;
			m_floorSlot.m_missionPlan.m_currentAsset = aSlot;

			((LairApp)m_parentApp).planMissionMenu.isDirty = true;

			ParentApp.PopMenu ();
		}
	}

	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
}
