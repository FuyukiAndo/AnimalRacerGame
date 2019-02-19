using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;
using FMOD.Studio;

public class AudioEffectController : MonoBehaviour {

	[SerializeField] private bool playEffectAuto;

	public enum StartEvent
	{
		none, start, update, collisionEnter, colisionStay, collisionExit, triggerEnter, triggerStay, triggerExit, destroyed
	}
	public StartEvent startEvent;
	public enum StopEvent
	{
		none, start, update, collisionEnter, colisionStay, collisionExit, triggerEnter, triggerStay, triggerExit, destroyed
	}
	public StopEvent stopEvent;

	[SerializeField] private float AliveTime;
	[EventRef] [SerializeField] private string sfxPath;
	EventInstance sfxInstance;

	void Start()
	{
		if (playEffectAuto)
		{
			if (startEvent == StartEvent.start)
			{
				//Play audio
			}
		}
		else
		{
			startEvent = StartEvent.none;
		}
	}

	void Update()
	{
		if (playEffectAuto)
		{
			if (startEvent == StartEvent.update)
			{
				//Play audio
			}
		}
		else
		{
			startEvent = StartEvent.none;
		}
	}

	public void PlayAudioOneShot(bool attached)
	{
		if (attached)
		{
			RuntimeManager.PlayOneShotAttached(sfxPath, gameObject);
		}
		else
		{
			RuntimeManager.PlayOneShot(sfxPath, transform.position);
		}
	}

	public void SetAudioVolume(float volume)
	{
		sfxInstance.setVolume(volume);
	}

	public float GetAudioVolume()
	{
		float volume, finalVolume;
		FMOD.RESULT result = sfxInstance.getVolume(out volume, out finalVolume);
		return volume;
	}

	public bool IsMuted()
	{
		return GetAudioVolume() <= 0f;
	}

	public void KillAudio()
	{
		sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		Destroy(gameObject);
	}

}
