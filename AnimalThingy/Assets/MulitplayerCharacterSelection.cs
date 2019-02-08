using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulitplayerCharacterSelection : MonoBehaviour {

    public GameObject characterSelectionChild;
    public GameObject joinButton;

    private void OnEnable()
    {
        characterSelectionChild.SetActive(false);
        joinButton.SetActive(true);
    }
}
