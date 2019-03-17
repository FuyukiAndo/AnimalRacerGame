using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTrajectory : MonoBehaviour {

    [Header("Player Interaction")]
    public float stunDuration = 1;

    [Header("Trajectory Attributs")]
    public float speed = 2;
    public LayerMask terrainTypesLayer;
    public ParticleSystem destroyParticle;

    protected Rigidbody2D rb2d;
    protected float startFalling;
    protected bool isOnLayer;

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerInput player = collision.gameObject.GetComponent<PlayerInput>();
        bool isOnLayer = terrainTypesLayer == (terrainTypesLayer | (1 << collision.gameObject.layer));
        if (player != null)
        {
            player.stunDurationLeft = stunDuration;
            Destroy(gameObject);
            
        }
        if(isOnLayer)
        {
            Instantiate(destroyParticle, transform.position, destroyParticle.gameObject.transform.rotation);
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
