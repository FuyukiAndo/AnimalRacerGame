using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTrajectory : MonoBehaviour {

    [Header("Player Interaction")]
    public float stunDuration;

    [Header("Trajectory Attributs")]
    public float speed;
    public LayerMask terrainTypesLayer;

    protected Rigidbody2D rb2d;
    protected float startFalling;
    protected bool isOnLayer;

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            //StartCoroutine(player.GetStunnedAndDestroy(stunDuration, gameObject));
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
        if(isOnLayer)
        {
            Destroy(gameObject);
        }
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        bool isOnLayer = terrainTypesLayer == (terrainTypesLayer | (1 << collision.gameObject.layer));
        if (isOnLayer)
        {
            Destroy(gameObject);
        }
    }
}
