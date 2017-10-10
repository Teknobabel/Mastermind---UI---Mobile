using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager {

	public enum PlayerPrefKeys
	{
		None,
		PlayTutorial,
		HelpEnabled,
		FirstTime_OmegaPlan,
		FirstTime_Contacts,
		FirstTime_Hire,
		FirstTime_Lair,
		FirstTime_World,
		FirstTime_Assets,
		FirstTime_Missions,
		FirstTime_PlanMission,

	}

	private Dictionary<PlayerPrefKeys, int> m_prefList = new Dictionary<PlayerPrefKeys, int>();

	public void Initialize ()
	{
		if (!PlayerPrefs.HasKey ("PlayTutorial")) {

			m_prefList.Add (PlayerPrefKeys.PlayTutorial, 0);
			m_prefList.Add (PlayerPrefKeys.HelpEnabled, 1);
			m_prefList.Add (PlayerPrefKeys.FirstTime_OmegaPlan, 1);
			m_prefList.Add (PlayerPrefKeys.FirstTime_Contacts, 1);
			m_prefList.Add (PlayerPrefKeys.FirstTime_Hire, 1);
			m_prefList.Add (PlayerPrefKeys.FirstTime_Lair, 1);
			m_prefList.Add (PlayerPrefKeys.FirstTime_World, 1);
			m_prefList.Add (PlayerPrefKeys.FirstTime_Assets, 1);
			m_prefList.Add (PlayerPrefKeys.FirstTime_Missions, 1);
			m_prefList.Add (PlayerPrefKeys.FirstTime_PlanMission, 1);

			foreach (KeyValuePair<PlayerPrefKeys, int> pair in m_prefList) {

				PlayerPrefs.SetInt (pair.Key.ToString (), pair.Value);
			}

			PlayerPrefs.Save ();

		} else {

			m_prefList.Add (PlayerPrefKeys.PlayTutorial, PlayerPrefs.GetInt( PlayerPrefKeys.PlayTutorial.ToString()));
			m_prefList.Add (PlayerPrefKeys.HelpEnabled, PlayerPrefs.GetInt( PlayerPrefKeys.HelpEnabled.ToString()));
			m_prefList.Add (PlayerPrefKeys.FirstTime_OmegaPlan, PlayerPrefs.GetInt( PlayerPrefKeys.FirstTime_OmegaPlan.ToString()));
			m_prefList.Add (PlayerPrefKeys.FirstTime_Contacts, PlayerPrefs.GetInt( PlayerPrefKeys.FirstTime_Contacts.ToString()));
			m_prefList.Add (PlayerPrefKeys.FirstTime_Hire, PlayerPrefs.GetInt( PlayerPrefKeys.FirstTime_Hire.ToString()));
			m_prefList.Add (PlayerPrefKeys.FirstTime_Lair, PlayerPrefs.GetInt( PlayerPrefKeys.FirstTime_Lair.ToString()));
			m_prefList.Add (PlayerPrefKeys.FirstTime_World, PlayerPrefs.GetInt( PlayerPrefKeys.FirstTime_World.ToString()));
			m_prefList.Add (PlayerPrefKeys.FirstTime_Assets, PlayerPrefs.GetInt( PlayerPrefKeys.FirstTime_Assets.ToString()));
			m_prefList.Add (PlayerPrefKeys.FirstTime_Missions, PlayerPrefs.GetInt( PlayerPrefKeys.FirstTime_Missions.ToString()));
			m_prefList.Add (PlayerPrefKeys.FirstTime_PlanMission, PlayerPrefs.GetInt( PlayerPrefKeys.FirstTime_PlanMission.ToString()));

		}
	}

	public void ResetPlayerPrefs ()
	{
		m_prefList.Add (PlayerPrefKeys.PlayTutorial, 0);
		m_prefList.Add (PlayerPrefKeys.HelpEnabled, 1);
		m_prefList.Add (PlayerPrefKeys.FirstTime_OmegaPlan, 1);
		m_prefList.Add (PlayerPrefKeys.FirstTime_Contacts, 1);
		m_prefList.Add (PlayerPrefKeys.FirstTime_Hire, 1);
		m_prefList.Add (PlayerPrefKeys.FirstTime_Lair, 1);
		m_prefList.Add (PlayerPrefKeys.FirstTime_World, 1);
		m_prefList.Add (PlayerPrefKeys.FirstTime_Assets, 1);
		m_prefList.Add (PlayerPrefKeys.FirstTime_Missions, 1);
		m_prefList.Add (PlayerPrefKeys.FirstTime_PlanMission, 1);

		foreach (KeyValuePair<PlayerPrefKeys, int> pair in m_prefList) {

			PlayerPrefs.SetInt (pair.Key.ToString (), pair.Value);
		}

		PlayerPrefs.Save ();

		m_prefList.Clear ();
	}

	public void SetPref (PlayerPrefKeys key, int value)
	{
		m_prefList [key] = value;
		PlayerPrefs.Save ();
	}

	public int GetPref (PlayerPrefKeys key)
	{
		return m_prefList [key];
	}

}
