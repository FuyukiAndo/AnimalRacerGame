﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    public float timeBeforeDestination;
    public LayerMask characterLayer;

    private float travelTime;
    private Vector2 currentPosition;
    private Vector2 travelPosition;
    private BubbleSpawner bubbleSpawner;
    private BoxCollider2D bc2d;
    private Collider2D[] colliders;
    private int newColliderCount;
    private float floatSpeed;
    public bool popOnDestination;

    private void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        if (transform.parent != null)
        {
            bubbleSpawner = GetComponentInParent<BubbleSpawner>();
            popOnDestination = bubbleSpawner.getPopDesition;
            travelPosition = bubbleSpawner.getTravelPosition;
            timeBeforeDestination = bubbleSpawner.getDestinationTime;
        }
    }
    // Update is called once per frame
    void Update () {
        currentPosition = transform.position;
        MoveToPosition();
        CollisionCheck();
	}

    private void MoveToPosition()
    {
        travelTime += Time.deltaTime / timeBeforeDestination ;
        if (popOnDestination && (Vector2)transform.position == travelPosition)
        {
            Destroy(gameObject);
            transform.position = Vector2.Lerp(currentPosition, travelPosition, travelTime);
        }
        else
        {
            transform.position = Vector2.Lerp(currentPosition, travelPosition, travelTime);
        }
    }

    private void CollisionCheck()
    {
        var oldColliderCount = newColliderCount;

        colliders = Physics2D.OverlapBoxAll(transform.position, bc2d.bounds.size, 0.0f);

        newColliderCount = colliders.Length;
        if (newColliderCount > oldColliderCount)
        {
            foreach (var collision in colliders)
            {
                bool isOnLayer = characterLayer == (characterLayer | (1 << collision.gameObject.layer));
                if (isOnLayer)
                {
                    StartCoroutine(PopBubble());
                }
            }
        }
    } 
    IEnumerator PopBubble()
    {
        yield return new WaitForSeconds(bubbleSpawner.getPopTime);
        Destroy(gameObject);
    }

}
