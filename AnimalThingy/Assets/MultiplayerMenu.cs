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
        gameManager.players.Clear();
        if (gameManager.player1.playerIsActive)
        {
            gameManager.SetPlayerInactive(1);
        }
        if (gameManager.player2.playerIsActive)
        {
            gameManager.SetPlayerInactive(2);
        }
        if (gameManager.player3.playerIsActive)
        {
            gameManager.SetPlayerInactive(3);
        }
        if (gameManager.player4.playerIsActive)
        {
            gameManager.SetPlayerInactive(4);
        }
    }
}
