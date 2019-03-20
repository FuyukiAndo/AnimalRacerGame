using System.Collections.Generic;
using FMOD;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioOneshotPlayer : MonoBehaviour
{
	[SerializeField] private FMODAudio sfx;
	[SerializeField] private AudioClip clip;
	private AudioSource source;

	void Start()
	{
		if (source == null)
		{
			source = GetComponent<AudioSource>();
		}
	}

	void Setup()
	{
		if (sfx.currentAudioPath != string.Empty)
		{
			sfx.audioInstance = RuntimeManager.CreateInstance(sfx.currentAudioPath);
			ATTRIBUTES_3D attributesAmb = FMODUnity.RuntimeUtils.To3DAttributes(transform.position);
			sfx.audioInstance.set3DAttributes(attributesAmb);
			SetAudioVolume(AudioManager.Instance.GetVolumeSFX());

			if (sfx.randomizeValue && sfx.additionalParamValues.Length > 0)
			{
				int rand = Random.Range(0, sfx.additionalParamValues.Length);
				SetParameterValue(sfx.additionalParamValues[rand]);
			}
			else
			{
				SetParameterValue(sfx.paramValue);
			}
		}
	}

	public bool IsAudioPathNull()
	{
		return sfx.currentAudioPath == string.Empty;
	}

	public void PlayAudioOneShot(bool attached)
	{
		if (IsAudioPathNull())return;
		Setup();
		if (attached)
		{
			RuntimeManager.PlayOneShotAttached(sfx.currentAudioPath, gameObject);
		}
		else
		{
			RuntimeManager.PlayOneShot(sfx.currentAudioPath, transform.position);
		}
	}

	public void PlayAudioOneShot()
	{
		if (AudioManager.Instance.useFMOD)
		{
			if (IsAudioPathNull())return;
			Setup();
			sfx.audioInstance.start();
			sfx.audioInstance.release();
		}
		else
		{
			source.volume = AudioManager.Instance.GetVolumeSFX();
			source.PlayOneShot(clip);
		}
	}

	public void SetAudioVolume(float volume)
	{
		if (AudioManager.Instance.useFMOD)
		{
			sfx.audioInstance.setVolume(volume);
		}
		else
		{
			source.volume = volume;
		}
	}

	public float GetAudioVolume()
	{
		float volume, finalVolume;
		if (AudioManager.Instance.useFMOD)
		{
			sfx.audioInstance.getVolume(out volume, out finalVolume);
			return volume;
		}
		else
		{
			return source.volume;
		}
	}

	public bool IsMuted()
	{
		return GetAudioVolume() <= 0f;
	}

	public void KillAudio(bool destroyObject)
	{
		if (AudioManager.Instance.useFMOD)
		{
			sfx.audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
		else
		{
			source.volume = 0.0f;
		}
		if (destroyObject)
		{
			Destroy(gameObject);
		}
	}

	public float GetParameterValue()
	{
		sfx.audioInstance.getParameter(sfx.paramName, out sfx.paramInstance);
		float tempValue;
		sfx.paramInstance.getValue(out tempValue);
		return tempValue;
	}

	public void SetParameterValue(float value)
	{
		sfx.audioInstance.setParameterValue(sfx.paramName, value);
	}

	public void SetAudioPath(string path)
	{
		sfx.currentAudioPath = path;
	}

	public void SetAudioClip(AudioClip clip)
	{
		source.clip = clip;
	}

	public void SetRandomizeParamValue(bool state)
	{
		sfx.randomizeValue = state;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere((Vector2)transform.position, 1f);
	}
}