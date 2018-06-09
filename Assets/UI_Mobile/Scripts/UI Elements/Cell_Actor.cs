using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Actor : UICell {

	public void SetActor (Player.ActorSlot actorSlot)
	{
		string nameString = actorSlot.m_actor.m_actorName;

		string statusString = "Level " + actorSlot.m_actor.level.ToString() + " " + (actorSlot.m_actor.traits [0]).m_name;

		if (actorSlot.m_actor.traits [0].m_name != "Agent")
		{
			statusString += " (XP: " + actorSlot.m_actor.xp.ToString () + "/" + actorSlot.m_actor.GetXPForLevelUp().ToString () + ")";

		}

//		switch (actorSlot.m_actor.m_rank) {
//
//		case 1:
//			statusString += "Novice ";
//			break;
//		case 2:
//			statusString += "Skilled ";
//			break;
//		case 3:
//			statusString += "Veteran ";
//			break;
//		case 4:
//			statusString += "Master ";
//			break;
//		}
//
//		if (actorSlot.m_actor.traits.Count > 0) {
//
//			Trait t = actorSlot.m_actor.traits [0];
//			statusString += t.m_name;
//		}

		m_headerText.text = nameString;
		m_bodyText.text = statusString;
//		m_image.texture = actorSlot.m_actor.m_portrait_Compact;
		m_image.texture = actorSlot.m_actor.m_portrait_Large;

//		if (actorSlot.m_new) {
//			m_rectTransforms [1].gameObject.SetActive (true);
//		}
	}

	public void DisplayPrice (int price)
	{
		// re enable when new ui is in place

//		m_rectTransforms [2].gameObject.SetActive (true);
//		m_text [0].text = price.ToString ();
	}

	public void DisplaySelectButton ()
	{
		m_buttons [0].gameObject.SetActive (true);
	}

	public void SetActor (Actor actor)
	{

	}
}
