using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPoint
{
    public GameObject spawnPos;
    public bool beenUsed = false;
}

public class StartManager : MonoBehaviour
{
    public SpawnPoint spawnPos1;
    public SpawnPoint spawnPos2;
    public SpawnPoint spawnPos3;
    public SpawnPoint spawnPos4;
    private List<SpawnPoint> spawnPoints;

    public static StartManager Instance
    {
        get
        {
            return instance;
        }
    }

    private static StartManager instance;
    //[SerializeField] private string playerMoveScript;
    private CharacterUIManager characterUIManager;
    public List<int> playerScoreList;

    public int TimeUntilStart
    {
        get
        {
            return timeUntilStart;
        }
        set
        {
            timeUntilStart = value;
        }
    }

    [SerializeField] private int timeUntilStart;
    [SerializeField] private bool startCountDownOnSceneLoad;

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
        playerScoreList = new List<int>();
        characterUIManager = FindObjectOfType<CharacterUIManager>();
        spawnPoints = new List<SpawnPoint>
            {
                spawnPos1,
                spawnPos2,
                spawnPos3,
                spawnPos4
            };
        SortPlayers();
        SpawnPlayers();
		GoalManager.Instance.Setup();
		FindObjectOfType<Checkpoint>().SetNextCheckPosToGoFor();
		TrapPlayers();
		StartCoroutine(CountDownStart());
    }

    IEnumerator CountDownStart()
    {
        while (timeUntilStart > 0)
        {
            timeUntilStart -= 1;
            yield return new WaitForSeconds(1f);
        }
        ReleasePlayers();
    }

    void ReleasePlayers()
    {
		foreach (var player in FindObjectsOfType<CheckpointTracker>())
        {
			PlayerInput input = player.GetComponent<PlayerInput>();
			input.isControllable = true;
			PlayerController controller = player.GetComponent<PlayerController>();
			//MonoBehaviour script = player.GetComponent(playerMoveScript) as MonoBehaviour;
			//script.enabled = true;
			controller.enabled = true;
		}
    }

    void TrapPlayers()
    {
		foreach (var player in FindObjectsOfType<CheckpointTracker>())
		{
			PlayerInput input = player.GetComponent<PlayerInput>();
			input.isControllable = false;
			PlayerController controller = player.GetComponent<PlayerController>();
			//MonoBehaviour script = player.GetComponent(playerMoveScript) as MonoBehaviour;
			//script.enabled = true;
			controller.enabled = false;
		}
    }

    public void SortPlayers()
    {
        foreach (Player player in InformationManager.Instance.players)
        {
            playerScoreList.Add(player.score);
        }
        playerScoreList.Sort();
        playerScoreList.Reverse();
    }
    void SpawnPlayers()
    {
        for (int i = 0; i < InformationManager.Instance.players.Count; i++)
        {
            GameObject spawnedPlayer = null;
            bool playerSpawned = false;
            for (int j = 0; j < playerScoreList.Count; j++)
            {
                if (InformationManager.Instance.players[i].score == playerScoreList[j] && spawnPoints[j].beenUsed == false && playerSpawned == false)
                {
                    spawnedPlayer = Instantiate(InformationManager.Instance.players[i].character, spawnPoints[j].spawnPos.transform.position, Quaternion.identity);
                    playerSpawned = true;
                    spawnPoints[j].beenUsed = true;
                }
            }
            if (InformationManager.Instance.players[i] == InformationManager.Instance.player1)
            {
                spawnedPlayer.name = "Player1";
                spawnedPlayer.GetComponent<PlayerInput>().playerLeftKey = KeybindingsManager.Instance.Player1Keys.left;
                spawnedPlayer.GetComponent<PlayerInput>().playerRightKey = KeybindingsManager.Instance.Player1Keys.right;
                spawnedPlayer.GetComponent<PlayerInput>().playerJumpKey = KeybindingsManager.Instance.Player1Keys.jump;
                spawnedPlayer.GetComponent<PlayerInput>().playerAbilityKey = KeybindingsManager.Instance.Player1Keys.ability;
                characterUIManager.BindUIToPlayer1(spawnedPlayer);
            }
            if (InformationManager.Instance.players[i] == InformationManager.Instance.player2)
            {
                spawnedPlayer.name = "Player2";
                spawnedPlayer.GetComponent<PlayerInput>().playerLeftKey = KeybindingsManager.Instance.Player2Keys.left;
                spawnedPlayer.GetComponent<PlayerInput>().playerRightKey = KeybindingsManager.Instance.Player2Keys.right;
                spawnedPlayer.GetComponent<PlayerInput>().playerJumpKey = KeybindingsManager.Instance.Player2Keys.jump;
                spawnedPlayer.GetComponent<PlayerInput>().playerAbilityKey = KeybindingsManager.Instance.Player2Keys.ability;
                characterUIManager.BindUIToPlayer2(spawnedPlayer);
            }
            if (InformationManager.Instance.players[i] == InformationManager.Instance.player3)
            {
                spawnedPlayer.name = "Player3";
                spawnedPlayer.GetComponent<PlayerInput>().playerLeftKey = KeybindingsManager.Instance.Player3Keys.left;
                spawnedPlayer.GetComponent<PlayerInput>().playerRightKey = KeybindingsManager.Instance.Player3Keys.right;
                spawnedPlayer.GetComponent<PlayerInput>().playerJumpKey = KeybindingsManager.Instance.Player3Keys.jump;
                spawnedPlayer.GetComponent<PlayerInput>().playerAbilityKey = KeybindingsManager.Instance.Player3Keys.ability;
                characterUIManager.BindUIToPlayer3(spawnedPlayer);
            }
            if (InformationManager.Instance.players[i] == InformationManager.Instance.player4)
            {
                spawnedPlayer.name = "Player4";
                spawnedPlayer.GetComponent<PlayerInput>().playerLeftKey = KeybindingsManager.Instance.Player4Keys.left;
                spawnedPlayer.GetComponent<PlayerInput>().playerRightKey = KeybindingsManager.Instance.Player4Keys.right;
                spawnedPlayer.GetComponent<PlayerInput>().playerJumpKey = KeybindingsManager.Instance.Player4Keys.jump;
                spawnedPlayer.GetComponent<PlayerInput>().playerAbilityKey = KeybindingsManager.Instance.Player4Keys.ability;
                characterUIManager.BindUIToPlayer4(spawnedPlayer);
            }
        }
        GameObject.FindObjectOfType<CameraScript>().BindPlayersToCamera();
    }
}