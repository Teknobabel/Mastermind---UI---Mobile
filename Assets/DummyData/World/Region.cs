using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Region : ScriptableObject {

	public enum RegionType {
		None,
		Politics,
		Military,
		Economy,
	}


	public string m_name = "Null";

	public RegionType m_type = RegionType.None;

	public Asset[] m_assets;

}
