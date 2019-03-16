using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

	//FMOD
	public FMODManagerAudio Background
	{
		get
		{
			return background;
		}
	}
	public FMODManagerAudio Ambience
	{
		get
		{
			return ambience;
		}
	}

	[SerializeField] private FMODManagerAudio background, ambience;

	//Unity
	[SerializeField] private AudioClip backgroundClip;
	private AudioSource source;

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

	[SerializeField][Range(0f, 1f)] private float backgroundVolume = .5f, ambienceVolume = .5f, sfxVolume = .5f, masterVolume = .5f;

	void OnValidate()
	{
		SetVolumeSFX(sfxVolume);
		SetVolumeBackground(backgroundVolume);
		SetVolumeAmbience(ambienceVolume);
		SetVolumeMaster(masterVolume);
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelLoaded;
	}

	void OnLevelLoaded(Scene scene, LoadSceneMode sceneMode)
	{
		foreach (var audio in background.audioPaths)
		{
			if (scene.name.Contains(audio.mapName) || audio.mapName.Contains(scene.name))
			{
				StopBackAudioLooping();
				SetBackgroundAudio(audio.audioPath);
				PlayBackAudioLooping();
			}
		}
	}

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
			if (background.randomizeValue && background.additionalParamValues.Length > 0)
			{
				int rand = Random.Range(0, background.additionalParamValues.Length);
				SetBackParameterValue(background.additionalParamValues[rand]);
			}
			else
			{
				SetBackParameterValue(background.paramValue);
			}
			if (ambience.randomizeValue && ambience.additionalParamValues.Length > 0)
			{
				int rand = Random.Range(0, ambience.additionalParamValues.Length);
				SetAmbienceParameterValue(ambience.additionalParamValues[rand]);
			}
			else
			{
				SetAmbienceParameterValue(ambience.paramValue);
			}
			SetVolumeSFX(sfxVolume);
			SetVolumeBackground(backgroundVolume);
			SetVolumeAmbience(ambienceVolume);
			SetVolumeMaster(masterVolume);
			if (!IsBackPlaying())
			{
				Setup();
				StartCoroutine(PlayBackAudio());
			}
			//StartCoroutine(PlayAmbience());
		}
		else
		{
			if (!GetComponent<AudioSource>())
			{
				gameObject.AddComponent<AudioSource>();
			}
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

	void Setup()
	{
		if (background.currentAudioPath != string.Empty)
		{
			background.audioInstance = RuntimeManager.CreateInstance(background.currentAudioPath);
			ATTRIBUTES_3D attributesBack = FMODUnity.RuntimeUtils.To3DAttributes(transform.position);
			background.audioInstance.set3DAttributes(attributesBack);
		}

		if (ambience.currentAudioPath != string.Empty)
		{
			ambience.audioInstance = RuntimeManager.CreateInstance(ambience.currentAudioPath);
			ATTRIBUTES_3D attributesAmb = FMODUnity.RuntimeUtils.To3DAttributes(transform.position);
			ambience.audioInstance.set3DAttributes(attributesAmb);
		}
	}

	IEnumerator PlayBackAudio()
	{
		background.audioInstance.start();
		PLAYBACK_STATE playState;
		background.audioInstance.getPlaybackState(out playState);
		while (!shouldStopBack)
		{
			if (playState == PLAYBACK_STATE.STOPPED)
			{
				background.audioInstance.start();
			}
			background.audioInstance.getPlaybackState(out playState);
			yield return null;
		}
		FadeBackTo(background.currentAudioPath);
		//background.audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

	IEnumerator PlayAmbience()
	{
		ambience.audioInstance.start();
		PLAYBACK_STATE playState;
		ambience.audioInstance.getPlaybackState(out playState);
		while (!shouldStopAmbience)
		{
			if (playState == PLAYBACK_STATE.STOPPED)
			{
				ambience.audioInstance.start();
			}
			ambience.audioInstance.getPlaybackState(out playState);
			yield return null;
		}
		FadeAmbienceTo(ambience.currentAudioPath);
		//ambience.audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
		sfxVolume = volume;
		foreach (var controller in FindObjectsOfType<AudioEffectController>())
		{
			controller.SetAudioVolume(volume);
		}
	}

	public float GetVolumeSFX()
	{
		return sfxVolume;
	}

	public void FadeBackTo(string path)
	{
		background.audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (path != null)
		{
			Setup();
			StartCoroutine(PlayBackAudio());
		}
	}

	public void FadeAmbienceTo(string path)
	{
		ambience.audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (path != null)
		{
			Setup();
			//ambience.audioInstance = RuntimeManager.CreateInstance(path);
			ambience.audioInstance.start();
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
		StopCoroutine(PlayBackAudio());
	}

	public void StopAmbienceLooping()
	{
		shouldStopAmbience = true;
		StopCoroutine(PlayAmbience());
	}

	public void SetVolumeBackground(float volume)
	{
		if (useFMOD)
		{
			backgroundVolume = volume;
			background.audioInstance.setVolume(volume);
		}
		else
		{
			source.volume = volume;
		}
	}

	public float GetVolumeBackground()
	{
		float volume, finalVolume;
		background.audioInstance.getVolume(out volume, out finalVolume);
		return volume;
	}

	public void SetVolumeAmbience(float volume)
	{
		ambienceVolume = volume;
		ambience.audioInstance.setVolume(volume);
	}

	public float GetVolumeAmbience()
	{
		float volume, finalVolume;
		ambience.audioInstance.getVolume(out volume, out finalVolume);
		return volume;
	}

	public void SetVolumeMaster(float volume)
	{
		SetVolumeSFX(GetVolumeSFX() * volume);
		SetVolumeBackground(GetVolumeBackground() * volume);
		SetVolumeAmbience(GetVolumeAmbience() * volume);
	}

	public void SetBackgroundAudio(string path)
	{
		background.currentAudioPath = path;
	}

	public void SetAmbience(string path)
	{
		ambience.currentAudioPath = path;
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

	public float GetBackParameterValue()
	{
		background.audioInstance.getParameter(background.paramName, out background.paramInstance);
		float tempValue;
		background.paramInstance.getValue(out tempValue);
		return tempValue;
	}

	public float GetAmbienceParameterValue()
	{
		ambience.audioInstance.getParameter(ambience.paramName, out ambience.paramInstance);
		float tempValue;
		ambience.paramInstance.getValue(out tempValue);
		return tempValue;
	}

	public void SetBackParameterValue(float value)
	{
		background.audioInstance.setParameterValue(background.paramName, value);
	}

	public void SetAmbienceParameterValue(float value)
	{
		ambience.audioInstance.setParameterValue(background.paramName, value);
	}

	private bool IsBackPlaying()
	{
		PLAYBACK_STATE state;
		background.audioInstance.getPlaybackState(out state);
		return state == PLAYBACK_STATE.PLAYING;
	}

	private bool IsAmbiencePlaying()
	{
		PLAYBACK_STATE state;
		ambience.audioInstance.getPlaybackState(out state);
		return state == PLAYBACK_STATE.PLAYING;
	}

	[System.Serializable]
	public struct FMODManagerAudio
	{
		public MapAudio[] audioPaths;
		[EventRef] public string currentAudioPath;
		public string paramName;
		public float paramValue;
		public float[] additionalParamValues;
		public bool randomizeValue;

		public ParameterInstance paramInstance;
		public EventInstance audioInstance;

		[System.Serializable]
		public struct MapAudio
		{
			[EventRef] public string audioPath;
			public string mapName;
		}
	}
}

[System.Serializable]
public struct FMODAudio
{
	[EventRef] public string currentAudioPath;
	public string paramName;
	public float paramValue;
	public float[] additionalParamValues;
	public bool randomizeValue;

	public ParameterInstance paramInstance;
	public EventInstance audioInstance;
}