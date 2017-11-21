using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour, IObserver {

	public Text
	m_turnNumber,
	m_battleryLife;

	public Texture m_intelNormal;
	public Texture m_intelContested;
	public Texture m_intelStolen;
	public GameObject m_intelIcon;
	public Transform m_intelContentParent;
	private List<GameObject> m_intelIcons = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		
	}
	
	public void Initialize()
	{
		GameController.instance.AddObserver (this);
		UpdateBatteryLife ();
		UpdateIntel ();
	}

	private void UpdateBatteryLife ()
	{
		
//		float batteryLife = SystemInfo.
	}

	private void UpdateIntel ()
	{
		while (m_intelIcons.Count > 0) {

			GameObject go = m_intelIcons [0];
			m_intelIcons.RemoveAt (0);
			Destroy (go);
		}

		Player player = GameController.instance.game.playerList [0];

		foreach (Player.IntelSlot iSlot in player.intel) {

			GameObject go = (GameObject)Instantiate (m_intelIcon, m_intelContentParent);
			RawImage ri = (RawImage)go.GetComponent<RawImage> ();
			m_intelIcons.Add (go);

			switch (iSlot.m_intelState) {

			case Player.IntelSlot.IntelState.Owned:

				ri.texture = m_intelNormal;

				break;
			case Player.IntelSlot.IntelState.Contested:

				ri.texture = m_intelContested;

				break;

			case Player.IntelSlot.IntelState.Stolen:

				ri.texture = m_intelStolen;

				break;
			}
		}
	}

	private void UpdateTurnNumber ()
	{
		int turnNumber = GameController.instance.game.currentTurn;

		string s = "TURN " + turnNumber.ToString("000");

		m_turnNumber.text = s;
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {

		case GameEvent.Player_IntelChanged:

			UpdateIntel ();

			break;
		case GameEvent.Turn_PlayerPhaseStarted:

			UpdateTurnNumber ();

			break;
		}
	}
}
