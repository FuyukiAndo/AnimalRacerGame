using System.Collections;
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

    private void Start()
    {
        bubbleSpawner = GetComponentInParent<BubbleSpawner>();
        travelPosition = bubbleSpawner.GetTravelPosition();
        bc2d = GetComponent<BoxCollider2D>();
    }
    // Update is called once per frame
    void Update () {
        currentPosition = transform.position;
        MoveToPosition();
        CollisionCheck();
	}

    private void MoveToPosition()
    {
       travelTime += Time.deltaTime / timeBeforeDestination;
       transform.position = Vector2.Lerp(currentPosition, travelPosition, travelTime);
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
        yield return new WaitForSeconds(bubbleSpawner.GetPopTime());
        Destroy(gameObject);
    }

}
