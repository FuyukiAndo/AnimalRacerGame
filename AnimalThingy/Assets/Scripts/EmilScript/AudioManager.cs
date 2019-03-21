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
	private Scene scene;

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
	{
		this.scene = scene;
		if (useFMOD)
		{
			for (int i = 0; i < background.audioPaths.Length; i++)
			{
				if (background.audioPaths[i].mapName.Contains(scene.name) || scene.name.Contains(background.audioPaths[i].mapName))
				{
					SetBackgroundAudio(GetBackAudioPathForScene(scene));
					SetAmbience(GetAmbienceAudioPathForScene(scene));
					StopBackAudioLooping();
					StopAmbienceLooping();
					SetupBack();
					SetupAmbience();
					SetVolumeSFX(GetVolumeSFX());
					SetVolumeBackground(GetVolumeBackground());
					SetVolumeAmbience(GetVolumeAmbience());
					PlayBackAudioLooping();
					PlayAmbienceLooping();
					return;
				}
			}
		}
		else
		{
			for (int i = 0; i < backgroundUnity.audioClips.Length; i++)
			{
				if (backgroundUnity.audioClips[i].mapName.Contains(scene.name) || scene.name.Contains(backgroundUnity.audioClips[i].mapName))
				{
					SetBackgroundAudio(backgroundUnity.audioClips[i].audioClip);
					SetAmbience(backgroundUnity.audioClips[i].audioClip);
					StopBackAudioLooping();
					StopAmbienceLooping();
					SetupBack();
					SetupAmbience();
					SetVolumeSFX(GetVolumeSFX());
					SetVolumeBackground(GetVolumeBackground());
					SetVolumeAmbience(GetVolumeAmbience());
					PlayBackAudioLooping();
					PlayAmbienceLooping();
					return;
				}
			}
		}
	}

	string GetBackAudioPathForScene(Scene scene)
	{
		for (int i = 0; i < background.audioPaths.Length; i++)
		{
			if (background.audioPaths[i].mapName.Contains(scene.name) || scene.name.Contains(background.audioPaths[i].mapName))
			{
				return background.audioPaths[i].audioPath;
			}
		}
		return string.Empty;
	}

	string GetAmbienceAudioPathForScene(Scene scene)
	{
		for (int i = 0; i < ambience.audioPaths.Length; i++)
		{
			if (ambience.audioPaths[i].mapName.Contains(scene.name) || scene.name.Contains(ambience.audioPaths[i].mapName))
			{
				return ambience.audioPaths[i].audioPath;
			}
		}
		return string.Empty;
	}

	string GetBackParamNameForScene(Scene scene)
	{
		for (int i = 0; i < background.audioPaths.Length; i++)
		{
			if (background.audioPaths[i].mapName.Contains(scene.name) || scene.name.Contains(background.audioPaths[i].mapName))
			{
				return background.audioPaths[i].paramName;
			}
		}
		return string.Empty;
	}

	string GetAmbienceParamNameForScene(Scene scene)
	{
		for (int i = 0; i < ambience.audioPaths.Length; i++)
		{
			if (ambience.audioPaths[i].mapName.Contains(scene.name) || scene.name.Contains(ambience.audioPaths[i].mapName))
			{
				return ambience.audioPaths[i].paramName;
			}
		}
		return string.Empty;
	}

	float GetBackParamValueForScene(Scene scene)
	{
		for (int i = 0; i < background.audioPaths.Length; i++)
		{
			if (background.audioPaths[i].mapName.Contains(scene.name) || scene.name.Contains(background.audioPaths[i].mapName))
			{
				return background.audioPaths[i].paramValue;
			}
		}
		return 0f;
	}

	float GetAmbienceParamValueForScene(Scene scene)
	{
		for (int i = 0; i < ambience.audioPaths.Length; i++)
		{
			if (ambience.audioPaths[i].mapName.Contains(scene.name) || scene.name.Contains(ambience.audioPaths[i].mapName))
			{
				return ambience.audioPaths[i].paramValue;
			}
		}
		return 0f;
	}

	float GetRandomAdditionalBackParamValueForScene(Scene scene)
	{
		for (int i = 0; i < background.audioPaths.Length; i++)
		{
			if (background.audioPaths[i].mapName.Contains(scene.name) || scene.name.Contains(background.audioPaths[i].mapName))
			{
				int rand = Random.Range(0, background.audioPaths[i].additionalParamValues.Length);
				return background.audioPaths[i].additionalParamValues[rand];
			}
		}
		return 0f;
	}

	float GetRandomAdditionalAmbienceParamValueForScene(Scene scene)
	{
		for (int i = 0; i < ambience.audioPaths.Length; i++)
		{
			if (ambience.audioPaths[i].mapName.Contains(scene.name) || scene.name.Contains(ambience.audioPaths[i].mapName))
			{
				int rand = Random.Range(0, ambience.audioPaths[i].additionalParamValues.Length);
				return ambience.audioPaths[i].additionalParamValues[rand];
			}
		}
		return 0f;
	}

	bool GetRandomizeBackParamValue(Scene scene)
	{
		for (int i = 0; i < background.audioPaths.Length; i++)
		{
			if (background.audioPaths[i].mapName.Contains(scene.name) || scene.name.Contains(background.audioPaths[i].mapName))
			{
				return background.audioPaths[i].randomizeValue;
			}
		}
		return false;
	}

	bool GetRandomizeAmbienceParamValue(Scene scene)
	{
		for (int i = 0; i < ambience.audioPaths.Length; i++)
		{
			if (ambience.audioPaths[i].mapName.Contains(scene.name) || scene.name.Contains(ambience.audioPaths[i].mapName))
			{
				return ambience.audioPaths[i].randomizeValue;
			}
		}
		return false;
	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			backgroundRoutine = PlayBackAudio();
			ambienceRoutine = PlayAmbience();
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
			if (GetRandomizeBackParamValue(scene))
			{
				SetBackParameterValue(GetRandomAdditionalBackParamValueForScene(scene));
			}
			else
			{
				SetBackParameterValue(GetBackParamValueForScene(scene));
			}
			if (GetRandomizeAmbienceParamValue(scene))
			{
				SetAmbienceParameterValue(GetRandomAdditionalAmbienceParamValueForScene(scene));
			}
			else
			{
				SetAmbienceParameterValue(GetAmbienceParamValueForScene(scene));
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
		//PositionAudioSource();
	}

	void PositionAudioSource()
	{
		GameObject cam = FindObjectOfType<Camera>().gameObject;
		Vector3 position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0f);
		transform.position = position;
		RuntimeManager.AttachInstanceToGameObject(background.audioInstance, transform, GetComponent<Rigidbody2D>());
		RuntimeManager.AttachInstanceToGameObject(ambience.audioInstance, transform, GetComponent<Rigidbody2D>());
	}

	void SetupBack()
	{
		if (background.currentAudioPath != string.Empty)
		{
			background.audioInstance = RuntimeManager.CreateInstance(background.currentAudioPath);
			ATTRIBUTES_3D attributesBack = FMODUnity.RuntimeUtils.To3DAttributes(transform.position);
			background.audioInstance.set3DAttributes(attributesBack);
		}
	}

	void SetupAmbience()
	{
		if (ambience.currentAudioPath != string.Empty)
		{
			ambience.audioInstance = RuntimeManager.CreateInstance(ambience.currentAudioPath);
			ATTRIBUTES_3D attributesAmb = FMODUnity.RuntimeUtils.To3DAttributes(transform.position);
			ambience.audioInstance.set3DAttributes(attributesAmb);
		}
	}

	IEnumerator PlayBackAudio()
	{
		if (useFMOD)
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
		PlayerPrefs.SetFloat("SFX", volume);
		sfxVolume = volume;
		foreach (var controller in FindObjectsOfType<AudioEffectController>())
		{
			controller.SetAudioVolume(volume);
		}
	}

	public float GetVolumeSFX()
	{
		return PlayerPrefs.GetFloat("SFX");
	}

	public void FadeBackTo(string path)
	{
		background.audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
		background.audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		StopCoroutine(backgroundRoutine);
		//FadeBackTo("");
	}

	public void StopAmbienceLooping()
	{
		shouldStopAmbience = true;
		ambience.audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		StopCoroutine(ambienceRoutine);
		//FadeAmbienceTo();
	}

	public void SetVolumeBackground(float volume)
	{
		PlayerPrefs.SetFloat("Background", volume);
		backgroundVolume = volume;
		if (useFMOD)
		{
			background.audioInstance.setVolume(volume);
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
			background.audioInstance.getVolume(out volume, out finalVolume);
			return PlayerPrefs.GetFloat("Background");
		}
		else
		{
			return backSource.volume;
		}
	}

	public void SetVolumeAmbience(float volume)
	{
		PlayerPrefs.SetFloat("Ambience", volume);
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
			return PlayerPrefs.GetFloat("Ambience");
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
		background.audioInstance.getParameter(GetBackParamNameForScene(scene), out background.paramInstance);
		float tempValue;
		background.paramInstance.getValue(out tempValue);
		return tempValue;
	}

	public float GetAmbienceParameterValue()
	{
		ambience.audioInstance.getParameter(GetAmbienceParamNameForScene(scene), out ambience.paramInstance);
		float tempValue;
		ambience.paramInstance.getValue(out tempValue);
		return tempValue;
	}

	public void SetBackParameterValue(float value)
	{
		background.audioInstance.setParameterValue(GetBackParamNameForScene(scene), value);
	}

	public void SetAmbienceParameterValue(float value)
	{
		ambience.audioInstance.setParameterValue(GetAmbienceParamNameForScene(scene), value);
	}

	private bool IsBackPlaying()
	{
		if (useFMOD)
		{
			PLAYBACK_STATE state;
			background.audioInstance.getPlaybackState(out state);
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
		[HideInInspector] public string currentAudioPath;

		public ParameterInstance paramInstance;
		public EventInstance audioInstance;

		[System.Serializable]
		public struct MapAudio
		{
			[EventRef] public string audioPath;
			public float[] additionalParamValues;
			public string paramName;
			public float paramValue;
			public string mapName;
			public bool randomizeValue;
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