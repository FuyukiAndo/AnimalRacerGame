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

                switch (currentKey.transform.name)
                {
                    case "Player1LeftButton":
                        KeybindingsManager.Instance.Player1Keys.left = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;
                    case "Player2LeftButton":
                        KeybindingsManager.Instance.Player2Keys.left = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;
                    case "Player3LeftButton":
                        KeybindingsManager.Instance.Player3Keys.left = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;
                    case "Player4LeftButton":
                        KeybindingsManager.Instance.Player4Keys.left = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;


                    case "Player1RightButton":
                        KeybindingsManager.Instance.Player1Keys.right = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;
                    case "Player2RightButton":
                        KeybindingsManager.Instance.Player2Keys.right = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;
                    case "Player3RightButton":
                        KeybindingsManager.Instance.Player3Keys.right = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;
                    case "Player4RightButton":
                        KeybindingsManager.Instance.Player4Keys.right = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;


                    case "Player1JumpButton":
                        KeybindingsManager.Instance.Player1Keys.jump = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;
                    case "Player2JumpButton":
                        KeybindingsManager.Instance.Player2Keys.jump = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;
                    case "Player3JumpButton":
                        KeybindingsManager.Instance.Player3Keys.jump = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;
                    case "Player4JumpButton":
                        KeybindingsManager.Instance.Player4Keys.jump = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;


                    case "Player1AbilityButton":
                        KeybindingsManager.Instance.Player1Keys.ability = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;
                    case "Player2AbilityButton":
                        KeybindingsManager.Instance.Player2Keys.ability = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;
                    case "Player3AbilityButton":
                        KeybindingsManager.Instance.Player3Keys.ability = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        break;
                    case "Player4AbilityButton":
                        KeybindingsManager.Instance.Player4Keys.ability = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
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
