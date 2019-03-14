using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChanger : MonoBehaviour
{

    public float speedChangeAmount;
    public float speedDuration;
    private Collider2D c2d;
    private Collider2D[] collision;

    private void Start()
    {
        c2d = GetComponent<Collider2D>();
        if (transform.parent != null)
        {
            if (gameObject.name == "GoodMushroom(Clone)")
            {
                speedChangeAmount = GetComponentInParent<SpeedChangerSpawner>().getGoodSpeed;
            }
            else
            {
                speedChangeAmount = GetComponentInParent<SpeedChangerSpawner>().getBadSpeed;
            }
        }
    }

    void CollisionEnter2D()
    {
        collision = Physics2D.OverlapBoxAll(transform.position, c2d.bounds.size, 0.0f);
        foreach (var collider in collision)
        {
            PlayerController player = collider.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                StartCoroutine(player.movementSpeedChanger(speedChangeAmount, speedDuration));
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
    private void Update()
    {
        CollisionEnter2D();
    }
}
