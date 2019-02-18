using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPoint
{
    public GameObject spawnPos;
    public Player player;
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
	[SerializeField] private GameObject[] unplacedPlayers;
	[SerializeField] private string playerMoveScript;
    private InformationManager informationManager;
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

	void Start()
	{
        spawnPoints = new List<SpawnPoint>
        {
            spawnPos1,
            spawnPos2,
            spawnPos3,
            spawnPos4
        };
        InformationManager.Instance.player3.score += 10;
        informationManager = FindObjectOfType<InformationManager>();
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
        SpawnPlayers();
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
    void SpawnPlayers()
    {
        //Fungerar inte riktigt!
        foreach(Player player in InformationManager.Instance.players)
        {
            bool playerSpawned = false;

            if(spawnPos1.beenUsed == false && playerSpawned == false)
            {
                playerSpawned = true;
                Instantiate(player.character, spawnPos1.spawnPos.transform.position, Quaternion.identity);
                spawnPos1.player = player;
                spawnPos1.beenUsed = true;
            }
            if (spawnPos2.beenUsed == false && playerSpawned == false)
            {
                playerSpawned = true;
                Instantiate(player.character, spawnPos2.spawnPos.transform.position, Quaternion.identity);
                spawnPos2.player = player;
                spawnPos2.beenUsed = true;
            }
            if (spawnPos3.beenUsed == false && playerSpawned == false)
            {
                playerSpawned = true;
                Instantiate(player.character, spawnPos3.spawnPos.transform.position, Quaternion.identity);
                spawnPos3.player = player;
                spawnPos3.beenUsed = true;
            }
            if (spawnPos4.beenUsed == false && playerSpawned == false)
            {
                playerSpawned = true;
                Instantiate(player.character, spawnPos4.spawnPos.transform.position, Quaternion.identity);
                spawnPos4.player = player;
                spawnPos4.beenUsed = true;
            }
        }
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if(spawnPoint.player.score > 0)
            {
                if(spawnPoints[spawnPoints.IndexOf(spawnPoint) - 1]  != null && spawnPoint.player.score > spawnPoints[spawnPoints.IndexOf(spawnPoint) - 1].player.score)
                {
                    spawnPoint.player.character.transform.Translate(spawnPoints[spawnPoints.IndexOf(spawnPoint) - 1].player.character.transform.position);
                    spawnPoints[spawnPoints.IndexOf(spawnPoint) - 1].player.character.transform.Translate(spawnPoint.player.character.transform.position);
                }
            }
        }
    }
}