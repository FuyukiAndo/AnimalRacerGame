using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MultiplayerManager : MonoBehaviour {

    public int NumberOfPlayersActive;

    private void Start()
    {
        if (InformationManager.Instance.player1.playerIsActive)
        {
            SetPlayerInactive(1);
        }
        if (InformationManager.Instance.player2.playerIsActive)
        {
            SetPlayerInactive(2);
        }
        if (InformationManager.Instance.player3.playerIsActive)
        {
            SetPlayerInactive(3);
        }
        if (InformationManager.Instance.player4.playerIsActive)
        {
            SetPlayerInactive(4);
        }
    }

    private void Update()
    {
        if (NumberOfPlayersActive < 0)
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
                InformationManager.Instance.players.Add(InformationManager.Instance.player1);
                InformationManager.Instance.player1.playerIsActive = true;
                break;
            case 2:
                InformationManager.Instance.players.Add(InformationManager.Instance.player2);
                InformationManager.Instance.player2.playerIsActive = true;
                break;
            case 3:
                InformationManager.Instance.players.Add(InformationManager.Instance.player3);
                InformationManager.Instance.player3.playerIsActive = true;
                break;
            case 4:
                InformationManager.Instance.players.Add(InformationManager.Instance.player4);
                InformationManager.Instance.player4.playerIsActive = true;
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
                InformationManager.Instance.players.Remove(InformationManager.Instance.player1);
                InformationManager.Instance.player1.character = null;
                InformationManager.Instance.player1.playerIsActive = false;
                InformationManager.Instance.player1.playerIsReady = false;
                break;
            case 2:
                InformationManager.Instance.players.Remove(InformationManager.Instance.player2);
                InformationManager.Instance.player2.character = null;
                InformationManager.Instance.player2.playerIsActive = false;
                InformationManager.Instance.player2.playerIsReady = false;
                break;
            case 3:
                InformationManager.Instance.players.Remove(InformationManager.Instance.player3);
                InformationManager.Instance.player3.character = null;
                InformationManager.Instance.player3.playerIsActive = false;
                InformationManager.Instance.player3.playerIsReady = false;
                break;
            case 4:
                InformationManager.Instance.players.Remove(InformationManager.Instance.player4);
                InformationManager.Instance.player4.character = null;
                InformationManager.Instance.player4.playerIsActive = false;
                InformationManager.Instance.player4.playerIsReady = false;
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
                if (InformationManager.Instance.player1.playerIsReady == false)
                {
                    if (InformationManager.Instance.player1.character != null)
                    {
                        InformationManager.Instance.player1.playerIsReady = true;
                    }
                    else
                    {
                        Debug.Log("Select a character");
                    }
                }
                else
                    InformationManager.Instance.player1.playerIsReady = false;

                break;

            case 2:
                if (InformationManager.Instance.player2.playerIsReady == false)
                {
                    if (InformationManager.Instance.player2.character != null)
                    {
                        InformationManager.Instance.player2.playerIsReady = true;
                    }
                    else
                    {
                        Debug.Log("Select a character");
                    }
                }
                else
                    InformationManager.Instance.player2.playerIsReady = false;

                break;

            case 3:
                if (InformationManager.Instance.player3.playerIsReady == false)
                {
                    if (InformationManager.Instance.player3.character != null)
                    {
                        InformationManager.Instance.player3.playerIsReady = true;
                    }
                    else
                    {
                        Debug.Log("Select a character");
                    }
                }
                else
                    InformationManager.Instance.player3.playerIsReady = false;

                break;

            case 4:
                if (InformationManager.Instance.player4.playerIsReady == false)
                {
                    if (InformationManager.Instance.player4.character != null)
                    {
                        InformationManager.Instance.player4.playerIsReady = true;
                    }
                    else
                    {
                        Debug.Log("Select a character");
                    }
                }
                else
                    InformationManager.Instance.player4.playerIsReady = false;

                break;

            default:
                Debug.Log("Invalid PlayerNumber entered (SetPlayerReady)");
                break;

        }
    }

    public void SetCharacterForPlayer1(GameObject character)
    {
        InformationManager.Instance.player1.character = character;
    }
    public void SetCharacterForPlayer2(GameObject character)
    {
        InformationManager.Instance.player2.character = character;
    }
    public void SetCharacterForPlayer3(GameObject character)
    {
        InformationManager.Instance.player3.character = character;
    }
    public void SetCharacterForPlayer4(GameObject character)
    {
        InformationManager.Instance.player4.character = character;
    }

    public void StartGame()
    {
        int amountOfReadyPlayers = 0;
        for (int i = 0; i < InformationManager.Instance.players.Count; i++)
        {
            if (InformationManager.Instance.players[i].playerIsReady)
            {
                amountOfReadyPlayers += 1;
            }
            if (amountOfReadyPlayers == NumberOfPlayersActive)
            {
                Debug.Log("Starting game...");
                LoadRandomScene();
            }
        }
    }

    private void LoadRandomScene()
    {
        for (int i = 0; i < InformationManager.Instance.multiplayerLevels.Length; i++)
        {
            if (InformationManager.Instance.usedMultiplayerLevels.Contains(InformationManager.Instance.multiplayerLevels[i]) == false)
            {
                InformationManager.Instance.usedMultiplayerLevels.Add(InformationManager.Instance.multiplayerLevels[i]);
                SceneManager.LoadScene(InformationManager.Instance.multiplayerLevels[i].name);
            }
        }
    }
}
