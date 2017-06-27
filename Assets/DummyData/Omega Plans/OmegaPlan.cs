using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OmegaPlan : ScriptableObject {

	[System.Serializable]
	public struct Phase
	{
		public int m_phaseNumber;
		public OmegaPlan_Goal[] m_goals;
	}


	public string m_name = "Null";

	public Phase[] m_phases;
}
