using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MultiplayerMaps
{
    public string[] maps;
}

public class MultiplayerManager : MonoBehaviour {

    public MultiplayerMaps iceMaps;
    public MultiplayerMaps jungleMaps;
    public MultiplayerMaps farmMaps;
    public MultiplayerMaps coastMaps;

    [HideInInspector] public int NumberOfPlayersActive;
    private List<string> allMaps;
    public string iceCharacterTag = "Ice_Character";
    public string jungleCharacterTag = "Jungle_Character";
    public string farmCharacterTag = "Farm_Character";
    public string coastCharacterTag = "Coast_Character";

    private void Start()
    {
        allMaps = new List<string>();
        InformationManager.Instance.multiplayerLevels.Clear();
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
        //Aktivera när 4 eller fler kompletta scener finns och ligger i InformationManagers folders för banor
        AddAllMapsToList();
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
        if(InformationManager.Instance.player1.playerIsReady == true)
        {
            InformationManager.Instance.player1.playerIsReady = false;
        }
    }
    public void SetCharacterForPlayer2(GameObject character)
    {
        InformationManager.Instance.player2.character = character;
        if (InformationManager.Instance.player2.playerIsReady == true)
        {
            InformationManager.Instance.player2.playerIsReady = false;
        }
    }
    public void SetCharacterForPlayer3(GameObject character)
    {
        InformationManager.Instance.player3.character = character;
        if (InformationManager.Instance.player3.playerIsReady == true)
        {
            InformationManager.Instance.player3.playerIsReady = false;
        }
    }
    public void SetCharacterForPlayer4(GameObject character)
    {
        InformationManager.Instance.player4.character = character;
        if (InformationManager.Instance.player4.playerIsReady == true)
        {
            InformationManager.Instance.player4.playerIsReady = false;
        }
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
                LoadRandomCharacterScene();

                //Aktivera när 4 eller fler kompletta scener finns och ligger i InformationManagers folders för banor
                if (amountOfReadyPlayers < 4)
                {
                    for (int j = 0; j < 4 - amountOfReadyPlayers; j++)
                    {
                        LoadRandomScene();
                    }
                }


                SceneManager.LoadScene(InformationManager.Instance.multiplayerLevels[0]);
			}
        }
    }

    private void LoadRandomCharacterScene()
    {
        foreach (Player player in InformationManager.Instance.players)
        {
            if(player.character.tag == iceCharacterTag)
            {
                List<string> tempMaps = new List<string>(iceMaps.maps);
                bool levelSelected = false;
                for (int i = 0; i < iceMaps.maps.Length; i++)
                {
                    if(levelSelected == false)
                    {
                        int temp = Random.Range(0, tempMaps.Count);
                        if (InformationManager.Instance.multiplayerLevels.Contains(tempMaps[temp]))
                        {
                            tempMaps.Remove(tempMaps[i]);
                        }
                        else
                        {
                            InformationManager.Instance.multiplayerLevels.Add(tempMaps[i]);
                            levelSelected = true;
                        }
                    }

                }
            }
            if (player.character.tag == jungleCharacterTag)
            {
                List<string> tempMaps = new List<string>(jungleMaps.maps);
                bool levelSelected = false;
                for (int i = 0; i < jungleMaps.maps.Length; i++)
                {
                    if(levelSelected == false)
                    {
                        int temp = Random.Range(0, tempMaps.Count);
                        if (InformationManager.Instance.multiplayerLevels.Contains(tempMaps[temp]))
                        {
                            tempMaps.Remove(tempMaps[i]);
                        }
                        else
                        {
                            InformationManager.Instance.multiplayerLevels.Add(tempMaps[i]);
                            levelSelected = true;
                        }
                    }
                }
            }
            if (player.character.tag == farmCharacterTag)
            {
                List<string> tempMaps = new List<string>(farmMaps.maps);
                bool levelSelected = false;
                for (int i = 0; i < farmMaps.maps.Length; i++)
                {
                    if(levelSelected == false)
                    {
                        int temp = Random.Range(0, tempMaps.Count);
                        if (InformationManager.Instance.multiplayerLevels.Contains(tempMaps[temp]))
                        {
                            tempMaps.Remove(tempMaps[i]);
                        }
                        else
                        {
                            InformationManager.Instance.multiplayerLevels.Add(tempMaps[i]);
                            levelSelected = true;
                        }
                    }
                }
            }
            if (player.character.tag == coastCharacterTag)
            {
                List<string> tempMaps = new List<string>(coastMaps.maps);
                bool levelSelected = false;
                for (int i = 0; i < coastMaps.maps.Length; i++)
                {
                    if (levelSelected == false)
                    {
                        int temp = Random.Range(0, tempMaps.Count);
                        if (InformationManager.Instance.multiplayerLevels.Contains(tempMaps[temp]))
                        {
                            tempMaps.Remove(tempMaps[i]);
                        }
                        else
                        {
                            InformationManager.Instance.multiplayerLevels.Add(tempMaps[i]);
                            levelSelected = true;
                        }
                    }
                }
            }
        }
    }

    private void AddAllMapsToList()
    {
        for (int i = 0; i < iceMaps.maps.Length; i++)
        {
            allMaps.Add(iceMaps.maps[i]);
        }

        for (int i = 0; i < jungleMaps.maps.Length; i++)
        {
            allMaps.Add(jungleMaps.maps[i]);
        }

        for (int i = 0; i < farmMaps.maps.Length; i++)
        {
            allMaps.Add(farmMaps.maps[i]);
        }

        for (int i = 0; i < coastMaps.maps.Length; i++)
        {
            allMaps.Add(coastMaps.maps[i]);
        }
    }

    private void LoadRandomScene()
    {
        bool levelSelected = false;
        for(int i = 0; i < allMaps.Count; i++)
        {
            if (levelSelected == false)
            {
                int temp = Random.Range(0, allMaps.Count);
                if (InformationManager.Instance.multiplayerLevels.Contains(allMaps[temp]))
                {
                    allMaps.Remove(allMaps[i]);
                }
                else
                {
                    InformationManager.Instance.multiplayerLevels.Add(allMaps[i]);
                    levelSelected = true;
                }
            }
        }
    }
}

// även om 4 spelare väljer samma typ av karaktär (ex: is) måste fyra unika banor adderas till multiplayerlevels.