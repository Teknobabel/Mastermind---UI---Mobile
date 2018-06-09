using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World_HomeMenu : BaseMenu, IUIObserver  {
	
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
	m_filterOptionsCellGO,
	m_agentCellGO,
	m_noSitesCellGO,
	m_newHeader,
	m_spacer,
	m_siteAlertLevelPanel,
	m_siteTraitPanel,
	m_assetCell_Card;

	public Transform
	m_worldListParent;

	public SegmentedToggle m_infoPanelToggle;

	private AssetTypeFilter m_assetFilterType = AssetTypeFilter.AllAssets;
	private Site.Type m_siteTypeFilter = Site.Type.Economy;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		m_appNameText.text = parentApp.Name;
		m_infoPanelToggle.AddObserver (this);

		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);
		base.OnEnter (animate);

		// display first time help popup if enabled

//		int helpEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.HelpEnabled);
//		int firstTimeEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.FirstTime_World);

//		if (helpEnabled == 1 && firstTimeEnabled == 1) {
//
//			string header = "World App";
//			string message = "Each Site contains Assets you can uncover and acquire. However if you cause too much chaos, Agents will be dispatched to bring you to justice.";
//
//			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
//			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
//			b2.onClick.AddListener (delegate {
//				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
//			});
//			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_World, 0);
//
//		} else if (helpEnabled == 0 && firstTimeEnabled == 1) {
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_World, 0);
//		}
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		// clear any new flags

		foreach (KeyValuePair<int, Site> pair in GameController.instance.game.siteList) {

			if (pair.Value.visibility == Site.VisibilityState.Revealed && pair.Value.isNew) {

				Action_SetSiteNewState newState = new Action_SetSiteNewState ();
				newState.m_site = pair.Value;
				newState.m_newState = false;
				GameController.instance.ProcessAction (newState);
			}
		}

//		this.gameObject.SetActive (false);
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

		if (m_isDirty) {

			DisplayContent ();
		}
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		Player player = GameController.instance.game.playerList [0];
		List<Player.ActorSlot> henchmenPool = GameController.instance.GetHiredHenchmen (0);
		List<MissionPlan> missions = GameController.instance.GetMissions (0);

		// gather traits from currently assigned henchmen

		List<Trait> currentHenchmenTraits = new List<Trait> ();

		foreach (Player.ActorSlot aSlot in henchmenPool) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_actor != null)
			{
				foreach (Trait t in aSlot.m_actor.traits) {

					if (!currentHenchmenTraits.Contains (t)) {

						currentHenchmenTraits.Add (t);
					}
				}
			}
		}

		// gather assets

//		List<Asset> assets = new List<Asset> ();
//
//		foreach (Site.AssetSlot aSlot in player.assets) {
//
//			if (aSlot.m_state != Site.AssetSlot.State.InUse) {
//				assets.Add (aSlot.m_asset);
//			}
//		}

		List<Asset> assetsNeededForOP = (GameController.instance.GetOmegaPlan (0)).m_omegaPlan.GetNeededAssets ();

		List<Region> regionList = GameController.instance.GetWorld ();

		bool emptyList = true;

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
//					if (displaySite) { 												// debug
						emptyList = false;

						// display new flag if needed

//						if (s.isNew) {
//
//							GameObject newCell = (GameObject)Instantiate (m_newHeader, m_worldListParent);
//							UICell nCell = (UICell)newCell.GetComponent<UICell> ();
//							m_cells.Add (nCell);
//						}

						// create site info cell

						GameObject siteInfo = (GameObject)Instantiate (m_siteInfoCellGO, m_worldListParent);
						Cell_Site siteInfoCell = (Cell_Site)siteInfo.GetComponent<Cell_Site> ();
						siteInfoCell.SetSite (s);
