using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen_CPBreakdownMenu : BaseMenu {

	public Text m_cpBreakdownText;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

//		m_appNameText.text = parentApp.Name;
		this.gameObject.SetActive (false);
	}

	public override void OnEnter (bool animate)
	{
		base.OnEnter (animate);

		this.gameObject.SetActive (true);

		string breakdown = "Command Pool Breakdown:\n";
		breakdown += "\nBase Command Pool: " + GameController.instance.game.director.m_startingCommandPool.ToString () + " CP\n";

		bool hasHenchmen = false;
		bool hasBaseBonus = false;

		// get upkeep from henchmen

		List<Player.ActorSlot> henchmen = GameController.instance.GetHiredHenchmen (0);

		if (henchmen.Count > 0) {

			foreach (Player.ActorSlot aSlot in henchmen) {

				if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

					if (!hasHenchmen) {

						hasHenchmen = true;
						breakdown += "\nHenchmen Upkeep:\n";
					}

					breakdown += aSlot.m_actor.m_actorName + ": -" + aSlot.m_actor.m_turnCost.ToString () + " CP\n";
				}
			}
		}

		// get any upkeep from having too many assets

//		int numAssetSlots = GameController.instance.GetNumAssetSlots (0);
//		int numAssets = 0;
//		List<Site.AssetSlot> assets = GameController.instance.GetAssets (0);
//
//		foreach (Site.AssetSlot aSlot in assets) {
//
//			if (aSlot.m_state != Site.AssetSlot.State.None) {
//				numAssets++;
//			}
//		}
//
//		if (numAssets > numAssetSlots) {
//
//			breakdown += "\nAssets: -" + (numAssets - numAssetSlots).ToString () + " CP\n";
//		}

		int assetUpkeep = GameController.instance.GetAssetUpkeep (0);

		if (assetUpkeep > 0) {

			breakdown += "\nAssets: -" + assetUpkeep.ToString () + " CP\n";
		}

		// check for any bonuses from lair floors

		Lair l = GameController.instance.GetLair (0);

		foreach (Lair.FloorSlot fSlot in l.floorSlots) {

			if (fSlot.m_state != Lair.FloorSlot.FloorState.Empty) {

				if (fSlot.m_floor is Floor_IncCommandPool) {

					if (!hasBaseBonus) {

						hasBaseBonus = true;
						breakdown += "\nLair Bonuses:\n";
					}

					Floor_IncCommandPool f = (Floor_IncCommandPool)fSlot.m_floor;

					int bonus = f.m_bonus;

					// check for upgrades

					if (f.completedUpgrades.Count > 0) {

						bonus += f.m_bonus * f.completedUpgrades.Count;
					}

					breakdown += f.m_name + ": +" + bonus.ToString () + " CP\n";
				}
			}
		}

		m_cpBreakdownText.text = breakdown;

	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		this.gameObject.SetActive (false);

	}

	public void DismissButtonClicked ()
	{
		m_parentApp.PopMenu ();
	}
}
