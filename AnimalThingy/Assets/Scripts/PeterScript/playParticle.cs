using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class playParticle : MonoBehaviour {

    public Object soundObject;
    private ParticleSystem system;
    [SerializeField]
    private AudioOneshotPlayer OneshotPlayer;

    private void Start()
    {
      system = GetComponent<ParticleSystem>();
      OneshotPlayer = GetComponent<AudioOneshotPlayer>();
      system.Play();
      OneshotPlayer.PlayAudioOneShot();
    }
    private void Update()
    {
        if (!system.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
