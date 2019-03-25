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
	public float yRotation;
	public SpeechBubble commentator;

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
	private List<GameObject> trappedPlayers = new List<GameObject>();
	private List<PlayerCheckCount> playerChecks = new List<PlayerCheckCount>();
	private bool startCountDown;
	[SerializeField] private List<CheckpointTracker> unplacedPlayers = new List<CheckpointTracker>();
	[SerializeField] private GameObject[] playerGoalPositions;
	private float totalTimeBeforeAutoPlacements;
	private int initialPlayerCount;
	[SerializeField] private LayerMask playerLayer, ignorePlayerLayer;
	[SerializeField] private float nextSceneDelay, stressSignalDelay;
	private bool startedSceneSwitch;
	private GameObject checkpointToGoFor;
	private int currentCheckToGoFor;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		boxSize = GetComponent<BoxCollider2D>().size;
		commentator = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name.Contains("Commentator")).FirstOrDefault();
		StartCoroutine(CountDownToStressSignal());
	}

	public void Setup()
	{
		foreach (var tracker in FindObjectsOfType<CheckpointTracker>())
		{
			unplacedPlayers.Add(tracker);
		}
		for (int i = 0; i < unplacedPlayers.Count(); i++)
		{
			playerChecks.Add(new PlayerCheckCount());
			playerChecks[i].tracker = unplacedPlayers[i].GetComponent<CheckpointTracker>();
			playerChecks[i].checks = 0;
		}
		totalTimeBeforeAutoPlacements = timeBeforeAutoPlacements;
		initialPlayerCount = unplacedPlayers.Count;
		print(initialPlayerCount + " count at beginning");
	}

	IEnumerator CountDownToStressSignal()
	{
		yield return new WaitForSeconds(stressSignalDelay);
		AudioManager.Instance.SetBackParameterValue(0.7f);
	}

	void Update()
	{
		if (placedPlayers.Count == InformationManager.Instance.players.Count)
		{
			endScreenUI.SetActive(true);
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

	public void NotifyOfCheckpointCount(CheckpointTracker tracker)
	{
		PlayerCheckCount checkCount = playerChecks.Where(player => player.tracker == tracker).FirstOrDefault();
		checkCount.checks++;
		CheckForCommentatorEvent();
	}

	void CheckForCommentatorEvent()
	{
		bool allOneLeft = true, allTwo = true, allThree = true, allAll = true;
		foreach (var player in playerChecks)
		{
			if (player.checks != checksToPass.Count() - 1)
			{
				allOneLeft = false;
			}
			if (player.checks != 2)
			{
				allTwo = false;
			}
			if (player.checks != 3)
			{
				allThree = false;
			}
			if (player.checks != checksToPass.Count())
			{
				allAll = false;
			}
		}
		for (int i = 0; i < playerChecks.Count(); i++)
		{
			for (int j = 0; j < playerChecks.Count(); j++)
			{
				if (playerChecks[i].checks == checksToPass.Count() - 1 && playerChecks[j].checks == checksToPass.Count() - 1)
				{
					commentator.SetCommentatorSpeechActive(false, CommentatorSpeechType.twoOneLeft);
					return;
				}
				else if (playerChecks[i].checks == 2 && playerChecks[j].checks == 2
					|| playerChecks[i].checks == 3 && playerChecks[j].checks == 3)
				{
					commentator.SetCommentatorSpeechActive(false, CommentatorSpeechType.twoTwo);
					return;
				}
				else if (playerChecks[i].checks == checksToPass.Count() && playerChecks[j].checks == checksToPass.Count())
				{
					commentator.SetCommentatorSpeechActive(false, CommentatorSpeechType.twoAll);
					return;
				}
			}
			if (playerChecks[i].checks == checksToPass.Count() - 1)
			{
				commentator.SetCommentatorSpeechActive(false, CommentatorSpeechType.oneOneLeft);
				return;
			}
			else if (playerChecks[i].checks == checksToPass.Count())
			{
				commentator.SetCommentatorSpeechActive(false, CommentatorSpeechType.oneAll);
				return;
			}
		}
		if (allOneLeft)
		{
			commentator.SetCommentatorSpeechActive(false, CommentatorSpeechType.multipleOneLeft);
		}
		else if (allAll)
		{
			commentator.SetCommentatorSpeechActive(false, CommentatorSpeechType.multipleAll);
		}
		else if (allThree)
		{
			commentator.SetCommentatorSpeechActive(false, CommentatorSpeechType.multipleThree);
		}
		else if (allTwo)
		{
			commentator.SetCommentatorSpeechActive(false, CommentatorSpeechType.multipleTwo);
		}
	}

	void ValidateForGoal()
	{
		Collider2D collider = Physics2D.OverlapBox(transform.position, boxSize, 0f, playerLayer);

		if (collider == null || !collider.transform.GetComponent<CheckpointTracker>())return;
		if (ValidateTracker(collider.transform.GetComponent<CheckpointTracker>()))
		{
			PlacePlayers();
			if (countDownOnFirstPlayer && !startCountDown)
			{
				startCountDown = true;
			}
		}
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

	void PlacePlayers()
	{
		CheckpointTracker closestTracker = GetPlayerClosestToGoal();
		int index = unplacedPlayers.IndexOf(closestTracker);
		AssignPlacementPoint(GetPlayerClosestToGoal());
		placedPlayers.Add(GetPlayerClosestToGoal().gameObject);
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
		if (placedPlayers.Count == initialPlayerCount)
		{
			commentator.SetCommentatorSpeechActive(true, CommentatorSpeechType.none);
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
		foreach (var player in placedPlayers)
		{
			if (trappedPlayers.Contains(player))
			{
				continue;
			}
			PlayerInput input = player.GetComponent<PlayerInput>();
			input.isControllable = false;
			input.changeAngle = false;
			PlayerController controller = player.GetComponent<PlayerController>();
			controller.enabled = false;
			trappedPlayers.Add(player);
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
					print("assigned player 1 time");
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
		InformationManager.Instance.sceneIndex += 1;
		SceneManager.LoadScene(InformationManager.Instance.multiplayerLevels[InformationManager.Instance.sceneIndex]);
	}
}

public class PlayerCheckCount
{
	public int checks;
	public CheckpointTracker tracker;
}