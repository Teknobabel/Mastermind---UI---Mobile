using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Actor_Small : UICell {

	public void SetActor (Player.ActorSlot actorSlot)
	{
		string nameString = actorSlot.m_actor.m_actorName;

		string statusString = "";

		switch (actorSlot.m_actor.m_rank) {

		case 1:
			statusString += "Novice ";
			break;
		case 2:
			statusString += "Skilled ";
			break;
		case 3:
			statusString += "Veteran ";
			break;
		case 4:
			statusString += "Master ";
			break;
		}

		if (actorSlot.m_actor.traits.Count > 0) {

			Trait t = actorSlot.m_actor.traits [0];
			statusString += t.m_name;
		}

		m_headerText.text = nameString;
		m_bodyText.text = statusString;
		m_image.texture = actorSlot.m_actor.m_portrait_Large;
	}
}
