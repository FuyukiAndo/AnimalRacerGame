using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using FMODUnity;
using FMOD;
using FMOD.Studio;

public class AudioEffectController : MonoBehaviour {

	[SerializeField] private bool playEffectAuto, oneshot, attached;

	public enum StartEvent
	{
		none, start, update, collisionEnter, collisionStay, collisionExit, triggerEnter, triggerExit, destroyed
	}
	public StartEvent startEvent;
	public enum StopEvent
	{
		none, collisionEnter, collisionStay, collisionExit, triggerEnter, triggerExit, destroyed
	}
	public StopEvent stopEvent;

	[SerializeField] private float AliveTime, waitTime;
	[EventRef] [SerializeField] private string sfxPath;
	private EventInstance sfxInstance;
	private float nextWait;
	private Collider2D[] colliders;
	private int oldColliderCount, newColliderCount;

	[SerializeField] private AudioClip clip;
	private AudioSource source;

	[SerializeField] private Vector2 boxSize = new Vector2(1f, 1f);

	void Start()
	{
		if (AudioManager.Instance.useFMOD)
		{
			sfxInstance = RuntimeManager.CreateInstance(sfxPath);
			RuntimeManager.AttachInstanceToGameObject(sfxInstance, transform, new Rigidbody2D());
			if (playEffectAuto)
			{
				if (startEvent == StartEvent.start)
				{
					//Play audio
					if (!oneshot)
					{
						sfxInstance.start();
						StartCoroutine(AliveCountDown());
					}
					else
					{
						if (!attached)
						{
							RuntimeManager.PlayOneShot(sfxPath, transform.position);
						}
						else
						{
							RuntimeManager.PlayOneShotAttached(sfxPath, gameObject);
						}
					}
				}
			}
			else
			{
				startEvent = StartEvent.none;
			}
		}
		else
		{
			source = GetComponent<AudioSource>();
			if (playEffectAuto)
			{
				if (startEvent == StartEvent.update)
				{
					//Play audio
					if (!oneshot)
					{
						source.Play();
						StartCoroutine(AliveCountDown());
					}
					else
					{
						source.PlayOneShot(clip);
					}
					nextWait = Time.time + waitTime;
				}
			}
		}
	}

