using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntelPanel : MonoBehaviour, IObserver {

	public Texture m_intelNormal;
	public Texture m_intelContested;
	public Texture m_intelStolen;

	public GameObject m_intelIcon;

	public Transform m_contentParent;

	private List<GameObject> m_intelIcons = new List<GameObject> ();

	public void Initialize ()
	{
		GameController.instance.AddObserver (this);
		UpdateIntel ();
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

			GameObject go = (GameObject)Instantiate (m_intelIcon, m_contentParent);
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

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {

		case GameEvent.Player_IntelChanged:

			UpdateIntel ();

			break;

		}
	}
}
