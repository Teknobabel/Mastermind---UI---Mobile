using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CackleApp : ScriptableObject, IApp {

	public string m_name;
	public Sprite m_icon;
	public Sprite m_icon_Pressed;
	public List<GameObject> m_menuBank;

	public void InitializeApp ()
	{

	}

	public void EnterApp ()
	{

	}

	public void UpdateApp ()
	{

	}

	public void ExitApp ()
	{

	}

	public void HoldApp ()
	{

	}

	public void AppReturn ()
	{

	}

	public void PushMenu (IMenu menu)
	{

	}

	public void PopMenu ()
	{

	}

	public string Name 
	{
		get
		{
			return m_name;
		}
		set
		{
			m_name = value;
		}   
	}

	public Sprite Icon 
	{
		get
		{
			return m_icon;
		}
		set
		{
			m_icon = value;
		}   
	}

	public Sprite Icon_Pressed 
	{
		get
		{
			return m_icon_Pressed;
		}
		set
		{
			m_icon_Pressed = value;
		}   
	}

	public List<GameObject> MenuBank 
	{ get { return m_menuBank; }}

	public bool WantsSystemNavBar 
	{ get{ return true; } }
}
