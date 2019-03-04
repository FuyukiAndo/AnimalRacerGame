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
	private bool queveUpdateAnimation;

	void Start()
	{
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
		if (EventAnimationStopped())
		{
			queveUpdateAnimation = false;
		}
		if (forUpdate != null && !queveUpdateAnimation)
		{
			if (Time.time < forUpdate.initialAnimationDelay)
			{
				return;
			}
			if (Time.time >= forUpdate.NextAnimation)
			{
				switch (forUpdate.triggerType)
				{
					case AnimationTriggerType.boolean:
						SetAnimatorBool(forUpdate.animationName,
						forUpdate.OnFirstAnimation ? forUpdate.animationActiveState : !forUpdate.animationActiveState);
						break;
					case AnimationTriggerType.floating:
						if (forUpdate.secondAnimationValue != 0.0f)
						{
							SetAnimatorFloat(forUpdate.animationName,
						forUpdate.OnFirstAnimation ? forUpdate.animationValue : forUpdate.secondAnimationValue);
						}
						else
						{
							SetAnimatorFloat(forUpdate.animationName, forUpdate.animationValue);
						}
						break;
					case AnimationTriggerType.trigger:
						if (forUpdate.secondAnimationTrigger != null)
						{
							SetAnimatorTrigger(
							forUpdate.OnFirstAnimation ? forUpdate.animationTrigger : forUpdate.secondAnimationTrigger);
						}
						else
						{
							SetAnimatorTrigger(forUpdate.animationTrigger);
						}
						break;
				}
				forUpdate.SwitchedAnimation();
				forUpdate.NextAnimation = Time.time + forUpdate.animationInterval;
			}
		}
	}

	void OnCollisionEnter2D()
	{
		queveUpdateAnimation = true;
		if (forCollisionEnter != null)
		{
			switch (forCollisionEnter.triggerType)
			{
				case AnimationTriggerType.boolean:
					SetAnimatorBool(forCollisionEnter.animationName, forCollisionEnter.animationActiveState);
					break;
				case AnimationTriggerType.floating:
					SetAnimatorFloat(forCollisionEnter.animationName, forCollisionEnter.animationValue);
					break;
				case AnimationTriggerType.trigger:
					SetAnimatorTrigger(forCollisionEnter.animationTrigger);
					break;
			}
		}
	}

	void OnCollisionStay2D()
	{
		queveUpdateAnimation = true;
		if (forCollisionStay != null)
		{
			switch (forCollisionStay.triggerType)
			{
				case AnimationTriggerType.boolean:
					SetAnimatorBool(forCollisionStay.animationName, forCollisionStay.animationActiveState);
					break;
				case AnimationTriggerType.floating:
					SetAnimatorFloat(forCollisionStay.animationName, forCollisionStay.animationValue);
					break;
				case AnimationTriggerType.trigger:
					SetAnimatorTrigger(forCollisionStay.animationTrigger);
					break;
			}
		}
	}

	void OnCollisionExit2D()
	{
		queveUpdateAnimation = true;
		if (forCollisionExit != null)
		{
			switch (forCollisionExit.triggerType)
			{
				case AnimationTriggerType.boolean:
					SetAnimatorBool(forCollisionExit.animationName, forCollisionExit.animationActiveState);
					break;
				case AnimationTriggerType.floating:
					SetAnimatorFloat(forCollisionExit.animationName, forCollisionExit.animationValue);
					break;
				case AnimationTriggerType.trigger:
					SetAnimatorTrigger(forCollisionExit.animationTrigger);
					break;
			}
		}
	}

	void OnTriggerEnter2D()
	{
		queveUpdateAnimation = true;
		if (forTriggerEnter != null)
		{
			switch (forTriggerEnter.triggerType)
			{
				case AnimationTriggerType.boolean:
					SetAnimatorBool(forTriggerEnter.animationName, forTriggerEnter.animationActiveState);
					break;
				case AnimationTriggerType.floating:
					SetAnimatorFloat(forTriggerEnter.animationName, forTriggerEnter.animationValue);
					break;
				case AnimationTriggerType.trigger:
					SetAnimatorTrigger(forTriggerEnter.animationTrigger);
					break;
			}
		}
	}

	void OnTriggerStay2D()
	{
		queveUpdateAnimation = true;
		if (forTriggerStay != null)
		{
			switch (forTriggerStay.triggerType)
			{
				case AnimationTriggerType.boolean:
					SetAnimatorBool(forTriggerStay.animationName, forTriggerStay.animationActiveState);
					break;
				case AnimationTriggerType.floating:
					SetAnimatorFloat(forTriggerStay.animationName, forTriggerStay.animationValue);
					break;
				case AnimationTriggerType.trigger:
					SetAnimatorTrigger(forTriggerStay.animationTrigger);
					break;
			}
		}
	}

	void OnTriggerExit2D()
	{
		queveUpdateAnimation = true;
		if (forTriggerExit != null)
		{
			switch (forTriggerExit.triggerType)
			{
				case AnimationTriggerType.boolean:
					SetAnimatorBool(forTriggerExit.animationName, forTriggerExit.animationActiveState);
					break;
				case AnimationTriggerType.floating:
					SetAnimatorFloat(forTriggerExit.animationName, forTriggerExit.animationValue);
					break;
				case AnimationTriggerType.trigger:
					SetAnimatorTrigger(forTriggerExit.animationTrigger);
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

	public void SetApplyRootMotion(bool state)
	{
		animator.applyRootMotion = state;
	}

	private bool EventAnimationStopped()
	{
		return animator.IsInTransition(0);
	}
}