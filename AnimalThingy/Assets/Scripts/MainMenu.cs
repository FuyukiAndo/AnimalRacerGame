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

    public void ExitGame()
    {
        Debug.Log("Exit!");
        Application.Quit();
    }

    public void DisableCurrentMenu()
    {
        for (int i = 0; i < mainMenuOptions.Length; i++)
        {
            mainMenuOptions[i].gameObject.SetActive(false);
        }
    }
}

