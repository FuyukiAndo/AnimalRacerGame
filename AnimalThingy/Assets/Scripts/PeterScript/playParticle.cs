using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playParticle : MonoBehaviour {

    public Object soundObject;
    private ParticleSystem system;
    [SerializeField]
    private AudioEffectController effectController;

    private void Start()
    {
      system = GetComponent<ParticleSystem>();
      effectController = GetComponent<AudioEffectController>();
      system.Play();
      effectController.PlayAudioOneShot();
    }
    private void Update()
    {
        if (!system.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
