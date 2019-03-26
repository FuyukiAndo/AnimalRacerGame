using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{

	[SerializeField] private Speech[] speeches;
	[SerializeField] private GameObject speechBubble, speechTextUI;
	[SerializeField] private float speechSpeedMult = .4f;
	[SerializeField] private int minSpeechSizeToIgnoreSpeedMult = 8;
	[SerializeField] private bool isCommentator;
	[SerializeField] private CommentatorSpeech[] commentatorSpeeches;
	[SerializeField] private string levelCompleteSpeech;
	[SerializeField] private float commentingDelay;
	[SerializeField] private GameObject commentatorSpeechBubble, commentatorSpeechText, nibiPortrait;
	private float nextComment, actualCommentDelay;
	private TextMeshProUGUI commentatorText, textUI;
	private bool disruptUpdateCommenting = false;

	void Start()
	{
		if (isCommentator)
		{
			commentatorText = commentatorSpeechText.GetComponent<TextMeshProUGUI>();
			commentatorSpeechBubble.SetActive(false);
			nibiPortrait.SetActive(false);
		}
		else
		{
			textUI = speechTextUI.GetComponent<TextMeshProUGUI>();
			speechBubble.SetActive(false);
		}
	}

	void Update()
	{
		if (!disruptUpdateCommenting && commentatorSpeeches.Length > 0 && isCommentator)
		{
			if (Time.time > nextComment)
			{
				SetCommentatorSpeechActive(false, CommentatorSpeechType.none);
				actualCommentDelay = commentingDelay + (commentatorText.text.Length * speechSpeedMult);
				nextComment = Time.time + actualCommentDelay;
			}
		}
	}

	public void SetSpeechActive(SpeechType speechType, PlayerCharacterType playerType)
	{
		SetRandomSpeechFromType(speechType, playerType);
		speechBubble.SetActive(true);
		StartCoroutine(SetSpeechInactive());
	}

	public void SetCommentatorSpeechActive(bool levelComplete, CommentatorSpeechType type)
	{
		if (levelComplete)
		{
			commentatorText.text = levelCompleteSpeech;
			disruptUpdateCommenting = true;
		}
		else
		{
			SetRandomSpeechFromCommentator(type);
		}
		if (commentatorText.text == string.Empty)
		{
			return;
		}
		commentatorSpeechBubble.SetActive(true);
		nibiPortrait.SetActive(true);
		StartCoroutine(SetSpeechInactive());
	}

	public void SetCommentatorSpeechActive(CommentatorSpeechType type, string nameA, string nameB)
	{
		SetRandomSpeechFromCommentator(type, nameA, nameB);
		commentatorSpeechBubble.SetActive(true);
		nibiPortrait.SetActive(true);
		StartCoroutine(SetSpeechInactive());
	}

	public void SetCommentatorSpeechActive(CommentatorSpeechType type, string name)
	{
		SetRandomSpeechFromCommentator(type, name);
		commentatorSpeechBubble.SetActive(true);
		nibiPortrait.SetActive(true);
		StartCoroutine(SetSpeechInactive());
	}

	void SetRandomSpeechFromType(SpeechType speechType, PlayerCharacterType playerType)
	{
		Speech speech = speeches.Where(tempSpeech => tempSpeech.speechType == speechType && tempSpeech.playerCharacterType == playerType).FirstOrDefault();
		int rand = Random.Range(0, speech.speeches.Length);
		textUI.text = speech.speeches[rand];
	}

	void SetRandomSpeechFromCommentator(CommentatorSpeechType type)
	{
		if (type != CommentatorSpeechType.none)disruptUpdateCommenting = true;
		List<CommentatorSpeech> speeches = commentatorSpeeches.ToList();
		CommentatorSpeech speechSearch = speeches.Find(x => x.speechType == type);
		if (speechSearch == null)return;
		CommentatorSpeech speech = commentatorSpeeches.Where(tempSpeech => tempSpeech.speechType == type).FirstOrDefault();
		int rand = Random.Range(0, speech.speeches.Length);
		commentatorText.text = speech.speeches[rand];
	}

	void SetRandomSpeechFromCommentator(CommentatorSpeechType type, string nameA, string nameB)
	{
		List<CommentatorSpeech> speeches = commentatorSpeeches.ToList();
		CommentatorSpeech speechSearch = speeches.Find(x => x.speechType == type);
		if (speechSearch == null)return;
		CommentatorSpeech speech = commentatorSpeeches.Where(tempSpeech => tempSpeech.speechType == type).FirstOrDefault();
		int rand = Random.Range(0, speech.speeches.Length);
		switch (nameA)
		{
			case "Player1":
				nameA = "Player 1";
				break;
			case "Player2":
				nameA = "Player 2";
				break;
			case "Player3":
				nameA = "Player 3";
				break;
			case "Player4":
				nameA = "Player 4";
				break;
		}
		switch (nameB)
		{
			case "Player1":
				nameA = "Player 1";
				break;
			case "Player2":
				nameA = "Player 2";
				break;
			case "Player3":
				nameA = "Player 3";
				break;
			case "Player4":
				nameA = "Player 4";
				break;
		}
		commentatorText.text = speech.speeches[rand].Replace("@", nameA).Replace("@@", nameB);
	}

	void SetRandomSpeechFromCommentator(CommentatorSpeechType type, string name)
	{
		List<CommentatorSpeech> speeches = commentatorSpeeches.ToList();
		CommentatorSpeech speechSearch = speeches.Find(x => x.speechType == type);
		if (speechSearch == null)return;
		CommentatorSpeech speech = commentatorSpeeches.Where(tempSpeech => tempSpeech.speechType == type).FirstOrDefault();
		int rand = Random.Range(0, speech.speeches.Length);
		switch (name)
		{
			case "Player1":
				name = "Player 1";
				break;
			case "Player2":
				name = "Player 2";
				break;
			case "Player3":
				name = "Player 3";
				break;
			case "Player4":
				name = "Player 4";
				break;
		}
		commentatorText.text = speech.speeches[rand].Replace("@", name);
	}

	IEnumerator SetSpeechInactive()
	{
		if (isCommentator)
		{
			if (commentatorText.text.Length > minSpeechSizeToIgnoreSpeedMult)
			{
				yield return new WaitForSeconds(commentatorText.text.Length * speechSpeedMult);
			}
			else
			{
				yield return new WaitForSeconds(commentatorText.text.Length);
			}
			commentatorText.text = string.Empty;
			commentatorSpeechBubble.SetActive(false);
			nibiPortrait.SetActive(false);
			if (disruptUpdateCommenting)disruptUpdateCommenting = false;
		}
		else
		{
			if (textUI.text.Length > minSpeechSizeToIgnoreSpeedMult)
			{
				yield return new WaitForSeconds(textUI.text.Length * speechSpeedMult);
			}
			else
			{
				yield return new WaitForSeconds(textUI.text.Length);
			}
			textUI.text = string.Empty;
			speechBubble.SetActive(false);
		}
	}

}

[System.Serializable]
public class Speech
{
	public SpeechType speechType;
	public PlayerCharacterType playerCharacterType;
	public string[] speeches;
}

[System.Serializable]
public class CommentatorSpeech
{
	public CommentatorSpeechType speechType;
	public string[] speeches;
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

public enum CommentatorSpeechType
{
	oneAll,
	oneTwo,
	oneThree,
	oneOneLeft,
	twoAll,
	twoTwo,
	twoThree,
	twoOneLeft,
	multipleAll,
	multipleTwo,
	multipleThree,
	multipleOneLeft,
	none
}