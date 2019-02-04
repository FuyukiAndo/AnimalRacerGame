using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
	public bool passInSequence, countDownOnFirstPlayer;
	public Checkpoint[] checksToPass;
	public float timeBeforeAutoPlacements;

	public static CheckpointManager Instance
	{
		get
		{
			return instance;
		}
	}
	private static CheckpointManager instance;

	private List<GameObject> placedPlayers;
	private bool startCountDown;
	private List<CheckpointTracker> unplacedPlayers;

	void Start()
	{
		if (instance == null)
		{
			instance = this;
			foreach (var tracker in FindObjectsOfType<CheckpointTracker>())
			{
				unplacedPlayers.Add(tracker);
			}
		}
		else if (instance != this)
		{
			Destroy(gameObject);
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

	void OnTriggerEnter2D(Collider2D other)
	{
		if (ValidateTracker(other.GetComponent<CheckpointTracker>()) && countDownOnFirstPlayer && !startCountDown)
		{
			placedPlayers.Add(other.gameObject);
			int index = Array.IndexOf(unplacedPlayers.ToArray(), other.GetComponent<CheckpointTracker>());
			unplacedPlayers.RemoveAt(index);
			startCountDown = true;
		}
		else if (ValidateTracker(other.GetComponent<CheckpointTracker>()) && startCountDown)
		{
			placedPlayers.Add(other.gameObject);
			int index = Array.IndexOf(unplacedPlayers.ToArray(), other.GetComponent<CheckpointTracker>());
			unplacedPlayers.RemoveAt(index);
		}
	}

	bool ValidateTracker(CheckpointTracker tracker)
	{
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
		int index = Array.IndexOf(unplacedPlayers.ToArray(), closestTracker);
		placedPlayers.Add(GetPlayerClosestToGoal().gameObject);
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
				trackerIndex = Array.IndexOf(unplacedPlayers.ToArray(),
				GetPlayerClosestToNextCheckpoint(unplacedPlayers[trackerIndex], tracker));
			}
		}
		return unplacedPlayers[trackerIndex];
	}

}