using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleplayerManager : MonoBehaviour {

    public GameObject pig;
    public int pigLevelIndex;
    public GameObject albatross;
    public int albatrossLevelIndex;
    public GameObject monkey;
    public int monkeyLevelIndex;
    public GameObject pinguin;
    public int pinguinLevelIndex;

	void Start () {

        if (InformationManager.Instance.Singleplayer.playerIsActive)
        {
            InformationManager.Instance.Singleplayer.playerIsActive = false;
        }
	}

    public void SetCharacterForPlayer(GameObject character)
    {
        InformationManager.Instance.Singleplayer.character = character;
    }

    public void StartCharacterLevel()
    {
        if (InformationManager.Instance.Singleplayer.character == pig)
        {
            SceneManager.LoadScene(pigLevelIndex);
        }
        if (InformationManager.Instance.Singleplayer.character == albatross)
        {
            SceneManager.LoadScene(albatrossLevelIndex);
        }
        if (InformationManager.Instance.Singleplayer.character == monkey)
        {
            SceneManager.LoadScene(monkeyLevelIndex);
        }
        if (InformationManager.Instance.Singleplayer.character == pinguin)
        {
            SceneManager.LoadScene(pinguinLevelIndex);
        }
    }

}
