using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Isflak : MonoBehaviour {

    public int durability = 2;
    public GameObject player;
    public float timeUntillBroken;
    public float floatSpeed;
    

    private int timeBeforeDestroyed;
    private float breakTime;
    private Rigidbody2D rb2d;
    private Vector2 hitDir;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        timeBeforeDestroyed = durability;

    }

    // Update is called once per frame
    void Update()
    {
        whichHitDir();
        moveFlak();
        destroyIce();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == player.tag)
        {
            timeBeforeDestroyed--;
        }
        if (collision.gameObject.layer == 8)
        {
            floatSpeed = -floatSpeed;
        }
    }
    void whichHitDir()
    {
        if(floatSpeed > 0)
        {
            hitDir = Vector2.right;
        }
        else if(floatSpeed < 0)
        {
            hitDir = Vector2.left;
        }
        else
        {
            hitDir = new Vector2(0, 0);
        }
    }
    void moveFlak()
    {
       Vector2 flak = new Vector2(floatSpeed, rb2d.velocity.y);
       rb2d.velocity = flak;
    }
    void destroyIce()
    {
        if (timeBeforeDestroyed <= 0)
        {
            breakTime += Time.deltaTime;
            Debug.Log(breakTime);
            if (breakTime > timeUntillBroken) {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 4)
        {
            rb2d.velocity = new Vector2(floatSpeed, 0);
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
