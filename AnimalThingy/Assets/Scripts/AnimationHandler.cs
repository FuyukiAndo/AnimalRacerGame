using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class AnimationHandler : MonoBehaviour
{
	[SerializeField] private bool autoAnimate;
	[SerializeField] private Animator animator;
	[Tooltip("One animation per event type")] [SerializeField] private AnimationType[] animationType;
	private AnimationType forUpdate, forCollisionEnter, forCollisionExit, forCollisionStay, forTriggerEnter,
	forTriggerStay, forTriggerExit;
	private AnimationTriggerType triggerType;
	private AnimationEventBehaviour eventBehaviour;

	public static AnimationHandler Instance
	{
		get
		{
			return instance;
		}
	}
	private static AnimationHandler instance;

	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		if (animator == null)
		{
			animator = GetComponent<Animator>();
		}
		if (autoAnimate)
		{
			foreach (var anim in animationType)
			{
				if (anim.eventType == AnimationEventType.update)
				{
					forUpdate = anim;
				}
				else if (anim.eventType == AnimationEventType.collisionEnter)
				{
					forCollisionEnter = anim;
				}
				else if (anim.eventType == AnimationEventType.collisionStay)
				{
					forCollisionStay = anim;
				}
				else if (anim.eventType == AnimationEventType.collisionExit)
				{
					forCollisionExit = anim;
				}
				else if (anim.eventType == AnimationEventType.triggerEnter)
				{
					forTriggerEnter = anim;
				}
				else if (anim.eventType == AnimationEventType.triggerStay)
				{
					forTriggerStay = anim;
				}
				else if (anim.eventType == AnimationEventType.triggerExit)
				{
					forTriggerExit = anim;
				}
			}
		}
	}

	void Update()
	{
		if (forUpdate != null)
		{
			forUpdate.AnimationTimer += Time.deltaTime;
			if (forUpdate.AnimationTimer >= forUpdate.NextAnimation)
			{
				switch (triggerType)
				{
					case AnimationTriggerType.boolean:
						switch (eventBehaviour)
						{
							case AnimationEventBehaviour.setOn:
								SetAnimatorBool(forUpdate.animationName, forUpdate.animationActiveState);
								break;
							case AnimationEventBehaviour.setOff:
								SetAnimatorBool(forUpdate.animationName, forUpdate.animationActiveState);
								break;
						}
						break;
					case AnimationTriggerType.floating:
						switch (eventBehaviour)
						{
							case AnimationEventBehaviour.setMin:
								SetAnimatorFloat(forUpdate.animationName, forUpdate.animationValueMin);
								break;
							case AnimationEventBehaviour.setMax:
								SetAnimatorFloat(forUpdate.animationName, forUpdate.animationValueMax);
								break;
						}
						break;
					case AnimationTriggerType.trigger:
						switch (eventBehaviour)
						{
							case AnimationEventBehaviour.setOn:
								SetAnimatorTrigger(forUpdate.animationTriggerOn);
								break;
							case AnimationEventBehaviour.setOff:
								SetAnimatorTrigger(forUpdate.animationTriggerOff);
								break;
						}
						break;
				}
				forUpdate.NextAnimation = Time.time + forUpdate.animationInterval;
			}
		}
		/*if (continuously)
		{
			animationTimer += Time.deltaTime;
			if (animationTimer >= nextAnimation)
			{
				//PlayAnimationOnce();
				nextAnimation = Time.time + animationInterval;
			}
		}*/
	}

	void OnCollisionEnter2D()
	{
		if (forCollisionEnter != null)
		{
			switch (triggerType)
			{
				case AnimationTriggerType.boolean:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setOn:
							SetAnimatorBool(forCollisionEnter.animationName, forCollisionEnter.animationActiveState);
							break;
						case AnimationEventBehaviour.setOff:
							SetAnimatorBool(forCollisionEnter.animationName, forCollisionEnter.animationActiveState);
							break;
					}
					break;
				case AnimationTriggerType.floating:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setMin:
							SetAnimatorFloat(forCollisionEnter.animationName, forCollisionEnter.animationValueMin);
							break;
						case AnimationEventBehaviour.setMax:
							SetAnimatorFloat(forCollisionEnter.animationName, forCollisionEnter.animationValueMax);
							break;
					}
					break;
				case AnimationTriggerType.trigger:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setOn:
							SetAnimatorTrigger(forCollisionEnter.animationTriggerOn);
							break;
						case AnimationEventBehaviour.setOff:
							SetAnimatorTrigger(forCollisionEnter.animationTriggerOff);
							break;
					}
					break;
			}
		}
	}

	void OnCollisionStay2D()
	{
		if (forCollisionStay != null)
		{
			switch (triggerType)
			{
				case AnimationTriggerType.boolean:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setOn:
							SetAnimatorBool(forCollisionStay.animationName, forCollisionStay.animationActiveState);
							break;
						case AnimationEventBehaviour.setOff:
							SetAnimatorBool(forCollisionStay.animationName, forCollisionStay.animationActiveState);
							break;
					}
					break;
				case AnimationTriggerType.floating:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setMin:
							SetAnimatorFloat(forCollisionStay.animationName, forCollisionStay.animationValueMin);
							break;
						case AnimationEventBehaviour.setMax:
							SetAnimatorFloat(forCollisionStay.animationName, forCollisionStay.animationValueMax);
							break;
					}
					break;
				case AnimationTriggerType.trigger:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setOn:
							SetAnimatorTrigger(forCollisionStay.animationTriggerOn);
							break;
						case AnimationEventBehaviour.setOff:
							SetAnimatorTrigger(forCollisionStay.animationTriggerOff);
							break;
					}
					break;
			}
		}
	}

	void OnCollisionExit2D()
	{
		if (forCollisionExit != null)
		{
			switch (triggerType)
			{
				case AnimationTriggerType.boolean:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setOn:
							SetAnimatorBool(forCollisionExit.animationName, forCollisionExit.animationActiveState);
							break;
						case AnimationEventBehaviour.setOff:
							SetAnimatorBool(forCollisionExit.animationName, forCollisionExit.animationActiveState);
							break;
					}
					break;
				case AnimationTriggerType.floating:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setMin:
							SetAnimatorFloat(forCollisionExit.animationName, forCollisionExit.animationValueMin);
							break;
						case AnimationEventBehaviour.setMax:
							SetAnimatorFloat(forCollisionExit.animationName, forCollisionExit.animationValueMax);
							break;
					}
					break;
				case AnimationTriggerType.trigger:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setOn:
							SetAnimatorTrigger(forCollisionExit.animationTriggerOn);
							break;
						case AnimationEventBehaviour.setOff:
							SetAnimatorTrigger(forCollisionExit.animationTriggerOff);
							break;
					}
					break;
			}
		}
	}

	void OnTriggerEnter2D()
	{
		if (forTriggerEnter != null)
		{
			switch (triggerType)
			{
				case AnimationTriggerType.boolean:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setOn:
							SetAnimatorBool(forTriggerEnter.animationName, forTriggerEnter.animationActiveState);
							break;
						case AnimationEventBehaviour.setOff:
							SetAnimatorBool(forTriggerEnter.animationName, forTriggerEnter.animationActiveState);
							break;
					}
					break;
				case AnimationTriggerType.floating:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setMin:
							SetAnimatorFloat(forTriggerEnter.animationName, forTriggerEnter.animationValueMin);
							break;
						case AnimationEventBehaviour.setMax:
							SetAnimatorFloat(forTriggerEnter.animationName, forTriggerEnter.animationValueMax);
							break;
					}
					break;
				case AnimationTriggerType.trigger:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setOn:
							SetAnimatorTrigger(forTriggerEnter.animationTriggerOn);
							break;
						case AnimationEventBehaviour.setOff:
							SetAnimatorTrigger(forTriggerEnter.animationTriggerOff);
							break;
					}
					break;
			}
		}
	}

	void OnTriggerStay2D()
	{
		if (forTriggerStay != null)
		{
			switch (triggerType)
			{
				case AnimationTriggerType.boolean:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setOn:
							SetAnimatorBool(forTriggerStay.animationName, forTriggerStay.animationActiveState);
							break;
						case AnimationEventBehaviour.setOff:
							SetAnimatorBool(forTriggerStay.animationName, forTriggerStay.animationActiveState);
							break;
					}
					break;
				case AnimationTriggerType.floating:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setMin:
							SetAnimatorFloat(forTriggerStay.animationName, forTriggerStay.animationValueMin);
							break;
						case AnimationEventBehaviour.setMax:
							SetAnimatorFloat(forTriggerStay.animationName, forTriggerStay.animationValueMax);
							break;
					}
					break;
				case AnimationTriggerType.trigger:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setOn:
							SetAnimatorTrigger(forTriggerStay.animationTriggerOn);
							break;
						case AnimationEventBehaviour.setOff:
							SetAnimatorTrigger(forTriggerStay.animationTriggerOff);
							break;
					}
					break;
			}
		}
	}

	void OnTriggerExit2D()
	{
		if (forTriggerExit != null)
		{
			switch (triggerType)
			{
				case AnimationTriggerType.boolean:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setOn:
							SetAnimatorBool(forTriggerExit.animationName, forTriggerExit.animationActiveState);
							break;
						case AnimationEventBehaviour.setOff:
							SetAnimatorBool(forTriggerExit.animationName, forTriggerExit.animationActiveState);
							break;
					}
					break;
				case AnimationTriggerType.floating:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setMin:
							SetAnimatorFloat(forTriggerExit.animationName, forTriggerExit.animationValueMin);
							break;
						case AnimationEventBehaviour.setMax:
							SetAnimatorFloat(forTriggerExit.animationName, forTriggerExit.animationValueMax);
							break;
					}
					break;
				case AnimationTriggerType.trigger:
					switch (eventBehaviour)
					{
						case AnimationEventBehaviour.setOn:
							SetAnimatorTrigger(forTriggerExit.animationTriggerOn);
							break;
						case AnimationEventBehaviour.setOff:
							SetAnimatorTrigger(forTriggerExit.animationTriggerOff);
							break;
					}
					break;
			}
		}
	}

	public void SetAnimatorTrigger(string trigger)
	{
		if (animator == null)
		{
			Debug.LogWarning("No animator for trigger!");
			return;
		}
		animator.SetTrigger(trigger);
	}

	public void SetAnimatorFloat(string name, float value)
	{
		if (animator == null)
		{
			Debug.LogWarning("No animator for float!");
			return;
		}
		animator.SetFloat(name, value);
	}

	public void SetAnimatorBool(string name, bool state)
	{
		if (animator == null)
		{
			Debug.LogWarning("No animator for bool!");
			return;
		}
		animator.SetBool(name, state);
	}
}