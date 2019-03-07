using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MainMenuOptions
{
    public GameObject gameObject;
}

public class MainMenu : MonoBehaviour {

    public MainMenuOptions[] mainMenuOptions;


    private void Start()
    {
        foreach(Player player in InformationManager.Instance.players)
        {
            player.character = null;
            player.playerIsActive = false;
            player.playerIsReady = false;
            player.score = 0;
            player.level1Time = 0;
            player.level2Time = 0;
            player.level3Time = 0;
            player.level4Time = 0;
        }

        InformationManager.Instance.players.Clear();
        InformationManager.Instance.multiplayerLevels.Clear();
    }

    public void ExitGame()
    {
        Debug.Log("Exit!");
        Application.Quit();
    }

    public void DisableMenus(GameObject menu)
    {
        for (int i = 0; i < mainMenuOptions.Length; i++)
        {
            if (mainMenuOptions[i].gameObject != menu)
                mainMenuOptions[i].gameObject.SetActive(false);
        }
    }

    public void MenuOnOffSwitch(GameObject menu)
    {
        menu.SetActive(!menu.activeInHierarchy);
    }
}

