using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{

	public static StartManager Instance
	{
		get
		{
			return instance;
		}
	}

	private static StartManager instance;
	[SerializeField] private GameObject[] unplacedPlayers;
	[SerializeField] private string playerMoveScript;
	public int TimeUntilStart
	{
		get
		{
			return timeUntilStart;
		}
		set
		{
			timeUntilStart = value;
		}
	}

	[SerializeField] private int timeUntilStart;
	[SerializeField] private Vector2[] playerStartPositions;
	[SerializeField] private bool startCountDownOnSceneLoad;

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
		PlacePlayers();
		TrapPlayers();
		if (startCountDownOnSceneLoad)
		{
			StartCoroutine(CountDownStart());
		}
	}

	IEnumerator CountDownStart()
	{
		while (timeUntilStart > 0)
		{
			timeUntilStart -= 1;
			yield return new WaitForSeconds(1f);
		}
		ReleasePlayers();
	}

	void ReleasePlayers()
	{
		//string componentName = playerMoveScript.GetType().ToString();
		foreach (var player in unplacedPlayers)
		{
			MonoBehaviour script = player.GetComponent(playerMoveScript)as MonoBehaviour;
			script.enabled = true;
		}
	}

	void TrapPlayers()
	{
		//string componentName = playerMoveScript.GetType().ToString();
		foreach (var player in unplacedPlayers)
		{
			MonoBehaviour script = player.GetComponent(playerMoveScript)as MonoBehaviour;
			script.enabled = false;
		}
	}

	void PlacePlayers()
	{
		int place = 0;
		foreach (var player in unplacedPlayers)
		{
			player.transform.position = playerStartPositions[place];
			place++;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		foreach (var start in playerStartPositions)
		{
			Gizmos.DrawWireSphere(start, .5f);
		}
	}
}