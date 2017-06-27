using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContactsMenu : MonoBehaviour, IMenu, IObserver {

	public enum DisplayType {
		Alpha,
		Trait,
		Mission,
	}

	public Text
	m_appNameText;

	public GameObject
	m_henchmenCellGO,
	m_headerCellGO;

	public Transform
	m_contactsListParent;

	public SegmentedToggle m_infoPanelToggle;

	private IApp m_parentApp;

	private List<UICell> m_cells = new List<UICell>();

	private DisplayType m_displayType = DisplayType.Alpha;

//	private Transform m_menuParent;

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
		m_appNameText.text = parentApp.Name;
		m_infoPanelToggle.AddObserver (this);
		m_infoPanelToggle.ToggleButtonClicked (0);
		this.gameObject.SetActive (false);
	}

	public void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		DisplayHenchmen ();

		// slide in animation
		if (animate) {
			
			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			Rect r = rt.rect;
			rt.anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);

			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, 0), 0.5f);

			for (int i = 0; i < m_cells.Count; i++) {

				UICell c = m_cells [i];
				c.m_rectTransforms[0].anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);
				DOTween.To (() => c.m_rectTransforms [0].anchoredPosition, x => c.m_rectTransforms [0].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetEase (Ease.OutCirc).SetDelay (0.25f + (i * 0.07f));

				if (c.m_image != null) {
					c.m_image.transform.localScale = Vector3.zero;
					DOTween.To (() => c.m_image.transform.localScale, x => c.m_image.transform.localScale = x, new Vector3 (1, 1, 1), 0.5f).SetEase (Ease.OutCirc).SetDelay (0.75f + (i * 0.09f));
				}
			}
		} 
		else {

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

			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0), 0.5f);

		} else {

			OnExitComplete ();
		}

	}

	public void OnExitComplete ()
	{

		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		RectTransform rt = gameObject.GetComponent<RectTransform> ();
		rt.anchoredPosition = Vector2.zero;

		this.gameObject.SetActive (false);
	}

	public void OnHold ()
	{

	}

	public void OnReturn ()
	{

	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent)
		{
		case GameEvent.UI_ToggleButtonPressed:
			

			switch (m_infoPanelToggle.activeButton) {
			case 0:
				m_displayType = DisplayType.Alpha;
				break;
			case 1:
				m_displayType = DisplayType.Trait;
				break;
			case 2:
				m_displayType = DisplayType.Mission;
				break;
				
			}
			DisplayHenchmen ();
			break;
		}
	}

	private void DisplayHenchmen ()
	{
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

		List<Henchmen> hList = GetDummyData.instance.GetHenchmenList ();

		switch (m_displayType)
		{
		case DisplayType.Alpha:

			hList.Sort (delegate(Henchmen a, Henchmen b) {
				return a.m_name.CompareTo (b.m_name);
			});

			foreach (Henchmen h in hList) {

				GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
				UICell c = (UICell)hCell.GetComponent<UICell> ();
				m_cells.Add (c);

				string nameString = h.m_name;
				string statusString = "Active";

				c.m_headerText.text = nameString;
				c.m_bodyText.text = statusString;
				c.m_image.texture = h.m_portrait_Small;

				hCell.GetComponent<Button> ().onClick.AddListener (delegate {
					((HenchmenApp)m_parentApp).HenchmenCellClicked (h.m_id);
				});
			}
			break;

		case DisplayType.Trait:

			Dictionary<string, List<Henchmen>> hListByTrait = new Dictionary<string, List<Henchmen>> ();

			foreach (Henchmen h in hList) {

				foreach (Trait t in h.m_traits) {

					if (hListByTrait.ContainsKey (t.m_name)) {

						List<Henchmen> l = hListByTrait [t.m_name];
						l.Add (h);
						hListByTrait [t.m_name] = l;

					} else {

						List<Henchmen> newList = new List<Henchmen> ();
						newList.Add (h);
						hListByTrait.Add (t.m_name, newList);
					}
				}
			}

			if (hListByTrait.Count > 0) {

				foreach(KeyValuePair<string, List<Henchmen>> entry in hListByTrait)
				{
					GameObject header = (GameObject)Instantiate (m_headerCellGO, m_contactsListParent);
					UICell headerCell = (UICell)header.GetComponent<UICell> ();
					headerCell.m_headerText.text = entry.Key.ToString ();
					m_cells.Add (headerCell);

					List<Henchmen> sortedList = entry.Value;

					sortedList.Sort (delegate(Henchmen a, Henchmen b) {
						return a.m_name.CompareTo (b.m_name);
					});

					foreach (Henchmen h in sortedList) {

						GameObject hCell = (GameObject)Instantiate (m_henchmenCellGO, m_contactsListParent);
						UICell c = (UICell)hCell.GetComponent<UICell> ();
						m_cells.Add (c);

						string nameString = h.m_name;
						string statusString = "Active";

						c.m_headerText.text = nameString;
						c.m_bodyText.text = statusString;
						c.m_image.texture = h.m_portrait_Small;

						hCell.GetComponent<Button> ().onClick.AddListener (delegate {
							((HenchmenApp)m_parentApp).HenchmenCellClicked (h.m_id);
						});
					}
				}
			}
			
			break;
		}
	}
	
	public IApp ParentApp 
	{ get{ return m_parentApp; }}
}
