using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeybindingsMenu : MonoBehaviour {

    public GameObject[] keybindingWindows;
    public GameObject currentKey;

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

    private void OnGUI()
    {
        if(currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                //FEL SOM FAN, fix pleb
                switch (currentKey.transform.root.name)
                {
                    case "Player1Background":
                        if(KeybindingsManager.Instance.Player1Keys.left.ToString() == currentKey.name.ToString())
                        {
                            KeybindingsManager.Instance.Player1Keys.left = e.keyCode;
                            currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        }
                        if (KeybindingsManager.Instance.Player1Keys.right.ToString() == currentKey.name.ToString())
                        {
                            KeybindingsManager.Instance.Player1Keys.right = e.keyCode;
                            currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        }
                        if (KeybindingsManager.Instance.Player1Keys.jump.ToString() == currentKey.name.ToString())
                        {
                            KeybindingsManager.Instance.Player1Keys.jump = e.keyCode;
                            currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        }
                        if (KeybindingsManager.Instance.Player1Keys.ability.ToString() == currentKey.name.ToString())
                        {
                            KeybindingsManager.Instance.Player1Keys.ability = e.keyCode;
                            currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        }
                        break;
                    case "Player2Background":
                        break;
                    case "Player3Background":
                        break;
                    case "Player4Background":
                        break;
                }
                    

                currentKey = null;
            }
        }
    }
    public void ChangeKey(GameObject clicked)
    {
        currentKey = clicked;
    }
}