//						siteInfoCell.m_buttons [0].gameObject.SetActive (false);
						m_cells.Add ((UICell)siteInfoCell);

						// check if site is already targeted by a mission

						bool alreadyTargeted = false;

						foreach (MissionPlan thisMP in missions) {

							if (thisMP.m_currentAsset == null && thisMP.m_missionSite != null && thisMP.m_missionSite.id == s.id) {

								alreadyTargeted = true;
								break;
							}
						}

						// set mission select button

						if (alreadyTargeted) {

							siteInfoCell.m_buttons [0].gameObject.SetActive (false);
							siteInfoCell.m_buttons [2].gameObject.SetActive (true);

							siteInfoCell.m_buttons [2].onClick.AddListener (delegate {
								DisplayBusySiteAlert();
							});

						} else {
							
							siteInfoCell.m_buttons [0].onClick.AddListener (delegate {
								SelectMissionButtonPressed (s, Mission.TargetType.Site, null, null);
							});
						}

						// add alert panel

						GameObject alertPanelGO = (GameObject)Instantiate (m_siteAlertLevelPanel, m_worldListParent);
						Cell_SiteAlertPanel alertPanel = (Cell_SiteAlertPanel)alertPanelGO.GetComponent<Cell_SiteAlertPanel> ();
						alertPanel.SetAlertLevel (s, currentHenchmenTraits);

						m_cells.Add (alertPanel);

						// create site trait cells

						if (s.traits.Count > 0) {

							GameObject siteTraitPanelGO = (GameObject)Instantiate (m_siteTraitPanel, m_worldListParent);
							Cell_DetailPanel siteTraitPanel = (Cell_DetailPanel)siteTraitPanelGO.GetComponent<Cell_DetailPanel> ();
							siteTraitPanel.SetSiteTraits (s, currentHenchmenTraits);
							m_cells.Add (siteTraitPanel);

						}

