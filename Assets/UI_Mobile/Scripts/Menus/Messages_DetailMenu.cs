using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Messages_DetailMenu : MonoBehaviour, IMenu, IUISubject {

	public GameObject
	m_messageCellGO;

	public Transform
	m_contentParent;

	public UICell
	m_henchmenInfo;

	private IApp m_parentApp;

	protected int m_henchmenID = -1;

	private List<IUIObserver>
	m_observers = new List<IUIObserver> ();

	private List<UICell> m_cells = new List<UICell>();

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
//		m_appNameText.text = parentApp.Name;
//		m_infoPanelToggle.AddObserver (this);
//		m_infoPanelToggle.ToggleButtonClicked (0);
		this.gameObject.SetActive (false);
	}

	public void NewMessageButtonClicked ()
	{
		m_parentApp.PushMenu (((MessagesApp)m_parentApp).newMessageOverlay);
	}

	public void DisplayMessages ()
	{
		DummyMessageCenter.Conversation convo = GetDummyData.instance.GetConversation (m_henchmenID);
		Henchmen h = GetDummyData.instance.GetHenchmen (m_henchmenID);

		m_henchmenInfo.m_image.texture = h.m_portrait_Small;
		m_henchmenInfo.m_bodyText.text = h.m_name;

//		Debug.Log (convo.m_messages.Count);
		foreach (Message m in convo.m_messages) {

//			GameObject cellGO = (GameObject)Instantiate (m_messageCellGO, m_contentParent);
			GameObject cellGO = (GameObject)Instantiate (m_messageCellGO, Vector3.zero, Quaternion.identity);

			UICell cell = (UICell)cellGO.GetComponent<UICell> ();
			VerticalLayoutGroup vlg = (VerticalLayoutGroup)cellGO.GetComponent<VerticalLayoutGroup> ();
			float dist = MobileUIEngine.instance.m_mainCanvas.rect.width * 0.25f;

			if (m.m_origin == Message.MessageOrigin.Player) {

				vlg.padding.left = (int)dist;
				Image i = (Image)cell.m_rectTransforms [0].GetComponent<Image> ();
				i.color = Color.green;
				cell.m_bodyText.color = Color.white;

			} else {
				vlg.padding.right = (int)dist;
			}



			cell.m_bodyText.text = m.m_messageText;

			cellGO.transform.SetParent (m_contentParent, false);

			m_cells.Add (cell);
		}

//		LayoutRebuilder.MarkLayoutForRebuild (m_contentParent.GetComponent<RectTransform> ());
		LayoutRebuilder.ForceRebuildLayoutImmediate(m_contentParent.GetComponent<RectTransform> ());
	}

	public void OnEnter (bool animate)
	{
		this.gameObject.SetActive (true);

		DisplayMessages ();

		// slide in animation
		if (animate) {

			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			Rect r = rt.rect;
			rt.anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);

			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, 0), 0.5f);

//			for (int i = 0; i < m_cells.Count; i++) {
//
//				UICell c = m_cells [i];
//				c.m_rectTransforms[0].anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);
//				DOTween.To (() => c.m_rectTransforms [0].anchoredPosition, x => c.m_rectTransforms [0].anchoredPosition = x, new Vector2 (0, 0), 0.5f).SetEase (Ease.OutCirc).SetDelay (0.25f + (i * 0.07f));
//
//				if (c.m_image != null) {
//					c.m_image.transform.localScale = Vector3.zero;
//					DOTween.To (() => c.m_image.transform.localScale, x => c.m_image.transform.localScale = x, new Vector3 (1, 1, 1), 0.5f).SetEase (Ease.OutCirc).SetDelay (0.75f + (i * 0.09f));
//				}
//			}
		} 
//		else {
//
//			RectTransform rt = gameObject.GetComponent<RectTransform> ();
//			rt.anchoredPosition = Vector2.zero;
//
//		}
	}

	public void OnExit (bool animate)
	{
//		if (animate) {
//			// slide out animation
//			RectTransform rt = gameObject.GetComponent<RectTransform> ();
//			Rect r = rt.rect;
//
//			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0), 0.5f);
//
//		} else {
//
			OnExitComplete ();
//		}

	}

	public void OnExitComplete ()
	{

		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}

//		RectTransform rt = gameObject.GetComponent<RectTransform> ();
//		rt.anchoredPosition = Vector2.zero;
//
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
		Notify (this, UIEvent.UI_BackButtonPressed);
	}

	public void AddObserver (IUIObserver observer)	
	{
		m_observers.Add (observer);
	}

	public void RemoveObserver (IUIObserver observer)
	{
		if (m_observers.Contains(observer))
		{
			m_observers.Remove(observer);
		}
	}

	public void Notify (IUISubject subject, UIEvent thisUIEvent)
	{
		for (int i=0; i < m_observers.Count; i++)
		{
			m_observers[i].OnNotify(subject, thisUIEvent);
		}
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}
}
