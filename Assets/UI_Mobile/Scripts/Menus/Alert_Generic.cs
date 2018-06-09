using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alert_Generic : BaseMenu {

	public GameObject
	m_buttonGO;

	public Text 
	m_messageHeader,
	m_messageBody;

	public Transform 
	m_contentParent;

	private bool
		m_alertActive = false;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
//		base.OnEnter (animate);

		this.gameObject.SetActive (true);

		m_alertActive = true;
	}

	public override void OnExit (bool animate)
	{
//		base.OnExit (animate);

		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		this.gameObject.SetActive (false);

		m_alertActive = false;
	}

	public void SetAlert (string header, string body, IApp parentApp)
	{
		m_messageHeader.text = header;
		m_messageBody.text = body;
		m_parentApp = parentApp;
	}

	public Button AddButton (string buttonName)
	{
		GameObject buttonGO = (GameObject)Instantiate (m_buttonGO, m_contentParent);
		UICell buttonCell = (UICell)buttonGO.GetComponent<UICell> ();
		buttonCell.m_headerText.text = buttonName;
		m_cells.Add (buttonCell);
		return buttonCell.m_buttons [0];

//		LayoutRebuilder.ForceRebuildLayoutImmediate (m_contentParent.GetComponent<RectTransform>());
	}

	public void DismissButtonTapped ()
	{
		m_parentApp.PopMenu ();
	}

	public bool alertActive {get{ return m_alertActive; }}
}
