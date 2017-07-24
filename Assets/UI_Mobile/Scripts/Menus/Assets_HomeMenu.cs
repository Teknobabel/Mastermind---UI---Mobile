using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Assets_HomeMenu : MonoBehaviour, IMenu {

	public Text
	m_appNameText;

	public Transform
	m_contentParent;

	public GameObject
	m_assetCellGO;

	private IApp m_parentApp;

	private List<UICell> m_cells = new List<UICell>();

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
		m_appNameText.text = parentApp.Name;
		//		m_infoPanelToggle.AddObserver (this);
		//		m_infoPanelToggle.ToggleButtonClicked (0);
		this.gameObject.SetActive (false);
	}

	public void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		DisplayAssets ();
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

	public void DisplayAssets ()
	{
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		List<Site.AssetSlot> assets = GameController.instance.GetAssets (0);

		foreach (Site.AssetSlot aSlot in assets) {

			GameObject assetGO = (GameObject)Instantiate (m_assetCellGO, m_contentParent);
			UICell assetCell = (UICell)assetGO.GetComponent<UICell> ();
			assetCell.m_headerText.text = aSlot.m_asset.m_name;
			m_cells.Add (assetCell);

			if (aSlot.m_new) {

				Action_SetAssetNewState setNewState = new Action_SetAssetNewState ();
				setNewState.m_assetSlot = aSlot;
				setNewState.m_newState = false;
				GameController.instance.ProcessAction (setNewState);
			}
		}

	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}
}
