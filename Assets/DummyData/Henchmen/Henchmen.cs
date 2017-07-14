using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Henchmen : ScriptableObject {

	public string 
	m_name = "Null",
	m_status = "Null",
	m_location = "Null";

	public int
	m_id = 0,
	m_cost = 1;

	public Texture 
	m_portrait_Small,
	m_portrait_Large;

	public DummyTrait[]
	m_traits;
}
