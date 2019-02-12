using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMenu : MonoBehaviour {

    private GameManager gameManager;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnDisable()
    {
        if (InformationManager.Instance.player1.playerIsActive)
        {
            gameManager.NumberOfPlayersActive -= 1;
        }
        if (InformationManager.Instance.player2.playerIsActive)
        {
            gameManager.NumberOfPlayersActive -= 1;
        }
        if (InformationManager.Instance.player3.playerIsActive)
        {
            gameManager.NumberOfPlayersActive -= 1;
        }
        if (InformationManager.Instance.player4.playerIsActive)
        {
            gameManager.NumberOfPlayersActive -= 1;
        }
    }
}
