using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Player
{
    public GameObject character;
    public bool playerIsActive = false;
    public bool playerIsReady = false;
}
public class GameManager : MonoBehaviour {

    public int[] levels;
    private List<int> usedLevels;
    public List<Player> players;
    public Player player1;
    public Player player2;
    public Player player3;
    public Player player4;
    public int criteriaToStartGame = 0;

    private void Awake()
    {
        player1 = new Player();
        player2 = new Player();
        player3 = new Player();
        player4 = new Player();
        players = new List<Player>();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (players.Count > 4)
        {
            Debug.Log("ToManyPlayersInList");
        }
    }

    public void SetPlayerActive(int playerNumber)
    {
        criteriaToStartGame += 1;
        switch (playerNumber)
        {
            case 1:
                players.Add(player1);
                player1.playerIsActive = true;
                break;
            case 2:
                players.Add(player2);
                player2.playerIsActive = true;
                break;
            case 3:
                players.Add(player3);
                player3.playerIsActive = true;
                break;
            case 4:
                players.Add(player4);
                player4.playerIsActive = true;
                break;
            default:
                Debug.Log("Invalid PlayerNumber entered (SetPlayerActive)");
                break;

        }
    }

    public void SetPlayerInactive(int playerNumber)
    {
        criteriaToStartGame -= 1;
        switch (playerNumber)
        {
            case 1:
                players.Remove(player1);
                player1.character = null;
                player1.playerIsActive = false;
                player1.playerIsReady = false;
                break;
            case 2:
                players.Remove(player2);
                player2.character = null;
                player2.playerIsActive = false;
                player2.playerIsReady = false;
                break;
            case 3:
                players.Remove(player3);
                player3.character = null;
                player3.playerIsActive = false;
                player3.playerIsReady = false;
                break;
            case 4:
                players.Remove(player4);
                player4.character = null;
                player4.playerIsActive = false;
                player4.playerIsReady = false;
                break;
            default:
                Debug.Log("Invalid PlayerNumber entered (SetPlayerInactive)");
                break;

        }
    }

    public void SetPlayerReady(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                if (player1.playerIsReady == false)
                {
                    if (player1.character != null)
                    {
                        player1.playerIsReady = true;
                    }
                    else
                    {
                        Debug.Log("Select a character");
                    }
                }
                else
                    player1.playerIsReady = false;

            break;

            case 2:
                if (player2.playerIsReady == false)
                {
                    if (player2.character != null)
                    {
                        player2.playerIsReady = true;
                    }
                    else
                    {
                        Debug.Log("Select a character");
                    }
                }
                else
                    player2.playerIsReady = false;

                break;

            case 3:
                if (player3.playerIsReady == false)
                {
                    if (player3.character != null)
                    {
                        player3.playerIsReady = true;
                    }
                    else
                    {
                        Debug.Log("Select a character");
                    }
                }
                else
                    player3.playerIsReady = false;

                break;

            case 4:
                if (player4.playerIsReady == false)
                {
                    if (player4.character != null)
                    {
                        player4.playerIsReady = true;
                    }
                    else
                    {
                        Debug.Log("Select a character");
                    }
                }
                else
                    player4.playerIsReady = false;

                break;

            default:
                Debug.Log("Invalid PlayerNumber entered (SetPlayerReady)");
                break;

        }
    }

    public void SetCharacterForPlayer1(GameObject character)
    {
        player1.character = character;
    }
    public void SetCharacterForPlayer2(GameObject character)
    {
        player2.character = character;
    }
    public void SetCharacterForPlayer3(GameObject character)
    {
        player3.character = character;
    }
    public void SetCharacterForPlayer4(GameObject character)
    {
        player4.character = character;
    }

    public void StartGame()
    {
        int amountOfReadyPlayers = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerIsReady)
            {
                amountOfReadyPlayers += 1;
            }
            if(amountOfReadyPlayers == criteriaToStartGame)
            {
                Debug.Log("Starting game...");
                LoadRandomScene();
            }
        }
    }

    private void LoadRandomScene()
    {
        int level = Random.Range(0, levels.Length);
        usedLevels.Add(level);

        SceneManager.LoadScene(levels[level]);
    }

    // ta in en pool av levels

    // instanciera en random level. vid nästa tillfälle instanciera en level som != en använd





}
