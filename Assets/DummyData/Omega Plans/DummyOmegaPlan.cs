using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DummyOmegaPlan : ScriptableObject {

	[System.Serializable]
	public struct Phase
	{
		public int m_phaseNumber;
		public DummyOmegaPlan_Goal[] m_goals;
	}


	public string m_name = "Null";

	public Phase[] m_phases;
}
