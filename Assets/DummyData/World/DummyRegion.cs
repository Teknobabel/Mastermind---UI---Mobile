using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DummyRegion : ScriptableObject {

	public enum DummyRegionType {
		None,
		Politics,
		Military,
		Economy,
	}


	public string m_name = "Null";

	public DummyRegionType m_type = DummyRegionType.None;

	public DummyAsset[] m_assets;

}
