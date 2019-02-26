using System;
using UnityEngine;
using UnityEngine.Animations;

[Serializable]
public class AnimationType
{
	public AnimationEventType eventType;
	public AnimationTriggerType triggerType;
	public AnimationEventBehaviour eventBehaviour;
	[Tooltip("Animation Trigger On")] public string animationTriggerOn;
	[Tooltip("Animation Trigger Off")] public string animationTriggerOff;
	[Tooltip("Animation Name")] public string animationName;
	[Tooltip("Animation Active State")] public bool animationActiveState;
	[Tooltip("Animation Min Value")] public float animationValueMin;
	[Tooltip("Animation Max Value")] public float animationValueMax;
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
	public float AnimationTimer
	{
		get
		{
			return animationTimer;
		}
		set
		{
			animationTimer = value;
		}
	}
	private float animationTimer;
	[Tooltip("Delay Until Next Animation Cycle")] public float animationInterval;
}

public enum AnimationEventType
{
	update, triggerEnter, triggerStay, triggerExit, collisionEnter, collisionStay, collisionExit
}

public enum AnimationEventBehaviour
{
	setMin, setMax, setOn, setOff, setNext
}

public enum AnimationTriggerType
{
	trigger, boolean, floating
}