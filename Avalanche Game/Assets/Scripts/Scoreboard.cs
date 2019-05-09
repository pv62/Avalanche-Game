using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    Text scoreText;
    int score;
    string player;

    public string Player { get => player; set => player = value; }
    public int Score { get => score; set => score = value; }

    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    public void ScorePoint()
    {
        Score++;
        scoreText.text = "player " + Player + " - " + Score.ToString();
    }
}
