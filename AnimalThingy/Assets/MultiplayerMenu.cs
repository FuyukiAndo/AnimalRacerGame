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
        PlayersStats.Instance.players.Clear();
        if (PlayersStats.Instance.player1.playerIsActive)
        {
            gameManager.NumberOfPlayersActive -= 1;
        }
        if (PlayersStats.Instance.player2.playerIsActive)
        {
            gameManager.NumberOfPlayersActive -= 1;
        }
        if (PlayersStats.Instance.player3.playerIsActive)
        {
            gameManager.NumberOfPlayersActive -= 1;
        }
        if (PlayersStats.Instance.player4.playerIsActive)
        {
            gameManager.NumberOfPlayersActive -= 1;
        }
    }
}
