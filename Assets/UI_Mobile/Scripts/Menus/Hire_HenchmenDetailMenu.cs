using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hire_HenchmenDetailMenu : ContactsDetailViewMenu {

	public Text 
	m_cost,
	m_turnCost;

	public override void OnEnter (bool animate)
	{
		Actor h = GameController.instance.GetActor (m_henchmenID);

		if (h != null) {

			m_cost.text = "-" + h.m_startingCost.ToString ();
			m_turnCost.text = "-" + h.m_turnCost.ToString () + "/Turn";

		}

		base.OnEnter (animate);
	}


	public void HireButtonPressed ()
	{
		Player.CommandPool cp = GameController.instance.GetCommandPool (0);
		Actor h = GameController.instance.GetActor (m_henchmenID);

		// check for open henchmen slot

		bool vacancy = false;

		List<Player.ActorSlot> pool = GameController.instance.GetHiredHenchmen (0);

		foreach (Player.ActorSlot aSlot in pool) {

			if (aSlot.m_state == Player.ActorSlot.ActorSlotState.Empty) {

				vacancy = true;
				break;
			}
		}

		// can player afford the actor

		if (cp.m_currentPool >= h.m_startingCost && vacancy) {

			Action_HireAgent hireAction = new Action_HireAgent ();
			hireAction.m_playerNumber = 0;
			hireAction.m_henchmenID = m_henchmenID;
			GameController.instance.ProcessAction (hireAction);

			Action_SpendCommandPoints payForHenchmen = new Action_SpendCommandPoints ();
			payForHenchmen.m_amount = h.m_startingCost;
			payForHenchmen.m_playerID = 0;
			GameController.instance.ProcessAction (payForHenchmen);

			// set home menu to reload henchmen list

			((HireApp)ParentApp).homeMenu.isDirty = true;

			ParentApp.PopMenu ();
		}
	}

	public void DismissButtonPressed ()
	{
		Debug.Log ("Dismiss Henchmen Button Pressed");

		Action_RemoveHireable dismissHenchmen = new Action_RemoveHireable ();
		dismissHenchmen.m_playerID = 0;
		dismissHenchmen.m_actorID = m_henchmenID;
		GameController.instance.ProcessAction (dismissHenchmen);

//		Player player = GameEngine.instance.game.playerList [0];
//
//		foreach (Player.ActorSlot aSlot in player.hiringPool.m_hireSlots) {
//
//			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty && aSlot.m_actor.id == m_henchmenID) {
//
//				Actor a = aSlot.m_actor;
//
//				aSlot.RemoveHenchmen ();
//
//				player.hiringPool.m_availableHenchmen.Add (a);
//
//				break;
//			}
//		}

		((HireApp)ParentApp).homeMenu.isDirty = true;

		ParentApp.PopMenu ();
	}
}
