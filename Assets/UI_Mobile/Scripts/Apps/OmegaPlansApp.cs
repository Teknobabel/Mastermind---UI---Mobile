using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[CreateAssetMenu]
public class OmegaPlansApp : BaseApp {

	private OmegaPlanHomeMenu m_homeMenu;

	private Transform m_menuParent = null;

	private OmegaPlan_GoalInfoMenu m_opGoalInfoOverlay;

	public override void InitializeApp ()
	{
		// set canvas size

		float screenWidth = MobileUIEngine.instance.m_mainCanvas.sizeDelta.x;
		float screenHeight = MobileUIEngine.instance.m_mainCanvas.sizeDelta.y;

		// instantiate home screen base

		GameObject homeScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[0], Vector3.zero, Quaternion.identity);
		m_menuParent = homeScreenGO.transform;
		homeScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);

		OmegaPlanHomeContainerMenu homeScreenMenu = (OmegaPlanHomeContainerMenu)homeScreenGO.GetComponent<OmegaPlanHomeContainerMenu> ();

		LayoutElement hsle = homeScreenMenu.m_contentParent.GetComponent<LayoutElement> ();
		hsle.preferredWidth = screenWidth;
		hsle.preferredHeight = screenHeight;

		OmegaPlan op = GetDummyData.instance.GetOmegaPlan ();

		int numPages = op.m_phases.Length;

		for (int i = 0; i < numPages; i++) {

			GameObject go = (GameObject)GameObject.Instantiate (m_menuBank[1], homeScreenMenu.m_contentParent);
			LayoutElement le = go.GetComponent<LayoutElement> ();
			le.preferredWidth = screenWidth;
			le.preferredHeight = screenHeight;

			OmegaPlanHomeMenu screen = go.GetComponent<OmegaPlanHomeMenu> ();
			screen.phaseGoals = op.m_phases [i];
			screen.Initialize (this);

		}

		ScrollRectSnap srt = (ScrollRectSnap) homeScreenGO.GetComponent<ScrollRectSnap> ();
		srt.screens = numPages;
		srt.Initialize ();
		srt.AddObserver (homeScreenMenu);

		homeScreenMenu.m_pageIndicator.SetNumPages (numPages);
		homeScreenMenu.m_pageIndicator.SetPage (0);

		GameObject infoScreenGO = (GameObject)GameObject.Instantiate (m_menuBank[2], Vector3.zero, Quaternion.identity);
//		m_menuParent = homeScreenGO.transform;
		infoScreenGO.transform.SetParent (MobileUIEngine.instance.m_mainCanvas, false);
		m_opGoalInfoOverlay = (OmegaPlan_GoalInfoMenu) infoScreenGO.GetComponent<OmegaPlan_GoalInfoMenu> ();
		m_opGoalInfoOverlay.Initialize (this);

		m_menuParent.gameObject.SetActive (false);

		base.InitializeApp ();
	}

	public void GoalButtonClicked ()
	{
		PushMenu (m_opGoalInfoOverlay);
	}

	public override void EnterApp ()
	{
		m_menuParent.gameObject.SetActive (true);

		base.EnterApp ();
	}

	public override void ExitApp ()
	{
		m_menuParent.gameObject.SetActive (false);

		base.ExitApp ();
	}
}
