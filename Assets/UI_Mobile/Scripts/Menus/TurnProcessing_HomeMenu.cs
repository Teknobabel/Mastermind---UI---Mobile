using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnProcessing_HomeMenu : BaseMenu {

	private List<MissionSummary> m_completedMissions = new List<MissionSummary> ();

	public override void Initialize (IApp parentApp)
	{
		base.Initialize (parentApp);

		this.gameObject.SetActive (false);
	}
	public override void OnEnter (bool animate)
	{
		base.OnEnter (animate);

		this.gameObject.SetActive (true);

		Action_EndPhase endTurn = new Action_EndPhase ();
		GameController.instance.ProcessAction (endTurn);

		Player player = GameController.instance.game.playerList [0];

		m_completedMissions = player.missionsCompletedThisTurn;

		if (m_completedMissions.Count > 0) {

			Vector3 v = this.gameObject.transform.position;
			DOTween.To (() => this.gameObject.transform.position, x => this.gameObject.transform.position = x, new Vector3 (v.x,v.y,v.z), 0.25f).SetDelay (2.0f).OnComplete(ShowNextMissionSummary);

		} else {
			Vector3 v = this.gameObject.transform.position;
			DOTween.To (() => this.gameObject.transform.position, x => this.gameObject.transform.position = x, new Vector3 (v.x,v.y,v.z), 0.25f).SetDelay (2.0f).OnComplete(m_parentApp.PopMenu);
		}
	}

	private void ShowNextMissionSummary ()
	{
		if (m_completedMissions.Count > 0) {

			MissionSummary ms = m_completedMissions [0];
			m_completedMissions.RemoveAt (0);

			((TurnProcessingApp)m_parentApp).missionReport.missionSummary = ms;
			m_parentApp.PushMenu (((TurnProcessingApp)m_parentApp).missionReport);

		} else {

			Debug.Log ("No more mission summaries to get");
		}
	}

	public override void OnReturn ()
	{
		base.OnReturn ();

		if (m_completedMissions.Count > 0) {

			Vector3 v = this.gameObject.transform.position;
			DOTween.To (() => this.gameObject.transform.position, x => this.gameObject.transform.position = x, new Vector3 (v.x,v.y,v.z), 0.25f).SetDelay (1.0f).OnComplete(ShowNextMissionSummary);

		} else {
			Vector3 v = this.gameObject.transform.position;
			DOTween.To (() => this.gameObject.transform.position, x => this.gameObject.transform.position = x, new Vector3 (v.x,v.y,v.z), 0.25f).SetDelay (1.0f).OnComplete(m_parentApp.PopMenu);
		}
	}

	public override void OnExit (bool animate)
	{
		base.OnExit (animate);

		this.gameObject.SetActive (false);
	}
}
