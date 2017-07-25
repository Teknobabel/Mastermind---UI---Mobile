using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OmegaPlanHomeContainerMenu : MonoBehaviour, IMenu, IUIObserver {

	public Transform
	m_contentParent;

	public PageIndicator
	m_pageIndicator;

	private IApp m_parentApp;

	// Use this for initialization
//	void Start () {
//
//	}

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
//		m_appNameText.text = parentApp.Name;
	//		m_infoPanelToggle.AddObserver (this);
	//		m_infoPanelToggle.ToggleButtonClicked (0);

		float screenWidth = MobileUIEngine.instance.m_mainCanvas.sizeDelta.x;
		float screenHeight = MobileUIEngine.instance.m_mainCanvas.sizeDelta.y;

		LayoutElement hsle = m_contentParent.GetComponent<LayoutElement> ();
		hsle.preferredWidth = screenWidth;
		hsle.preferredHeight = screenHeight;

		Player.OmegaPlanSlot op = GameController.instance.GetOmegaPlan (0);

		int numPages = op.m_omegaPlan.m_phases.Length;

		for (int i = 0; i < numPages; i++) {

			GameObject go = (GameObject)GameObject.Instantiate (m_parentApp.MenuBank[1], m_contentParent);
			LayoutElement le = go.GetComponent<LayoutElement> ();
			le.preferredWidth = screenWidth;
			le.preferredHeight = screenHeight;

			OmegaPlanHomeMenu screen = go.GetComponent<OmegaPlanHomeMenu> ();
			screen.phaseGoals = op.m_omegaPlan.m_phases [i];
			screen.phaseNumber = i + 1;
			screen.Initialize (m_parentApp);

		}

		ScrollRectSnap srt = (ScrollRectSnap) this.GetComponent<ScrollRectSnap> ();
		srt.screens = numPages;
		srt.Initialize ();
		srt.AddObserver (this);

		m_pageIndicator.SetNumPages (numPages);
		m_pageIndicator.SetPage (0);

		this.gameObject.SetActive (false);
	}

	public void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

	}

	public void OnExit (bool animate)
	{
		Player.OmegaPlanSlot op = GameController.instance.GetOmegaPlan (0);

		if (op.m_state == Player.OmegaPlanSlot.State.New) {

			Action_SetOmegaPlanNewState opState = new Action_SetOmegaPlanNewState ();
			opState.m_newState = false;
			opState.m_omegaPlanSlot = op;
			GameController.instance.ProcessAction (opState);
		}

		this.gameObject.SetActive (false);
	}

	public void OnHold ()
	{

	}

	public void OnReturn ()
	{

	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}

	public void OnNotify (IUISubject subject, UIEvent thisUIEvent)
	{
		switch (thisUIEvent)
		{
		case UIEvent.UI_PageChanged:

			m_pageIndicator.SetPage (((ScrollRectSnap)subject).target);
			break;
		}

	}
}
