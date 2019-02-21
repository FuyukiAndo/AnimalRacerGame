using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Isflak : MonoBehaviour {

    public int durability = 2;
    public float timeUntillBroken;
    public float floatSpeed;
    public LayerMask characterLayer;
    public float stunDuration;


    private int timeBeforeDestroyed;
    private float breakTime;
    private Rigidbody2D rb2d;
    private Vector2 hitDir;
    private IsflakSpawner isflakSpawner;
    private float speed;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        timeBeforeDestroyed = durability;
        if (transform.parent != null)
        {
            isflakSpawner = GetComponentInParent<IsflakSpawner>();
            speed = isflakSpawner.GetSpeed();
            durability = isflakSpawner.GetDurability();
        }
        else
        {
            speed = floatSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0)
        {
            WhichHitDir();
            MoveFlak();
        }

        DestroyIce();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        bool isOnLayer = characterLayer == (characterLayer | (1 << collision.gameObject.layer));
        TemporaryCodeDump player = collision.gameObject.GetComponent<TemporaryCodeDump>();
        if (isOnLayer && GetRelativeVelocityY() != 0.0f)
        {
            StartCoroutine(player.GetStunnedAndDestroy(stunDuration, gameObject));
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
        if (isOnLayer)
        {
            timeBeforeDestroyed--;
        }
        switch (collision.gameObject.tag)
        {
            case "Ground":
                Destroy(gameObject);
                break;
            case "Wall":
                speed = -speed;
                break;
        }
    }

    void WhichHitDir()
    {
        if(speed > 0)
        {
            hitDir = Vector2.right;
        }
        else if(speed < 0)
        {
            hitDir = Vector2.left;
        }
        else
        {
            hitDir = new Vector2(0, 0);
        }
    }
    float GetRelativeVelocityY()
    {
        return rb2d.velocity.y;
    }
    void MoveFlak()
    {
       Vector2 flak = new Vector2(speed, rb2d.velocity.y);
       rb2d.velocity = flak;
        Debug.Log(breakTime);
    }
    void DestroyIce()
    {
        Debug.Log(timeBeforeDestroyed);
        if (timeBeforeDestroyed <= 0)
        {
            breakTime += Time.deltaTime;
            if (breakTime > timeUntillBroken) {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 4)
        {
            rb2d.velocity = new Vector2(speed, 0);
            rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic;
        }
    }

}