//						foreach (Site.SiteTraitSlot sts in s.traits) {
//
////							if (sts.m_state != Site.SiteTraitSlot.State.Hidden) {
//								
//								GameObject siteTrait = (GameObject)Instantiate (m_siteTraitCellGO, m_worldListParent);
//								Cell_Trait siteTraitCell = (Cell_Trait)siteTrait.GetComponent<Cell_Trait> ();
//								siteTraitCell.SetSiteTrait (sts.m_trait);
//								m_cells.Add (siteTraitCell);
//
//								Button b = siteTraitCell.m_buttons [0];
//								b.onClick.AddListener (delegate {
//									SiteTraitButtonPressed (sts.m_trait);
//								});
////							}
//						}

						// create agent cells if present

						if (s.agents.Count > 0) {

							foreach (Player.ActorSlot agentSlot in s.agents) {

								GameObject agentCellGO = (GameObject)Instantiate (m_agentCellGO, m_worldListParent);
								Cell_Actor_Small agentCell = (Cell_Actor_Small)agentCellGO.GetComponent<Cell_Actor_Small> ();
								agentCell.SetActor (agentSlot);
								m_cells.Add (agentCell);

								// set select mission button

								alreadyTargeted = false;

								foreach (MissionPlan thisMP in missions) {

									if (thisMP.m_targetActor != null && thisMP.m_targetActor.m_actor.id == agentSlot.m_actor.id) {

										alreadyTargeted = true;
										break;
									}
								}

								if (alreadyTargeted) {

									agentCell.m_buttons [0].gameObject.SetActive (false);
									agentCell.m_buttons [2].gameObject.SetActive (true);

									agentCell.m_buttons [2].onClick.AddListener (delegate {
										DisplayBusySiteAlert();
									});

								} else {

									agentCell.m_buttons [0].gameObject.SetActive (true);
									agentCell.m_buttons [0].onClick.AddListener (delegate {
										SelectMissionButtonPressed (s, Mission.TargetType.Actor, null, agentSlot);
									});
								}

								// display traits for agent

								GameObject agentTraitPanelGO = (GameObject)Instantiate (m_siteTraitPanel, m_worldListParent);
								Cell_DetailPanel agentTraitPanel = (Cell_DetailPanel)agentTraitPanelGO.GetComponent<Cell_DetailPanel> ();
								agentTraitPanel.SetAgentTraits (agentSlot, currentHenchmenTraits);
								m_cells.Add (agentTraitPanel);
							}
						}

						// create site asset cells

						if (s.assets.Count > 0) {
							GameObject spacerGO3 = (GameObject)Instantiate (m_spacer, m_worldListParent);
							Cell_Spacer spacer3 = (Cell_Spacer)spacerGO3.GetComponent<Cell_Spacer> ();
							spacer3.SetHeight (10);
							m_cells.Add (spacer3);
						}

						foreach (Site.AssetSlot aSlot in s.assets) {

//							if (aSlot.m_state != Site.AssetSlot.State.Hidden) {
							GameObject siteAsset = (GameObject)Instantiate (m_assetCell_Card, m_worldListParent);
							Cell_Asset_Card siteAssetCell = (Cell_Asset_Card)siteAsset.GetComponent<Cell_Asset_Card> ();
								siteAssetCell.SetAsset (aSlot);
								m_cells.Add (siteAssetCell);

							// set select mission button

								// check if already targeted

								bool assetAlreadyTargeted = false;

								foreach (MissionPlan thisMP in missions) {

									if (thisMP.m_currentAsset != null && thisMP.m_currentAsset == aSlot) {

										assetAlreadyTargeted = true;
										break;
									}
								}

							if (aSlot.m_state == Site.AssetSlot.State.Hidden) {
								siteAssetCell.m_buttons [0].gameObject.SetActive (false);
//								siteAssetCell.m_buttons [0].onClick.AddListener (delegate {
//									SelectMissionButtonPressed (s, Mission.TargetType.Asset_Hidden, aSlot);
//								});
							} else {

								if (assetAlreadyTargeted) {

									siteAssetCell.m_buttons [0].gameObject.SetActive (false);
									siteAssetCell.m_buttons [2].gameObject.SetActive (true);
									siteAssetCell.m_buttons [2].onClick.AddListener (delegate {
										DisplayBusySiteAlert();
									});

								} else {

									siteAssetCell.m_buttons [0].gameObject.SetActive (true);
									siteAssetCell.m_buttons [0].onClick.AddListener (delegate {
										SelectMissionButtonPressed (s, Mission.TargetType.Asset, aSlot, null);
									});
								}
							}

								if (aSlot.m_asset.m_requiredTraits.Length > 0) {

									GameObject assetTraitPanelGO = (GameObject)Instantiate (m_siteTraitPanel, m_worldListParent);
									Cell_DetailPanel assetTraitPanel = (Cell_DetailPanel)assetTraitPanelGO.GetComponent<Cell_DetailPanel> ();
									assetTraitPanel.SetTraits (aSlot.m_asset);
//									assetTraitPanel.SetSiteTraits (s, currentHenchmenTraits);
									m_cells.Add (assetTraitPanel);
								}

							GameObject spacerGO4 = (GameObject)Instantiate (m_spacer, m_worldListParent);
							Cell_Spacer spacer4 = (Cell_Spacer)spacerGO4.GetComponent<Cell_Spacer> ();
							spacer4.SetHeight (10);
							m_cells.Add (spacer4);
//							}
						}

						// display any Agents located at this Site

//						foreach (Player.ActorSlot aSlot in s.agents) {
//
//							if (aSlot.m_visibilityState == Player.ActorSlot.VisibilityState.Visible)
//							{
//								GameObject aCell = (GameObject)Instantiate (m_agentCellGO, m_worldListParent);
//								Cell_Actor_Small c = (Cell_Actor_Small)aCell.GetComponent<Cell_Actor_Small> ();
//								m_cells.Add ((UICell)c);
//								c.SetActor (aSlot);
//							}
//						}

						GameObject spacerGO2 = (GameObject)Instantiate (m_spacer, m_worldListParent);
						Cell_Spacer spacer2 = (Cell_Spacer)spacerGO2.GetComponent<Cell_Spacer> ();
						spacer2.SetHeight (100);
						m_cells.Add (spacer2);

