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

