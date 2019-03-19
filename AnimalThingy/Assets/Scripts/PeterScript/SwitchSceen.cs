using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SwitchSceen : MonoBehaviour {

    private VideoPlayer vp;
    public string scene;
	// Use this for initialization
	void Start () {
        vp = GetComponent<VideoPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
		if(vp.frame >= (long)vp.frameCount)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
	}
}
