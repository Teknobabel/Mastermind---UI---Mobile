using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lair_SelectSiteMenu : MonoBehaviour, IMenu {

	public GameObject
	m_regionHeaderCellGO,
	m_siteInfoCellGO,
	m_siteAlertCellGO,
	m_siteAssetCellGO,
	m_siteTraitCellGO;

	public Transform
	m_contentParent;

	private IApp m_parentApp;

	private List<UICell> m_cells = new List<UICell>();

	private Lair.FloorSlot m_floorSlot;

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
		//		m_appNameText.text = parentApp.Name;
		//		m_infoPanelToggle.AddObserver (this);
		//		m_infoPanelToggle.ToggleButtonClicked (0);
		this.gameObject.SetActive (false);
	}

	public void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		DisplayWorld ();
	}

	public void OnExit (bool animate)
	{
		this.gameObject.SetActive (false);
	}

	public void OnHold ()
	{

	}

	public void OnReturn ()
	{

	}

	private void DisplayWorld ()
	{
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		List<Region> regionList = GameController.instance.GetWorld ();

		foreach (Region r in regionList) {

			// create header

			GameObject header = (GameObject)Instantiate (m_regionHeaderCellGO, m_contentParent);
			UICell headerCell = (UICell)header.GetComponent<UICell> ();
			headerCell.m_headerText.text = r.m_regionName;
			headerCell.m_headerText.color = Color.black;
			m_cells.Add (headerCell);

			foreach (Site s in r.m_sites) {

				// create site info cell

				GameObject siteInfo = (GameObject)Instantiate (m_siteInfoCellGO, m_contentParent);
				UICell siteInfoCell = (UICell)siteInfo.GetComponent<UICell> ();
				siteInfoCell.m_headerText.text = s.m_siteName;
				siteInfoCell.m_bodyText.text = s.m_type.ToString ();
				m_cells.Add (siteInfoCell);

				Button b = siteInfoCell.m_buttons [0];
				b.onClick.AddListener (delegate {
					SiteSelected (s);
				});

				// create site alert cell

				GameObject siteAlert = (GameObject)Instantiate (m_siteAlertCellGO, m_contentParent);
				UICell siteAlertCell = (UICell)siteAlert.GetComponent<UICell> ();
				m_cells.Add (siteAlertCell);

				// create site trait cells

				foreach (Trait t in s.traits) {

					GameObject siteTrait = (GameObject)Instantiate (m_siteAssetCellGO, m_contentParent);
					UICell siteTraitCell = (UICell)siteTrait.GetComponent<UICell> ();
					siteTraitCell.m_headerText.text = "Trait: " + t.m_name;
					m_cells.Add (siteTraitCell);
				}

				// create site asset cells

				foreach (Site.AssetSlot aSlot in s.assets) {

					GameObject siteAsset = (GameObject)Instantiate (m_siteAssetCellGO, m_contentParent);
					UICell siteAssetCell = (UICell)siteAsset.GetComponent<UICell> ();
					siteAssetCell.m_headerText.text = "Asset: " + aSlot.m_asset.m_name;
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

	public IApp ParentApp 
	{ get{ return m_parentApp; }}

	public Lair.FloorSlot floorSlot {set {m_floorSlot = value;}}
}
