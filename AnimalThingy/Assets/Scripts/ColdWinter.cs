using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdWinter : MonoBehaviour {

    public float timeForSurvival;

    private GameObject player;
   public float timeUntillDeath;

    private void Start()
    {
        player = gameObject.transform.parent.gameObject;
        timeUntillDeath = timeForSurvival;
    }
    // Update is called once per frame
    void Update () {
        timeUntillDeath -= Time.deltaTime*2;
        FreezingToDeath(); 
	}

   void FreezingToDeath()
    {
        if(timeUntillDeath < 0)
        {
            Destroy(player);
        }
    }
    public void WarmedUp()
    {
        timeUntillDeath = timeForSurvival;
    }

}
