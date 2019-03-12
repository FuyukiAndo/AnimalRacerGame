using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryCodeDump : MonoBehaviour {

    //public IEnumerator SpeedChange(float boostChangeAmount, float boostDuration, GameObject speedObject)
    //{
    //    speed = speed + boostChangeAmount;
    //    yield return new WaitForSeconds(boostDuration);
    //    speed = originalSpeed;
    //    Destroy(speedObject);
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    bool isOnLayer = waterLayer == (waterLayer | (1 << collision.gameObject.layer));
    //    /*Mathf.Log(waterLayer.value, 2) == collision.gameObject.layer*/
    //    /*(waterLayer | (1 << collision.gameObject.layer)) - Kollar bitmaskens position på collisionen och jämför den med bitmasken*/

    //    if (isOnLayer && pengvin == false)
    //    {
    //        GetKilled();
    //    }
    //}
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Bonefire")
    //    {
    //        coldWinter.WarmedUp();
    //    }
    //}
    //public IEnumerator GetStunnedAndDestroy(float stunDuration, GameObject stunObject)
    //{
    //    isStunned = true;
    //    yield return new WaitForSeconds(stunDuration);
    //    isStunned = false;
    //    Destroy(stunObject);
    //}
    //public IEnumerator GetStunned(float stunDurtation)
    //{
    //    Debug.Log(stunDurtation);
    //    isStunned = true;
    //    yield return new WaitForSeconds(stunDurtation);
    //    isStunned = false;
    //}
    //public void GetKilled()
    //{
    //    Destroy(gameObject);
    //}
}
