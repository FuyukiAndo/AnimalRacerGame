using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TerrainSnowball : MonoBehaviour
{

    public float speed = 5.0f;
    public LayerMask terrainLayer, characterLayer;
    public float pushForce = 5.0f;
    public float stunDuration;
    public float maxPushBack;

    private bool gotHit = false;
    private Rigidbody2D rb2d;
    private Collider2D c2d;
    private Collider2D[] collision;
    private float dir;
    private float pushback;
    private float usedPushForce;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        c2d = GetComponent<Collider2D>();
        if (transform.parent != null)
        {
            speed = GetComponentInParent<SpawnTerrainSnowball>().getAcc;
            pushForce = GetComponentInParent<SpawnTerrainSnowball>().getForce;
            stunDuration = GetComponentInParent<SpawnTerrainSnowball>().getStunDuration;
            maxPushBack = GetComponentInParent<SpawnTerrainSnowball>().getMaxPushBack;
        }
        if (speed > 0)
        {
            dir = gameObject.transform.lossyScale.x;
        }
        else
        {
            dir = -gameObject.transform.lossyScale.x;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gotHit) return;
        Debug.DrawRay(transform.position, Vector2.right * (dir), Color.red);
        HitWall();
        HitPlayer();
        Vector2 movement = new Vector2(speed, 0);
        rb2d.AddForce(movement);
    }
    public void HitPlayer()
    {
        Debug.Log(rb2d.velocity.x);
        collision = Physics2D.OverlapBoxAll(transform.position, c2d.bounds.size, 0.0f);

        foreach (var collider in collision)
        {
            if (collider.gameObject.GetComponent<Spike>())
            {

                Physics2D.IgnoreCollision(c2d, collider.gameObject.GetComponent<Collider2D>());

            }
            bool isOnLayer = characterLayer == (characterLayer | (1 << collider.gameObject.layer));
            if (isOnLayer)
            {
                if (maxPushBack < Mathf.Abs(rb2d.velocity.x * pushForce))
                {
                    pushback = maxPushBack;
                }
                else
                {
                    pushback = rb2d.velocity.x * pushForce;
                }
                if (maxPushBack < pushForce)
                {
                    usedPushForce = maxPushBack;
                }
                else
                {
                    usedPushForce = pushForce;
                }
                Debug.Log(pushback);
                Physics2D.IgnoreCollision(collider, c2d);
                collider.GetComponent<PlayerInput>().isStunned = true;
                collider.GetComponent<PlayerInput>().stunDurationTimer = stunDuration;
                if (transform.position.x < collider.transform.position.x)
                {

                    collider.GetComponent<PlayerController>().movement.y += usedPushForce;
                    collider.GetComponent<PlayerController>().movement.x += pushback;
                    gotHit = true;

                }
                else
                {
                    collider.GetComponent<PlayerController>().movement.y += usedPushForce;
                    collider.GetComponent<PlayerController>().movement.x -= pushback;
                    gotHit = true;
                }
            }
        }
    }
    public void HitWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, dir, terrainLayer);
        if (hit.collider != null)
        {
            dir = -dir;
            speed = -speed;
        }
    }
}