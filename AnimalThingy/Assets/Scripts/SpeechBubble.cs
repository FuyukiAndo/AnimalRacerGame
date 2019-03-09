using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{

	[SerializeField] private Speech[] speeches;
	[SerializeField] private GameObject speechBubble, characterCrystal;
	[SerializeField] private float speechSpeedMult = .4f;
	[SerializeField] private bool isCommentator;
	[SerializeField] private CommentatorSpeech commentator;
	private TextMeshPro textMesh;
	private SpriteRenderer sprite;

	void Start()
	{
		textMesh = speechBubble.GetComponent<TextMeshPro>();
		textMesh.autoSizeTextContainer = true;
		sprite = speechBubble.GetComponentInChildren<SpriteRenderer>();
	}

	void Update()
	{
		if (commentator.speeches.Length > 0 && isCommentator)
		{
			if (Time.time > commentator.nextComment)
			{
				SetCommentatorSpeechActive();
			}
		}
	}

	public void SetSpeechActive(SpeechType type)
	{
		SetRandomSpeechFromType(type);
		characterCrystal.SetActive(false);
		speechBubble.SetActive(true);
		StartCoroutine(SetSpeechInactive());
	}

	public void SetCommentatorSpeechActive()
	{
		SetRandomSpeechFromCommentator();
		characterCrystal.SetActive(false);
		speechBubble.SetActive(true);
		StartCoroutine(SetSpeechInactive());
		commentator.nextComment = Time.time + commentator.commentingDelay;
	}

	void SetRandomSpeechFromType(SpeechType type)
	{
		Speech speech;
		speech = (Speech)speeches.Where(tempSpeech => tempSpeech.speechType == type);
		int rand = Random.Range(0, speech.speeches.Length - 1);
		textMesh.text = speech.speeches[rand];
	}

	void SetRandomSpeechFromCommentator()
	{
		int rand = Random.Range(0, commentator.speeches.Length - 1);
		textMesh.text = commentator.speeches[rand];
	}

	IEnumerator SetSpeechInactive()
	{
		yield return new WaitForSeconds(textMesh.text.Length * speechSpeedMult);
		speechBubble.SetActive(false);
		characterCrystal.SetActive(true);
	}

}

[System.Serializable]
public class Speech
{
	public SpeechType speechType;
	public string[] speeches;
	public float commentingDelay;
	[HideInInspector] public float nextComment;
}

[System.Serializable]
public class CommentatorSpeech
{
	public string[] speeches;
	public float commentingDelay;
	[HideInInspector] public float nextComment;
}

public enum SpeechType
{
	stun,
	run,
	idle,
	victory,
	ability,
	checkpoint,
	respawn,
	start
}