using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World_HomeMenu : MonoBehaviour, IMenu {

	public Text
	m_appNameText;

	public GameObject
	m_regionHeaderCellGO,
	m_siteInfoCellGO,
	m_siteAlertCellGO,
	m_siteAssetCellGO,
	m_siteTraitCellGO;

	public Transform
	m_worldListParent;

	private IApp m_parentApp;

	private List<UICell> m_cells = new List<UICell>();

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
		m_appNameText.text = parentApp.Name;
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

			GameObject header = (GameObject)Instantiate (m_regionHeaderCellGO, m_worldListParent);
			UICell headerCell = (UICell)header.GetComponent<UICell> ();
			headerCell.m_headerText.text = r.m_regionName;
			headerCell.m_headerText.color = Color.black;
			m_cells.Add (headerCell);

			foreach (Site s in r.sites) {

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

					GameObject siteTrait = (GameObject)Instantiate (m_siteAssetCellGO, m_worldListParent);
					UICell siteTraitCell = (UICell)siteTrait.GetComponent<UICell> ();
					siteTraitCell.m_headerText.text = "Trait: " + t.m_name;
					m_cells.Add (siteTraitCell);
				}

				// create site asset cells

				foreach (Site.AssetSlot aSlot in s.assets) {

					GameObject siteAsset = (GameObject)Instantiate (m_siteAssetCellGO, m_worldListParent);
					UICell siteAssetCell = (UICell)siteAsset.GetComponent<UICell> ();

					if (aSlot.m_state == Site.AssetSlot.State.Hidden) {

						siteAssetCell.m_headerText.text = "Asset: ?????";
					} else {

						siteAssetCell.m_headerText.text = "Asset: " + aSlot.m_asset.m_name;

					}

					m_cells.Add (siteAssetCell);
				}
			}
		}
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}
}
