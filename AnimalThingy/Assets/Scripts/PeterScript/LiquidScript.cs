using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LiquidScript : MonoBehaviour
{

    private BoxCollider2D bc2d;
    private Collider2D[] collider2d;
    private List<GameObject> checkpointPositions = new List<GameObject>();
    private PlayerInput playerInput;
    private SpeechBubble playerSpeech1, playerSpeech2, playerSpeech3, playerSpeech4;

    private void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();

        foreach (var checkpoint in FindObjectsOfType<Checkpoint>())
        {
            checkpointPositions.Add(checkpoint.gameObject);
        }
        playerSpeech1 = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == "Player1").FirstOrDefault();
        playerSpeech2 = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == "Player2").FirstOrDefault();
        playerSpeech3 = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == "Player3").FirstOrDefault();
        playerSpeech4 = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == "Player4").FirstOrDefault();
    }

    void Update()
    {
        collider2d = Physics2D.OverlapBoxAll(transform.position, bc2d.bounds.size, 0,LayerMask.GetMask("Player"));
        CollisionDetect();
    }

    void CollisionDetect()
    {
            for(int i = 0; i < collider2d.Length; i++)
            {
                if (collider2d[i].gameObject.layer == 8)
                {
                    playerInput = collider2d[i].gameObject.GetComponent<PlayerInput>();
                }
                if (!playerInput) break;
                
                if (playerInput.playerCharacterType == PlayerCharacterType.PlayerPenguin) break;

                if (collider2d[i].gameObject.GetComponent<CheckpointTracker>().CheckpointsPassed.Count <= 0 || checkpointPositions.Count <= 0)
                {
                    switch (collider2d[i].gameObject.name)
                    {
                        case "Player1":
                            playerSpeech1.SetSpeechActive(SpeechType.respawn, collider2d[i].gameObject.GetComponent<PlayerInput>().playerCharacterType);
                            break;
                        case "Player2":
                            playerSpeech2.SetSpeechActive(SpeechType.respawn, collider2d[i].gameObject.GetComponent<PlayerInput>().playerCharacterType);
                            break;
                        case "Player3":
                            playerSpeech2.SetSpeechActive(SpeechType.respawn, collider2d[i].gameObject.GetComponent<PlayerInput>().playerCharacterType);
                            break;
                        case "Player4":
                            playerSpeech4.SetSpeechActive(SpeechType.respawn, collider2d[i].gameObject.GetComponent<PlayerInput>().playerCharacterType);
                            break;
                    }
                    collider2d[i].gameObject.transform.position = StartManager.Instance.spawnPos1.spawnPos.transform.position;
                }

                for (int j = 0; j < checkpointPositions.Count; j++)
                {
                    CheckpointTracker checkpointTracker = collider2d[i].gameObject.GetComponent<CheckpointTracker>();
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
                        collider2d[i].gameObject.transform.position = checkpointPositions[i].transform.position;
                    }
                
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector2(transform.position.x - bc2d.offset.x, transform.position.y - bc2d.offset.y), bc2d.bounds.size);
    }
}
