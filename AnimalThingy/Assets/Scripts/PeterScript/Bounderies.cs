using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bounderies : MonoBehaviour {

    private BoxCollider2D bc2d;
    private Collider2D collider2d;
    private List<GameObject> checkpointPositions = new List<GameObject>();
	private SpeechBubble playerSpeech1, playerSpeech2, playerSpeech3, playerSpeech4;

	private void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        
        foreach(var checkpoint in FindObjectsOfType<Checkpoint>())
        {
            checkpointPositions.Add(checkpoint.gameObject);
        }
		playerSpeech1 = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == "Player1").FirstOrDefault();
		playerSpeech2 = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == "Player2").FirstOrDefault();
		playerSpeech3 = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == "Player3").FirstOrDefault();
		playerSpeech4 = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == "Player4").FirstOrDefault();
	}
    void Update () {
        collider2d = Physics2D.OverlapBox(transform.position,bc2d.bounds.size,0);
        Physics2D.IgnoreCollision(collider2d, bc2d);

        var checkpointTracker = collider2d.gameObject.GetComponent<CheckpointTracker>();

        if (!checkpointTracker) return;
        if (checkpointPositions.Count <= 0 || checkpointTracker.CheckpointsPassed.Count <= 0 )
        {
            switch (checkpointTracker.name)
            {
                case "Player1":
					playerSpeech1.SetSpeechActive(SpeechType.respawn, checkpointTracker.GetComponent<PlayerInput>().playerCharacterType);
					break;
				case "Player2":
					playerSpeech2.SetSpeechActive(SpeechType.respawn, checkpointTracker.GetComponent<PlayerInput>().playerCharacterType);
					break;
				case "Player3":
					playerSpeech2.SetSpeechActive(SpeechType.respawn, checkpointTracker.GetComponent<PlayerInput>().playerCharacterType);
					break;
				case "Player4":
					playerSpeech4.SetSpeechActive(SpeechType.respawn, checkpointTracker.GetComponent<PlayerInput>().playerCharacterType);
					break;
			}
            collider2d.gameObject.transform.position = StartManager.Instance.spawnPos1.spawnPos.transform.position;
        }
        for (int i = 0; i < checkpointPositions.Count; i++)
        {
            int index = checkpointTracker.CheckpointsPassed[checkpointTracker.CheckpointsPassed.Count - 1];
            if (checkpointPositions[i].GetComponent<Checkpoint>().Index == index)
            {
				switch (checkpointTracker.name)
				{
					case "Player1":
						playerSpeech1.SetSpeechActive(SpeechType.respawn, checkpointTracker.GetComponent<PlayerInput>().playerCharacterType);
						break;
					case "Player2":
						playerSpeech2.SetSpeechActive(SpeechType.respawn, checkpointTracker.GetComponent<PlayerInput>().playerCharacterType);
						break;
					case "Player3":
						playerSpeech2.SetSpeechActive(SpeechType.respawn, checkpointTracker.GetComponent<PlayerInput>().playerCharacterType);
						break;
					case "Player4":
						playerSpeech4.SetSpeechActive(SpeechType.respawn, checkpointTracker.GetComponent<PlayerInput>().playerCharacterType);
						break;
				}
                collider2d.gameObject.transform.position = checkpointPositions[i].transform.position;
            }
        }
	}
}
