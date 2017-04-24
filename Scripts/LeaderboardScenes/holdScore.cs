﻿using UnityEngine;
using UnityEngine.UI;

public class holdScore : MonoBehaviour {


	[SerializeField]leaderboard _leaderboard;
	[SerializeField] Text titleText;
	private fadeOutFadein fadeIn;

	string _name;
	float score;

	void Awake()
	{
		titleText.text = string.Format("Knowledge gained:\n{0}\nYour name shall be remembered", mcStats.knowledge.ToString("N0"));
		_leaderboard = GetComponent<leaderboard>();
	}

	void Start()
	{
		score = mcStats.knowledge;
	}

	public void RegisterScore()
	{
		fadeIn = GameObject.FindGameObjectWithTag("fadeIn").GetComponent<fadeOutFadein>();         
		_name = GetComponent<InputField>().text;
		_leaderboard.GetScore(_name, score);
		fadeIn.enabled = true;
		//controller.dungeonLevel = 1;
	}
}
