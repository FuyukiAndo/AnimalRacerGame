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

    private void Start()
    {
        bubbleSpawner = GetComponentInParent<BubbleSpawner>();
        travelPosition = bubbleSpawner.GetTravelPosition();
    }
    // Update is called once per frame
    void Update () {
        currentPosition = transform.position;
        MoveToPosition();
	}

    public void MoveToPosition()
    {
       travelTime += Time.deltaTime / timeBeforeDestination;
       transform.position = Vector2.Lerp(currentPosition, travelPosition, travelTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isOnLayer = characterLayer == (characterLayer | (1 << collision.gameObject.layer));
        if (isOnLayer) {
            StartCoroutine(popBubble());
        }
    }
    IEnumerator popBubble()
    {
        yield return new WaitForSeconds(bubbleSpawner.GetPopTime());
        Destroy(gameObject);
    }
}
