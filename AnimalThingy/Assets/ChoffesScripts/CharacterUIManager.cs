using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerUI
{
    public GameObject playerUIObject;
    public Slider playerSlider;
    public GameObject playerCheckpoints;
    [HideInInspector] public float playerEnergy = 0f;
    public GameObject player;
}

public class CharacterUIManager : MonoBehaviour {


    public PlayerUI player1UI;
    public PlayerUI player2UI;
    public PlayerUI player3UI;
    public PlayerUI player4UI;
    private List<PlayerUI> playersUI;


    private void Start()
    {
        playersUI = new List<PlayerUI>();
        if (InformationManager.Instance.player1.playerIsActive)
        {
            playersUI.Add(player1UI);
        }
        if (InformationManager.Instance.player2.playerIsActive)
        {
            playersUI.Add(player2UI);
        }
        if (InformationManager.Instance.player3.playerIsActive)
        {
            playersUI.Add(player3UI);
        }
        if (InformationManager.Instance.player4.playerIsActive)
        {
            playersUI.Add(player4UI);
        }

        if (player1UI.player != null)
        {
            player1UI.playerUIObject.SetActive(true);
        }
        if (player2UI.player != null)
        {
            player2UI.playerUIObject.SetActive(true);
        }
        if (player3UI.player != null)
        {
            player3UI.playerUIObject.SetActive(true);
        }
        if (player4UI.player != null)
        {
            player4UI.playerUIObject.SetActive(true);
        }

    }

    private void Update()
    {
        foreach (PlayerUI player in playersUI)
        {
            UpdateEnergy(player);
            AddEnergy(player);
            UpdateSlider(player);
            SetCurrentCheckpointProgress(player);
        }
    }
    public void BindUIToPlayer1(GameObject playerCharacter)
    {
        player1UI.player = playerCharacter;
        player1UI.playerCheckpoints = GameObject.Find("Player1Checkpoints");
    }
    public void BindUIToPlayer2(GameObject playerCharacter)
    {
        player2UI.player = playerCharacter;
        player2UI.playerCheckpoints = GameObject.Find("Player2Checkpoints");
    }
    public void BindUIToPlayer3(GameObject playerCharacter)
    {
        player3UI.player = playerCharacter;
        player3UI.playerCheckpoints = GameObject.Find("Player3Checkpoints");
    }
    public void BindUIToPlayer4(GameObject playerCharacter)
    {
        player4UI.player = playerCharacter;
        player4UI.playerCheckpoints = GameObject.Find("Player4Checkpoints");
    }

    private void UpdateEnergy(PlayerUI playerUI)
    {
        if(playerUI.playerEnergy >= 10)
        {
            playerUI.playerEnergy = 10;
            //check input??
            //run abilityFunction in playerscript
        }

        //abilitykey check -> getComponent().keyisdownbool
    }
    private void AddEnergy(PlayerUI player)
    {
        if (player.playerEnergy <= 10)
            player.playerEnergy += Time.deltaTime; //player.player.GetComponent</*Filips playerability*/>().EnergyRegen;
    }
    private void UpdateSlider(PlayerUI player)
    {
        player.playerSlider.value = player.playerEnergy;
    }

    private void SetCurrentCheckpointProgress(PlayerUI player)
    {
        player.playerCheckpoints.GetComponent<TextMeshProUGUI>().text = player.player.GetComponent<CheckpointTracker>().CheckpointsPassed.Count + "/" + FindObjectOfType<GoalManager>().checksToPass.Length;
        //get checkpoint tracker
       // player.playerCheckpoints.text = /*playercheckpointtracker.currentprogress*/ + "/" + /*player.checkpointtracker.total checkpoints
    }
}
