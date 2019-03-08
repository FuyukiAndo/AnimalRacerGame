using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSceneManager : MonoBehaviour {

    public GameObject player1SpawnPoint;
    public GameObject player2SpawnPoint;
    public GameObject player3SpawnPoint;
    public GameObject player4SpawnPoint;
    public List<GameObject> spawnPoints;



    public InformationManager informationManager;

    void Awake()
    {
       informationManager =  FindObjectOfType<InformationManager>();
        spawnPoints = new List<GameObject>();
        spawnPoints.Add(player1SpawnPoint);
        spawnPoints.Add(player2SpawnPoint);
        spawnPoints.Add(player3SpawnPoint);
        spawnPoints.Add(player4SpawnPoint);
    }

    void Start()
    {
        if (informationManager.player1.playerIsActive)
        {
            Instantiate(informationManager.player1.character, spawnPoints[1].transform);
            spawnPoints.Remove(spawnPoints[1]);
        }
        if (informationManager.player2.playerIsActive)
        {
            Instantiate(informationManager.player2.character, spawnPoints[1].transform);
            spawnPoints.Remove(spawnPoints[1]);
        }
        if (informationManager.player3.playerIsActive)
        {
            Instantiate(informationManager.player3.character, spawnPoints[1].transform);
            spawnPoints.Remove(spawnPoints[1]);
        }
        if (informationManager.player4.playerIsActive)
        {
            Instantiate(informationManager.player4.character, spawnPoints[1].transform);
            spawnPoints.Remove(spawnPoints[1]);
        }
    }
}


// level1 start position = random
// andra levels, beroende av current score i turneringen.

// mät spelarnas tid för varje bana - emil redan gjort
// ge poäng vid målgång för varje spelare
// vid slutet av turneringen, av 2 eller fler spelare med samma score får dem position efter snabbast tid i alla banor (level1 + level2 + level3 + level4)

// banor grupperas efter typ