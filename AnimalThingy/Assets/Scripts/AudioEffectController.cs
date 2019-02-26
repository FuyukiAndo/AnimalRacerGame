using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	void Start()
	{
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

	void Update()
	{
		if (playEffectAuto)
		{
			if (startEvent == StartEvent.update && Time.time > nextWait)
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
				nextWait = Time.time + waitTime;
			}
		}
		else
		{
			startEvent = StartEvent.none;
		}

		//Trigger event replacement
		oldColliderCount = newColliderCount;
		colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1f, 1f), 0f);
		newColliderCount = colliders.Length;
		if (playEffectAuto)
		{
			if (newColliderCount > oldColliderCount) //Trigger Enter
			{
				if (startEvent == StartEvent.triggerEnter)
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
				else if (stopEvent == StopEvent.triggerEnter)
				{
					if (!oneshot)
					{
						sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
				else if (stopEvent == StopEvent.triggerExit)
				{
					if (!oneshot)
					{
						sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
			sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}

	void OnCollisionEnter2D()
	{
		if (playEffectAuto)
		{
			if (startEvent == StartEvent.collisionEnter)
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
			else if (stopEvent == StopEvent.collisionEnter)
			{
				sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
			else if (stopEvent == StopEvent.collisionStay)
			{
				sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
			else if (stopEvent == StopEvent.collisionExit)
			{
				sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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

	IEnumerator AliveCountDown()
	{
		while (AliveTime > 0f)
		{
			AliveTime -= Time.deltaTime;
			yield return null;
		}
		sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

}
