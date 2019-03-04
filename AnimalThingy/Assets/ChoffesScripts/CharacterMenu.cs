using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BioCharacters
{
    public GameObject character;
    public GameObject bio;
}

public class CharacterMenu : MonoBehaviour {

    public BioCharacters[] bioCharacters;

    public void DisableCurrentMenu()
    {
        for (int i = 0; i < bioCharacters.Length; i++)
        {
            bioCharacters[i].bio.SetActive(false);
        }
    }
}
