using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanMission_SelectSiteMenu : BaseMenu {

	public GameObject
	m_regionHeaderCellGO,
	m_siteInfoCellGO,
	m_siteAlertCellGO,
	m_siteAssetCellGO,
	m_siteTraitCellGO;

	public Transform
	m_contentParent;

//	private Lair.FloorSlot m_floorSlot;

	private MissionPlan m_missionPlan;

	private BaseMenu m_parentMenu;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);
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

			if (m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Region) {

				Button b = headerCell.m_buttons [0];
				b.interactable = true;
				b.onClick.AddListener (delegate {
					RegionSelected(r);
				});
			}

			foreach (Site s in r.sites) {

				bool validSite = true;


				if (m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Asset) {

					validSite = false;

					// don't display site if it has no valid assets

					foreach (Site.AssetSlot aSlot in s.assets) {

						if (aSlot.m_state == Site.AssetSlot.State.Revealed) {

							validSite = true;
							break;
						}
					}
				}


				if (validSite)
				{
					// create site info cell

					GameObject siteInfo = (GameObject)Instantiate (m_siteInfoCellGO, m_contentParent);
					Cell_Site siteCell = (Cell_Site)siteInfo.GetComponent<Cell_Site> ();
					siteCell.SetSite (s);

					if (m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Site) {

						Button b = siteCell.m_buttons [0];
						b.interactable = true;
						b.onClick.AddListener (delegate {
							SiteSelected(s);
						});

						b = siteCell.m_buttons [1];
						b.interactable = true;
						b.onClick.AddListener (delegate {
							SiteSelected(s);
						});
					}

					// create site trait cells

					foreach (SiteTrait t in s.traits) {

						GameObject siteTrait = (GameObject)Instantiate (m_siteTraitCellGO, m_contentParent);
						Cell_Trait siteTraitCell = (Cell_Trait)siteTrait.GetComponent<Cell_Trait> ();
						siteTraitCell.SetSiteTrait (t);
						m_cells.Add (siteTraitCell);

						if (m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.SiteTrait) {

							Button b = siteTraitCell.m_buttons [0];
							b.interactable = true;
							b.onClick.AddListener (delegate {
								SiteTraitSelected (t, s);
							});
						}
					}

					// create site asset cells

					foreach (Site.AssetSlot aSlot in s.assets) {

						GameObject siteAsset = (GameObject)Instantiate (m_siteAssetCellGO, m_contentParent);
						Cell_Asset siteAssetCell = (Cell_Asset)siteAsset.GetComponent<Cell_Asset> ();
						siteAssetCell.SetAsset (aSlot);
					
						if (m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Asset) {

							Button b = siteAssetCell.m_buttons [0];
							b.interactable = true;
							b.onClick.AddListener (delegate {
								AssetSelected (aSlot, s);
							});
						}

						m_cells.Add (siteAssetCell);
					}
				}
			}
		}
	}

	public void SiteSelected (Site s)
	{
		Debug.Log( "Site: " + s.m_siteName + " selected");

		m_missionPlan.m_missionSite = s;

		m_parentMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

	public void SiteTraitSelected (SiteTrait trait, Site s)
	{
		Debug.Log("Site Trait: " + trait.m_name + " Selected");

		m_missionPlan.m_missionSite = s;
		m_missionPlan.m_targetSiteTrait = trait;

		m_parentMenu.isDirty = true;

		ParentApp.PopMenu ();

	}

	public void RegionSelected (Region r)
	{
		m_missionPlan.m_targetRegion = r;

		m_parentMenu.isDirty = true;

		ParentApp.PopMenu ();
	}

	public void AssetSelected (Site.AssetSlot aSlot, Site s)
	{
		Debug.Log (aSlot + " / " + s);

		if (aSlot.m_state == Site.AssetSlot.State.Revealed) {

			m_missionPlan.m_missionSite = s;
			m_missionPlan.m_currentAsset = aSlot;

			m_parentMenu.isDirty = true;

			ParentApp.PopMenu ();
		}
	}

//	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
	public MissionPlan missionPlan {set{ m_missionPlan = value;}}
	public BaseMenu parentMenu {set{ m_parentMenu = value; }}
}
