using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int NumberOfPlayersActive;

    private void Start()
    {
        if (PlayersStats.Instance.player1.playerIsActive)
        {
            SetPlayerInactive(1);
        }
        if (PlayersStats.Instance.player2.playerIsActive)
        {
            SetPlayerInactive(2);
        }
        if (PlayersStats.Instance.player3.playerIsActive)
        {
            SetPlayerInactive(3);
        }
        if (PlayersStats.Instance.player4.playerIsActive)
        {
            SetPlayerInactive(4);
        }
    }

    private void Update()
    {
        if(NumberOfPlayersActive < 0)
        {
            NumberOfPlayersActive = 0;
        }
    }
    public void SetPlayerActive(int playerNumber)
    {
        NumberOfPlayersActive += 1;
        switch (playerNumber)
        {
            case 1:
                PlayersStats.Instance.players.Add(PlayersStats.Instance.player1);
                PlayersStats.Instance.player1.playerIsActive = true;
                break;
            case 2:
                PlayersStats.Instance.players.Add(PlayersStats.Instance.player2);
                PlayersStats.Instance.player2.playerIsActive = true;
                break;
            case 3:
                PlayersStats.Instance.players.Add(PlayersStats.Instance.player3);
                PlayersStats.Instance.player3.playerIsActive = true;
                break;
            case 4:
                PlayersStats.Instance.players.Add(PlayersStats.Instance.player4);
                PlayersStats.Instance.player4.playerIsActive = true;
                break;
            default:
                Debug.Log("Invalid PlayerNumber entered (SetPlayerActive)");
                break;

        }
    }

    public void SetPlayerInactive(int playerNumber)
    {
        NumberOfPlayersActive -= 1;
        switch (playerNumber)
        {
            case 1:
                PlayersStats.Instance.players.Remove(PlayersStats.Instance.player1);
                PlayersStats.Instance.player1.character = null;
                PlayersStats.Instance.player1.playerIsActive = false;
                PlayersStats.Instance.player1.playerIsReady = false;
                break;
            case 2:
                PlayersStats.Instance.players.Remove(PlayersStats.Instance.player2);
                PlayersStats.Instance.player2.character = null;
                PlayersStats.Instance.player2.playerIsActive = false;
                PlayersStats.Instance.player2.playerIsReady = false;
                break;
            case 3:
                PlayersStats.Instance.players.Remove(PlayersStats.Instance.player3);
                PlayersStats.Instance.player3.character = null;
                PlayersStats.Instance.player3.playerIsActive = false;
                PlayersStats.Instance.player3.playerIsReady = false;
                break;
            case 4:
                PlayersStats.Instance.players.Remove(PlayersStats.Instance.player4);
                PlayersStats.Instance.player4.character = null;
                PlayersStats.Instance.player4.playerIsActive = false;
                PlayersStats.Instance.player4.playerIsReady = false;
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
                if (PlayersStats.Instance.player1.playerIsReady == false)
                {
                    if (PlayersStats.Instance.player1.character != null)
                    {
                        PlayersStats.Instance.player1.playerIsReady = true;
                    }
                    else
                    {
                        Debug.Log("Select a character");
                    }
                }
                else
                    PlayersStats.Instance.player1.playerIsReady = false;

            break;

            case 2:
                if (PlayersStats.Instance.player2.playerIsReady == false)
                {
                    if (PlayersStats.Instance.player2.character != null)
                    {
                        PlayersStats.Instance.player2.playerIsReady = true;
                    }
                    else
                    {
                        Debug.Log("Select a character");
                    }
                }
                else
                    PlayersStats.Instance.player2.playerIsReady = false;

                break;

            case 3:
                if (PlayersStats.Instance.player3.playerIsReady == false)
                {
                    if (PlayersStats.Instance.player3.character != null)
                    {
                        PlayersStats.Instance.player3.playerIsReady = true;
                    }
                    else
                    {
                        Debug.Log("Select a character");
                    }
                }
                else
                    PlayersStats.Instance.player3.playerIsReady = false;

                break;

            case 4:
                if (PlayersStats.Instance.player4.playerIsReady == false)
                {
                    if (PlayersStats.Instance.player4.character != null)
                    {
                        PlayersStats.Instance.player4.playerIsReady = true;
                    }
                    else
                    {
                        Debug.Log("Select a character");
                    }
                }
                else
                    PlayersStats.Instance.player4.playerIsReady = false;

                break;

            default:
                Debug.Log("Invalid PlayerNumber entered (SetPlayerReady)");
                break;

        }
    }

    public void SetCharacterForPlayer1(GameObject character)
    {
        PlayersStats.Instance.player1.character = character;
    }
    public void SetCharacterForPlayer2(GameObject character)
    {
        PlayersStats.Instance.player2.character = character;
    }
    public void SetCharacterForPlayer3(GameObject character)
    {
        PlayersStats.Instance.player3.character = character;
    }
    public void SetCharacterForPlayer4(GameObject character)
    {
        PlayersStats.Instance.player4.character = character;
    }

    public void StartGame()
    {
        int amountOfReadyPlayers = 0;
        for (int i = 0; i < PlayersStats.Instance.players.Count; i++)
        {
            if (PlayersStats.Instance.players[i].playerIsReady)
            {
                amountOfReadyPlayers += 1;
            }
            if(amountOfReadyPlayers == NumberOfPlayersActive)
            {
                Debug.Log("Starting game...");
                LoadRandomScene();
            }
        }
    }

    private void LoadRandomScene()
    {
        for(int i = 0; i < PlayersStats.Instance.levels.Length; i++)
        {
            if(PlayersStats.Instance.usedLevels.Contains(PlayersStats.Instance.levels[i]) == false)
            {
                PlayersStats.Instance.usedLevels.Add(PlayersStats.Instance.levels[i]);
                SceneManager.LoadScene(PlayersStats.Instance.levels[i]);
            }
        }
    }
}
