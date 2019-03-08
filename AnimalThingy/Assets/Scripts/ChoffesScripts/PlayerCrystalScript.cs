using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrystalScript : MonoBehaviour {

    public Material redCrystalMaterial;
    public Material blueCrystalMaterial;
    public Material greenCrystalMaterial;
    public Material yellowCrystalMaterial;

    private void Start()
    {
        switch (transform.parent.name)
        {
            case "Player1":
                GetComponent<MeshRenderer>().material = redCrystalMaterial;
                break;
            case "Player2":
                GetComponent<MeshRenderer>().material = blueCrystalMaterial;
                break;
            case "Player3":
                GetComponent<MeshRenderer>().material = greenCrystalMaterial;
                break;
            case "Player4":
                GetComponent<MeshRenderer>().material = yellowCrystalMaterial;
                break;
        }

    }
}
