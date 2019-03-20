using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{

	[SerializeField] private Speech[] speeches;
	[SerializeField] private GameObject speechBubble, speechTextUI, characterCrystal;
	[SerializeField] private float speechSpeedMult = .4f;
	[SerializeField] private bool isCommentator, isUi;
	[SerializeField] private CommentatorSpeech commentator;
	private TextMeshPro textMesh;
	private TextMeshProUGUI commentatorText, textUI;

	void Start()
	{
		if (!isUi && isCommentator)
		{
			commentatorText = commentator.commentatorSpeechBubble.GetComponent<TextMeshProUGUI>();
			commentatorText.autoSizeTextContainer = true;
			commentator.commentatorSpeechBubble.SetActive(false);
			commentator.commentatorSpeechImage.SetActive(false);
		}
		else if (!isUi && !isCommentator)
		{
			textMesh = speechBubble.GetComponent<TextMeshPro>();
			textMesh.autoSizeTextContainer = true;
			characterCrystal.SetActive(true);
			speechBubble.SetActive(false);
		}
		else if (isUi && !isCommentator)
		{
			textUI = speechTextUI.GetComponent<TextMeshProUGUI>();
			textUI.autoSizeTextContainer = true;
			speechBubble.SetActive(false);
		}
	}

	void Update()
	{
		if (commentator.speeches.Length > 0 && isCommentator)
		{
			if (Time.time > commentator.nextComment)
			{
				SetCommentatorSpeechActive(false);
			}
		}
	}

	public void SetSpeechActive(SpeechType type)
	{
		SetRandomSpeechFromType(type);
		if (!isUi)
		{
			characterCrystal.SetActive(false);
			speechBubble.SetActive(true);
		}
		else
		{
			speechBubble.SetActive(true);
		}
		StartCoroutine(SetSpeechInactive());
	}

	public void SetCommentatorSpeechActive(bool levelComplete)
	{
		if (levelComplete)
		{
			commentatorText.text = commentator.levelCompleteSpeech;
		}
		else
		{
			SetRandomSpeechFromCommentator();
		}
		commentator.commentatorSpeechImage.SetActive(true);
		commentator.commentatorSpeechBubble.SetActive(true);
		StartCoroutine(SetSpeechInactive());
		commentator.nextComment = Time.time + commentator.commentingDelay;
	}

	void SetRandomSpeechFromType(SpeechType type)
	{
		Speech speech;
		speech = (Speech)speeches.Where(tempSpeech => tempSpeech.speechType == type);
		int rand = Random.Range(0, speech.speeches.Length - 1);
		if (!isUi)
		{
			textMesh.text = speech.speeches[rand];
		}
		else
		{
			textUI.text = speech.speeches[rand];
		}
	}

	void SetRandomSpeechFromCommentator()
	{
		int rand = Random.Range(0, commentator.speeches.Length - 1);
		commentatorText.text = commentator.speeches[rand];
	}

	IEnumerator SetSpeechInactive()
	{
		if (isCommentator)
		{
			yield return new WaitForSeconds(commentatorText.text.Length * speechSpeedMult);
			commentatorText.text = string.Empty;
			commentator.commentatorSpeechBubble.SetActive(false);
			commentator.commentatorSpeechImage.SetActive(false);
		}
		else
		{
			if (!isUi)
			{
				yield return new WaitForSeconds(textMesh.text.Length * speechSpeedMult);
				textMesh.text = string.Empty;
				speechBubble.SetActive(false);
				characterCrystal.SetActive(true);
			}
			else
			{
				yield return new WaitForSeconds(textUI.text.Length * speechSpeedMult);
				textUI.text = string.Empty;
				speechBubble.SetActive(false);
			}
		}
	}

}

[System.Serializable]
public class Speech
{
	public SpeechType speechType;
	public string[] speeches;
}

[System.Serializable]
public class CommentatorSpeech
{
	public string[] speeches;
	public string levelCompleteSpeech;
	public float commentingDelay;
	public GameObject commentatorSpeechBubble;
	public GameObject commentatorSpeechImage;
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