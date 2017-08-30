using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}

	public virtual void OnExit (bool animate)
	{
		m_isDirty = false;
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
