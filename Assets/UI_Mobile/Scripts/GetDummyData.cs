using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDummyData : MonoBehaviour {
	public static GetDummyData instance;

//	public class Henchmen
//	{
//		public string 
//		m_name = "Null",
//		m_status = "Null",
//		m_location = "Null";
//
//		public int
//		m_costPerTurn = 0,
//		m_id = 0;
//	}

	public List<ScriptableObject> m_henchmenBank;

	public OmegaPlan m_omegaPlan;

	public Lair m_lair;

	public MessageCenter m_messageCenter;

	public Region[] m_regions;

//	private List<Henchmen> m_henchmenList = new List<Henchmen>();

	// Use this for initialization
	void Awake () {
		if(!instance) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	public void Initialize ()
	{
//		Henchmen h1 = new Henchmen ();
//		h1.m_name = "Henchmen 1";
//		h1.m_status = "Normal";
//		h1.m_location = "USA";
//		h1.m_costPerTurn = 1;
//		h1.m_id = 0;
//		m_henchmenList.Add (h1);
//
//		Henchmen h2 = new Henchmen ();
//		h2.m_name = "Henchmen 2";
//		h2.m_status = "Normal";
//		h2.m_location = "Russia";
//		h2.m_costPerTurn = 2;
//		h2.m_id = 1;
//		m_henchmenList.Add (h2);
//
//		Henchmen h3 = new Henchmen ();
//		h3.m_name = "Henchmen 3";
//		h3.m_status = "Normal";
//		h3.m_location = "China";
//		h3.m_costPerTurn = 3;
//		h3.m_id = 2;
//		m_henchmenList.Add (h3);
	}

	public MessageCenter.Conversation GetConversation (int henchmenID)
	{

//		MessageCenter.Conversation convo = null;
		foreach (MessageCenter.Conversation c in m_messageCenter.m_conversations) {

			if (c.m_henchmenID == henchmenID) {

				return c;
			}
		}

		return new MessageCenter.Conversation();
	}

	public Dictionary<int, List<string>> GetActivityFeed ()
	{
		Dictionary<int, List<string>> feed = new Dictionary<int, List<string>> ();

		for (int i = 0; i < 20; i++) {

			List<string> sList = new List<string> ();

			int numEntries = Random.Range (1, 8);

			for (int j = 0; j < numEntries; j++) {

				string s = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

				if (Random.Range (0.0f, 1.0f) > 0.5f) {

					s += "\n\n Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ";

					if (Random.Range (0.0f, 1.0f) > 0.5f) {
						s += " Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.";
					
						if (Random.Range (0.0f, 1.0f) > 0.5f) {
							s += "\n\n Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
						}
					}
				}

				sList.Add (s);
			}

			feed.Add (i, sList);
		}

		return feed;
	}

	public Henchmen GetHenchmen (int id)
	{

		foreach (ScriptableObject so in m_henchmenBank) {

			Henchmen h = (Henchmen)so;

			if (h.m_id == id) {

				return h;
			}
		}

		return null;
	}


	public List<Henchmen> GetHenchmenList ()
	{
		List<Henchmen> henchmenList = new List<Henchmen> ();

		foreach (ScriptableObject so in m_henchmenBank) {

			henchmenList.Add ((Henchmen)so);
		}

		return henchmenList;
	}

	public OmegaPlan GetOmegaPlan ()
	{
		return m_omegaPlan;
	}
	
//	// Update is called once per frame
//	void Update () {
//		
//	}
}
