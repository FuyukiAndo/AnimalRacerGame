using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTrajectory : MonoBehaviour {

    [Header("Player Interaction")]
    public float stunDuration;

    [Header("Trajectory Attributs")]
    public float speed;

    protected Rigidbody2D rb2d;
    protected float startFalling;

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        PetersPlayerController player = collision.gameObject.GetComponent<PetersPlayerController>();
        if (player != null)
        {
            StartCoroutine(player.GetStunned(stunDuration, gameObject));
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            Debug.Log("llo");
            Destroy(gameObject);
        }
    }
}
