using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DummyMessageCenter : ScriptableObject {

	[SerializeField]
	public List<Conversation> m_conversations;

	[System.Serializable]
	public struct Conversation
	{
		public int m_henchmenID;
		public List<Message> m_messages;
	}
}
