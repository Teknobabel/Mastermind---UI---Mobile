using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanMissionMenu : BaseMenu {

	public GameObject
	m_missionOverviewCellGO,
	m_selectMissionCellGO,
	m_selectSiteCellGO,
	m_selectHenchmenCellGO,
	m_traitCellGO,
	m_assetCellGO,
	m_effectCellGO,
	m_floorMinimalCellGO,
	m_startMissionCellGO,
	m_cancelMissionCellGO,
	m_repeatMissionCellGO,
	m_emptyMissionCellGO,
	m_emptyHenchmenCellGO,
	m_emptyTargetCellGO,
	m_missionStatsCompiledCellGO,
	m_separatorCellGO,
	m_headerCellGO,
	m_spacer,
	m_cellDetailPanel,
	m_cellCostPanel,
	m_missionSummaryPanel,
	m_siteAlertLevelPanel;

	public GameObject
		m_selectHenchmenMenuGO,
		m_selectMissionMenuGO,
		m_selectSiteMenuGO,
		m_selectTargetActorMenuGO;

	public Text
	m_locationText;

	public Transform
	m_contentParent;

//	private Lair.FloorSlot m_floorSlot;

	private MissionPlan m_missionPlan;

	private PlanMission_SelectHenchmenMenu m_selectHenchmenMenu;
	private PlanMission_SelectMissionMenu m_selectMissionMenu;
	private PlanMission_SelectSiteMenu m_selectSiteMenu;
	private PlanMission_SelectTargetActorMenu m_selectTargetActorMenu;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);
		this.gameObject.SetActive (false);

		GameObject selectHenchmenGO = (GameObject)GameObject.Instantiate (m_selectHenchmenMenuGO, Vector3.zero, Quaternion.identity);
		selectHenchmenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_selectHenchmenMenu = (PlanMission_SelectHenchmenMenu)selectHenchmenGO.GetComponent<PlanMission_SelectHenchmenMenu>();
		m_selectHenchmenMenu.Initialize (parentApp);
		m_selectHenchmenMenu.parentMenu = this;

		GameObject selectMissionGO = (GameObject)GameObject.Instantiate (m_selectMissionMenuGO, Vector3.zero, Quaternion.identity);
		selectMissionGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_selectMissionMenu = (PlanMission_SelectMissionMenu)selectMissionGO.GetComponent<PlanMission_SelectMissionMenu>();
		m_selectMissionMenu.Initialize (parentApp);
		m_selectMissionMenu.parentMenu = this;

		GameObject selectSiteGO = (GameObject)GameObject.Instantiate (m_selectSiteMenuGO, Vector3.zero, Quaternion.identity);
		selectSiteGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_selectSiteMenu = (PlanMission_SelectSiteMenu)selectSiteGO.GetComponent<PlanMission_SelectSiteMenu>();
		m_selectSiteMenu.Initialize (parentApp);
		m_selectSiteMenu.parentMenu = this;

		GameObject selectTargetActorGO = (GameObject)GameObject.Instantiate (m_selectTargetActorMenuGO, Vector3.zero, Quaternion.identity);
		selectTargetActorGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_selectTargetActorMenu = (PlanMission_SelectTargetActorMenu)selectTargetActorGO.GetComponent<PlanMission_SelectTargetActorMenu>();
		m_selectTargetActorMenu.Initialize (parentApp);
		m_selectTargetActorMenu.parentMenu = this;
	}

	public override void OnEnter (bool animate)
	{
		// recompile to account for any changes since last visit

		if (m_missionPlan.m_state == MissionPlan.State.Planning) {
			
			GameController.instance.CompileMission (m_missionPlan);
		}
	
		if (m_missionPlan.m_floorSlot != null && m_missionPlan.m_floorSlot.m_floor != null) {

			m_locationText.gameObject.SetActive (true);
			m_locationText.text = m_missionPlan.m_floorSlot.m_floor.m_name + ":";

		} else if (m_locationText.gameObject.activeSelf) {

			m_locationText.gameObject.SetActive (false);
		}

		this.gameObject.SetActive (true);

		base.OnEnter (animate);

		// display first time help popup if enabled

//		int helpEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.HelpEnabled);
//		int firstTimeEnabled = MobileUIEngine.instance.settingsManager.GetPref (SettingsManager.PlayerPrefKeys.FirstTime_PlanMission);
//
//		if (helpEnabled == 1 && firstTimeEnabled == 1) {
//
//			string header = "Planning Missions";
//			string message = "Planning a Mission usually requires you to choose a Mission, a Target, and a Henchmen to carry it out. Each Mission will require certain Traits, the more matching " +
//				"traits your Henchmen has, the greater the chance of success.";
//
//			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
//			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
//			b2.onClick.AddListener (delegate {
//				MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
//			});
//			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_PlanMission, 0);
//
//		} else if (helpEnabled == 0 && firstTimeEnabled == 1) {
//
//			MobileUIEngine.instance.settingsManager.SetPref (SettingsManager.PlayerPrefKeys.FirstTime_PlanMission, 0);
//		}
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		this.gameObject.SetActive (false);
	}

	public override void OnReturn ()
	{
		if (m_isDirty) {

//			m_floorSlot.m_missionPlan.m_actorSlots = m_floorSlot.m_actorSlots;

			GameController.instance.CompileMission (m_missionPlan);
		}

		base.OnReturn (); 
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		// display mission overview cell

//		GameObject missionOverviewCellGO = (GameObject)Instantiate (m_missionOverviewCellGO, m_contentParent);
//		UICell missionOverviewCell = (UICell)missionOverviewCellGO.GetComponent<UICell> ();
//		m_cells.Add (missionOverviewCell);
//		missionOverviewCell.m_bodyText.text = "Success Chance: " + m_missionPlan.m_successChance.ToString () + "%";


		int stepNum = 1;


		// display current state of mission selection

		GameObject selectMissionHeaderCellGO = (GameObject)Instantiate (m_headerCellGO, m_contentParent);
		Cell_Header selectMissionHeaderCell = (Cell_Header)selectMissionHeaderCellGO.GetComponent<Cell_Header> ();
		string headerString = "Step " + stepNum.ToString() + ":";
		stepNum++;
		if (m_missionPlan.m_currentMission != null) {
			headerString += " Complete";
		}
		selectMissionHeaderCell.SetHeader (headerString.ToUpper());
		m_cells.Add ((UICell)selectMissionHeaderCell);

		if (m_missionPlan.m_currentMission != null) {

			GameObject selectMissionCellGO = (GameObject)Instantiate (m_selectMissionCellGO, m_contentParent);
			Cell_Mission selectMissionCell = (Cell_Mission)selectMissionCellGO.GetComponent<Cell_Mission> ();
			selectMissionCell.SetMission (m_missionPlan, false);
			m_cells.Add ((UICell)selectMissionCell);

			// set detail panel - traits

			if (m_missionPlan.m_currentMission.m_requiredTraits.Length > 0) {

				Cell_DetailPanel.MissionState tState = Cell_DetailPanel.MissionState.Normal;

				if (m_missionPlan.m_state == MissionPlan.State.Planning) {

					tState = Cell_DetailPanel.MissionState.Planning;
				}

				GameObject traitPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
				Cell_DetailPanel dPanel = (Cell_DetailPanel)traitPanel.GetComponent<Cell_DetailPanel> ();
				dPanel.SetTraits (m_missionPlan, tState);
				m_cells.Add (dPanel);
			}

			// set detail panel - assets

			if (m_missionPlan.m_currentMission.m_requiredAssets.Length > 0) {

				Cell_DetailPanel.MissionState tState = Cell_DetailPanel.MissionState.Normal;

				if (m_missionPlan.m_state == MissionPlan.State.Planning) {

					tState = Cell_DetailPanel.MissionState.Planning;
				}

				GameObject assetPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
				Cell_DetailPanel aPanel = (Cell_DetailPanel)assetPanel.GetComponent<Cell_DetailPanel> ();
				aPanel.SetAssets (m_missionPlan, tState);
				m_cells.Add (aPanel);
			}

			// set detail panel - facilities

			if (m_missionPlan.m_currentMission.m_requiredFloors.Length > 0) {

				GameObject facilityPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
				Cell_DetailPanel fPanel = (Cell_DetailPanel)facilityPanel.GetComponent<Cell_DetailPanel> ();
				fPanel.SetFacilities (m_missionPlan.m_currentMission, Cell_DetailPanel.MissionState.Normal);
				m_cells.Add (fPanel);
			}

			// set cost panel

			GameObject costPanel = (GameObject)Instantiate (m_cellCostPanel, m_contentParent);
			Cell_CostPanel cPanel = (Cell_CostPanel)costPanel.GetComponent<Cell_CostPanel> ();
			cPanel.SetCostPanel (m_missionPlan.m_currentMission);
			m_cells.Add (cPanel);





			if (m_missionPlan.m_state == MissionPlan.State.Planning && m_missionPlan.m_missionOptions.Count > 0) {

//				selectMissionCell.DisplaySelectButton ();
				selectMissionCell.m_buttons[0].gameObject.SetActive(false);
				selectMissionCell.m_buttons[1].gameObject.SetActive(true);

				Button b = selectMissionCell.m_buttons [1];
				b.onClick.AddListener (delegate {
					SelectMissionButtonPressed ();
				});

//				Button b = selectMissionCell.m_buttons [0];
//				b.onClick.AddListener (delegate {
//					SelectMissionButtonPressed ();
//				});
//				b = selectMissionCell.m_buttons [1];
//				b.onClick.AddListener (delegate {
//					SelectMissionButtonPressed ();
//				});

			} 

			GameObject spacerGO2 = (GameObject)Instantiate (m_spacer, m_contentParent);
			Cell_Spacer spacer2 = (Cell_Spacer)spacerGO2.GetComponent<Cell_Spacer> ();
			m_cells.Add (spacer2);

		} else {

			GameObject selectMissionCellGO = (GameObject)Instantiate (m_emptyMissionCellGO, m_contentParent);
			UICell selectMissionCell = (UICell)selectMissionCellGO.GetComponent<UICell> ();
			m_cells.Add (selectMissionCell);

			Button b = selectMissionCell.m_buttons [0];
			b.onClick.AddListener (delegate {
				SelectMissionButtonPressed ();
			});
		}





		// if mission has been compiled, display traits

//		foreach (Trait t in m_missionPlan.m_requiredTraits) {
//
//			GameObject traitCellGO = (GameObject)Instantiate (m_traitCellGO, m_contentParent);
//			Cell_Trait traitCell = (Cell_Trait)traitCellGO.GetComponent<Cell_Trait> ();
//			m_cells.Add (traitCell);
//
//			if (m_missionPlan.m_matchingTraits.Contains (t)) {
//
//				traitCell.SetTrait (t, Cell_Trait.TraitState.Positive);
//			} else {
//				traitCell.SetTrait (t);
//			}
//		}

//		// if mission has been compiled, display assets
//
//		if (m_missionPlan.m_requiredAssets.Count > 0) {
//
//			Player player = GameEngine.instance.game.playerList [0];
//			List<Asset> assets = new List<Asset> ();
//
//			foreach (Site.AssetSlot aSlot in m_missionPlan.m_linkedPlayerAssets) {
//
//				assets.Add (aSlot.m_asset);
//			}
//
//			foreach (Asset a in m_missionPlan.m_requiredAssets) {
//
//				GameObject assetCellGO = (GameObject)Instantiate (m_assetCellGO, m_contentParent);
//				Cell_Asset assetCell = (Cell_Asset)assetCellGO.GetComponent<Cell_Asset> ();
//				m_cells.Add (assetCell);
//
//				if (assets.Contains (a)) {
//					assets.Remove (a);
//					assetCell.SetAsset (a, Cell_Asset.AssetState.Positive);
//				} else {
//
//					assetCell.SetAsset (a);
//				}
//			}
//		}

		// if mission has been compiled, display any required floors

//		if (m_missionPlan.m_requiredFloors.Count > 0) {
//
//			foreach (Floor floor in m_missionPlan.m_requiredFloors) {
//
//				GameObject floorCellGO = (GameObject)Instantiate (m_floorMinimalCellGO, m_contentParent);
//				Cell_Floor_Minimal floorCell = (Cell_Floor_Minimal)floorCellGO.GetComponent<Cell_Floor_Minimal> ();
//				m_cells.Add ((UICell)floorCell);
//
//				bool hasFloor = false;
//
//				foreach (Lair.FloorSlot fSlot in m_missionPlan.m_matchingFloors) {
//
//					if (fSlot.m_floor.m_name == floor.m_name) {
//
//						hasFloor = true;
//						break;
//					}
//				}
//
//				if (hasFloor) {
//
//					floorCell.SetFloor (floor, Cell_Floor_Minimal.FloorState.Positive);
//				} else {
//					floorCell.SetFloor (floor);
//				}
//
//			}
//		}

		// display any active effects

		if (m_missionPlan.m_effects.Count > 0) {

			foreach (EffectPool.EffectSlot eSlot in m_missionPlan.m_effects)
			{
				GameObject effectCellGO = (GameObject)Instantiate (m_effectCellGO, m_contentParent);
				Cell_Effect effectCell = (Cell_Effect)effectCellGO.GetComponent<Cell_Effect> ();
				effectCell.SetEffect (eSlot.m_effect);
				m_cells.Add ((UICell)effectCell);
			}
		}


//		if (m_missionPlan.m_currentMission != null) {
//
//			GameObject separatorGO = (GameObject)Instantiate (m_separatorCellGO, m_contentParent);
//			UICell separator = (UICell)separatorGO.GetComponent<UICell> ();
//			m_cells.Add (separator);
//		}



		// display current state of target selection

//		if (m_missionPlan.m_currentMission == null || (m_missionPlan.m_currentMission != null &&
//		    m_missionPlan.m_currentMission.m_targetType != Mission.TargetType.Lair && m_missionPlan.m_currentMission.m_targetType != Mission.TargetType.Region)) {
//
//			GameObject selectTargetHeaderCellGO = (GameObject)Instantiate (m_headerCellGO, m_contentParent);
//			Cell_Header selectTargetHeaderCell = (Cell_Header)selectTargetHeaderCellGO.GetComponent<Cell_Header> ();
//			headerString = "Step " + stepNum.ToString() + ":";
//			stepNum++;
//			selectTargetHeaderCell.SetHeader (headerString.ToUpper());
//			m_cells.Add ((UICell)selectTargetHeaderCell);
//		}




		// display current state of site selection

		if (m_missionPlan.m_currentMission != null && m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Actor) {

			if (m_missionPlan.m_targetActor != null) {

				GameObject selectHenchmenCellGO = (GameObject)Instantiate (m_selectHenchmenCellGO, m_contentParent);
				UICell selectHenchmenCell = (UICell)selectHenchmenCellGO.GetComponent<UICell> ();
				m_cells.Add (selectHenchmenCell);

				selectHenchmenCell.m_headerText.text = "Target Henchmen: " + m_missionPlan.m_targetActor.m_actor.m_actorName;

				Button b2 = selectHenchmenCell.m_buttons [0];

				if (m_missionPlan.m_state == MissionPlan.State.Planning) {

					b2.onClick.AddListener (delegate {
						SelectTargetActorButtonPressed ();
					});
				} else {

					b2.gameObject.SetActive (false);
				}

			} else {

//				GameObject selectHenchmenCellGO = (GameObject)Instantiate (m_emptyTargetCellGO, m_contentParent);
//				UICell selectHenchmenCell = (UICell)selectHenchmenCellGO.GetComponent<UICell> ();
//				m_cells.Add (selectHenchmenCell);

				GameObject selectTargetCellGO = (GameObject)Instantiate (m_emptyTargetCellGO, m_contentParent);
				UICell selectTargetCell = (UICell)selectTargetCellGO.GetComponent<UICell> ();
				selectTargetCell.m_headerText.text = "Select Henchmen";
				m_cells.Add (selectTargetCell);

				Button b2 = selectTargetCell.m_buttons [0];
				b2.onClick.AddListener (delegate {
					SelectTargetActorButtonPressed ();
				});
			}

		} else if (m_missionPlan.m_currentMission != null && 
			(m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Region || m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Lair)) {



//			if (m_missionPlan.m_targetRegion != null) {
//
//				GameObject selectSiteCellGO = (GameObject)Instantiate (m_selectSiteCellGO, m_contentParent);
//				UICell selectSiteCell = (UICell)selectSiteCellGO.GetComponent<UICell> ();
//				m_cells.Add (selectSiteCell);
//
//				string s = "Current Region: ";
//
//				s += m_missionPlan.m_targetRegion.m_regionName;
//
//				selectSiteCell.m_headerText.text = s;
//
//				Button b2 = selectSiteCell.m_buttons [0];
//
//				if (m_missionPlan.m_state == MissionPlan.State.Planning) {
//
//					Text t = b2.GetComponentInChildren<Text> ();
//					t.text = "Select Region";
//
//					b2.onClick.AddListener (delegate {
//						SelectSiteButtonPressed ();
//					});
//
//				} else {
//
//					b2.gameObject.SetActive (false);
//				}
//
//			} else {
//
//				GameObject selectSiteCellGO = (GameObject)Instantiate (m_emptyTargetCellGO, m_contentParent);
//				UICell selectSiteCell = (UICell)selectSiteCellGO.GetComponent<UICell> ();
//				m_cells.Add (selectSiteCell);
//
//				Button b2 = selectSiteCell.m_buttons [0];
//
//				b2.onClick.AddListener (delegate {
//					SelectSiteButtonPressed ();
//				});
//			}



		} else if (m_missionPlan.m_currentMission != null && m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Site) {

			GameObject selectTargetHeaderCellGO = (GameObject)Instantiate (m_headerCellGO, m_contentParent);
			Cell_Header selectTargetHeaderCell = (Cell_Header)selectTargetHeaderCellGO.GetComponent<Cell_Header> ();
			headerString = "Step " + stepNum.ToString() + ":";
			stepNum++;


			if (m_missionPlan.m_missionSite != null) {

				headerString += " Complete";

				// display site

				GameObject siteInfo = (GameObject)Instantiate (m_selectSiteCellGO, m_contentParent);
				Cell_Site siteInfoCell = (Cell_Site)siteInfo.GetComponent<Cell_Site> ();
				siteInfoCell.SetSite (m_missionPlan.m_missionSite);
				siteInfoCell.m_buttons [0].gameObject.SetActive (false);
				m_cells.Add ((UICell)siteInfoCell);

				if (m_missionPlan.m_state == MissionPlan.State.Planning) {

					siteInfoCell.m_buttons [1].gameObject.SetActive (true);

					Button b3 = siteInfoCell.m_buttons [1];

					b3.onClick.AddListener (delegate {
						SelectSiteButtonPressed ();
					});
				} 

				// add alert panel

				GameObject alertPanelGO = (GameObject)Instantiate (m_siteAlertLevelPanel, m_contentParent);
				Cell_SiteAlertPanel alertPanel = (Cell_SiteAlertPanel)alertPanelGO.GetComponent<Cell_SiteAlertPanel> ();
//				alertPanel.SetAlertLevel (m_missionPlan.m_missionSite);

				m_cells.Add (alertPanel);

				// create site trait cells

				if (m_missionPlan.m_missionSite.traits.Count > 0) {

					GameObject siteTraitPanelGO = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
					Cell_DetailPanel siteTraitPanel = (Cell_DetailPanel)siteTraitPanelGO.GetComponent<Cell_DetailPanel> ();
					siteTraitPanel.SetSiteTraits (m_missionPlan.m_missionSite);
					m_cells.Add (siteTraitPanel);

				}

				GameObject spacerGO2 = (GameObject)Instantiate (m_spacer, m_contentParent);
				Cell_Spacer spacer2 = (Cell_Spacer)spacerGO2.GetComponent<Cell_Spacer> ();
				m_cells.Add (spacer2);

			} else {

				GameObject selectTargetCellGO = (GameObject)Instantiate (m_emptyTargetCellGO, m_contentParent);
				UICell selectTargetCell = (UICell)selectTargetCellGO.GetComponent<UICell> ();
				selectTargetCell.m_headerText.text = "SELECT TARGET SITE";
				m_cells.Add (selectTargetCell);

				Button b2 = selectTargetCell.m_buttons [0];

				b2.onClick.AddListener (delegate {
					SelectSiteButtonPressed ();
				});
			}

			selectTargetHeaderCell.SetHeader (headerString.ToUpper());
			m_cells.Add ((UICell)selectTargetHeaderCell);

		} else if (m_missionPlan.m_currentMission != null && m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Asset ) {

			GameObject selectTargetHeaderCellGO = (GameObject)Instantiate (m_headerCellGO, m_contentParent);
			Cell_Header selectTargetHeaderCell = (Cell_Header)selectTargetHeaderCellGO.GetComponent<Cell_Header> ();
			headerString = "Step " + stepNum.ToString() + ":";
			stepNum++;

			if (m_missionPlan.m_currentAsset != null) {

				headerString += " Complete";

				// display site

				GameObject siteInfo = (GameObject)Instantiate (m_selectSiteCellGO, m_contentParent);
				Cell_Site siteInfoCell = (Cell_Site)siteInfo.GetComponent<Cell_Site> ();
				siteInfoCell.SetSite (m_missionPlan.m_missionSite);
				siteInfoCell.m_buttons [0].gameObject.SetActive (false);
				m_cells.Add ((UICell)siteInfoCell);

				// add alert panel

				GameObject alertPanelGO = (GameObject)Instantiate (m_siteAlertLevelPanel, m_contentParent);
				Cell_SiteAlertPanel alertPanel = (Cell_SiteAlertPanel)alertPanelGO.GetComponent<Cell_SiteAlertPanel> ();
//				alertPanel.SetAlertLevel (m_missionPlan.m_missionSite);

				m_cells.Add (alertPanel);

				// create site trait cells

				if (m_missionPlan.m_missionSite.traits.Count > 0) {

					GameObject siteTraitPanelGO = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
					Cell_DetailPanel siteTraitPanel = (Cell_DetailPanel)siteTraitPanelGO.GetComponent<Cell_DetailPanel> ();
					siteTraitPanel.SetSiteTraits (m_missionPlan.m_missionSite);
					m_cells.Add (siteTraitPanel);

				}

				GameObject spacerGO3 = (GameObject)Instantiate (m_spacer, m_contentParent);
				Cell_Spacer spacer3 = (Cell_Spacer)spacerGO3.GetComponent<Cell_Spacer> ();
				spacer3.SetHeight (10);
				m_cells.Add (spacer3);

				// display asset

				GameObject siteAsset = (GameObject)Instantiate (m_assetCellGO, m_contentParent);
				Cell_Asset_Card siteAssetCell = (Cell_Asset_Card)siteAsset.GetComponent<Cell_Asset_Card> ();
				siteAssetCell.SetAsset (m_missionPlan.m_currentAsset);
				m_cells.Add (siteAssetCell);


				if (m_missionPlan.m_state == MissionPlan.State.Planning) {

					siteAssetCell.m_buttons [1].gameObject.SetActive (true);
//					siteInfoCell.m_buttons [1].gameObject.SetActive (true);
//
//					Button b3 = siteInfoCell.m_buttons [1];
//
					siteAssetCell.m_buttons [1].onClick.AddListener (delegate {
						SelectSiteButtonPressed ();
					});
				} 

				if (m_missionPlan.m_currentAsset.m_asset.m_requiredTraits.Length > 0) {

					GameObject siteTraitPanelGO = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
					Cell_DetailPanel siteTraitPanel = (Cell_DetailPanel)siteTraitPanelGO.GetComponent<Cell_DetailPanel> ();
					siteTraitPanel.SetTraits (m_missionPlan.m_currentAsset.m_asset);
					m_cells.Add (siteTraitPanel);

				}

				GameObject spacerGO2 = (GameObject)Instantiate (m_spacer, m_contentParent);
				Cell_Spacer spacer2 = (Cell_Spacer)spacerGO2.GetComponent<Cell_Spacer> ();
				m_cells.Add (spacer2);

			} else {
				
				GameObject selectTargetCellGO = (GameObject)Instantiate (m_emptyTargetCellGO, m_contentParent);
				UICell selectTargetCell = (UICell)selectTargetCellGO.GetComponent<UICell> ();
				selectTargetCell.m_headerText.text = "SELECT TARGET ASSET";
				m_cells.Add (selectTargetCell);

				Button b2 = selectTargetCell.m_buttons [0];

				b2.onClick.AddListener (delegate {
					SelectSiteButtonPressed ();
				});
			}

			selectTargetHeaderCell.SetHeader (headerString.ToUpper());
			m_cells.Add ((UICell)selectTargetHeaderCell);

		} else if (m_missionPlan.m_currentMission == null) {

			GameObject selectTargetHeaderCellGO = (GameObject)Instantiate (m_headerCellGO, m_contentParent);
			Cell_Header selectTargetHeaderCell = (Cell_Header)selectTargetHeaderCellGO.GetComponent<Cell_Header> ();
			headerString = "Step " + stepNum.ToString() + ":";
			stepNum++;
			selectTargetHeaderCell.SetHeader (headerString.ToUpper());
			m_cells.Add ((UICell)selectTargetHeaderCell);

			GameObject selectTargetCellGO = (GameObject)Instantiate (m_emptyTargetCellGO, m_contentParent);
			UICell selectTargetCell = (UICell)selectTargetCellGO.GetComponent<UICell> ();
			m_cells.Add (selectTargetCell);
		}




		// display current state of henchmen selection

		GameObject selectHenchmenHeaderCellGO = (GameObject)Instantiate (m_headerCellGO, m_contentParent);
		Cell_Header selectHenchmenHeaderCell = (Cell_Header)selectHenchmenHeaderCellGO.GetComponent<Cell_Header> ();
		headerString = "Step " + stepNum.ToString() + ":";

		if (m_missionPlan.m_actorSlots.Count > 0) {

			headerString += " Complete";
		}

		selectHenchmenHeaderCell.SetHeader (headerString.ToUpper());
		m_cells.Add ((UICell)selectHenchmenHeaderCell);

//		int numHenchmenPresent = 0;

		for (int i = 0; i < m_missionPlan.m_maxActorSlots; i++) {

			if (i < m_missionPlan.m_actorSlots.Count) {

				Player.ActorSlot aSlot = m_missionPlan.m_actorSlots [i];

				GameObject selectHenchmenCellGO = (GameObject)Instantiate (m_selectHenchmenCellGO, m_contentParent);
				Cell_Actor selectHenchmenCell = (Cell_Actor)selectHenchmenCellGO.GetComponent<Cell_Actor> ();
				selectHenchmenCell.SetActor (aSlot);
				m_cells.Add (selectHenchmenCell);



				// set detail panel - traits

				GameObject traitPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
				Cell_DetailPanel dPanel = (Cell_DetailPanel)traitPanel.GetComponent<Cell_DetailPanel> ();
				dPanel.SetTraits (aSlot, m_missionPlan, Cell_DetailPanel.MissionState.Planning);
				m_cells.Add (dPanel);

				GameObject spacerGO1 = (GameObject)Instantiate (m_spacer, m_contentParent);
				Cell_Spacer spacer1 = (Cell_Spacer)spacerGO1.GetComponent<Cell_Spacer> ();
				m_cells.Add (spacer1);

//				numHenchmenPresent++;
				if (m_missionPlan.m_state == MissionPlan.State.Planning) {
					
				selectHenchmenCell.m_buttons [0].gameObject.SetActive (false);
				selectHenchmenCell.m_buttons [2].gameObject.SetActive (true);

					Button b3 = selectHenchmenCell.m_buttons [2];

					b3.onClick.AddListener (delegate {
						SelectHenchmenButtonPressed (aSlot);
					});

					// enable remove henchmen button

					selectHenchmenCell.m_buttons[1].onClick.AddListener (delegate {
						RemoveHenchmenButtonPressed (aSlot);
					});

				} else {

					selectHenchmenCell.m_buttons [0].gameObject.SetActive (false);
				}

			} else if (m_missionPlan.m_state == MissionPlan.State.Planning && i < m_missionPlan.m_actorSlots.Count+1)  {

				GameObject selectHenchmenCellGO = (GameObject)Instantiate (m_emptyHenchmenCellGO, m_contentParent);
				UICell selectHenchmenCell = (UICell)selectHenchmenCellGO.GetComponent<UICell> ();
				m_cells.Add (selectHenchmenCell);

				Button b3 = selectHenchmenCell.m_buttons [0];

				if (m_missionPlan.m_actorSlots.Count > 0) {

					selectHenchmenCell.m_headerText.text = "ADD HENCHMEN";
				}

				b3.onClick.AddListener (delegate {
					SelectHenchmenButtonPressed ();
				});


//				if (m_missionPlan.m_state == MissionPlan.State.Planning) {
//					b3.onClick.AddListener (delegate {
//						SelectHenchmenButtonPressed ();
//					});
//				} else {
//
//					b3.gameObject.SetActive (false);
//				}
			}
		}

//		if (m_missionPlan.m_actorSlots.Count > 0) {
//			
//			GameObject separatorGO = (GameObject)Instantiate (m_separatorCellGO, m_contentParent);
//			UICell separator = (UICell)separatorGO.GetComponent<UICell> ();
//			m_cells.Add (separator);
//
//		}

		// start mission button

		if (m_missionPlan.m_state == MissionPlan.State.Planning) {

			// mission summary panel

			GameObject mSummaryHeaderGO = (GameObject)Instantiate (m_headerCellGO, m_contentParent);
			Cell_Header mSummaryHeader = (Cell_Header)mSummaryHeaderGO.GetComponent<Cell_Header> ();
			mSummaryHeader.SetHeader("MISSION SUMMARY");
			m_cells.Add ((UICell)mSummaryHeader);

			GameObject mSummaryGO = (GameObject)Instantiate (m_missionSummaryPanel, m_contentParent);
			UICell mSummary = (UICell)mSummaryGO.GetComponent<UICell> ();
			int sc = Mathf.Clamp (m_missionPlan.m_successChance, 0, 100);
			mSummary.m_headerText.text = sc.ToString () + "%";
			m_cells.Add (mSummary);

			// set detail panel - traits

			if (m_missionPlan.m_requiredTraits.Count > 0) {

				GameObject traitPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
				Cell_DetailPanel dPanel = (Cell_DetailPanel)traitPanel.GetComponent<Cell_DetailPanel> ();
				dPanel.SetTraits (m_missionPlan, Cell_DetailPanel.MissionState.Planning);
				m_cells.Add (dPanel);
			}

			// set detail panel - assets

			if (m_missionPlan.m_requiredAssets.Count > 0) {

				GameObject assetPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
				Cell_DetailPanel aPanel = (Cell_DetailPanel)assetPanel.GetComponent<Cell_DetailPanel> ();
				aPanel.SetAssets (m_missionPlan, Cell_DetailPanel.MissionState.Planning);
				m_cells.Add (aPanel);
			}

			// set detail panel - facilities

			if (m_missionPlan.m_requiredFloors.Count > 0) {

				GameObject facilityPanel = (GameObject)Instantiate (m_cellDetailPanel, m_contentParent);
				Cell_DetailPanel fPanel = (Cell_DetailPanel)facilityPanel.GetComponent<Cell_DetailPanel> ();
				fPanel.SetFacilities (m_missionPlan, Cell_DetailPanel.MissionState.Planning);
				m_cells.Add (fPanel);
			}

			// set cost panel
			if (m_missionPlan.m_currentMission != null) {
				
				GameObject costPanel = (GameObject)Instantiate (m_cellCostPanel, m_contentParent);
				Cell_CostPanel cPanel = (Cell_CostPanel)costPanel.GetComponent<Cell_CostPanel> ();
				cPanel.SetCostPanel (m_missionPlan.m_currentMission);
				m_cells.Add (cPanel);
			}


			GameObject startMissionCellGO = (GameObject)Instantiate (m_startMissionCellGO, m_contentParent);
			UICell startMissionCell = (UICell)startMissionCellGO.GetComponent<UICell> ();
			m_cells.Add (startMissionCell);

//			ColorBlock cb = startMissionCell.m_buttons [0].colors;
//			cb.normalColor = Color.green;
//			cb.disabledColor = Color.gray;
//			startMissionCell.m_buttons [0].colors = cb;

			Player.CommandPool cp = GameController.instance.GetCommandPool (0);

			if (m_missionPlan.m_successChance > 0 && m_missionPlan.m_currentMission.m_cost <= cp.m_currentPool && m_missionPlan.m_actorSlots.Count > 0 &&
				((m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Actor && m_missionPlan.m_targetActor != null) ||
					(m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Asset && m_missionPlan.m_currentAsset != null) ||
					(m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Site && m_missionPlan.m_missionSite != null) ||
					(m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Lair) || 
					(m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.SiteTrait && m_missionPlan.m_targetSiteTrait != null) ||
					(m_missionPlan.m_currentMission.m_targetType == Mission.TargetType.Region && m_missionPlan.m_targetRegion != null))) {



			} 
//			else {
//
//				startMissionCell.m_buttons [0].interactable = false;
//			}

			startMissionCell.m_buttons [0].interactable = true;

			startMissionCell.m_buttons [0].onClick.AddListener (delegate {
				StartMissionButtonPressed ();
			});



			// spawn repeat mission toggle if enabled

//			if (m_missionPlan.m_allowRepeat) {
//
//				GameObject repeatMissionCellGO = (GameObject)Instantiate (m_repeatMissionCellGO, m_contentParent);
//				UICell repeatMissionCell = (UICell)repeatMissionCellGO.GetComponent<UICell> ();
//				m_cells.Add (repeatMissionCell);
//
//				if (m_missionPlan.m_doRepeat) {
//					repeatMissionCell.m_image.texture = repeatMissionCell.m_sprites [1].texture;
//				}
//
//				Button b3 = repeatMissionCell.m_buttons [0];
//				b3.onClick.AddListener (delegate {
//					RepeatMissionButtonPressed (repeatMissionCell);
//				});
//			}

		} else if (m_missionPlan.m_state == MissionPlan.State.Active) {

			// spawn cancel mission cell

			GameObject cancelMissionCellGO = (GameObject)Instantiate (m_cancelMissionCellGO, m_contentParent);
			UICell cancelMissionCell = (UICell)cancelMissionCellGO.GetComponent<UICell> ();
			m_cells.Add (cancelMissionCell);

			ColorBlock cb = cancelMissionCell.m_buttons [0].colors;
			cb.normalColor = Color.red;
			cb.disabledColor = Color.gray;
			cancelMissionCell.m_buttons [0].colors = cb;

			Button b3 = cancelMissionCell.m_buttons [0];
			b3.onClick.AddListener (delegate {
				CancelMissionButtonPressed ();
			});
		}

		GameObject spacerGO0 = (GameObject)Instantiate (m_spacer, m_contentParent);
		Cell_Spacer spacer0 = (Cell_Spacer)spacerGO0.GetComponent<Cell_Spacer> ();
		spacer0.SetHeight (150);
		m_cells.Add (spacer0);
	}

	public void RepeatMissionButtonPressed (UICell repeatCell)
	{

		if (m_missionPlan.m_doRepeat) {

			m_missionPlan.m_doRepeat = false;
			repeatCell.m_image.texture = repeatCell.m_sprites [0].texture;

		} else {

			m_missionPlan.m_doRepeat = true;
			repeatCell.m_image.texture = repeatCell.m_sprites [1].texture;
		}

	}

	public void StartMissionButtonPressed ()
	{
		// alert if there isn't a henchmen attached

		if (m_missionPlan.m_actorSlots.Count == 0 || m_missionPlan.m_currentMission == null) {

			string header = "Can't Start Mission";
			string message = "You need to select a Mission and assign at least 1 Henchmen to start a Mission.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

			return;
		}


		// alert if player can't afford mission

		Player.CommandPool cp = GameController.instance.GetCommandPool (0);

		if (m_missionPlan.m_currentMission.m_cost > cp.m_currentPool) {

			string header = "Can't Afford Mission";
			string message = "There aren't enough points in your Command Pool to start this Mission.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

			return;
		}


		// alert if there is 0% success rate

		if (m_missionPlan.m_successChance <= 0) {

			string header = "Can't Start Mission";
			string message = "You can't start a Mission with a 0% Success Chance.";

			MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
			Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Okay");
			b2.onClick.AddListener(delegate { MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();});
			m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);

			return;
		}


		Debug.Log ("Starting new mission: " + m_missionPlan.m_currentMission.m_name);
