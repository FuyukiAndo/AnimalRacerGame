using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITimerScript : MonoBehaviour {

    public TextMeshProUGUI timer;

    private void Update()
    {
        int temp = (int)GoalManager.Instance.timeBeforeAutoPlacements;
        timer.text = temp.ToString();
        
    }
}
