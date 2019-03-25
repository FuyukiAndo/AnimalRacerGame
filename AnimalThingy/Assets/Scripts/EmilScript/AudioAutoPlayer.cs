using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioAutoPlayer : MonoBehaviour
{
	[SerializeField] private bool oneshot, attached;

	public enum StartEvent
	{
		none,
		start,
		update,
		collisionEnter,
		collisionStay,
		collisionExit,
		triggerEnter,
		triggerExit,
		destroyed
	}
	public StartEvent startEvent;
	public enum StopEvent
	{
		none,
		collisionEnter,
		collisionStay,
		collisionExit,
		triggerEnter,
		triggerExit,
		destroyed
	}
	public StopEvent stopEvent;

	[SerializeField] private float updateDelay;
	[SerializeField] private FMODAudio sfx;
	[SerializeField] private LayerMask layer;
	[SerializeField] private Vector2 boxSize = new Vector2(1f, 1f);
	[SerializeField] private Vector2 boxOffset = new Vector2(0f, 0f);
	[SerializeField] private AudioClip clip;

	private float nextWait;
	private Collider2D[] colliders;
	private int oldColliderCount, newColliderCount;
	private AudioSource source;

	void Start()
	{
		if (AudioManager.Instance.useFMOD)
		{
			Setup();
			if (startEvent == StartEvent.start)
			{
				//Play audio
				if (!oneshot)
				{
					Setup();
					sfx.audioInstance.start();
				}
				else
				{
					if (!attached)
					{
						if (sfx.paramName != string.Empty)
						{
							Setup();
							sfx.audioInstance.start();
							sfx.audioInstance.release();
						}
						else
						{
							RuntimeManager.PlayOneShot(sfx.currentAudioPath, transform.position);
						}
					}
					else
					{
						if (sfx.paramName != string.Empty)
						{
							Setup();
							sfx.audioInstance.start();
							sfx.audioInstance.release();
						}
						else
						{
							RuntimeManager.PlayOneShotAttached(sfx.currentAudioPath, gameObject);
						}
					}
				}
			}
		}
		else
		{
			if (!GetComponent<AudioSource>())
			{
				gameObject.AddComponent<AudioSource>();
			}
			source = GetComponent<AudioSource>();
			if (startEvent == StartEvent.start)
			{
				//Play audio
				if (!oneshot)
				{
					source.Play();
				}
				else
				{
					source.PlayOneShot(clip);
				}
			}
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

	void Update()
	{
		if (startEvent == StartEvent.update && Time.time > nextWait)
		{
			if (AudioManager.Instance.useFMOD)
			{
				//Play audio
				if (!oneshot)
				{
					Setup();
					sfx.audioInstance.start();
				}
				else
				{
					if (!attached)
					{
						if (sfx.paramName != string.Empty)
						{
							Setup();
							sfx.audioInstance.start();
							sfx.audioInstance.release();
						}
						else
						{
							RuntimeManager.PlayOneShot(sfx.currentAudioPath, transform.position);
						}
					}
					else
					{
						if (sfx.paramName != string.Empty)
						{
							Setup();
							sfx.audioInstance.start();
							sfx.audioInstance.release();
						}
						else
						{
							RuntimeManager.PlayOneShotAttached(sfx.currentAudioPath, gameObject);
						}
					}
				}
			}
			else
			{
				if (!oneshot)
				{
					source.Play();
				}
				else
				{
					source.PlayOneShot(clip);
				}
			}
			nextWait = Time.time + updateDelay;
		}

		//Trigger event replacement
		oldColliderCount = newColliderCount;
		colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + boxOffset, boxSize, 0f, layer);
		newColliderCount = colliders.Length;
		if (newColliderCount > oldColliderCount) //Trigger Enter
		{
			if (startEvent == StartEvent.triggerEnter)
			{
				if (AudioManager.Instance.useFMOD)
				{
					//Play audio
					if (!oneshot)
					{
						Setup();
						sfx.audioInstance.start();
					}
					else
					{
						if (!attached)
						{
							if (sfx.paramName != string.Empty)
							{
								Setup();
								sfx.audioInstance.start();
								sfx.audioInstance.release();
							}
							else
							{
								RuntimeManager.PlayOneShot(sfx.currentAudioPath, transform.position);
							}
						}
						else
						{
							if (sfx.paramName != string.Empty)
							{
								Setup();
								sfx.audioInstance.start();
								sfx.audioInstance.release();
							}
							else
							{
								RuntimeManager.PlayOneShotAttached(sfx.currentAudioPath, gameObject);
							}
						}
					}
				}
				else
				{
					if (!oneshot)
					{
						source.Play();
					}
					else
					{
						source.PlayOneShot(clip);
					}
				}
			}
			else if (stopEvent == StopEvent.triggerEnter)
			{
				if (AudioManager.Instance.useFMOD)
				{
					if (!oneshot)
					{
						sfx.audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
					}
				}
				else
				{
					source.Stop();
				}
			}
		}
		else if (newColliderCount == oldColliderCount) //Trigger Stay
		{

		}
		else if (newColliderCount < oldColliderCount) //Trigger Exit
		{
			if (startEvent == StartEvent.triggerExit)
			{
				if (AudioManager.Instance.useFMOD)
				{
					//Play audio
					if (!oneshot)
					{
						Setup();
						sfx.audioInstance.start();
					}
					else
					{
						if (!attached)
						{
							if (sfx.paramName != string.Empty)
							{
								Setup();
								sfx.audioInstance.start();
								sfx.audioInstance.release();
							}
							else
							{
								RuntimeManager.PlayOneShot(sfx.currentAudioPath, transform.position);
							}
						}
						else
						{
							if (sfx.paramName != string.Empty)
							{
								Setup();
								sfx.audioInstance.start();
								sfx.audioInstance.release();
							}
							else
							{
								RuntimeManager.PlayOneShotAttached(sfx.currentAudioPath, gameObject);
							}
						}
					}
				}
				else
				{
					if (!oneshot)
					{
						source.Play();
					}
					else
					{
						source.PlayOneShot(clip);
					}
				}
			}
			else if (stopEvent == StopEvent.triggerExit)
			{
				if (AudioManager.Instance.useFMOD)
				{
					if (!oneshot)
					{
						sfx.audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
					}
				}
				else
				{
					source.Stop();
				}
			}
		}
	}

	void OnDestroy()
	{
		if (startEvent == StartEvent.destroyed)
		{
			if (AudioManager.Instance.useFMOD)
			{
				Setup();
				sfx.audioInstance.start();
				sfx.audioInstance.release();
			}
			else
			{
				source.PlayOneShot(clip);
			}
		}
		if (stopEvent == StopEvent.destroyed)
		{
			if (AudioManager.Instance.useFMOD)
			{
				sfx.audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				source.Stop();
			}
		}
	}

	void OnCollisionEnter2D()
	{
		if (startEvent == StartEvent.collisionEnter)
		{
			if (AudioManager.Instance.useFMOD)
			{
				if (!oneshot)
				{
					Setup();
					sfx.audioInstance.start();
				}
				else
				{
					if (!attached)
					{
						if (sfx.paramName != string.Empty)
						{
							Setup();
							sfx.audioInstance.start();
							sfx.audioInstance.release();
						}
						else
						{
							RuntimeManager.PlayOneShot(sfx.currentAudioPath, transform.position);
						}
					}
					else
					{
						if (sfx.paramName != string.Empty)
						{
							Setup();
							sfx.audioInstance.start();
							sfx.audioInstance.release();
						}
						else
						{
							RuntimeManager.PlayOneShotAttached(sfx.currentAudioPath, gameObject);
						}
					}
				}
			}
			else
			{
				if (!oneshot)
				{
					source.Play();
				}
				else
				{
					source.PlayOneShot(clip);
				}
			}
		}
		else if (stopEvent == StopEvent.collisionEnter)
		{
			if (AudioManager.Instance.useFMOD)
			{
				sfx.audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				source.Stop();
			}
		}
	}

	void OnCollisionStay2D()
	{
		if (startEvent == StartEvent.collisionStay)
		{
			if (AudioManager.Instance.useFMOD)
			{
				if (!oneshot)
				{
					Setup();
					sfx.audioInstance.start();
				}
				else
				{
					if (!attached)
					{
						if (sfx.paramName != string.Empty)
						{
							Setup();
							sfx.audioInstance.start();
							sfx.audioInstance.release();
						}
						else
						{
							RuntimeManager.PlayOneShot(sfx.currentAudioPath, transform.position);
						}
					}
					else
					{
						if (sfx.paramName != string.Empty)
						{
							Setup();
							sfx.audioInstance.start();
							sfx.audioInstance.release();
						}
						else
						{
							RuntimeManager.PlayOneShotAttached(sfx.currentAudioPath, gameObject);
						}
					}
				}
			}
			else
			{
				if (!oneshot)
				{
					source.Play();
				}
				else
				{
					source.PlayOneShot(clip);
				}
			}
		}
		else if (stopEvent == StopEvent.collisionStay)
		{
			if (AudioManager.Instance.useFMOD)
			{
				sfx.audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				source.Stop();
			}
		}
	}

	void OnCollisionExit2D()
	{
		if (startEvent == StartEvent.collisionExit)
		{
			if (AudioManager.Instance.useFMOD)
			{
				if (!oneshot)
				{
					Setup();
					sfx.audioInstance.start();
				}
				else
				{
					if (!attached)
					{
						if (sfx.paramName != string.Empty)
						{
							Setup();
							sfx.audioInstance.start();
							sfx.audioInstance.release();
						}
						else
						{
							RuntimeManager.PlayOneShot(sfx.currentAudioPath, transform.position);
						}
					}
					else
					{
						if (sfx.paramName != string.Empty)
						{
							Setup();
							sfx.audioInstance.start();
							sfx.audioInstance.release();
						}
						else
						{
							RuntimeManager.PlayOneShotAttached(sfx.currentAudioPath, gameObject);
						}
					}
				}
			}
			else
			{
				if (!oneshot)
				{
					source.Play();
				}
				else
				{
					source.PlayOneShot(clip);
				}
			}
		}
		else if (stopEvent == StopEvent.collisionExit)
		{
			if (AudioManager.Instance.useFMOD)
			{
				sfx.audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				source.Stop();
			}
		}
	}

	public void PlayAudioOneShot(bool attached)
	{
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
			Setup();
			sfx.audioInstance.start();
			sfx.audioInstance.release();
		}
		else
		{
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

	public bool IsAudioPathNull()
	{
		return sfx.currentAudioPath == string.Empty;
	}

	public void SetAudioClip(AudioClip clip)
	{
		source.clip = clip;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere((Vector2)transform.position, 1f);
	}
}