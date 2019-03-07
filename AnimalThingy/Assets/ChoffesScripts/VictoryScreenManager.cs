using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreenManager : MonoBehaviour {


    public ScoreScreenChild firstChild;
    public ScoreScreenChild secondChild;
    public ScoreScreenChild thirdChild;
    public ScoreScreenChild fourthChild;

    public string videoPlayerName = "Video Player";

    private List<ScoreScreenChild> childObjects;

    private Dictionary<string, float> playerTimeList = new Dictionary<string, float>();
    private Dictionary<string, int> playerScoreList = new Dictionary<string, int>();

    private bool player1Set = false;
    private bool player2Set = false;
    private bool player3Set = false;
    private bool player4Set = false;

    private void Start()
    {
        AddChildrenToList();
        EnableChildObjects();
        AddScoresInVictoryScene();
        AddTimeInVictoryScene();
        SortPlayersInVictoryScene();  
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
        foreach (ScoreScreenChild child in childObjects)
        {
            child.parent.SetActive(true);
        }
    }

    private void AddTimeInVictoryScene()
    {
        foreach (Player player in InformationManager.Instance.players)
        {
            float combinedTime = 0f;
            combinedTime = player.level1Time + player.level2Time + player.level3Time + player.level4Time;
            if (player == InformationManager.Instance.player1)
            {
                playerTimeList.Add("Player1", combinedTime);
            }
            if (player == InformationManager.Instance.player2)
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
        foreach (var player in playerTimeList)
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
        SetVictoryVideo(sortedPlayerTimes);
        for (int i = 0; i < childObjects.Count; i++)
        {
            if (sortedPlayerTimes[i] == InformationManager.Instance.player1.level1Time + InformationManager.Instance.player1.level2Time + 
                InformationManager.Instance.player1.level3Time + InformationManager.Instance.player1.level4Time && player1Set == false)
            {
                childObjects[i].nameText.text = "Player1";
                childObjects[i].timeText.text = sortedPlayerTimes[i].ToString();
                childObjects[i].scoreText.text = playerScoreList["Player1"].ToString();
                player1Set = true;
                goto END;
            }
            if (sortedPlayerTimes[i] == InformationManager.Instance.player2.level1Time + InformationManager.Instance.player2.level2Time +
                InformationManager.Instance.player2.level3Time + InformationManager.Instance.player2.level4Time && player2Set == false)
            {
                childObjects[i].nameText.text = "Player2";
                childObjects[i].timeText.text = sortedPlayerTimes[i].ToString();
                childObjects[i].scoreText.text = playerScoreList["Player2"].ToString();
                player2Set = true;
                goto END;
            }
            if (sortedPlayerTimes[i] == InformationManager.Instance.player3.level1Time + InformationManager.Instance.player3.level2Time +
                InformationManager.Instance.player3.level3Time + InformationManager.Instance.player3.level4Time && player3Set == false)
            {
                childObjects[i].nameText.text = "Player3";
                childObjects[i].timeText.text = sortedPlayerTimes[i].ToString();
                childObjects[i].scoreText.text = playerScoreList["Player3"].ToString();
                player3Set = true;
                goto END;
            }
            if (sortedPlayerTimes[i] == InformationManager.Instance.player4.level1Time + InformationManager.Instance.player4.level2Time +
                InformationManager.Instance.player4.level3Time + InformationManager.Instance.player4.level4Time && player4Set == false)
            {
                childObjects[i].nameText.text = "Player4";
                childObjects[i].timeText.text = sortedPlayerTimes[i].ToString();
                childObjects[i].scoreText.text = playerScoreList["Player4"].ToString();
                player4Set = true;
                goto END;
            }
        END:;
        }
    }

    private void SetVictoryVideo(List<float> sortedPlayerTimes)
    {
        for(int i = 0; i < playerTimeList.Count; i++)
        {
            if(sortedPlayerTimes[0] == playerTimeList["Player1"])
            {
                GameObject.Find(videoPlayerName).GetComponent<VideoPlayerScript>().SetVictoryVideo("Player1");
            }
            if (sortedPlayerTimes[0] == playerTimeList["Player2"])
            {
                GameObject.Find(videoPlayerName).GetComponent<VideoPlayerScript>().SetVictoryVideo("Player2");
            }
            if (sortedPlayerTimes[0] == playerTimeList["Player3"])
            {
                GameObject.Find(videoPlayerName).GetComponent<VideoPlayerScript>().SetVictoryVideo("Player3");
            }
            if (sortedPlayerTimes[0] == playerTimeList["Player4"])
            {
                GameObject.Find(videoPlayerName).GetComponent<VideoPlayerScript>().SetVictoryVideo("Player4");
            }
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene("StartMenu");
    }

}
