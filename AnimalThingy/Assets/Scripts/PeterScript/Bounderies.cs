using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounderies : MonoBehaviour {

    private BoxCollider2D bc2d;
    private Collider2D collider2d;
    private List<GameObject> checkpointPositions = new List<GameObject>();

    private void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        
        foreach(var checkpoint in FindObjectsOfType<Checkpoint>())
        {
            checkpointPositions.Add(checkpoint.gameObject);
        }   
    }
    void Update () {
        collider2d = Physics2D.OverlapBox(transform.position,bc2d.bounds.size,0);
		if (collider2d == this.GetComponent<Collider2D>()) return;

        if (checkpointPositions.Count <= 0 || collider2d.gameObject.GetComponent<CheckpointTracker>().CheckpointsPassed.Count <= 0 )
        {
            collider2d.gameObject.transform.position = StartManager.Instance.spawnPos1.spawnPos.transform.position;
        }
        for (int i = 0; i < checkpointPositions.Count; i++)
        {
            CheckpointTracker checkpointTracker = collider2d.gameObject.GetComponent<CheckpointTracker>();
            int index = checkpointTracker.CheckpointsPassed[checkpointTracker.CheckpointsPassed.Count - 1];
            if (checkpointPositions[i].GetComponent<Checkpoint>().Index == index)
            {
                collider2d.gameObject.transform.position = checkpointPositions[i].transform.position;
            }
        }
	}
}
