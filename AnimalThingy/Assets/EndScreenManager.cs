using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[System.Serializable]
public class ScoreScreenChild
{
    public GameObject parent;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI NameText;
}
    public class EndScreenManager : MonoBehaviour {

    public ScoreScreenChild firstChild;
    public ScoreScreenChild secondChild;
    public ScoreScreenChild thirdChild;
    public ScoreScreenChild fourthChild;

    private List<ScoreScreenChild> childObjects;

    public TextMeshProUGUI FirstPlayerName;
    public TextMeshProUGUI FirstPlayerTime;

    public TextMeshProUGUI secondPlayerName;
    public TextMeshProUGUI secondPlayerTime;

    public TextMeshProUGUI thirdPlayerName;
    public TextMeshProUGUI thirdPlayerTime;

    public TextMeshProUGUI fourthPlayerName;
    public TextMeshProUGUI fourthPlayerTime;

    private void OnEnable()
    {
        AddChildrenToList();
        EnableChildObjects();
        AddNamesToChildObjects();
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
        
    }
}
