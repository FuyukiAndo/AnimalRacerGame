using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{

	public static StartManager Instance
	{
		get
		{
			return instance;
		}
	}

	private static StartManager instance;
	[SerializeField] private GameObject[] unplacedPlayers;
	[SerializeField] private string playerMoveScript;
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
	[SerializeField] private GameObject[] playerStartPositions;
	[SerializeField] private bool startCountDownOnSceneLoad;

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
		PlacePlayers();
		TrapPlayers();
		if (startCountDownOnSceneLoad)
		{
			StartCoroutine(CountDownStart());
		}
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
		//string componentName = playerMoveScript.GetType().ToString();
		foreach (var player in unplacedPlayers)
		{
			MonoBehaviour script = player.GetComponent(playerMoveScript)as MonoBehaviour;
			script.enabled = true;
		}
	}

	void TrapPlayers()
	{
		//string componentName = playerMoveScript.GetType().ToString();
		foreach (var player in unplacedPlayers)
		{
			MonoBehaviour script = player.GetComponent(playerMoveScript)as MonoBehaviour;
			script.enabled = false;
		}
	}

    public void SortPlayers()
    {
        foreach(Player player in InformationManager.Instance.players)
        {
            playerScoreList.Add(player.score);
        }
        playerScoreList.Sort();
        playerScoreList.Reverse();
    }
    void SpawnPlayers()
    {
        for(int i = 0; i < InformationManager.Instance.players.Count; i++)
        {
            GameObject spawnedPlayer = null;
            bool playerSpawned = false;
            for(int j = 0; j < playerScoreList.Count; j++)
            {
                if(InformationManager.Instance.players[i].score == playerScoreList[j] && spawnPoints[j].beenUsed == false && playerSpawned == false)
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