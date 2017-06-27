using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial_BaseMenu : MonoBehaviour, IMenu {

	[System.Serializable]
	public struct ProgressText
	{
		public float m_time;
		public string m_text;
	}

	public Image m_progressBarImage;

	public Text 
		m_progressTextField;

	public Button
	m_skipButton;

	[SerializeField]
	public List<ProgressText> m_progressText;

	protected IApp m_parentApp;

	protected float 
	m_holdTime = 3.0f,
	m_currentHoldTime = 0.0f;

	protected bool m_buttonHeld = false;

	protected List<ProgressText> m_currentProgressText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
		m_currentProgressText = new List<ProgressText> (m_progressText);

		this.gameObject.SetActive (false);
	}

	public void SkipTutorialButtonClicked ()
	{
		m_parentApp.ExitApp ();
	}

	public void DismissButtonClicked ()
	{
		((TutorialApp)m_parentApp).NextTutorialMenu ();
	}

	public void ButtonDown ()
	{
		m_buttonHeld = true;
	}

	public void ButtonUp ()
	{
		m_buttonHeld = false;

		if (m_currentHoldTime > 0 && m_currentHoldTime < m_holdTime) {

			m_currentHoldTime = 0;
			m_progressTextField.text = "";
			m_currentProgressText = new List<ProgressText> (m_progressText);
			DOTween.To (() => m_progressBarImage.fillAmount, x => m_progressBarImage.fillAmount = x, 0, 0.5f);
		}
	}

	public virtual void OnEnter (bool animate)
	{

	}

	public virtual void OnExit (bool animate)
	{

	}


	public void OnExitComplete ()
	{
		//		Color c = m_bgPanel.color;
		//		c.a = m_darkenAmount;
		//		m_bgPanel.color = c;
		//
		//		m_infoPanel.anchoredPosition = Vector2.zero;
		//
		this.gameObject.SetActive (false);
	}

	public virtual void OnHold ()
	{

	}

	public virtual void OnReturn ()
	{

	}



	public IApp ParentApp 
	{ get{ return m_parentApp; }}


}
