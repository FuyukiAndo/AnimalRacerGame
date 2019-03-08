using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour {

    public GameObject albatrossPrefab;
    public GameObject pigPrefab;
    public GameObject monkeyPrefab;
    public GameObject pinguinPrefab;

    public VideoClip albatrossClip;
    public VideoClip pigClip;
    public VideoClip monkeyClip;
    public VideoClip pinguinClip;

    public VideoPlayer vp;

    private void Start()
    {
        vp.source = VideoSource.VideoClip;
    }

    public void SetVictoryVideo(string playerName)
    {
        Debug.Log(playerName);
        switch (playerName)
        {
            case "Player1":
                if(InformationManager.Instance.player1.character == albatrossPrefab)
                {
                    vp.clip = albatrossClip;

                }
                if (InformationManager.Instance.player1.character == pigPrefab)
                {
                    vp.clip = pigClip;
                }
                if (InformationManager.Instance.player1.character == monkeyPrefab)
                {
                    vp.clip = monkeyClip;
                }
                if (InformationManager.Instance.player1.character == pinguinPrefab)
                {
                    vp.clip = pinguinClip;
                }
                break;
            case "Player2":
                if (InformationManager.Instance.player2.character == albatrossPrefab)
                {
                    vp.clip = albatrossClip;
                }
                if (InformationManager.Instance.player2.character == pigPrefab)
                {
                    vp.clip = pigClip;
                }
                if (InformationManager.Instance.player2.character == monkeyPrefab)
                {
                    vp.clip = monkeyClip;
                }
                if (InformationManager.Instance.player2.character == pinguinPrefab)
                {
                    vp.clip = pinguinClip;
                }
                break;
            case "Player3":
                if (InformationManager.Instance.player3.character == albatrossPrefab)
                {
                    vp.clip = albatrossClip;
                }
                if (InformationManager.Instance.player3.character == pigPrefab)
                {
                    vp.clip = pigClip;
                }
                if (InformationManager.Instance.player3.character == monkeyPrefab)
                {
                    vp.clip = monkeyClip;
                }
                if (InformationManager.Instance.player3.character == pinguinPrefab)
                {
                    vp.clip = pinguinClip;
                }
                break;
            case "Player4":
                if (InformationManager.Instance.player4.character == albatrossPrefab)
                {
                    vp.clip = albatrossClip;
                }
                if (InformationManager.Instance.player4.character == pigPrefab)
                {
                    vp.clip = pigClip;
                }
                if (InformationManager.Instance.player4.character == monkeyPrefab)
                {
                    vp.clip = monkeyClip;
                }
                if (InformationManager.Instance.player4.character == pinguinPrefab)
                {
                    vp.clip = pinguinClip;
                }
                break;

        }
    }
}
