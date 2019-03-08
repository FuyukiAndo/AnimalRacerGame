using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{

	[EventRef][SerializeField] private string backgroundAudioPath, ambiencePath;
	[SerializeField] private ParameterInstance backgroundParameter, ambienceParameter;
	[SerializeField] private string backgroundParameterName, ambienceParameterName;

	[SerializeField] private AudioClip backgroundClip;
	private AudioSource source;

	EventInstance backgroundAudioInstance, ambienceInstance;
	private bool shouldStopBack, shouldStopAmbience;

	public static AudioManager Instance
	{
		get
		{
			return instance;
		}
	}
	private static AudioManager instance;
	public bool useFMOD;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start()
	{
		if (useFMOD)
		{
			backgroundAudioInstance = RuntimeManager.CreateInstance(backgroundAudioPath);
			backgroundAudioInstance.getParameter(backgroundParameterName, out backgroundParameter);
			ambienceInstance = RuntimeManager.CreateInstance(ambiencePath);
			ambienceInstance.getParameter(ambienceParameterName, out ambienceParameter);
			StartCoroutine(PlayBackAudio());
		}
		else
		{
			source = GetComponent<AudioSource>();
			if (source != null)
			{
				SetBackgroundAudio(backgroundClip);
				StartCoroutine(PlayBackgroundAudio());
			}
			else
			{
				UnityEngine.Debug.LogWarning("AudioManager has no AudioSource component!");
			}
		}
	}

	IEnumerator PlayBackAudio()
	{
		backgroundAudioInstance.start();
		PLAYBACK_STATE playState;
		backgroundAudioInstance.getPlaybackState(out playState);
		while (!shouldStopBack)
		{
			if (playState != PLAYBACK_STATE.PLAYING)
			{
				backgroundAudioInstance.start();
			}
			backgroundAudioInstance.getPlaybackState(out playState);
			yield return null;
		}
		backgroundAudioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

	IEnumerator PlayAmbience()
	{
		ambienceInstance.start();
		PLAYBACK_STATE playState;
		ambienceInstance.getPlaybackState(out playState);
		while (!shouldStopAmbience)
		{
			if (playState != PLAYBACK_STATE.PLAYING)
			{
				ambienceInstance.start();
			}
			ambienceInstance.getPlaybackState(out playState);
			yield return null;
		}
		ambienceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

	IEnumerator PlayBackgroundAudio()
	{
		source.Play();
		while (!shouldStopBack)
		{
			if (!source.isPlaying)
			{
				source.Play();
			}
			yield return null;
		}
		source.Stop();
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

	public void FadeBackTo(string path)
	{
		backgroundAudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (path != null)
		{
			backgroundAudioInstance = RuntimeManager.CreateInstance(path);
			backgroundAudioInstance.start();
		}
	}

	public void FadeAmbienceTo(string path)
	{
		ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (path != null)
		{
			ambienceInstance = RuntimeManager.CreateInstance(path);
			ambienceInstance.start();
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

	public void PlayBackAudioLooping()
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

	public void PlayAmbienceLooping()
	{
		shouldStopAmbience = false;
		StartCoroutine(PlayAmbience());
	}

	public void StopBackAudioLooping()
	{
		shouldStopBack = true;
	}

	public void StopAmbienceLooping()
	{
		shouldStopAmbience = true;
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

	public float GetVolumeBackground()
	{
		float volume, finalVolume;
		backgroundAudioInstance.getVolume(out volume, out finalVolume);
		return finalVolume;
	}

	public void SetVolumeAmbience(float volume)
	{
		ambienceInstance.setVolume(volume);
	}

	public float GetVolumeAmbience()
	{
		float volume, finalVolume;
		ambienceInstance.getVolume(out volume, out finalVolume);
		return finalVolume;
	}

	public void SetVolumeMaster(float volume)
	{
		SetVolumeBackground(GetVolumeBackground() * volume);
		SetVolumeAmbience(GetVolumeAmbience() * volume);
	}

	public void SetBackgroundAudio(string path)
	{
		backgroundAudioPath = path;
	}

	public void SetAmbience(string path)
	{
		ambiencePath = path;
	}

	public void SetBackgroundAudio(AudioClip clip)
	{
		if (useFMOD)
		{
			print("Using FMOD enabled, no need for setting AudioClip.");
			return;
		}
		backgroundClip = clip;
		source.clip = backgroundClip;
	}

	public float GetBackParameterValue(string paramName)
	{
		backgroundAudioInstance.getParameter(paramName, out backgroundParameter);
		float tempValue;
		backgroundParameter.getValue(out tempValue);
		return tempValue;
	}

	public float GetAmbienceParameterValue(string parameter)
	{
		ambienceInstance.getParameter(parameter, out ambienceParameter);
		float tempValue;
		ambienceParameter.getValue(out tempValue);
		return tempValue;
	}

	public void SetBackParameterValue(string paramName, float value)
	{
		backgroundAudioInstance.setParameterValue(paramName, value);
	}

	public void SetAmbienceParameterValue(string paramName, float value)
	{
		ambienceInstance.setParameterValue(paramName, value);
	}
}