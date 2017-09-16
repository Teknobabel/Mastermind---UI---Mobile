using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_FilterMenu : BaseMenu, IUIObserver {

	public SegmentedToggle m_assetTypeToggle;
	public SegmentedToggle m_siteTypeToggle;

	private World_HomeMenu m_parentMenu; 

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);
		this.gameObject.SetActive (false);

		m_assetTypeToggle.AddObserver (this);
		m_siteTypeToggle.AddObserver (this);
	}

	public override void OnEnter (bool animate)
	{
		base.OnEnter (animate);
		this.gameObject.SetActive (true);
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);
		this.gameObject.SetActive (false);
	}

	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent)
		{
		case UIEvent.UI_World_AssetTypeTogglePressed:

			switch (m_assetTypeToggle.activeButton) {
			case 0:
				m_parentMenu.assetFilterType = World_HomeMenu.AssetTypeFilter.AllAssets;
				break;
			case 1:
				m_parentMenu.assetFilterType = World_HomeMenu.AssetTypeFilter.RevealedAssets;
				break;
			case 2:
				m_parentMenu.assetFilterType = World_HomeMenu.AssetTypeFilter.OPAssets;
				break;
			}

			break;

		case UIEvent.UI_World_SiteTypeTogglePressed:

			switch (m_siteTypeToggle.activeButton) {
			case 0:
				m_parentMenu.siteTypeFilter = Site.Type.None;
				break;
			case 1:
				m_parentMenu.siteTypeFilter = Site.Type.Military;
				break;
			case 2:
				m_parentMenu.siteTypeFilter = Site.Type.Politics;
				break;
			case 3:
				m_parentMenu.siteTypeFilter = Site.Type.Economy;
				break;
			}

			break;
		}

		m_parentMenu.isDirty = true;
	}

	public void DismissButtonClicked ()
	{
		m_parentApp.PopMenu ();
	}

	public World_HomeMenu parentMenu {set{ m_parentMenu = value; }}
}
