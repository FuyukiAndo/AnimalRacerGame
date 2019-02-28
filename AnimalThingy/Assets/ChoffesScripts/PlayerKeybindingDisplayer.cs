using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerKeybindingDisplayer : MonoBehaviour {
    public GameObject leftKeyButton;
    public GameObject rightKeyButton;
    public GameObject jumpKeyButton;
    public GameObject abilityKeyButton;

    private void OnEnable()
    {
        switch (leftKeyButton.name)
        {
            case "Player1LeftButton":
                leftKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player1Keys.left.ToString();
                break;
            case "Player2LeftButton":
                leftKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player2Keys.left.ToString();
                break;
            case "Player3LeftButton":
                leftKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player3Keys.left.ToString();
                break;
            case "Player4LeftButton":
                leftKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player4Keys.left.ToString();
                break;
        }
        switch (rightKeyButton.name)
        {
            case "Player1RightButton":
                rightKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player1Keys.right.ToString();
                break;
            case "Player2RightButton":
                rightKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player2Keys.right.ToString();
                break;
            case "Player3RightButton":
                rightKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player3Keys.right.ToString();
                break;
            case "Player4RightButton":
                rightKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player4Keys.right.ToString();
                break;
        }
        switch (jumpKeyButton.name)
        {
            case "Player1JumpButton":
                jumpKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player1Keys.jump.ToString();
                break;
            case "Player2JumpButton":
                jumpKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player2Keys.jump.ToString();
                break;
            case "Player3JumpButton":
                jumpKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player3Keys.jump.ToString();
                break;
            case "Player4JumpButton":
                jumpKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player4Keys.jump.ToString();
                break;
        }
        switch (abilityKeyButton.name)
        {
            case "Player1AbilityButton":
                abilityKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player1Keys.ability.ToString();
                break;
            case "Player2AbilityButton":
                abilityKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player2Keys.ability.ToString();
                break;
            case "Player3AbilityButton":
                abilityKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player3Keys.ability.ToString();
                break;
            case "Player4AbilityButton":
                abilityKeyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KeybindingsManager.Instance.Player4Keys.ability.ToString();
                break;
        }
    }
}
