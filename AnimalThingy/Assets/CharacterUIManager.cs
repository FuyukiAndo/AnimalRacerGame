using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerUI
{
    public Slider playerSlider;
    public Text playerCheckpoints;
    public float playerEnergy = 0f;
    public GameObject player;
}

public class CharacterUIManager : MonoBehaviour {

    private PlayerUI player1UI;
    private PlayerUI player2UI;
    private PlayerUI player3UI;
    private PlayerUI player4UI;
    private List<PlayerUI> playersUI;


    private void Start()
    {
        playersUI = new List<PlayerUI>();

        if (InformationManager.Instance.player1.playerIsActive)
        {
            player1UI.player = GameObject.Find("Player1");
            playersUI.Add(player1UI);

        }
        if (InformationManager.Instance.player2.playerIsActive)
        {
            player2UI.player = GameObject.Find("Player2");
            playersUI.Add(player2UI);
        }
        if (InformationManager.Instance.player3.playerIsActive)
        {
            player3UI.player = GameObject.Find("Player3");
            playersUI.Add(player3UI);
        }
        if (InformationManager.Instance.player4.playerIsActive)
        {
            player4UI.player = GameObject.Find("Player4");
            playersUI.Add(player4UI);
        }
    }

    private void Update()
    {
        foreach(PlayerUI player in playersUI)
        {
            UpdateEnergy(player);
            AddEnergy(player);
            UpdateSlider(player);
        }

    }

    private void UpdateEnergy(PlayerUI player)
    {
        if(player.playerEnergy > 100)
        {
            player.playerEnergy = 100;
        }
        //abilitykey check -> getComponent().keyisdownbool
    }
    private void AddEnergy(PlayerUI player)
    {
        if(player.playerEnergy <= 100)
        player.playerEnergy += Time.deltaTime;
    }
    private void UpdateSlider(PlayerUI player)
    {
        player.playerSlider.value = player.playerEnergy;
    }
}
