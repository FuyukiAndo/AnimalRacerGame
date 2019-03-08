using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class PlayerKeybindings
{
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode ability;
}

public class KeybindingsManager : MonoBehaviour {

    public static KeybindingsManager Instance { get; private set; }

    public PlayerKeybindings Player1Keys;
    public PlayerKeybindings Player2Keys;
    public PlayerKeybindings Player3Keys;
    public PlayerKeybindings Player4Keys;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Player1Keys = new PlayerKeybindings();
        Player2Keys = new PlayerKeybindings();
        Player3Keys = new PlayerKeybindings();
        Player4Keys = new PlayerKeybindings();
    }

    private void Start()
    {
        Player1Keys.left = KeyCode.A;
        Player1Keys.right = KeyCode.D;
        Player1Keys.jump = KeyCode.W;
        Player1Keys.ability = KeyCode.E;

        Player2Keys.left = KeyCode.LeftArrow;
        Player2Keys.right = KeyCode.RightArrow;
        Player2Keys.jump = KeyCode.UpArrow;
        Player2Keys.ability = KeyCode.RightControl;

        Player3Keys.left = KeyCode.J;
        Player3Keys.right = KeyCode.L;
        Player3Keys.jump = KeyCode.I;
        Player3Keys.ability = KeyCode.O;

        Player4Keys.left = KeyCode.Keypad4;
        Player4Keys.right = KeyCode.Keypad6;
        Player4Keys.jump = KeyCode.Keypad8;
        Player4Keys.ability = KeyCode.Keypad9;
    }
}
