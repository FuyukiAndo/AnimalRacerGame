using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlyingTrajectory : MonoBehaviour {

    [Header("Player Interaction")]
    public float stunDuration = 1;

    [Header("Trajectory Attributs")]
    public float speed = 2;
    public LayerMask terrainTypesLayer;

    protected Rigidbody2D rb2d;
    protected float startFalling;
    protected bool isOnLayer;
    protected SpeechBubble playerSpeech1, playerSpeech2, playerSpeech3, playerSpeech4;

    protected void Awake()
    {
        playerSpeech1 = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == "Player1").FirstOrDefault();
        playerSpeech2 = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == "Player2").FirstOrDefault();
        playerSpeech3 = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == "Player3").FirstOrDefault();
        playerSpeech4 = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == "Player4").FirstOrDefault();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerInput player = collision.gameObject.GetComponent<PlayerInput>();
        bool isOnLayer = terrainTypesLayer == (terrainTypesLayer | (1 << collision.gameObject.layer));
        if (player != null)
        {
            player.isStunned = true;
            player.stunDurationTimer = stunDuration;
            Destroy(gameObject);
            switch (player.name)
            {
                case "Player1":
                    playerSpeech1.SetSpeechActive(SpeechType.stun, player.playerCharacterType);
                    break;
                case "Player2":
                    playerSpeech2.SetSpeechActive(SpeechType.stun, player.playerCharacterType);
                    break;
                case "Player3":
                    playerSpeech3.SetSpeechActive(SpeechType.stun, player.playerCharacterType);
                    break;
                case "Player4":
                    playerSpeech4.SetSpeechActive(SpeechType.stun, player.playerCharacterType);
                    break;
            }
        }
        if(isOnLayer)
        {
            Destroy(gameObject);
        }
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        bool isOnLayer = terrainTypesLayer == (terrainTypesLayer | (1 << collision.gameObject.layer));
        if (isOnLayer)
        {
            Destroy(gameObject);
        }
    }
}
