using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageIndicator : MonoBehaviour {

	public Texture 
	m_filledSprite,
	m_emptySprite;

	public GameObject
	m_pageIndicator;

	public Transform
		m_pageIndicatorParent;


	private Dictionary <int, RawImage> m_pageObjects = new Dictionary<int, RawImage>();

	private int m_currentPage = -1;



	// Use this for initialization
	void Start () {
		
	}

	public void SetNumPages (int numPages)
	{

		if (numPages > 1) {

			for (int i = 0; i < numPages; i++) {
				
				GameObject go = (GameObject)Instantiate (m_pageIndicator, m_pageIndicatorParent);
				RawImage ri = (RawImage)go.GetComponent<RawImage> ();

				if (ri) {

					ri.texture = m_emptySprite;
				}

				m_pageObjects.Add (i, ri);
			}
		}

	}

	public void SetPage (int currentPage)
	{
		if (currentPage != m_currentPage) {
			
			if (m_pageObjects.Count > 0) {

				if (m_pageObjects.ContainsKey (m_currentPage)) {

					m_pageObjects [m_currentPage].texture = m_emptySprite;
				}

				if (m_pageObjects.ContainsKey (currentPage)) {

					m_pageObjects [currentPage].texture = m_filledSprite;
					m_currentPage = currentPage;
				}
			}
		}
	}



}
