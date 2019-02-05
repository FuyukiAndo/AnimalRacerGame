using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isflak : MonoBehaviour {

    public int durability = 2;
    public GameObject player;
    public float timeUntillBroken;

    private int timeBeforeDestroyed;
    private float breakTime;

    // Use this for initialization
    void Start()
    {
        timeBeforeDestroyed = durability;
    }

    // Update is called once per frame
    void Update()
    {
        destroyIce();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == player)
        {
            timeBeforeDestroyed--;
        }
            
            
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

}
