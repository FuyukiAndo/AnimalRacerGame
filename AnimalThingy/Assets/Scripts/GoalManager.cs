using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlatformEffector2D))]
public class GoalManager : MonoBehaviour
{
	public bool passInSequence, countDownOnFirstPlayer;
	public Checkpoint[] checksToPass;
	public float timeBeforeAutoPlacements;

	public static GoalManager Instance
	{
		get
		{
			return instance;
		}
	}
	private static GoalManager instance;

	private List<GameObject> placedPlayers = new List<GameObject>();
	private bool startCountDown;
	private List<CheckpointTracker> unplacedPlayers = new List<CheckpointTracker>();
	[SerializeField] private Vector2[] playerGoalPositions;
	[SerializeField] private string playerMoveScriptName;
	private float totalTimeBeforeAutoPlacements;

	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		foreach (var tracker in FindObjectsOfType<CheckpointTracker>())
		{
			unplacedPlayers.Add(tracker);
		}
		totalTimeBeforeAutoPlacements = timeBeforeAutoPlacements;
	}

	void Update()
	{
		if (startCountDown && unplacedPlayers.Count > 0)
		{
			timeBeforeAutoPlacements -= Time.deltaTime;
			if (timeBeforeAutoPlacements <= 0f)
			{
				PlacePlayers();
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (ValidateTracker(other.transform.GetComponent<CheckpointTracker>()) && countDownOnFirstPlayer && !startCountDown)
		{
			//placedPlayers.Add(other.gameObject);
			PlacePlayers();
			//int index = Array.IndexOf(unplacedPlayers.ToArray(), other.transform.GetComponent<CheckpointTracker>());
			//unplacedPlayers.RemoveAt(index);
			startCountDown = true;
		}
		else if (ValidateTracker(other.transform.GetComponent<CheckpointTracker>()) && startCountDown)
		{
			//placedPlayers.Add(other.gameObject);
			PlacePlayers();
			//int index = Array.IndexOf(unplacedPlayers.ToArray(), other.transform.GetComponent<CheckpointTracker>());
			//unplacedPlayers.RemoveAt(index);
		}
		else if (ValidateTracker(other.transform.GetComponent<CheckpointTracker>()) && !countDownOnFirstPlayer)
		{
			//placedPlayers.Add(other.gameObject);
			PlacePlayers();
			//int index = Array.IndexOf(unplacedPlayers.ToArray(), other.transform.GetComponent<CheckpointTracker>());
			//unplacedPlayers.RemoveAt(index);
		}
	}

	bool ValidateTracker(CheckpointTracker tracker)
	{
		foreach (var placed in placedPlayers)
		{
			if (tracker.gameObject == placed)
			{
				return false;
			}
		}
		if (passInSequence)
		{
			for (int i = 0; i < checksToPass.Length; i++)
			{
				if (tracker.CheckpointsPassed[i] != checksToPass[i].Index)
				{
					return false;
				}
			}
			return true;
		}
		else
		{
			if (tracker.CheckpointsPassed.Count == checksToPass.Length)
			{
				return true;
			}
		}
		return false;
	}

	public Dictionary<GameObject, float> GetPlayerPlacements()
	{
		Dictionary<GameObject, float> _placedPlayers = new Dictionary<GameObject, float>();
		for (int i = 0; i < placedPlayers.Count; i++)
		{
			_placedPlayers.Add(placedPlayers[i], placedPlayers[i].GetComponent<CheckpointTracker>().FinishingTime);
		}
		return _placedPlayers;
	}

	void PlacePlayers()
	{
		CheckpointTracker closestTracker = GetPlayerClosestToGoal();
		int index = unplacedPlayers.IndexOf(closestTracker);
		placedPlayers.Add(GetPlayerClosestToGoal().gameObject);
		AssignFinishingTime(GetPlayerClosestToGoal());
		int index1 = placedPlayers.IndexOf(closestTracker.gameObject);
		placedPlayers[index1].transform.position = playerGoalPositions[index1];
		TrapPlayers();
		unplacedPlayers.RemoveAt(index);
	}

	float GetDistToNextCheckpointInSequence(CheckpointTracker tracker)
	{
		int next = tracker.GetCurrentCheckpointIndex() + 1;
		return ((Vector2)checksToPass[next].transform.position - tracker.GetCurrentPosition()).magnitude;
	}

	bool AtSameCheckpoint(CheckpointTracker trackerA, CheckpointTracker trackerB)
	{
		return trackerA.GetCurrentCheckpointIndex() == trackerB.GetCurrentCheckpointIndex();
	}

	CheckpointTracker GetPlayerClosestToNextCheckpoint(CheckpointTracker trackerA, CheckpointTracker trackerB)
	{
		return GetDistToNextCheckpointInSequence(trackerA) < GetDistToNextCheckpointInSequence(trackerB) ? trackerA : trackerB;
	}

	CheckpointTracker GetPlayerClosestToGoal()
	{
		if (passInSequence)
		{
			int checkIndex = 0;
			int trackerIndex = 0;
			for (int i = 0; i < unplacedPlayers.Count; i++)
			{
				if (unplacedPlayers[i].GetCurrentCheckpointIndex() > checkIndex)
				{
					checkIndex = unplacedPlayers[i].GetCurrentCheckpointIndex();
					trackerIndex = i;
				}
			}
			foreach (var tracker in unplacedPlayers)
			{
				if (AtSameCheckpoint(unplacedPlayers[trackerIndex], tracker))
				{
					trackerIndex = unplacedPlayers.IndexOf(GetPlayerClosestToNextCheckpoint(unplacedPlayers[trackerIndex], tracker));
				}
			}
			return unplacedPlayers[trackerIndex];
		}
		else
		{
			int checksPassed = 0;
			int trackerIndex = 0;
			for (int i = 0; i < unplacedPlayers.Count; i++)
			{
				if (unplacedPlayers[i].CheckpointsPassed.Count > checksPassed)
				{
					checksPassed = unplacedPlayers[i].CheckpointsPassed.Count;
					trackerIndex = i;
				}
			}
			foreach (var tracker in unplacedPlayers)
			{
				if (SameAmountOfChecksPassed(unplacedPlayers[trackerIndex], tracker))
				{
					trackerIndex = unplacedPlayers.IndexOf(GetClosestToGoal(unplacedPlayers[trackerIndex], tracker));
				}
			}
			return unplacedPlayers[trackerIndex];
		}
	}

	float GetDistToGoal(CheckpointTracker tracker)
	{
		return ((Vector2)transform.position - tracker.GetCurrentPosition()).magnitude;
	}

	CheckpointTracker GetClosestToGoal(CheckpointTracker trackerA, CheckpointTracker trackerB)
	{
		return GetDistToGoal(trackerA) < GetDistToGoal(trackerB) ? trackerA : trackerB;
	}

	bool SameAmountOfChecksPassed(CheckpointTracker trackerA, CheckpointTracker trackerB)
	{
		return trackerA.CheckpointsPassed.Count == trackerB.CheckpointsPassed.Count;
	}

	void TrapPlayers()
	{
		//string componentName = playerMoveScript.GetType().ToString();
		foreach (var player in placedPlayers)
		{
			MonoBehaviour script = player.GetComponent(playerMoveScriptName)as MonoBehaviour;
			script.enabled = false;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		foreach (var checks in checksToPass)
		{
			Gizmos.DrawWireSphere(checks.transform.position, .5f);
		}
		Gizmos.color = Color.blue;
		foreach (var goal in playerGoalPositions)
		{
			Gizmos.DrawWireSphere(goal, .5f);
		}
	}

	void AssignFinishingTime(CheckpointTracker tracker)
	{
		//Assign time taken as points or remaining time as points?
		float finishingTime = totalTimeBeforeAutoPlacements - timeBeforeAutoPlacements;
		if (finishingTime > 0f)
		{
			tracker.FinishingTime = finishingTime;
			tracker.TotalFinishingTime += finishingTime;
		}
		else
		{
			tracker.FinishingTime = 0f;
		}
	}

	public float[] GetTotalPlayerFinishingTimes()
	{
		float[] finishingTimes = {
			placedPlayers[0].GetComponent<CheckpointTracker>().TotalFinishingTime,
			placedPlayers[1].GetComponent<CheckpointTracker>().TotalFinishingTime,
			placedPlayers[2].GetComponent<CheckpointTracker>().TotalFinishingTime,
			placedPlayers[3].GetComponent<CheckpointTracker>().TotalFinishingTime,
		};
		return finishingTimes;
	}

}