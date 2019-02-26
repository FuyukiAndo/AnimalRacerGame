using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;
using FMOD.Studio;

public class AudioManager : MonoBehaviour {

	[EventRef] [SerializeField] private string backgroundAudioPath;
	[Range(0f, 1f)] [SerializeField] private float sfxVolume, backgroundVolume;

	EventInstance backgroundAudioInstance;
	private bool shouldStopBack;

	public static AudioManager Instance
	{
		get
		{
			return instance;
		}
	}
	private static AudioManager instance;

	// Use this for initialization
	void Start () {
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		StartCoroutine(PlayBackAudio());
	}
	
	IEnumerator PlayBackAudio()
	{
		backgroundAudioInstance.start();
		PLAYBACK_STATE playState = new PLAYBACK_STATE();
		backgroundAudioInstance.getPlaybackState(out playState);
		while (playState == PLAYBACK_STATE.PLAYING && !shouldStopBack)
		{
			backgroundAudioInstance.getPlaybackState(out playState);
			yield return null;
		}
		if (shouldStopBack)
		{
			backgroundAudioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
		if (playState == PLAYBACK_STATE.STOPPED && !shouldStopBack)
		{
			//StartCoroutine(PlayBackAudio());
		}
	}

	IEnumerator IStopAll(bool fade)
	{
		foreach (var controller in FindObjectsOfType<AudioEffectController>())
		{
			while (controller.GetAudioVolume() > 0f)
			{
				if (fade)
				{
					controller.SetAudioVolume(controller.GetAudioVolume() - .1f);
				}
				else
				{
					controller.SetAudioVolume(0f);
				}
				yield return null;
			}
		}
	}

	public void StopAll(bool fade)
	{
		StartCoroutine(IStopAll(fade));
	}

	public void SetVolumeSFX(float volume)
	{
		foreach (var controller in FindObjectsOfType<AudioEffectController>())
		{
			controller.SetAudioVolume(volume);
		}
	}

	public void FadeTo(string path)
	{
		backgroundAudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (path != null)
		{
			backgroundAudioInstance = RuntimeManager.CreateInstance(path);
			backgroundAudioInstance.start();
		}
	}

	public void PlayAudioLooping()
	{
		shouldStopBack = false;
		StartCoroutine(PlayBackAudio());
	}

	public void StopAudioLooping()
	{
		shouldStopBack = true;
	}

	public void SetVolumeBackground(float volume)
	{
		backgroundAudioInstance.setVolume(volume);
	}

	public void SetBackgroundAudio(string path)
	{
		backgroundAudioPath = path;
	}
}
