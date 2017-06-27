using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Message : ScriptableObject {

	public enum MessageOrigin
	{
		None,
		Henchmen,
		Player,
	}

	public MessageOrigin m_origin = MessageOrigin.None;

	public string m_messageText = "Null";
}
