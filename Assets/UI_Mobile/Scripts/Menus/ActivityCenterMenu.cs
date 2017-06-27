using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class ActivityCenterMenu : MonoBehaviour {

	public Transform
	m_scrollViewParent;

	public GameObject
	m_activityHeader,
	m_activityCell;

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize ()
	{
		Dictionary<int, List<string>> feed = GetDummyData.instance.GetActivityFeed ();
		SetActivity (feed);
	}

	public void SetActivity (Dictionary<int, List<string>> activityList)
	{
		foreach(KeyValuePair<int, List<string>> entry in activityList.Reverse())
		{
			GameObject header = (GameObject)Instantiate (m_activityHeader, m_scrollViewParent);
			UICell headerCell = (UICell)header.GetComponent<UICell> ();
			headerCell.m_headerText.text = "Turn " + entry.Key.ToString ();

			List<string> sList = entry.Value;

			for (int i = 0; i < sList.Count; i++) {
					
				string s = sList[i];
				GameObject cellGO = (GameObject)Instantiate (m_activityCell, m_scrollViewParent);
				UICell cell = (UICell)cellGO.GetComponent<UICell> ();
				cell.m_bodyText.text = s;

				if (entry.Key == activityList.Count-1) {
//					Debug.Log (MobileUIEngine.instance.m_mainCanvas.sizeDelta.x);

					// animate
//					cell.m_bodyText.gameObject.SetActive(false);
					cell.m_rectTransforms [0].anchoredPosition = new Vector2(MobileUIEngine.instance.m_mainCanvas.sizeDelta.x, 0);
//					DOTween.To (() => cell.m_rectTransforms [0].anchoredPosition, x => cell.m_rectTransforms [0].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetEase (Ease.OutCirc).SetDelay (0.25f + (i * 0.07f));
				}
			}
		}
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
