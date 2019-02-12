using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChanger : MonoBehaviour
{

    public float speedChangeAmount;
    public float speedDuration;

    void OnTriggerEnter2D(Collider2D collision)
    {
        PetersPlayerController player = collision.gameObject.GetComponent<PetersPlayerController>();
        if (player != null)
        {
           StartCoroutine(player.SpeedChange(speedChangeAmount, speedDuration, gameObject));
           GetComponent<MeshRenderer>().enabled = false;
           GetComponent<Collider2D>().enabled = false;
        }
    }
}
