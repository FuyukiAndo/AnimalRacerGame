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
	private List<GameObject> unplacedPlayers = new List<GameObject>();
	[SerializeField] private Vector2[] playerGoalPositions;
	[SerializeField] private string playerMoveScriptName;

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
			unplacedPlayers.Add(tracker.transform.parent.gameObject);
		}
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

	public List<GameObject> GetPlayerPlacements()
	{
		return placedPlayers;
	}

	void PlacePlayers()
	{
		CheckpointTracker closestTracker = GetPlayerClosestToGoal();
		int index = unplacedPlayers.IndexOf(closestTracker.transform.parent.gameObject);
		placedPlayers.Add(GetPlayerClosestToGoal().gameObject);
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
				if (unplacedPlayers[i].GetComponentInChildren<CheckpointTracker>().GetCurrentCheckpointIndex() > checkIndex)
				{
					checkIndex = unplacedPlayers[i].GetComponentInChildren<CheckpointTracker>().GetCurrentCheckpointIndex();
					trackerIndex = i;
				}
			}
			foreach (var tracker in unplacedPlayers)
			{
				if (AtSameCheckpoint(unplacedPlayers[trackerIndex].GetComponentInChildren<CheckpointTracker>(), 
				tracker.GetComponentInChildren<CheckpointTracker>()))
				{
					trackerIndex = Array.IndexOf(unplacedPlayers.ToArray(),
						GetPlayerClosestToNextCheckpoint(unplacedPlayers[trackerIndex].GetComponentInChildren<CheckpointTracker>(), 
						tracker.GetComponentInChildren<CheckpointTracker>()));
				}
			}
			return unplacedPlayers[trackerIndex].GetComponentInChildren<CheckpointTracker>();
		}
		else
		{
			int checksPassed = 0;
			int trackerIndex = 0;
			for (int i = 0; i < unplacedPlayers.Count; i++)
			{
				if (unplacedPlayers[i].GetComponentInChildren<CheckpointTracker>().CheckpointsPassed.Count > checksPassed)
				{
					checksPassed = unplacedPlayers[i].GetComponentInChildren<CheckpointTracker>().CheckpointsPassed.Count;
					trackerIndex = i;
				}
			}
			foreach (var tracker in unplacedPlayers)
			{
				if (SameAmountOfChecksPassed(unplacedPlayers[trackerIndex].GetComponentInChildren<CheckpointTracker>(), 
				tracker.GetComponentInChildren<CheckpointTracker>()))
				{
					trackerIndex = Array.IndexOf(unplacedPlayers.ToArray(),
						GetClosestToGoal(unplacedPlayers[trackerIndex].GetComponentInChildren<CheckpointTracker>(), 
						tracker.GetComponentInChildren<CheckpointTracker>()));
				}
			}
			return unplacedPlayers[trackerIndex].GetComponentInChildren<CheckpointTracker>();
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
			if (player.GetComponentInChildren<Rigidbody2D>())
			{
				player.GetComponentInChildren<Rigidbody2D>().velocity = Vector2.zero;
				player.GetComponentInChildren<Rigidbody2D>().isKinematic = true;
				player.GetComponentInChildren<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
				player.GetComponentInChildren<Rigidbody2D>().simulated = false;
			}
		}
	}

}