//						GameObject separatorCellGO = (GameObject)Instantiate (m_separatorCellGO, m_worldListParent);
//						UICell separatorCell = (UICell)separatorCellGO.GetComponent<UICell> ();
//						m_cells.Add (separatorCell);
					}
				}
			}
		}

		if (emptyList) {

			// display an info cell if the list is empty

			GameObject emptyListCellGO = (GameObject)Instantiate (m_noSitesCellGO, m_worldListParent);
			UICell emptyListCell = (UICell)emptyListCellGO.GetComponent<UICell> ();
			m_cells.Add (emptyListCell);

		} else {

			// display filter options button

			// disabled for demo for now

//			GameObject filterButtonGO = (GameObject)Instantiate (m_filterOptionsCellGO, m_worldListParent);
//			UICell filterButton = (UICell)filterButtonGO.GetComponent<UICell> ();
//			m_cells.Add (filterButton);
//
//			// update text with current number of enabled filters
//
//			string filterText = "Filter Options";
//			int numFilters = 0;
//
//			if (m_assetFilterType != AssetTypeFilter.AllAssets) {
//				numFilters++;
//			}
//
//			if (m_siteTypeFilter != Site.Type.None) {
//				numFilters++;
//			}
//
//			switch (numFilters) {
//
//			case 1:
//				filterText += "\n(1 Filter Enabled)";
//				break;
//			case 2:
//				filterText += "\n(2 Filters Enabled)";
//				break;
//			}
//
//			filterButton.m_headerText.text = filterText;
//
//			filterButton.m_buttons[0].onClick.AddListener (delegate {
//				FilterButtonPressed ();
//			});


		}

	}

	public void FilterButtonPressed ()
	{
		m_parentApp.PushMenu (((WorldApp)m_parentApp).filterMenu);
	}

	public void SiteTraitButtonPressed (SiteTrait trait)
	{
		((WorldApp)m_parentApp).detailMenu.trait = trait;
		m_parentApp.PushMenu (((WorldApp)m_parentApp).detailMenu);
	}

	public void DisplayBusySiteAlert ()
	{
		string header = "Target is Busy";
		string message = "This Site, Asset or Agent is already the target of a Mission.";

		MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
		Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
		b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
		m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
	}

	public void SelectMissionButtonPressed (Site site, Mission.TargetType tType, Site.AssetSlot aSlot, Player.ActorSlot actorSlot)
	{
		MissionPlan mp = new MissionPlan ();
		mp.m_missionSite = site;
		mp.m_displayAdvancedFloors = true;
		mp.m_selectedTargetType = tType;

		if (tType == Mission.TargetType.Asset || tType == Mission.TargetType.Asset_Hidden) {

			mp.m_currentAsset = aSlot;
		} else if (tType == Mission.TargetType.Actor) {

			mp.m_targetActor = actorSlot;
		}

		Lair l = GameController.instance.GetLair (0);
		mp.m_missionOptions.Add (l.floorSlots [0]);

		((WorldApp)(m_parentApp)).planMissionMenu.missionPlan = mp;

		((WorldApp)(m_parentApp)).planMissionMenu.selectMissionMenu.missionPlan = mp;
		((WorldApp)(m_parentApp)).planMissionMenu.selectMissionMenu.parentMenu = this;
		m_parentApp.PushMenu (((WorldApp)(m_parentApp)).planMissionMenu.selectMissionMenu);
	}

	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent)
		{
		case UIEvent.UI_ToggleButtonPressed:

			switch (m_infoPanelToggle.activeButton) {
			case 0:
				m_siteTypeFilter = Site.Type.Economy;
				break;
			case 1:
				m_siteTypeFilter = Site.Type.Politics;
				break;
			case 2:
				m_siteTypeFilter = Site.Type.Military;
				break;

			}

			DisplayContent ();
			break;
		}
	}

	public AssetTypeFilter assetFilterType {get{ return m_assetFilterType; }set{m_assetFilterType = value; }}
	public Site.Type siteTypeFilter { get { return m_siteTypeFilter; } set { m_siteTypeFilter = value; } }
}
