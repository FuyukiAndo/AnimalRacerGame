using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryCodeDump : MonoBehaviour {

    public LayerMask waterLayer;

    private PlayerController playerController;
    private ColdWinter coldWinter;
    private float originalSpeed;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        coldWinter = GetComponentInChildren<ColdWinter>();
        originalSpeed = playerController.movementSpeed;
    }

    public IEnumerator SpeedChange(float boostChangeAmount, float boostDuration, GameObject speedObject)
    {
        playerController.movementSpeed = playerController.movementSpeed + boostChangeAmount;
        yield return new WaitForSeconds(boostDuration);
        playerController.movementSpeed = originalSpeed;
        Destroy(speedObject);
    }

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1f, 1f), 0f);
        foreach (var collider in colliders)
        {
            bool isOnLayer = waterLayer == (waterLayer | (1 << collider.gameObject.layer));
            if (isOnLayer && playerController.playerType != PlayerType.playerPenguin)
            {
                GetKilled();
                return;
            }
            if (collider.gameObject.tag == "Bonefire")
            {
                coldWinter.WarmedUp();
            }
        }
    }
    public IEnumerator GetStunnedAndDestroy(float stunDuration, GameObject stunObject)
    {
        playerController.isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        playerController.isStunned = false;
        Destroy(stunObject);
    }
    public IEnumerator GetStunned(float stunDurtation)
    {
        Debug.Log(stunDurtation);
        playerController.isStunned = true;
        yield return new WaitForSeconds(stunDurtation);
        playerController.isStunned = false;
    }
    public void GetKilled()
    {
        Destroy(gameObject);
    }
}
