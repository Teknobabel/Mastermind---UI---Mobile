using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_PlayerActivity : UICell {

	public void SetPlayer (Player player)
	{
		m_headerText.text = player.orgName;

		OmegaPlan op = GameController.instance.GetOmegaPlan (player.id).m_omegaPlan;

		int numCompleted = 0;
		int numTotal = 0;

		foreach (OmegaPlan.Phase phase in op.phases) {

			foreach (OmegaPlan.OPGoal goal in phase.m_goals) {

				numTotal++;

				if (goal.m_state == OmegaPlan.OPGoal.State.Complete) {

					numCompleted++;
				}
			}
		}

		int percentComplete = numCompleted / numTotal;

		string s = "Omega Plan Completion: " + percentComplete + "%";

		m_bodyText.text = s;
	}
}
