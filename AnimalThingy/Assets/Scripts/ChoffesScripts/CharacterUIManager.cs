using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerUI
{
    public GameObject playerUIObject;
    public Image playerImage;
    public Slider playerSlider;
    public GameObject playerCheckpoints;
    [HideInInspector] public float playerEnergy = 0f;
    public GameObject player;
}

public class CharacterUIManager : MonoBehaviour {

    public Sprite albatrossImage;
    public GameObject albatrossPrefab;
    public Sprite pigImage;
    public GameObject pigPrefab;
    public Sprite pinguinImage;
    public GameObject pinguinPrefab;
    public Sprite monkeyImage;
    public GameObject monkeyPrefab;

    public PlayerUI player1UI;
    public PlayerUI player2UI;
    public PlayerUI player3UI;
    public PlayerUI player4UI;
    public List<PlayerUI> playersUI;


    private void Start()
    {
        playersUI = new List<PlayerUI>();
        if (InformationManager.Instance.player1.playerIsActive)
        {
            playersUI.Add(player1UI);
            player1UI.playerUIObject.SetActive(true);
        }
        if (InformationManager.Instance.player2.playerIsActive)
        {
            playersUI.Add(player2UI);
            player2UI.playerUIObject.SetActive(true);
        }
        if (InformationManager.Instance.player3.playerIsActive)
        {
            playersUI.Add(player3UI);
            player3UI.playerUIObject.SetActive(true);
        }
        if (InformationManager.Instance.player4.playerIsActive)
        {
            playersUI.Add(player4UI);
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
        SetPlayerImage(player1UI);
    }
    public void BindUIToPlayer2(GameObject playerCharacter)
    {
        player2UI.player = playerCharacter;
        player2UI.playerCheckpoints = GameObject.Find("Player2Checkpoints");
        SetPlayerImage(player2UI);
    }
    public void BindUIToPlayer3(GameObject playerCharacter)
    {
        player3UI.player = playerCharacter;
        player3UI.playerCheckpoints = GameObject.Find("Player3Checkpoints");
        SetPlayerImage(player3UI);
    }
    public void BindUIToPlayer4(GameObject playerCharacter)
    {
        player4UI.player = playerCharacter;
        player4UI.playerCheckpoints = GameObject.Find("Player4Checkpoints");
        SetPlayerImage(player4UI);
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


    private void SetPlayerImage(PlayerUI playerUI)
    {
        switch (playerUI.player.name)
        {
            case "Player1":
                if (InformationManager.Instance.player1.character == albatrossPrefab)
                {
                    playerUI.playerImage.sprite = albatrossImage;
                }
                if (InformationManager.Instance.player1.character == monkeyPrefab)
                {
                    playerUI.playerImage.sprite = monkeyImage;
                }
                if (InformationManager.Instance.player1.character == pigPrefab)
                {
                    playerUI.playerImage.sprite = pigImage;
                }
                if (InformationManager.Instance.player1.character == pinguinPrefab)
                {
                    playerUI.playerImage.sprite = pinguinImage;
                }
                break;
            case "Player2":
                if (InformationManager.Instance.player2.character == albatrossPrefab)
                {
                    playerUI.playerImage.sprite = albatrossImage;
                }
                if (InformationManager.Instance.player2.character == monkeyPrefab)
                {
                    playerUI.playerImage.sprite = monkeyImage;
                }
                if (InformationManager.Instance.player2.character == pigPrefab)
                {
                    playerUI.playerImage.sprite = pigImage;
                }
                if (InformationManager.Instance.player2.character == pinguinPrefab)
                {
                    playerUI.playerImage.sprite = pinguinImage;
                }
                break;
            case "Player3":
                if (InformationManager.Instance.player3.character == albatrossPrefab)
                {
                    playerUI.playerImage.sprite = albatrossImage;
                }
                if (InformationManager.Instance.player3.character == monkeyPrefab)
                {
                    playerUI.playerImage.sprite = monkeyImage;
                }
                if (InformationManager.Instance.player3.character == pigPrefab)
                {
                    playerUI.playerImage.sprite = pigImage;
                }
                if (InformationManager.Instance.player3.character == pinguinPrefab)
                {
                    playerUI.playerImage.sprite = pinguinImage;
                }
                break;
            case "Player4":
                if (InformationManager.Instance.player4.character == albatrossPrefab)
                {
                    playerUI.playerImage.sprite = albatrossImage;
                }
                if (InformationManager.Instance.player4.character == monkeyPrefab)
                {
                    playerUI.playerImage.sprite = monkeyImage;
                }
                if (InformationManager.Instance.player4.character == pigPrefab)
                {
                    playerUI.playerImage.sprite = pigImage;
                }
                if (InformationManager.Instance.player4.character == pinguinPrefab)
                {
                    playerUI.playerImage.sprite = pinguinImage;
                }
                break;
        }
        
    }
    private void SetCurrentCheckpointProgress(PlayerUI player)
    {
        player.playerCheckpoints.GetComponent<TextMeshProUGUI>().text = player.player.GetComponent<CheckpointTracker>().CheckpointsPassed.Count + "/" + GoalManager.Instance.checksToPass.Length;
        //get checkpoint tracker
       // player.playerCheckpoints.text = /*playercheckpointtracker.currentprogress*/ + "/" + /*player.checkpointtracker.total checkpoints
    }
}