	void Update()
	{
		if (playEffectAuto)
		{
			if (startEvent == StartEvent.update && Time.time > nextWait)
			{
				if (AudioManager.Instance.useFMOD)
				{
					//Play audio
					if (!oneshot)
					{
						sfxInstance.start();
						StartCoroutine(AliveCountDown());
					}
					else
					{
						if (!attached)
						{
							RuntimeManager.PlayOneShot(sfxPath, transform.position);
						}
						else
						{
							RuntimeManager.PlayOneShotAttached(sfxPath, gameObject);
						}
					}
				}
				else
				{
					if (!oneshot)
					{
						source.Play();
						StartCoroutine(AliveCountDown());
					}
					else
					{
						source.PlayOneShot(clip);
					}
				}
				nextWait = Time.time + waitTime;
			}
		}
		else
		{
			startEvent = StartEvent.none;
		}

		//Trigger event replacement
		oldColliderCount = newColliderCount;
		colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);
		newColliderCount = colliders.Length;
		if (playEffectAuto)
		{
			if (newColliderCount > oldColliderCount) //Trigger Enter
			{
				if (startEvent == StartEvent.triggerEnter)
				{
					if (AudioManager.Instance.useFMOD)
					{
						//Play audio
						if (!oneshot)
						{
							sfxInstance.start();
							StartCoroutine(AliveCountDown());
						}
						else
						{
							if (!attached)
							{
								RuntimeManager.PlayOneShot(sfxPath, transform.position);
							}
							else
							{
								RuntimeManager.PlayOneShotAttached(sfxPath, gameObject);
							}
						}
					}
					else
					{
						if (!oneshot)
						{
							source.Play();
							StartCoroutine(AliveCountDown());
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
							sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
							sfxInstance.start();
							StartCoroutine(AliveCountDown());
						}
						else
						{
							if (!attached)
							{
								RuntimeManager.PlayOneShot(sfxPath, transform.position);
							}
							else
							{
								RuntimeManager.PlayOneShotAttached(sfxPath, gameObject);
							}
						}
					}
					else
					{
						if (!oneshot)
						{
							source.Play();
							StartCoroutine(AliveCountDown());
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
							sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
						}
					}
					else
					{
						source.Stop();
					}
				}
			}
		}
	}

	void OnDestroy()
	{
		if (startEvent == StartEvent.destroyed)
		{
			sfxInstance.start();
		}
		if (stopEvent == StopEvent.destroyed)
		{
			if (AudioManager.Instance.useFMOD)
			{
				sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				source.Stop();
			}
		}
	}

	void OnCollisionEnter2D()
	{
		if (playEffectAuto)
		{
			if (startEvent == StartEvent.collisionEnter)
			{
				if (AudioManager.Instance.useFMOD)
				{
					if (!oneshot)
					{
						sfxInstance.start();
						StartCoroutine(AliveCountDown());
					}
					else
					{
						if (!attached)
						{
							RuntimeManager.PlayOneShot(sfxPath, transform.position);
						}
						else
						{
							RuntimeManager.PlayOneShotAttached(sfxPath, gameObject);
						}
					}
				}
				else
				{
					if (!oneshot)
					{
						source.Play();
						StartCoroutine(AliveCountDown());
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
					sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				}
				else
				{
					source.Stop();
				}
			}
		}
		else
		{
			startEvent = StartEvent.none;
		}
	}

	void OnCollisionStay2D()
	{
		if (playEffectAuto)
		{
			if (startEvent == StartEvent.collisionStay)
			{
				if (AudioManager.Instance.useFMOD)
				{
					if (!oneshot)
					{
						sfxInstance.start();
						StartCoroutine(AliveCountDown());
					}
					else
					{
						if (!attached)
						{
							RuntimeManager.PlayOneShot(sfxPath, transform.position);
						}
						else
						{
							RuntimeManager.PlayOneShotAttached(sfxPath, gameObject);
						}
					}
				}
				else
				{
					if (!oneshot)
					{
						source.Play();
						StartCoroutine(AliveCountDown());
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
					sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				}
				else
				{
					source.Stop();
				}
			}
		}
		else
		{
			startEvent = StartEvent.none;
		}
	}

	void OnCollisionExit2D()
	{
		if (playEffectAuto)
		{
			if (startEvent == StartEvent.collisionExit)
			{
				if (AudioManager.Instance.useFMOD)
				{
					if (!oneshot)
					{
						sfxInstance.start();
						StartCoroutine(AliveCountDown());
					}
					else
					{
						if (!attached)
						{
							RuntimeManager.PlayOneShot(sfxPath, transform.position);
						}
						else
						{
							RuntimeManager.PlayOneShotAttached(sfxPath, gameObject);
						}
					}
				}
				else
				{
					if (!oneshot)
					{
						source.Play();
						StartCoroutine(AliveCountDown());
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
					sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				}
				else
				{
					source.Stop();
				}
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

	public void PlayAudioOneShot()
	{
		source.PlayOneShot(clip);
	}

	public void SetAudioVolume(float volume)
	{
		if (AudioManager.Instance.useFMOD)
		{
			sfxInstance.setVolume(volume);
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
			FMOD.RESULT result = sfxInstance.getVolume(out volume, out finalVolume);
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

	public void KillAudio()
	{
		if (AudioManager.Instance.useFMOD)
		{
			sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
		else
		{
			source.volume = 0.0f;
		}
		Destroy(gameObject);
	}

	IEnumerator AliveCountDown()
	{
		while (AliveTime > 0f)
		{
			AliveTime -= Time.deltaTime;
			yield return null;
		}
		if (AudioManager.Instance.useFMOD)
		{
			sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
		else
		{
			source.volume = 0.0f;
		}
	}

}
