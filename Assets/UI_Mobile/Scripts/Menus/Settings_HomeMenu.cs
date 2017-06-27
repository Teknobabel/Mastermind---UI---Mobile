using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings_HomeMenu : MonoBehaviour, IMenu {

	private IApp m_parentApp;

	public Texture
	m_toggleEmpty,
	m_toggleFilled;

	public UICell 
	m_tutorialToggleCell;

	private bool m_tutorialEnabled = true;

	// Use this for initialization
	void Start () {

		if (PlayerPrefs.HasKey ("PlayTutorial")) {

			int playTutorial = PlayerPrefs.GetInt ("PlayTutorial");

			if (playTutorial == 0) {

				m_tutorialToggleCell.m_image.texture = m_toggleEmpty;
				m_tutorialEnabled = false;

			} else {

				m_tutorialToggleCell.m_image.texture = m_toggleFilled;
				m_tutorialEnabled = true;

			}
		}
	}

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
//		m_appNameText.text = parentApp.Name;
		this.gameObject.SetActive (false);
	}
	
	public void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

//		List<Henchmen> hList = GetDummyData.instance.GetHenchmenList ();
//
//		foreach (Henchmen h in hList) {
//
//			GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
//			UICell c = (UICell)hCell.GetComponent<UICell> ();
//			m_cells.Add (c);
//
//			string nameString = h.m_name;
//			string statusString = "Active";
//			//			s += "\nStatus: " + h.m_status;
//			//			s += "\nLocation: " + h.m_location;
//
//			c.m_headerText.text = nameString;
//			c.m_bodyText.text = statusString;
//			c.m_image.texture = h.m_portrait_Small;
//
//			hCell.GetComponent<Button>().onClick.AddListener(delegate { ((HenchmenApp)m_parentApp).HenchmenCellClicked(h.m_id); });
//			c.m_rectTransforms[0].anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);
//
//		}
//
//		// slide in animation
//		if (animate) {
//
//			RectTransform rt = gameObject.GetComponent<RectTransform> ();
//			Rect r = rt.rect;
//			rt.anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);
//
//			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, 0), 0.5f);
//
//			for (int i = 0; i < m_cells.Count; i++) {
//
//				UICell c = m_cells [i];
//				DOTween.To (() => c.m_rectTransforms [0].anchoredPosition, x => c.m_rectTransforms [0].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetEase (Ease.OutCirc).SetDelay (0.25f + (i * 0.07f));
//
//				c.m_image.transform.localScale = Vector3.zero;
//				DOTween.To (() => c.m_image.transform.localScale, x => c.m_image.transform.localScale = x, new Vector3 (1, 1, 1), 0.5f).SetEase (Ease.OutCirc).SetDelay (0.75f + (i * 0.09f));
//			}
//		} 
//		else {
//
//			RectTransform rt = gameObject.GetComponent<RectTransform> ();
//			rt.anchoredPosition = Vector2.zero;
//
//		}
	}

	public void OnExit (bool animate)
	{
//		int playTutorial = PlayerPrefs.GetInt ("PlayTutorial");
//		bool savePrefs = false;
//
//		if (playTutorial == 0 && m_tutorialEnabled) {
//
//			PlayerPrefs.SetInt ("PlayTutorial", 1);
//			savePrefs = true;
//
//		} else if (playTutorial == 1 && !m_tutorialEnabled)
//		{
//			PlayerPrefs.SetInt ("PlayTutorial", 0);
//			savePrefs = true;
//		}
//
//		if (savePrefs)
//		{
//			PlayerPrefs.Save();
//		}
			
//		if (animate) {
//			// slide out animation
//			RectTransform rt = gameObject.GetComponent<RectTransform> ();
//			Rect r = rt.rect;
//
//			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0), 0.5f);
//
//		} else {

			OnExitComplete ();
//		}

	}

	public void TutorialButtonClicked ()
	{
		if (m_tutorialEnabled) 
		{

			m_tutorialEnabled = false;
			m_tutorialToggleCell.m_image.texture = m_toggleEmpty;

			PlayerPrefs.SetInt ("PlayTutorial", 0);
			PlayerPrefs.Save();
		} else {

			m_tutorialEnabled = true;
			m_tutorialToggleCell.m_image.texture = m_toggleFilled;

			PlayerPrefs.SetInt ("PlayTutorial", 1);
			PlayerPrefs.Save();
		}
	}

	public void ResetButtonClicked ()
	{
		SceneManager.LoadScene (0);
	}

	public void OnExitComplete ()
	{

//		while (m_cells.Count > 0) {
//
//			UICell c = m_cells [0];
//			m_cells.RemoveAt (0);
//			Destroy (c.gameObject);
//		}
//
//		RectTransform rt = gameObject.GetComponent<RectTransform> ();
//		rt.anchoredPosition = Vector2.zero;

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
}
