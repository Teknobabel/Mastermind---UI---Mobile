using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Assets_HomeMenu : BaseMenu {

	public Text
	m_appNameText;

	public Transform
	m_contentParent;

	public GameObject
	m_assetCellGO,
	m_headerCellGO;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		m_appNameText.text = parentApp.Name;
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

		// clear out new flags

		List<Site.AssetSlot> assets = GameController.instance.GetAssets (0);

		foreach (Site.AssetSlot aSlot in assets) {
			
			if (aSlot.m_new) {

				Action_SetAssetNewState setNewState = new Action_SetAssetNewState ();
				setNewState.m_assetSlot = aSlot;
				setNewState.m_newState = false;
				GameController.instance.ProcessAction (setNewState);
			}
		}

		this.gameObject.SetActive (false);
	}

	public override void DisplayContent ()
	{
		base.DisplayContent ();

		List<Site.AssetSlot> assets = GameController.instance.GetAssets (0);

		List<Asset> assetsNeededForOP = (GameController.instance.GetOmegaPlan (0)).m_omegaPlan.GetNeededAssets ();

		int numAssetSlots = GameController.instance.GetNumAssetSlots (0);

		for (int i=0; i < assets.Count; i++)
		{
			Site.AssetSlot aSlot = assets [i];

			if (i == numAssetSlots && i < assets.Count) {

				GameObject headerGO = (GameObject)Instantiate (m_headerCellGO, m_contentParent);
				Cell_Header headerCell = (Cell_Header)headerGO.GetComponent<Cell_Header> ();
				headerCell.SetHeader("Assets over the limit");
				m_cells.Add (headerCell);
			}

			GameObject assetGO = (GameObject)Instantiate (m_assetCellGO, m_contentParent);
			Cell_Asset assetCell = (Cell_Asset)assetGO.GetComponent<Cell_Asset> ();
			assetCell.SetAsset (aSlot);
			m_cells.Add (assetCell);

			Button b = assetCell.m_buttons [0];
			b.onClick.AddListener (delegate {
				AssetClicked (aSlot);
			});
		}

		int emptySlots = numAssetSlots - assets.Count;

		if (emptySlots > 0) {

			for (int i=0; i < emptySlots; i++)
			{
				GameObject assetGO = (GameObject)Instantiate (m_assetCellGO, m_contentParent);
				UICell assetCell = (UICell)assetGO.GetComponent<UICell> ();
				assetCell.m_headerText.text = "Empty";
				assetCell.m_headerText.color = Color.gray;
				m_cells.Add (assetCell);
			}
		}

	}

	public void AssetClicked (Site.AssetSlot assetSlot)
	{
		((AssetsApp)m_parentApp).assetDetailMenu.assetSlot = assetSlot;
		ParentApp.PushMenu (((AssetsApp)m_parentApp).assetDetailMenu);
	}

}
