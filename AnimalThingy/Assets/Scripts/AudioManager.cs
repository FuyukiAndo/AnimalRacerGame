using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using FMODUnity;
using FMOD;
using FMOD.Studio;

public class AudioManager : MonoBehaviour {

	[EventRef] [SerializeField] private string backgroundAudioPath;
	[Range(0f, 1f)] [SerializeField] private float sfxVolume, backgroundVolume;

	[SerializeField] private AudioClip backgroundClip;
	private AudioSource source;

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
	public bool useFMOD;

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
		if (useFMOD)
		{
			StartCoroutine(PlayBackAudio());
		}
		else
		{
			source = GetComponent<AudioSource>();
			StartCoroutine(PlayBackgroundAudio());
		}
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
			//Restart background audio if needed
			//StartCoroutine(PlayBackAudio());
		}
	}

	IEnumerator PlayBackgroundAudio()
	{
		source.Play();
		while (source.isPlaying && !shouldStopBack)
		{
			yield return null;
		}
		if (shouldStopBack)
		{
			source.Stop();
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

	public void FadeTo(AudioClip clip)
	{
		if (useFMOD)
		{
			print("Using FMOD enabled, no need for fading with AudioClip.");
			return;
		}
		StartCoroutine(IFadeTo(clip));
	}

	IEnumerator IFadeTo(AudioClip clip)
	{
		while (source.volume > 0.0f)
		{
			source.volume -= Time.deltaTime;
			yield return null;
		}
		backgroundClip = clip;
		source.clip = backgroundClip;
		while (source.volume < 1.0f)
		{
			source.volume += Time.deltaTime;
			yield return null;
		}
	}

	public void PlayAudioLooping()
	{
		shouldStopBack = false;
		if (useFMOD)
		{
			StartCoroutine(PlayBackAudio());
		}
		else
		{
			StartCoroutine(PlayBackgroundAudio());
		}
	}

	public void StopAudioLooping()
	{
		shouldStopBack = true;
	}

	public void SetVolumeBackground(float volume)
	{
		if (useFMOD)
		{
			backgroundAudioInstance.setVolume(volume);
		}
		else
		{
			source.volume = volume;
		}
	}

	public void SetBackgroundAudio(string path)
	{
		backgroundAudioPath = path;
	}

	public void SetBackgroundAudio(AudioClip clip)
	{
		if (useFMOD)
		{
			print("Using FMOD enabled, no need for setting AudioClip.");
			return;
		}
		backgroundClip = clip;
	}
}
