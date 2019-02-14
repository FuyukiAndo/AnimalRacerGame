using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public GameObject character;
    public bool playerIsActive = false;
    public bool playerIsReady = false;
}
[System.Serializable]
public class MultiplayerMaps
{
    public Object[] maps;
    public List<Object> usedMaps;
}

public class InformationManager : MonoBehaviour {

    public static InformationManager Instance { get; private set; }

    public MultiplayerMaps iceMaps;
    public MultiplayerMaps jungleMaps;
    public MultiplayerMaps farmMaps;
    public MultiplayerMaps coastMaps;


    public Player Singleplayer;
    public Player player1;
    public Player player2;
    public Player player3;
    public Player player4;
    public List<Player> players;
    public List<Object> multiplayerlevels;
    public List<Object> usedMultiplayerLevels;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        player1 = new Player();
        player2 = new Player();
        player3 = new Player();
        player4 = new Player();
        players = new List<Player>();
        multiplayerlevels = new List<Object>();
        usedMultiplayerLevels = new List<Object>();
    }
}
