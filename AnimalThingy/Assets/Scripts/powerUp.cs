using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{

    public float speedChange;

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SendMessage("SpeedChange", speedChange);
    }
}
