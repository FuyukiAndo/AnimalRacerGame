using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    public GameObject endScreenUI;
	public bool passInSequence, countDownOnFirstPlayer;
	public Checkpoint[] checksToPass;
	public float timeBeforeAutoPlacements;
	public Vector2 boxSize;

	public static GoalManager Instance
	{
		get
		{
			return instance;
		}
	}
	private static GoalManager instance;

	public List<GameObject> PlacedPlayers
	{
		get
		{
			return placedPlayers;
		}
	}
	private List<GameObject> placedPlayers = new List<GameObject>();
	private bool startCountDown;
	private List<CheckpointTracker> unplacedPlayers = new List<CheckpointTracker>();
	[SerializeField] private GameObject[] playerGoalPositions;
	[SerializeField] private string[] playerMoveScriptName;
	private float totalTimeBeforeAutoPlacements;
	private int initialPlayerCount;
	[SerializeField] private LayerMask playerLayer, ignorePlayerLayer;
	[SerializeField] private float nextSceneDelay;
	private bool startedSceneSwitch;

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
		initialPlayerCount = unplacedPlayers.Count;
		boxSize = GetComponent<BoxCollider2D>().size;
	}

	void Update()
	{
        if(placedPlayers.Count == InformationManager.Instance.players.Count)
        {
            endScreenUI.SetActive(true);
            StartCoroutine(Wait());
        }
		if (countDownOnFirstPlayer)
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
		else if (StartManager.Instance.TimeUntilStart <= 0f && unplacedPlayers.Count > 0)
		{
			timeBeforeAutoPlacements -= Time.deltaTime;
			if (timeBeforeAutoPlacements <= 0f)
			{
				PlacePlayers();
			}
		}

		ValidateForGoal();
	}
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5.0f);
        InformationManager.Instance.sceneIndex += 1;
        SceneManager.LoadScene(InformationManager.Instance.multiplayerLevels[InformationManager.Instance.sceneIndex]);
    }
	void ValidateForGoal()
	{
		Collider2D collider = Physics2D.OverlapBox(transform.position, boxSize, 0f, playerLayer);

		if (collider == null  || !collider.transform.GetComponent<CheckpointTracker>()) return;
		if (ValidateTracker(collider.transform.GetComponent<CheckpointTracker>()))
		{
			PlacePlayers();
			if (countDownOnFirstPlayer && !startCountDown)
			{
				startCountDown = true;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		/*if (ValidateTracker(other.transform.GetComponent<CheckpointTracker>()) && countDownOnFirstPlayer && !startCountDown)
		{
			//placedPlayers.Add(other.gameObject);
			PlacePlayers();
			//int index = Array.IndexOf(unplacedPlayers.ToArray(), other.transform.GetComponent<CheckpointTracker>());
			//unplacedPlayers.RemoveAt(index);
			startCountDown = true;
		}
		if (ValidateTracker(other.transform.GetComponent<CheckpointTracker>()))
		{
			print("Validated");
			PlacePlayers();
			if (countDownOnFirstPlayer && !startCountDown)
			{
				startCountDown = true;
			}
		}*/
	}

	bool ValidateTracker(CheckpointTracker tracker)
	{
		tracker.gameObject.layer = ignorePlayerLayer;
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

	public float[] GetPlayerTimesForCurrentRound()
	{
		List<float> playerTimes = new List<float>();
		for (int i = 0; i < placedPlayers.Count; i++)
		{
			playerTimes.Add(placedPlayers[i].GetComponent<CheckpointTracker>().FinishingTime);
		}
		return playerTimes.ToArray();
	}

	public int[] GetPlayerPointsForCurrentRound()
	{
		List<int> playerPoints = new List<int>();
		for (int i = 0; i < placedPlayers.Count; i++)
		{
			playerPoints.Add(placedPlayers[i].GetComponent<CheckpointTracker>().PlacementPoint);
		}
		return playerPoints.ToArray();
	}

	//Fast antal poäng per placering - har 2 spelare samma placering avgörs placeringen med deras finishingTime
	//vid målgång kolla och sätt rätt poäng för rätt spelare
	//lagra poängen och skicka poäng och tid per spelare när alla gått i mål
	void PlacePlayers()
	{
		CheckpointTracker closestTracker = GetPlayerClosestToGoal();
		int index = unplacedPlayers.IndexOf(closestTracker);
		placedPlayers.Add(GetPlayerClosestToGoal().gameObject);
		AssignPlacementPoint(GetPlayerClosestToGoal());
		AssignFinishingTime(GetPlayerClosestToGoal());
		int index1 = placedPlayers.IndexOf(closestTracker.gameObject);
		placedPlayers[index1].transform.position = playerGoalPositions[index1].transform.position;
		TrapPlayers();
		unplacedPlayers.RemoveAt(index);
		if (unplacedPlayers.Count <= 0 && !startedSceneSwitch)
		{
			startedSceneSwitch = true;
			StartCoroutine(LoadNextScene());
		}
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
			for (int i = 0; i < playerMoveScriptName.Length; i++)
			{
				MonoBehaviour script = player.GetComponent(playerMoveScriptName[i]) as MonoBehaviour;
				script.enabled = false;
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		if (checksToPass.Count() > 0)
		{
			foreach (var checks in checksToPass)
			{
				if (checks != null)
				{
					Gizmos.DrawWireSphere(checks.transform.position, .5f);
				}
			}
		}
		Gizmos.color = Color.blue;
		if (playerGoalPositions.Count() > 0)
		{
			foreach (var goal in playerGoalPositions)
			{
				Gizmos.DrawWireSphere(goal.transform.position, .5f);
			}
		}
	}

	void AssignFinishingTime(CheckpointTracker tracker)
	{
		float finishingTime = totalTimeBeforeAutoPlacements - timeBeforeAutoPlacements;
		if (finishingTime > 0f)
		{
			tracker.FinishingTime = finishingTime;
		}
		else
		{
			tracker.FinishingTime = 0f;
		}
		string name = tracker.name;
		switch (name)
		{
			case "Player1":
				if (InformationManager.Instance.player1.level1Time == 0f)
				{
					InformationManager.Instance.player1.level1Time = tracker.FinishingTime;
				}
				else if (InformationManager.Instance.player1.level2Time == 0f)
				{
					InformationManager.Instance.player1.level2Time = tracker.FinishingTime;
				}
				else if (InformationManager.Instance.player1.level3Time == 0f)
				{
					InformationManager.Instance.player1.level3Time = tracker.FinishingTime;
				}
				else
				{
					InformationManager.Instance.player1.level4Time = tracker.FinishingTime;
				}
				break;
			case "Player2":
				if (InformationManager.Instance.player2.level1Time == 0f)
				{
					InformationManager.Instance.player2.level1Time = tracker.FinishingTime;
				}
				else if (InformationManager.Instance.player2.level2Time == 0f)
				{
					InformationManager.Instance.player2.level2Time = tracker.FinishingTime;
				}
				else if (InformationManager.Instance.player2.level3Time == 0f)
				{
					InformationManager.Instance.player2.level3Time = tracker.FinishingTime;
				}
				else
				{
					InformationManager.Instance.player2.level4Time = tracker.FinishingTime;
				}
				break;
			case "Player3":
				if (InformationManager.Instance.player3.level1Time == 0f)
				{
					InformationManager.Instance.player3.level1Time = tracker.FinishingTime;
				}
				else if (InformationManager.Instance.player3.level2Time == 0f)
				{
					InformationManager.Instance.player3.level2Time = tracker.FinishingTime;
				}
				else if (InformationManager.Instance.player3.level3Time == 0f)
				{
					InformationManager.Instance.player3.level3Time = tracker.FinishingTime;
				}
				else
				{
					InformationManager.Instance.player3.level4Time = tracker.FinishingTime;
				}
				break;
			case "Player4":
				if (InformationManager.Instance.player4.level1Time == 0f)
				{
					InformationManager.Instance.player4.level1Time = tracker.FinishingTime;
				}
				else if (InformationManager.Instance.player4.level2Time == 0f)
				{
					InformationManager.Instance.player4.level2Time = tracker.FinishingTime;
				}
				else if (InformationManager.Instance.player4.level3Time == 0f)
				{
					InformationManager.Instance.player4.level3Time = tracker.FinishingTime;
				}
				else
				{
					InformationManager.Instance.player4.level4Time = tracker.FinishingTime;
				}
				break;
		}
	}

	void AssignPlacementPoint(CheckpointTracker tracker)
	{
		tracker.PlacementPoint = initialPlayerCount - placedPlayers.Count;
		string name = tracker.name;
		switch (name)
		{
			case "Player1":
				InformationManager.Instance.player1.score += tracker.PlacementPoint;
				break;
			case "Player2":
				InformationManager.Instance.player2.score += tracker.PlacementPoint;
				break;
			case "Player3":
				InformationManager.Instance.player3.score += tracker.PlacementPoint;
				break;
			case "Player4":
				InformationManager.Instance.player4.score += tracker.PlacementPoint;
				break;
		}
	}

	IEnumerator LoadNextScene()
	{
		yield return new WaitForSeconds(nextSceneDelay);
		SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
		int index = InformationManager.Instance.multiplayerLevels.IndexOf(
			InformationManager.Instance.multiplayerLevels.Find(x => x == SceneManager.GetActiveScene().name));
		if (index + 1 >= InformationManager.Instance.multiplayerLevels.Count)
		{
			SceneManager.LoadScene("StartMenu", LoadSceneMode.Additive);
		}
		else
		{
			SceneManager.LoadScene(InformationManager.Instance.multiplayerLevels[index + 1], LoadSceneMode.Additive);
		}
	}
}