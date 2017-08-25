using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppScreen : MonoBehaviour, IObserver, IMenu {

	public RectTransform m_gridView;
	public Text 
	m_currentCommandPoolText,
	m_CommandPoolUpkeepText;

	private IApp m_parentApp;

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize (IApp parentApp)
	{
		m_parentApp = parentApp;
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

		Player.CommandPool cp = GameController.instance.GetCommandPool (0);
		m_currentCommandPoolText.text = cp.m_currentPool.ToString ();

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

		Action_EndPhase endTurn = new Action_EndPhase ();
		GameController.instance.ProcessAction (endTurn);
	}

	public void CPBreakdownButtonPressed ()
	{
		m_parentApp.PushMenu (((HomeScreenApp)(m_parentApp)).cpBreakdownMenu);
	}

	private void UpdateUpkeep ()
	{
		string upkeep = "-";

		int upkeepCost = 0;

		List<Player.ActorSlot> henchmen = GameController.instance.GetHiredHenchmen (0);

		foreach (Player.ActorSlot aSlot in henchmen) {

			if (aSlot.m_state != Player.ActorSlot.ActorSlotState.Empty) {

				upkeepCost += aSlot.m_actor.m_turnCost;
			}
		}

		if (upkeepCost > 0) {

			upkeep += upkeepCost.ToString () + "/TURN";

		} else {

			upkeep = "";
		}

		m_CommandPoolUpkeepText.text = upkeep;
	}
	
	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {

		case GameEvent.Player_CommandPoolChanged:

			Player.CommandPool cp = GameController.instance.GetCommandPool (0);
			m_currentCommandPoolText.text = cp.m_currentPool.ToString ();

			break;

		case GameEvent.Player_HenchmenPoolChanged:

			UpdateUpkeep ();

			break;
		}
	}

	public void OnEnter (bool animate)
	{
	}

	public void OnExit (bool animate)
	{
	}

	public void OnHold ()
	{
	}

	public void OnReturn ()
	{
	}

	public IApp ParentApp 
	{ get{ return m_parentApp; }}
}
