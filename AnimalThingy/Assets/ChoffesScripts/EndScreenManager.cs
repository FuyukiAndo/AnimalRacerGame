using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class ScoreScreenChild
{
    public GameObject parent;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;
}
    public class EndScreenManager : MonoBehaviour {

    public ScoreScreenChild firstChild;
    public ScoreScreenChild secondChild;
    public ScoreScreenChild thirdChild;
    public ScoreScreenChild fourthChild;

    public bool victoryScene = false;

    private List<ScoreScreenChild> childObjects;

    public Color player1Color = Color.red;
    public Color player2Color = Color.blue;
    public Color player3Color = Color.green;
    public Color player4Color = Color.yellow;

    private Dictionary<string, float> playerTimeList = new Dictionary<string, float>();
    private Dictionary<string, int> playerScoreList = new Dictionary<string, int>();

    private void OnEnable()
    {
        AddChildrenToList();
        EnableChildObjects();
        if (victoryScene)
        {
            AddScoresInVictoryScene();
            AddTimeInVictoryScene();
            SortPlayersInVictoryScene();
        }
        else
        {
            AddNamesToChildObjects();
            AddTimeToChildObjects();
            SetBackgroundColor();
        }
    }

    private void AddChildrenToList()
    {
        childObjects = new List<ScoreScreenChild>();
        switch (InformationManager.Instance.players.Count)
        {
            case 1:
                childObjects.Add(firstChild);
                break;
            case 2:
                childObjects.Add(firstChild);
                childObjects.Add(secondChild);
                break;
            case 3:
                childObjects.Add(firstChild);
                childObjects.Add(secondChild);
                childObjects.Add(thirdChild);
                break;
            case 4:
                childObjects.Add(firstChild);
                childObjects.Add(secondChild);
                childObjects.Add(thirdChild);
                childObjects.Add(fourthChild);
                break;
        }
    }

    private void EnableChildObjects()
    {
        foreach(ScoreScreenChild child in childObjects)
        {
            child.parent.SetActive(true);
        }
    }

    private void AddNamesToChildObjects()
    {
        for(int i = 0; i < childObjects.Count; i++)
        {
            switch (GoalManager.Instance.PlacedPlayers[i].name)
            {
                case "Player1":
                    childObjects[i].nameText.text = GoalManager.Instance.PlacedPlayers[i].name;
                    break;
                case "Player2":
                    childObjects[i].nameText.text = GoalManager.Instance.PlacedPlayers[i].name;
                    break;
                case "Player3":
                    childObjects[i].nameText.text = GoalManager.Instance.PlacedPlayers[i].name;
                    break;
                case "Player4":
                    childObjects[i].nameText.text = GoalManager.Instance.PlacedPlayers[i].name;
                    break;
            }
        }
    }

    private void AddTimeToChildObjects()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        for(int i = 0; i < GoalManager.Instance.PlacedPlayers.Count; i++)
        {      
            if(GoalManager.Instance.PlacedPlayers[i].name == "Player1")
            {
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[0])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player1.level1Time.ToString();
                    continue;
                    }
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[1])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player1.level2Time.ToString();
                    continue;
                }
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[2])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player1.level3Time.ToString();
                    continue;
                }
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[3])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player1.level4Time.ToString();
                    continue;
                }
            }
            if(GoalManager.Instance.PlacedPlayers[i].name == "Player2")
            {
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[0])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player2.level1Time.ToString();
                    continue;
                }
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[1])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player2.level2Time.ToString();
                    continue;
                }
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[2])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player2.level3Time.ToString();
                    continue;
                }
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[3])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player2.level4Time.ToString();
                    continue;
                }
            }
            if (GoalManager.Instance.PlacedPlayers[i].name == "Player3")
            {
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[0])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player3.level1Time.ToString();
                    continue;
                }
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[1])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player3.level2Time.ToString();
                    continue;
                }
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[2])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player3.level3Time.ToString();
                    continue;
                }
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[3])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player3.level4Time.ToString();
                    continue;
                }
            }
            if (GoalManager.Instance.PlacedPlayers[i].name == "Player4")
            {
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[0])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player4.level1Time.ToString();
                    continue;
                }
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[1])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player4.level2Time.ToString();
                    continue;
                }
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[2])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player4.level3Time.ToString();
                    continue;
                }
                    if (currentScene.name == InformationManager.Instance.multiplayerLevels[3])
                    {
                        childObjects[i].timeText.text = InformationManager.Instance.player4.level4Time.ToString();
                    continue;
                }
            }
        }
    }

    private void AddTimeInVictoryScene()
    {
        foreach (Player player in InformationManager.Instance.players)
        {
            float combinedTime = 0f;
            combinedTime = player.level1Time + player.level2Time + player.level3Time + player.level4Time;
            if(player == InformationManager.Instance.player1)
            {
                playerTimeList.Add("Player1", combinedTime);
            }
            if(player == InformationManager.Instance.player2)
            {
                playerTimeList.Add("Player2", combinedTime);
            }
            if (player == InformationManager.Instance.player3)
            {
                playerTimeList.Add("Player3", combinedTime);
            }
            if (player == InformationManager.Instance.player4)
            {
                playerTimeList.Add("Player4", combinedTime);
            }
        }
    }

    private void AddScoresInVictoryScene()
    {
        foreach (Player player in InformationManager.Instance.players)
        {
            if (player == InformationManager.Instance.player1)
            {
                playerScoreList.Add("Player1", InformationManager.Instance.player1.score);
            }
            if (player == InformationManager.Instance.player2)
            {
                playerScoreList.Add("Player2", InformationManager.Instance.player2.score);
            }
            if (player == InformationManager.Instance.player3)
            {
                playerScoreList.Add("Player3", InformationManager.Instance.player3.score);
            }
            if (player == InformationManager.Instance.player4)
            {
                playerScoreList.Add("Player4", InformationManager.Instance.player4.score);
            }
        }
    }

    private void SortPlayersInVictoryScene()
    {
        List<float> sortedPlayerTimes = new List<float>();
        foreach(var player in playerTimeList)
        {
            sortedPlayerTimes.Add(player.Value);
        }
        sortedPlayerTimes.Sort();
        List<int> sortedPlayerScores = new List<int>();
        foreach (var player in playerScoreList)
        {
            sortedPlayerScores.Add(player.Value);
        }
        sortedPlayerScores.Sort();
        sortedPlayerScores.Reverse();



        for(int i = 0; i < childObjects.Count; i++)
        {
            if(sortedPlayerScores[i] == InformationManager.Instance.player1.score && childObjects[i].nameText.text == null)
            {
                childObjects[i].nameText.text = "Player1";
                childObjects[i].timeText.text = playerTimeList["Player1"].ToString();
                childObjects[i].scoreText.text = sortedPlayerScores[i].ToString();
                goto END;
            }
            if (sortedPlayerScores[i] == InformationManager.Instance.player2.score && childObjects[i].nameText.text == null)
            {
                childObjects[i].nameText.text = "Player2";
                childObjects[i].timeText.text = playerTimeList["Player2"].ToString();
                childObjects[i].scoreText.text = sortedPlayerScores[i].ToString();
                goto END;
            }
            if (sortedPlayerScores[i] == InformationManager.Instance.player3.score && childObjects[i].nameText.text == null)
            {
                childObjects[i].nameText.text = "Player3";
                childObjects[i].timeText.text = playerTimeList["Player3"].ToString();
                childObjects[i].scoreText.text = sortedPlayerScores[i].ToString();
                goto END;
            }
            if (sortedPlayerScores[i] == InformationManager.Instance.player4.score && childObjects[i].nameText.text == null)
            {
                childObjects[i].nameText.text = "Player4";
                childObjects[i].timeText.text = playerTimeList["Player4"].ToString();
                childObjects[i].scoreText.text = sortedPlayerScores[i].ToString();
                goto END;
            }
        END:;
        }
    }

    private void SetBackgroundColor()
    {
        switch (childObjects[0].nameText.text)
        {
            case "Player1":
                gameObject.GetComponent<Image>().color = player1Color;
                break;
            case "Player2":
                gameObject.GetComponent<Image>().color = player2Color;
                break;
            case "Player3":
                gameObject.GetComponent<Image>().color = player3Color;
                break;
            case "Player4":
                gameObject.GetComponent<Image>().color = player4Color;
                break;
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