//		Debug.Log (m_missionPlan.m_currentAsset.m_asset);

		Action_SpendCommandPoints payForMission = new Action_SpendCommandPoints ();
		payForMission.m_playerID = 0;
		payForMission.m_amount = m_missionPlan.m_currentMission.m_cost;
		GameController.instance.ProcessAction (payForMission);

		Action_StartNewMission newMission = new Action_StartNewMission ();
		newMission.m_missionPlan = m_missionPlan;
		newMission.m_playerID = 0;
		GameController.instance.ProcessAction (newMission);

		m_parentApp.homeMenu.isDirty = true;
		ParentApp.PopMenu ();
	}

	public void SelectMissionButtonPressed ()
	{
		m_selectMissionMenu.missionPlan = m_missionPlan;
		ParentApp.PushMenu (m_selectMissionMenu);
	}

	public void SelectSiteButtonPressed ()
	{
		m_selectSiteMenu.missionPlan = m_missionPlan;
		ParentApp.PushMenu (m_selectSiteMenu);
	}

	public void SelectTargetActorButtonPressed ()
	{
		m_selectTargetActorMenu.missionPlan = m_missionPlan;
		ParentApp.PushMenu (m_selectTargetActorMenu);
	}

	public void SelectHenchmenButtonPressed ()
	{
		m_selectHenchmenMenu.currentSlot = null;
		m_selectHenchmenMenu.missionPlan = m_missionPlan;
		ParentApp.PushMenu (m_selectHenchmenMenu);
	}

	public void SelectHenchmenButtonPressed (Player.ActorSlot currentSlot)
	{
		m_selectHenchmenMenu.currentSlot = currentSlot;
		m_selectHenchmenMenu.missionPlan = m_missionPlan;
		ParentApp.PushMenu (m_selectHenchmenMenu);
	}

	public void CancelMissionButtonPressed ()
	{
		string header = "Cancel Mission";
		string message = "Are you sure you want to cancel this Mission?";

		MobileUIEngine.instance.alertDialogue.SetAlert (header, message, m_parentApp);
		Button b1 = MobileUIEngine.instance.alertDialogue.AddButton ("Cancel Mission");
		b1.onClick.AddListener (delegate {
			CancelMission ();
		});
		Button b2 = MobileUIEngine.instance.alertDialogue.AddButton ("Cancel");
		b2.onClick.AddListener (delegate {
			MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();
		});
		m_parentApp.PushMenu (MobileUIEngine.instance.alertDialogue);
	}

	public void CancelMission ()
	{
		MobileUIEngine.instance.alertDialogue.DismissButtonTapped ();

		Action_CancelMission cancelMission = new Action_CancelMission ();
		cancelMission.m_missionPlan = m_missionPlan;
		cancelMission.m_playerID = 0;
		GameController.instance.ProcessAction (cancelMission);

		m_parentApp.homeMenu.isDirty = true;
		m_parentApp.PopMenu ();
	}

	public void RemoveHenchmenButtonPressed (Player.ActorSlot currentSlot)
	{
		Debug.Log ("Remove henchmen button pressed");

		for (int i = 0; i < m_missionPlan.m_actorSlots.Count; i++) {

			Player.ActorSlot aSlot = m_missionPlan.m_actorSlots [i];

			if (aSlot.m_actor != null && aSlot.m_actor.id == currentSlot.m_actor.id) {

				m_missionPlan.m_actorSlots.RemoveAt (i);
				break;
			}
		}

		DisplayContent ();
	}

//	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
	public MissionPlan missionPlan {set {m_missionPlan = value;}}

	public PlanMission_SelectMissionMenu selectMissionMenu {get{return m_selectMissionMenu;}}
}
