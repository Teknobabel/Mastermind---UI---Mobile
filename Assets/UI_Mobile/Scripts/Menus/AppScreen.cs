using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppScreen : MonoBehaviour {

	public RectTransform m_gridView;

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize (List<IApp> apps, GameObject appIcon)
	{
//		GridLayoutGroup glg = m_gridView.GetComponent<GridLayoutGroup> ();
//		float numColumns = (float)glg.constraintCount;
//		float gridWidth = (m_gridView.transform as RectTransform).rect.size.x;
//		float iconWidth = appIcon.GetComponent<RectTransform> ().sizeDelta.x;
//		float spaceRemaining = gridWidth - (iconWidth * numColumns);
//		float iconWidthSpacing = spaceRemaining / (numColumns - 1);
//		Debug.Log (gridWidth);
//		Vector2 spacing = glg.spacing;
//		spacing.x = iconWidthSpacing;
//		glg.spacing = spacing;

		foreach (IApp app in apps) {

			GameObject go = (GameObject)GameObject.Instantiate (appIcon, m_gridView);
			AppIcon ai = (AppIcon)go.GetComponent<AppIcon>();
			ai.Initialize(app);


		}
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
