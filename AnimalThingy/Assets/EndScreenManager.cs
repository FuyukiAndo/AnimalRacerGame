using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


[System.Serializable]
public class ScoreScreenChild
{
    public GameObject parent;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI nameText;
}
    public class EndScreenManager : MonoBehaviour {

    public ScoreScreenChild firstChild;
    public ScoreScreenChild secondChild;
    public ScoreScreenChild thirdChild;
    public ScoreScreenChild fourthChild;

    public List<ScoreScreenChild> childObjects;

    private void OnEnable()
    {
        AddChildrenToList();
        EnableChildObjects();
        AddNamesToChildObjects();
        AddTimeToChildObjects();
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
            //    switch (GoalManager.Instance.PlacedPlayers[i].name)
            //{
            //    case "Player1":

            //        break;

            //    case "Player2":

            //        break;

            //    case "Player3":

            //        break;
            //    case "Player4":

            //        break;
            //}
        }
    }
}
