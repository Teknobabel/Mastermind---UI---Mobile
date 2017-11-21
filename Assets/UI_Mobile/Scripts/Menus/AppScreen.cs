using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppScreen : BaseMenu, IObserver {

	public RectTransform m_gridView;
	public Text 
	m_currentCommandPoolText,
	m_CommandPoolUpkeepText;

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);
	}

	private void SetGridCellSize ()
	{
		GridLayoutGroup glg = m_gridView.GetComponent<GridLayoutGroup> ();

		float horizontalGutters = (glg.spacing.x *3) + glg.padding.right + glg.padding.left;
		float verticalGutters = (glg.spacing.y * 3) + glg.padding.top + glg.padding.bottom;

		float width = m_gridView.rect.width - horizontalGutters;
		float height = m_gridView.rect.height - verticalGutters;
//		Debug.Log (m_gridView.rect);
		Vector2 newSize = new Vector2(Mathf.Round( width / 2), Mathf.Round( height / 3));
		m_gridView.GetComponent<GridLayoutGroup>().cellSize = newSize;
	}

	public void Initialize (List<IApp> apps, GameObject appIcon, IApp parentApp)
	{
//		GridLayoutGroup glg = m_gridView.GetComponent<GridLayoutGroup> ();
//		float numColumns = (float)glg.constraintCount;
//		float gridWidth = (m_gridView.transform as RectTransform).rect.size.x;
//		float iconWidth = appIcon.GetComponent<RectTransform> ().sizeDelta.x;
//		float spaceRemaining = gridWidth - (iconWidth * numColumns);
//		float iconWidthSpacing = spaceRemaining / (numColumns - 1);
//		Debug.Log (gridWidth);
//		Vector2 spacing = glg.spacing;
//		spacing.x = iconWidthSpacing;
//		glg.spacing = spacing;

		Initialize (parentApp);

		GameController.instance.AddObserver (this);

		// set grid cell size

		Invoke("SetGridCellSize", 0.01f);

		Player.CommandPool cp = GameController.instance.GetCommandPool (0);
		m_currentCommandPoolText.text = cp.m_currentPool.ToString () + "/" + cp.m_basePool.ToString();

		UpdateUpkeep ();

		foreach (IApp app in apps) {

			GameObject go = (GameObject)GameObject.Instantiate (appIcon, m_gridView);
			AppIcon ai = (AppIcon)go.GetComponent<AppIcon>();
			ai.Initialize(app);

			app.AppIconInstance = ai;
			app.SetAlerts ();

		}
	}

	public void EndTurnButtonPressed ()
	{
		Debug.Log ("End Turn Button Pressed");

		MobileUIEngine.instance.PushApp (MobileUIEngine.instance.turnProcessingApp);

//		Action_EndPhase endTurn = new Action_EndPhase ();
//		GameController.instance.ProcessAction (endTurn);
	}

	public void CPBreakdownButtonPressed ()
	{
		m_parentApp.PushMenu (((HomeScreenApp)(m_parentApp)).cpBreakdownMenu);
	}

//	private void UpdateUpkeep ()
//	{
//		string upkeep = "-";
//
//		int upkeepCost = 0;
//
//		List<Player.ActorSlot> henchmen = GameController.instance.GetHiredHenchmen (0);
//
//		foreach (Player.ActorSlot aSlot in henchmen) {
//
//			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {
//
//				upkeepCost += aSlot.m_actor.m_turnCost;
//			}
//		}
//
//		// check for having too many assets
//
//		upkeepCost += GameController.instance.GetAssetUpkeep (0);
//
//		if (upkeepCost > 0) {
//
//			upkeep += upkeepCost.ToString () + "/TURN";
//
//		} else {
//
//			upkeep = "";
//		}
//
//		m_CommandPoolUpkeepText.text = upkeep;
//	}

	private void UpdateUpkeep ()
	{
		string upkeep = "";

		int upkeepCost = 0;
		int income = GameController.instance.game.playerList [0].commandPool.m_income;

		List<Player.ActorSlot> henchmen = GameController.instance.GetHiredHenchmen (0);

		foreach (Player.ActorSlot aSlot in henchmen) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				upkeepCost += aSlot.m_actor.m_turnCost;
			}
		}

		// check for having too many assets

		upkeepCost += GameController.instance.GetAssetUpkeep (0);

//		if (upkeepCost > 0) {
//
//			upkeep += upkeepCost.ToString () + "/TURN";
//
//		} else {
//
//			upkeep = "";
//		}

		upkeep += (income - upkeepCost).ToString() + "/TURN";

		m_CommandPoolUpkeepText.text = upkeep;
	}
	
	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {

		case GameEvent.Player_CommandPoolChanged:

			Player.CommandPool cp = GameController.instance.GetCommandPool (0);
			m_currentCommandPoolText.text = cp.m_currentPool.ToString () + "/" + cp.m_basePool.ToString();

			break;
		case GameEvent.Player_AssetsChanged:
		case GameEvent.Player_HenchmenPoolChanged:

			UpdateUpkeep ();

			break;
		}
	}
}
