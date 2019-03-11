using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMenu : MonoBehaviour {

    private MultiplayerManager multiplayerManager;


    private void Awake()
    {
        multiplayerManager = FindObjectOfType<MultiplayerManager>();
    }

    private void OnDisable()
    {
        if (InformationManager.Instance.player1.playerIsActive)
        {
            multiplayerManager.NumberOfPlayersActive -= 1;
        }
        if (InformationManager.Instance.player2.playerIsActive)
        {
            multiplayerManager.NumberOfPlayersActive -= 1;
        }
        if (InformationManager.Instance.player3.playerIsActive)
        {
            multiplayerManager.NumberOfPlayersActive -= 1;
        }
        if (InformationManager.Instance.player4.playerIsActive)
        {
            multiplayerManager.NumberOfPlayersActive -= 1;
        }
    }
}
