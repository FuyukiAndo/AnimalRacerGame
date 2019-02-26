using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindingsMenu : MonoBehaviour {

    public GameObject[] keybindingWindows;

    private void OnEnable()
    {
        foreach(GameObject window in keybindingWindows)
        {
            window.SetActive(false);
        }
        keybindingWindows[0].SetActive(true);
    }

    public void DisableOtherWindows()
    {
        foreach (GameObject window in keybindingWindows)
        {
            window.SetActive(false);
        }
    }

    public void SetWindowActive(int index)
    {
        keybindingWindows[index].SetActive(true);
    }
}
