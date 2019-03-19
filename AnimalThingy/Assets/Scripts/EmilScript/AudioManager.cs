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
	private EventInstance currentBackgroundInstance, currentAmbienceInstance;

	//Unity
	[SerializeField] private ManagerAudio backgroundUnity, ambienceUnity;
	[SerializeField] private AudioSource backSource, ambienceSource;

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
	private IEnumerator backgroundRoutine, ambienceRoutine;

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
	{
		if (useFMOD)
		{
			foreach (var path in background.audioPaths)
			{
				if (path.mapName.Contains(scene.name) || scene.name.Contains(path.mapName))
				{
					SetBackgroundAudio(path.audioPath);
					SetAmbience(path.audioPath);
					StopBackAudioLooping();
					StopAmbienceLooping();
					SetupBack();
					SetupAmbience();
					PlayBackAudioLooping();
					PlayAmbienceLooping();
					return;
				}
			}
		}
		else
		{
			foreach (var clip in backgroundUnity.audioClips)
			{
				if (clip.mapName.Contains(scene.name) || scene.name.Contains(clip.mapName))
				{
					SetBackgroundAudio(clip.audioClip);
					return;
				}
			}
		}
	}

	void OnAwake(Scene scene)
	{
		if (useFMOD)
		{
			foreach (var path in background.audioPaths)
			{
				if (path.mapName.Contains(scene.name) || scene.name.Contains(path.mapName))
				{
					SetBackgroundAudio(path.audioPath);
					return;
				}
			}
		}
		else
		{
			foreach (var clip in backgroundUnity.audioClips)
			{
				if (clip.mapName.Contains(scene.name) || scene.name.Contains(clip.mapName))
				{
					SetBackgroundAudio(clip.audioClip);
					return;
				}
			}
		}
	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			backgroundRoutine = PlayBackAudio();
			ambienceRoutine = PlayAmbience();
			OnAwake(SceneManager.GetActiveScene());
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
			SetVolumeSFX(sfxVolume);
			SetVolumeBackground(backgroundVolume);
			SetVolumeAmbience(ambienceVolume);
			SetupBack();
			SetupAmbience();
			PlayBackAudioLooping();
			PlayAmbienceLooping();
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
		}
		else
		{
			if (GetComponents<AudioSource>().Length >= 0)
			{
				if (GetComponents<AudioSource>().Length == 1)
				{
					gameObject.AddComponent<AudioSource>();
				}
				else
				{
					gameObject.AddComponent<AudioSource>();
					gameObject.AddComponent<AudioSource>();
				}
			}
			backSource = GetComponents(typeof(AudioSource))[0] as AudioSource;
			ambienceSource = GetComponents(typeof(AudioSource))[1] as AudioSource;
			if (backSource != null)
			{
				PlayBackAudioLooping();
			}
			else
			{
				UnityEngine.Debug.LogWarning("AudioManager has no AudioSource component!");
			}
			if (ambienceSource != null)
			{
				PlayAmbienceLooping();
			}
			else
			{
				UnityEngine.Debug.LogWarning("AudioManager has no AudioSource component!");
			}
		}
	}

	void Update()
	{
		PositionAudioSource();
	}

	void PositionAudioSource()
	{
		GameObject cam = FindObjectOfType<Camera>().gameObject;
		Vector3 position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0f);
		transform.position = position;
		RuntimeManager.AttachInstanceToGameObject(currentBackgroundInstance, transform, GetComponent<Rigidbody2D>());
		RuntimeManager.AttachInstanceToGameObject(currentAmbienceInstance, transform, GetComponent<Rigidbody2D>());
	}

	void SetupBack()
	{
		if (background.currentAudioPath != string.Empty)
		{
			currentBackgroundInstance = RuntimeManager.CreateInstance(background.currentAudioPath);
			//ATTRIBUTES_3D attributesBack = FMODUnity.RuntimeUtils.To3DAttributes(transform.position);
			//currentBackgroundInstance.set3DAttributes(attributesBack);
		}
	}

	void SetupAmbience()
	{
		if (ambience.currentAudioPath != string.Empty)
		{
			ambience.audioInstance = RuntimeManager.CreateInstance(ambience.currentAudioPath);
			//ATTRIBUTES_3D attributesAmb = FMODUnity.RuntimeUtils.To3DAttributes(transform.position);
			//ambience.audioInstance.set3DAttributes(attributesAmb);
		}
	}

	IEnumerator PlayBackAudio()
	{
		if (useFMOD)
		{
			currentBackgroundInstance.start();
			PLAYBACK_STATE playState;
			currentBackgroundInstance.getPlaybackState(out playState);
			while (!shouldStopBack)
			{
				if (playState == PLAYBACK_STATE.STOPPED)
				{
					currentBackgroundInstance.start();
				}
				currentBackgroundInstance.getPlaybackState(out playState);
				yield return null;
			}
		}
		else
		{
			backSource.Play();
			while (!shouldStopBack)
			{
				if (!backSource.isPlaying)
				{
					backSource.Play();
				}
				yield return null;
			}
			backSource.Stop();
		}
	}

	IEnumerator PlayAmbience()
	{
		if (useFMOD)
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
		}
		else
		{
			ambienceSource.Play();
			while (!shouldStopBack)
			{
				if (!ambienceSource.isPlaying)
				{
					ambienceSource.Play();
				}
				yield return null;
			}
			ambienceSource.Stop();
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
		currentBackgroundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (path != "")
		{
			SetupBack();
			StartCoroutine(PlayBackAudio());
		}
	}

	public void FadeAmbienceTo(string path = null)
	{
		ambience.audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (path != null || path != string.Empty)
		{
			SetupAmbience();
			StartCoroutine(PlayAmbience());
		}
	}

	public void FadeBackTo(AudioClip clip)
	{
		StartCoroutine(IFadeBackTo(clip));
	}

	public void FadeAmbienceTo(AudioClip clip)
	{
		StartCoroutine(IFadeAmbienceTo(clip));
	}

	IEnumerator IFadeBackTo(AudioClip clip)
	{
		float tempBack = backgroundVolume;
		while (backSource.volume > 0.0f)
		{
			backSource.volume -= Time.deltaTime;
			yield return null;
		}
		StopBackAudioLooping();
		SetBackgroundAudio(clip);
		PlayBackAudioLooping();
		while (backSource.volume < tempBack)
		{
			backSource.volume += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator IFadeAmbienceTo(AudioClip clip)
	{
		float tempAmb = ambienceVolume;
		while (ambienceSource.volume > 0.0f)
		{
			ambienceSource.volume -= Time.deltaTime;
			yield return null;
		}
		StopAmbienceLooping();
		SetAmbience(clip);
		PlayAmbienceLooping();
		while (ambienceSource.volume < tempAmb)
		{
			ambienceSource.volume += Time.deltaTime;
			yield return null;
		}
	}

	public void PlayBackAudioLooping()
	{
		shouldStopBack = false;
		StartCoroutine(backgroundRoutine);
	}

	public void PlayAmbienceLooping()
	{
		shouldStopAmbience = false;
		StartCoroutine(ambienceRoutine);
	}

	public void StopBackAudioLooping()
	{
		shouldStopBack = true;
		currentBackgroundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		StopCoroutine(backgroundRoutine);
		//FadeBackTo("");
	}

	public void StopAmbienceLooping()
	{
		shouldStopAmbience = true;
		currentAmbienceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		StopCoroutine(ambienceRoutine);
		//FadeAmbienceTo();
	}

	public void SetVolumeBackground(float volume)
	{
		backgroundVolume = volume;
		if (useFMOD)
		{
			currentBackgroundInstance.setVolume(volume);
		}
		else
		{
			backSource.volume = volume;
		}
	}

	public float GetVolumeBackground()
	{
		if (useFMOD)
		{
			float volume, finalVolume;
			currentBackgroundInstance.getVolume(out volume, out finalVolume);
			return volume;
		}
		else
		{
			return backSource.volume;
		}
	}

	public void SetVolumeAmbience(float volume)
	{
		ambienceVolume = volume;
		if (useFMOD)
		{
			ambience.audioInstance.setVolume(volume);
		}
		else
		{
			ambienceSource.volume = volume;
		}
	}

	public float GetVolumeAmbience()
	{
		if (useFMOD)
		{
			float volume, finalVolume;
			ambience.audioInstance.getVolume(out volume, out finalVolume);
			return volume;
		}
		else
		{
			return ambienceSource.volume;
		}
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
		backSource.clip = clip;
	}

	public void SetAmbience(AudioClip clip)
	{
		ambienceSource.clip = clip;
	}

	public float GetBackParameterValue()
	{
		currentBackgroundInstance.getParameter(background.paramName, out background.paramInstance);
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
		currentBackgroundInstance.setParameterValue(background.paramName, value);
	}

	public void SetAmbienceParameterValue(float value)
	{
		ambience.audioInstance.setParameterValue(background.paramName, value);
	}

	private bool IsBackPlaying()
	{
		if (useFMOD)
		{
			PLAYBACK_STATE state;
			currentBackgroundInstance.getPlaybackState(out state);
			print(state);
			return state == PLAYBACK_STATE.PLAYING;
		}
		else
		{
			return backSource.isPlaying;
		}
	}

	private bool IsAmbiencePlaying()
	{
		if (useFMOD)
		{
			PLAYBACK_STATE state;
			ambience.audioInstance.getPlaybackState(out state);
			return state == PLAYBACK_STATE.PLAYING;
		}
		else
		{
			return ambienceSource.isPlaying;
		}
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

	[System.Serializable]
	public struct ManagerAudio
	{
		public MapAudio[] audioClips;

		[System.Serializable]
		public struct MapAudio
		{
			public AudioClip audioClip;
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