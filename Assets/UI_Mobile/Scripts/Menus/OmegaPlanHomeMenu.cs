using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OmegaPlanHomeMenu : MonoBehaviour, IMenu {

	public Text
	m_appNameText,
	m_phaseText;

	public GameObject
	m_opGoalCell;

	public Transform
	m_opGoalListParent;

	private IApp m_parentApp;

	private List<UICell> m_cells = new List<UICell>();

	private OmegaPlan.Phase m_phaseGoals;

	private int m_phaseNumber = -1;

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;

		Player.OmegaPlanSlot omegaPlan = GameController.instance.GetOmegaPlan (0);

		string s = parentApp.Name + ":\n";
		s += omegaPlan.m_omegaPlan.m_name;
		m_appNameText.text = s;


//		DummyOmegaPlan op = GetDummyData.instance.GetDummyOmegaPlan ();

		m_phaseText.text = "Phase " + m_phaseNumber.ToString();

		// populate with goals

		foreach (OPGoal opGoal in m_phaseGoals.m_goals) {

			GameObject gCell = (GameObject)Instantiate (m_opGoalCell, m_opGoalListParent);
			UICell c = (UICell)gCell.GetComponent<UICell> ();
			m_cells.Add (c);

			gCell.GetComponent<Button>().onClick.AddListener(delegate { ((OmegaPlansApp)m_parentApp).GoalButtonClicked(); });

			c.m_headerText.text = opGoal.name;
			c.m_bodyText.text = "Inactive";
		}
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
//
//			// system nav bar slides up
//
//			RectTransform sysNavRT = MobileUIEngine.instance.systemNavBar.GetComponent<RectTransform> ();
//			sysNavRT.anchoredPosition = new Vector2 (0, sysNavRT.rect.height * -1);
//			DOTween.To (() => sysNavRT.anchoredPosition, x => sysNavRT.anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetDelay (0.35f);
//
//			for (int i = 0; i < m_cells.Count; i++) {
//
//				UICell c = m_cells [i];
//				DOTween.To (() => c.m_rectTransforms [0].anchoredPosition, x => c.m_rectTransforms [0].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetEase(Ease.OutCirc).SetDelay (0.25f + (i * 0.07f));
//
//				c.m_image.transform.localScale = Vector3.zero;
//				DOTween.To (() => c.m_image.transform.localScale, x => c.m_image.transform.localScale = x, new Vector3 (1, 1, 1), 0.5f).SetEase(Ease.OutCirc).SetDelay (0.75f + (i * 0.09f));
//			}
//		}
	}

	public void OnExit (bool animate)
	{
//		// slide out animation
//		RectTransform rt = gameObject.GetComponent<RectTransform>();
//		Rect r = rt.rect;
//
//		DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0), 0.5f);
//
//		RectTransform sysNavRT = MobileUIEngine.instance.systemNavBar.GetComponent<RectTransform> ();
//		DOTween.To (() => sysNavRT.anchoredPosition, x => sysNavRT.anchoredPosition = x, new Vector2 (0, sysNavRT.rect.height * -1), 0.25f).SetDelay (0.35f);
//
//		while (m_cells.Count > 0) {
//
//			UICell c = m_cells [0];
//			m_cells.RemoveAt (0);
//			Destroy (c.gameObject);
//		}
//
	}

	public void OnExitComplete ()
	{

	}

	public void OnHold ()
	{

	}

	public void OnReturn ()
	{

	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}

	public OmegaPlan.Phase phaseGoals {get{ return m_phaseGoals;}set{m_phaseGoals = value; }}
	public int phaseNumber {set{ m_phaseNumber = value; }}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
