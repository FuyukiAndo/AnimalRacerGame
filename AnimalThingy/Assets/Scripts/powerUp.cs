using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{

    public float speedChange;
    public float speedDuration;

    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
           StartCoroutine(player.SpeedChange(speedChange, speedDuration));
            Destroy(gameObject);
        }
    }
}
