using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerUI
{
    public GameObject playerUIObject;
    public Slider playerSlider;
    public Text playerCheckpoints;
    public float playerEnergy = 0f;
    public GameObject player;
}

public class CharacterUIManager : MonoBehaviour {

    public float energyRegMultiplier;

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

        if(player1UI.player != null)
        {
            player1UI.playerUIObject = GameObject.Find("Player1UI");
            player1UI.playerUIObject.SetActive(true);
        }
        if (player2UI.player != null)
        {
            player2UI.playerUIObject = GameObject.Find("Player2UI");
            player2UI.playerUIObject.SetActive(true);
        }
        if (player3UI.player != null)
        {
            player3UI.playerUIObject = GameObject.Find("Player3UI");
            player3UI.playerUIObject.SetActive(true);
        }
        if (player4UI.player != null)
        {
            player4UI.playerUIObject = GameObject.Find("Player4UI");
            player4UI.playerUIObject.SetActive(true);
        }

    }

    private void Update()
    {
        foreach(PlayerUI player in playersUI)
        {
            UpdateEnergy(player);
            AddEnergy(player);
            UpdateSlider(player);
            SetCurrentCheckpointProgress(player);
        }

    }

    private void UpdateEnergy(PlayerUI player)
    {
        if(player.playerEnergy >= 100)
        {
            player.playerEnergy = 100;
            //check input??
            //run abilityFunction in playerscript
        }

        //abilitykey check -> getComponent().keyisdownbool
    }
    private void AddEnergy(PlayerUI player)
    {
        if(player.playerEnergy <= 100)
        player.playerEnergy += Time.deltaTime * energyRegMultiplier;
    }
    private void UpdateSlider(PlayerUI player)
    {
        player.playerSlider.value = player.playerEnergy;
    }

    private void SetCurrentCheckpointProgress(PlayerUI player)
    {
        //get checkpoint tracker
       // player.playerCheckpoints.text = /*playercheckpointtracker.currentprogress*/ + "/" + /*playercheckpointtracker.total checkpoints
    }

}
