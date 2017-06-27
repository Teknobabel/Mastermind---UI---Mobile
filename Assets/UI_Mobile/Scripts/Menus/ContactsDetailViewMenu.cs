﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContactsDetailViewMenu : MonoBehaviour, ISubject, IObserver, IMenu {

	public enum InfoPanelType
	{
		None,
		Traits,
		History,
		Info,
	}

	public RawImage
	m_portrait;

	public Text
	m_name;

	public GameObject
	m_traitCellGO;

	public SegmentedToggle m_infoPanelToggle;

	public Transform[] m_infoPanels;

	public Transform[] m_panels;

	private List<IObserver>
	m_observers = new List<IObserver> ();

	private List<UICell>
	m_traitCells = new List<UICell>();

	private InfoPanelType 
	m_currentPanel = InfoPanelType.None;

	// Use this for initialization
	void Start () {
		
	}

	private IApp m_parentApp;

	protected int m_henchmenID = -1;

//	private Transform m_menuParent;

	private ContactsDetailViewMenu m_menuObject;

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
		m_infoPanelToggle.AddObserver (this);
		m_infoPanelToggle.ToggleButtonClicked (0);
		this.gameObject.SetActive (false);
	}

	public virtual void OnEnter (bool animate)
	{
		Henchmen h = GetDummyData.instance.GetHenchmen (m_henchmenID);

		if (h != null) {

//			ContactsDetailViewMenu cMenu = (ContactsDetailViewMenu)m_menuParent.GetComponent<ContactsDetailViewMenu> ();
			m_name.text = h.m_name;
			m_portrait.texture = h.m_portrait_Large;
		}
		this.gameObject.SetActive (true);

		if (m_currentPanel == InfoPanelType.Traits) {
			SetInfoPanel (InfoPanelType.Traits);
		}

		if (animate) {
			// slide in animation
			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			Rect r = rt.rect;
			rt.anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);

			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, 0), 0.25f);

			// portrait image slides slightly to the right
			RectTransform picRT = m_portrait.GetComponent<RectTransform> ();
			picRT.anchoredPosition = new Vector2 (-200, 0);
			DOTween.To (() => picRT.anchoredPosition, x => picRT.anchoredPosition = x, new Vector2 (0, 0), 0.25f);

			// info panel slides up slightly
			RectTransform infoPanelRT = m_panels [0].GetComponent<RectTransform> ();
			infoPanelRT.anchoredPosition = new Vector2 (0, -200);
			RectTransform infoPanel2RT = m_panels [1].GetComponent<RectTransform> ();
			infoPanel2RT.anchoredPosition = new Vector2 (0, -200);
			DOTween.To (() => infoPanelRT.anchoredPosition, x => infoPanelRT.anchoredPosition = x, new Vector2 (0, 0), 0.25f);
			DOTween.To (() => infoPanel2RT.anchoredPosition, x => infoPanel2RT.anchoredPosition = x, new Vector2 (0, 0), 0.25f);


			// back button slides down 
			RectTransform appNavRT = m_panels [2].GetComponent<RectTransform> ();
			appNavRT.anchoredPosition = new Vector2 (0, appNavRT.rect.height);
			DOTween.To (() => appNavRT.anchoredPosition, x => appNavRT.anchoredPosition = x, new Vector2 (0, 0), 0.25f).SetDelay (0.15f);

		} else {

			RectTransform appNavRT = m_panels[2].GetComponent<RectTransform> ();
			appNavRT.anchoredPosition = Vector2.zero;

			RectTransform infoPanelRT = m_panels[0].GetComponent<RectTransform> ();
			infoPanelRT.anchoredPosition = Vector2.zero;
			RectTransform infoPanel2RT = m_panels[1].GetComponent<RectTransform> ();
			infoPanel2RT.anchoredPosition = Vector2.zero;

			RectTransform picRT = m_portrait.GetComponent<RectTransform> ();
			picRT.anchoredPosition = Vector2.zero;

			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			rt.anchoredPosition = Vector2.zero;

		}

	}

	public void OnExit (bool animate)
	{
		if (animate) {
			// slide out animation
			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			Rect r = rt.rect;

			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0), 0.25f).OnComplete(OnExitComplete);

		} else {
			OnExitComplete ();
		}
	}

	public void OnExitComplete ()
	{
		RectTransform appNavRT = m_panels[2].GetComponent<RectTransform> ();
		appNavRT.anchoredPosition = Vector2.zero;

		RectTransform infoPanelRT = m_panels[0].GetComponent<RectTransform> ();
		infoPanelRT.anchoredPosition = Vector2.zero;
		RectTransform infoPanel2RT = m_panels[1].GetComponent<RectTransform> ();
		infoPanel2RT.anchoredPosition = Vector2.zero;

		RectTransform picRT = m_portrait.GetComponent<RectTransform> ();
		picRT.anchoredPosition = Vector2.zero;

		RectTransform rt = gameObject.GetComponent<RectTransform> ();
		rt.anchoredPosition = Vector2.zero;

		while (m_traitCells.Count > 0) {
			UICell c = m_traitCells [0];
			m_traitCells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		this.gameObject.SetActive (false);
	}

	public void OnHold ()
	{

	}

	public void OnReturn ()
	{

	}

	public void SetHenchmen (int id)
	{
		m_henchmenID = id;
	}

	public void BackButtonPressed ()
	{
		Notify (this, GameEvent.UI_BackButtonPressed);
	}

	public void AddObserver (IObserver observer)	
	{
		m_observers.Add (observer);
	}

	public void RemoveObserver (IObserver observer)
	{
		if (m_observers.Contains(observer))
		{
			m_observers.Remove(observer);
		}
	}

	public void Notify (ISubject subject, GameEvent thisGameEvent)
	{
		for (int i=0; i < m_observers.Count; i++)
		{
			m_observers[i].OnNotify(subject, thisGameEvent);
		}
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent)
		{
		case GameEvent.UI_ToggleButtonPressed:

			switch (m_infoPanelToggle.activeButton) {
			case 0:
				SetInfoPanel (InfoPanelType.Traits);
				break;
			case 1:
				SetInfoPanel (InfoPanelType.History);
				break;
			case 2:
				SetInfoPanel (InfoPanelType.Info);
				break;
			}

			break;
		}
	}

	private void SetInfoPanel (InfoPanelType pType)
	{
		
		switch (m_currentPanel) {
		case InfoPanelType.Traits:
			m_infoPanels [0].gameObject.SetActive (false);
			break;
		case InfoPanelType.History:
			m_infoPanels [1].gameObject.SetActive (false);
			break;
		case InfoPanelType.Info:
			m_infoPanels [2].gameObject.SetActive (false);
			break;

		}

		switch (pType) {
		case InfoPanelType.Traits:
			m_infoPanels [0].gameObject.SetActive (true);

			// remove old trait cells

			while (m_traitCells.Count > 0) {
				UICell c = m_traitCells [0];
				m_traitCells.RemoveAt (0);
				Destroy (c.gameObject);
			}

			// create new trait cells

			Henchmen h = GetDummyData.instance.GetHenchmen (m_henchmenID);

			if (h != null) {

				foreach (Trait t in h.m_traits) {

					GameObject traitCellGO = (GameObject)Instantiate (m_traitCellGO, m_panels[3]);
					UICell tCell = (UICell)traitCellGO.GetComponent<UICell> ();
					tCell.m_headerText.text = t.m_name;
					m_traitCells.Add (tCell);
				}
			}

			break;
		case InfoPanelType.History:
			m_infoPanels [1].gameObject.SetActive (true);
			break;
		case InfoPanelType.Info:
			m_infoPanels [2].gameObject.SetActive (true);
			break;

		}

		m_currentPanel = pType;

//		for (int i = 0; i < m_infoPanels.Length; i++) {
//
//			if (i == panelNum) {
//				m_infoPanels [i].gameObject.SetActive (true);
//			} else {
//				m_infoPanels [i].gameObject.SetActive (false);
//			}
//		}
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}
}
