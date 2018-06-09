using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseMenu : MonoBehaviour, IMenu {

	protected IApp m_parentApp;

	protected bool m_isDirty = false;

	protected List<UICell> m_cells = new List<UICell>();

	public virtual void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
	}

	public virtual void OnEnter (bool animate)
	{
		DisplayContent ();

		// slide in animation
		if (animate) {

			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			Rect r = rt.rect;
			rt.anchoredPosition = new Vector2 (MobileUIEngine.instance.m_mainCanvas.rect.width, 0);

			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2 (0, 0), 0.5f);
		} 
		else {

			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			rt.anchoredPosition = Vector2.zero;

		}
	}

	public void OnExitComplete ()
	{
		this.gameObject.SetActive (false);
	}

	public virtual void OnExit (bool animate)
	{
		m_isDirty = false;

		// slide in animation
		if (animate) {

			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			Rect r = rt.rect;

			DOTween.To (() => rt.anchoredPosition, x => rt.anchoredPosition = x, new Vector2(MobileUIEngine.instance.m_mainCanvas.rect.width, 0), 0.5f).OnComplete(OnExitComplete);
		} 
		else {

			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			rt.anchoredPosition = Vector2.zero;

		}
	}

	public virtual void OnHold (){}

	public virtual void OnReturn ()
	{
		if (m_isDirty) {

			m_isDirty = false;
			DisplayContent ();
		}
	}

	public virtual void DisplayContent ()
	{
		while (m_cells.Count > 0) {

			UICell c = m_cells [0];
			m_cells.RemoveAt (0);
			Destroy (c.gameObject);
		}
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}

	public bool isDirty {set{ m_isDirty = value; }}
}
