using System;
using UnityEngine;
using UnityEngine.Animations;

[Serializable]
public class AnimationType
{
	public AnimationEventType eventType;
	public AnimationTriggerType triggerType;
	[Tooltip("Animation Trigger")] public string animationTrigger;
	[Tooltip("Animation Trigger For Secondary Animation, leave empty if unnecessary")] public string secondAnimationTrigger;
	[Tooltip("Animation Trigger Name")] public string animationName;
	[Tooltip("Animation Active State")] public bool animationActiveState;
	[Tooltip("Let the animation control the motion")] public bool ApplyRootMotion;
	[Tooltip("Animation Value")] public float animationValue;
	[Tooltip("Animation Value For Secondary Animation, leave empty if unnecessary")] public float secondAnimationValue;
	[Tooltip("Initial Animation Delay")] public float initialAnimationDelay;
	public float NextAnimation
	{
		get
		{
			return nextAnimation;
		}
		set
		{
			nextAnimation = value;
		}
	}
	private float nextAnimation;
	[Tooltip("Delay Until Next Animation Cycle")] public float animationInterval;
	public bool OnFirstAnimation
	{
		get
		{
			return onFirstAnimation;
		}
	}
	private bool onFirstAnimation;

	public void SwitchedAnimation()
	{
		onFirstAnimation = !onFirstAnimation;
	}
}

public enum AnimationEventType
{
	update, triggerEnter, triggerStay, triggerExit, collisionEnter, collisionStay, collisionExit
}

public enum AnimationTriggerType
{
	trigger, boolean, floating
}