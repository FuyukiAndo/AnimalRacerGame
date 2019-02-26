using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


}
