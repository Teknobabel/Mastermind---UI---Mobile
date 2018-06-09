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
	m_siteTraitCellGO,
	m_spacer,
	m_cellDetailPanel,
	m_cellCostPanel,
	m_siteAlertLevelPanel;

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

//			GameObject header = (GameObject)Instantiate (m_regionHeaderCellGO, m_contentParent);
//			UICell headerCell = (UICell)header.GetComponent<UICell> ();
//			headerCell.m_headerText.text = r.m_regionName;
//			headerCell.m_headerText.color = Color.black;
//			m_cells.Add (headerCell);

//			if (m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Region) {
//
//				Button b = headerCell.m_buttons [0];
//				b.interactable = true;
//				b.onClick.AddListener (delegate {
//					RegionSelected(r);
//				});
//			}

			// gather traits from currently assigned henchmen

			List<Trait> currentHenchmenTraits = new List<Trait> ();

			foreach (Player.ActorSlot aSlot in m_missionPlan.m_actorSlots) {

				if (aSlot.m_actor != null)
				{
					foreach (Trait t in aSlot.m_actor.traits) {

						if (!currentHenchmenTraits.Contains (t)) {

							currentHenchmenTraits.Add (t);
						}
					}
				}
			}

			foreach (Site s in r.sites) {

				bool validSite = true;

				Debug.Log (m_missionPlan.m_currentMission.m_targetType);
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
				else if (m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Site && s.visibility == Site.VisibilityState.Hidden) {

					validSite = false;
				}


				if (validSite)
				{
					// create site info cell

					GameObject siteInfo = (GameObject)Instantiate (m_siteInfoCellGO, m_contentParent);
					Cell_Site siteCell = (Cell_Site)siteInfo.GetComponent<Cell_Site> ();
					siteCell.SetSite (s);
					m_cells.Add (siteCell);

					// create alert level panel

					GameObject alertPanelGO = (GameObject)Instantiate (m_siteAlertLevelPanel, m_contentParent);
					Cell_SiteAlertPanel alertPanel = (Cell_SiteAlertPanel)alertPanelGO.GetComponent<Cell_SiteAlertPanel> ();
					alertPanel.SetAlertLevel (s, currentHenchmenTraits);
					m_cells.Add (alertPanel);

					// create site trait panel

					if (s.traits.Count > 0) {

						GameObject siteTraitPanelGO = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
						Cell_DetailPanel siteTraitPanel = (Cell_DetailPanel)siteTraitPanelGO.GetComponent<Cell_DetailPanel> ();
						siteTraitPanel.SetSiteTraits (s, currentHenchmenTraits);
						m_cells.Add (siteTraitPanel);

					}

					// enable button for site selection

					if (m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Site) {

						Button b = siteCell.m_buttons [0];
						b.interactable = true;
						b.onClick.AddListener (delegate {
							SiteSelected(s);
						});

//						b = siteCell.m_buttons [1];
//						b.interactable = true;
//						b.onClick.AddListener (delegate {
//							SiteSelected(s);
//						});
					} else {
						siteCell.m_buttons [0].gameObject.SetActive (false);

					}
						

					// create revealed asset cells

					if (s.assets.Count > 0) {
						GameObject spacerGO3 = (GameObject)Instantiate (m_spacer, m_contentParent);
						Cell_Spacer spacer3 = (Cell_Spacer)spacerGO3.GetComponent<Cell_Spacer> ();
						spacer3.SetHeight (10);
						m_cells.Add (spacer3);
					}

					foreach (Site.AssetSlot aSlot in s.assets) {

						if (aSlot.m_state != Site.AssetSlot.State.Hidden) {
							GameObject siteAsset = (GameObject)Instantiate (m_siteAssetCellGO, m_contentParent);
							Cell_Asset_Card siteAssetCell = (Cell_Asset_Card)siteAsset.GetComponent<Cell_Asset_Card> ();
							siteAssetCell.SetAsset (aSlot);
							m_cells.Add (siteAssetCell);

							if (m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Asset) {

								Button b = siteAssetCell.m_buttons [0];
								b.gameObject.SetActive (true);
								b.interactable = true;
								b.onClick.AddListener (delegate {
									AssetSelected (aSlot, s);
								});

								// show linked traits if needed

								if (aSlot.m_asset.m_requiredTraits.Length > 0) {

									GameObject siteTraitPanelGO = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
									Cell_DetailPanel siteTraitPanel = (Cell_DetailPanel)siteTraitPanelGO.GetComponent<Cell_DetailPanel> ();
									siteTraitPanel.SetTraits (aSlot.m_asset);
									m_cells.Add (siteTraitPanel);

									GameObject spacerGO5 = (GameObject)Instantiate (m_spacer, m_contentParent);
									Cell_Spacer spacer5 = (Cell_Spacer)spacerGO5.GetComponent<Cell_Spacer> ();
									spacer5.SetHeight (10);
									m_cells.Add (spacer5);
								}

							} else {

								siteAssetCell.m_buttons [0].gameObject.SetActive (false);
							}

							GameObject spacerGO4 = (GameObject)Instantiate (m_spacer, m_contentParent);
							Cell_Spacer spacer4 = (Cell_Spacer)spacerGO4.GetComponent<Cell_Spacer> ();
							spacer4.SetHeight (10);
							m_cells.Add (spacer4);
						}
					}

					GameObject spacerGO2 = (GameObject)Instantiate (m_spacer, m_contentParent);
					Cell_Spacer spacer2 = (Cell_Spacer)spacerGO2.GetComponent<Cell_Spacer> ();
					spacer2.SetHeight (150);
					m_cells.Add (spacer2);
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
