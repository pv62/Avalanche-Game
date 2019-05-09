using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] Ball ballPrefab;
    [SerializeField] GameObject p1BallsParent, p2BallsParent; // Organization purposes
    [SerializeField] Scoreboard p1Score, p2Score;
    [SerializeField] int scoreToWin = 9;
    [SerializeField] Text turnText, aiLevelText, quitText;
    [SerializeField] GameObject[] moveHighlights;

    public static Game Instance { set; get; }

    string selectedMove = null;
    bool isPlayer1Turn = true;
    bool canEndTurn = false;
    bool canClick = true;
    List<Ball> p1Balls, p2Balls;
    int p1Count = 0, p2Count = 0;

    LevelManager lm;
    bool isAI = false;
    string aiLevel;
    AI ai;

    private void Start()
    {
        Instance = this;

        p1Score.Player = "1";
        p2Score.Player = "2";
        p1Balls = new List<Ball>();
        p2Balls = new List<Ball>();
        turnText.text = "player 1 Turn";

        lm = FindObjectOfType<LevelManager>();
        aiLevel = lm.AiLevel;
        if (aiLevel != null)
        {
            isAI = true;
            if (aiLevel == "Easy")
            {
                aiLevelText.text = "AI: easy";
            }
            else if (aiLevel == "Medium")
            {
                ai = gameObject.AddComponent<AI>();
                ai.IsHardDifficulty = false;
                aiLevelText.text = "AI: Medium";
            }
            else if (aiLevel == "Hard")
            {
                ai = gameObject.AddComponent<AI>();
                ai.IsHardDifficulty = true;
                ai.IsP1OnMatchPoint = false;
                ai.IsP2OnMatchPoint = false;
                aiLevelText.text = "AI: Hard";
            }
        }
    //    quitText.GetComponent<Button>().onClick.AddListener(lm.ResetAILevel);
    }
    
    private void Update()
    {
        UpdateSelected();
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedMove != null)
            {
                if ((!isAI || (isAI && isPlayer1Turn)) && canClick)
                {
                    PlaceMarble(selectedMove);
                }
            }
        }
    }

    public void PlaceMarble(string selectedMove)
    {
        
        Vector3 moveLocation;
        switch (selectedMove)
        {
            case "Move 1":
                moveLocation = new Vector3(0.65f, 4.3f, 0.1f);
                break;
            case "Move 2":
                moveLocation = new Vector3(0.4f, 4.3f, 0.1f);
                break;
            case "Move 3":
                moveLocation = new Vector3(0.15f, 4.3f, 0.1f);
                break;
            case "Move 4":
                moveLocation = new Vector3(-0.1f, 4.3f, 0.1f);
                break;
            case "Move 5":
                moveLocation = new Vector3(-0.35f, 4.3f, 0.1f);
                break;
            case "Move 6":
                moveLocation = new Vector3(-0.615f, 4.3f, 0.1f);
                break;
            default:
                return;
        }
        Ball newBall = Instantiate(ballPrefab, moveLocation, Quaternion.identity);
        if (isPlayer1Turn)
        {
            newBall.transform.SetParent(p1BallsParent.transform);
            newBall.Player = 1;
            newBall.name = "P1 Ball " + p1Count;
            p1Balls.Add(newBall);
            p1Count++;
        }
        else
        {
            newBall.transform.SetParent(p2BallsParent.transform);
            newBall.Player = 2;
            newBall.name = "P2 Ball " + p2Count;
            p2Balls.Add(newBall);
            p2Count++;
        }
        canClick = false;
        StartCoroutine(TryToEndTurn()); // End Turn if all marbles have stopped moving
    }
    
    IEnumerator TryToEndTurn()
    {
        int seconds = 0; // Number of seconds since ball has been released
        var balls = FindObjectsOfType<Ball>();
        canEndTurn = true;

        foreach (Ball b in balls)
        {
            if (!b.HasBallStopped())
            {
                canEndTurn = false;
                seconds++;
                yield return new WaitForSeconds(1f);
            }
        }
        
        if (canEndTurn || seconds >= 3) // If all marbles have stopped or it has been 3 or more seconds (failsafe)
        {
            EndTurn();
        }
        else
        {
            StartCoroutine(TryToEndTurn());
        }
    }

    // Select Move With Mouse Click
    public void UpdateSelected()
    {
        if (!Camera.main) { return; }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layer = LayerMask.GetMask("Move");
        if (Physics.Raycast(ray, out hit, 50f, layer))
        {
            GameObject objHit = hit.transform.gameObject;
            Renderer r = objHit.GetComponent<Renderer>();
            Material m = r.material;
            m.color = Color.green;
            selectedMove = objHit.name;
        }
        else
        {
            foreach (var mh in moveHighlights)
            {
                Renderer r = mh.GetComponent<Renderer>();
                Material m = r.material;
                m.color = Color.red;
            }
            selectedMove = null;
        }
    }

    public void RandomAIMove()
    {
        int r = Random.Range(1, 7);
        PlaceMarble("Move " + r);
    }

    public void CalculatedAIMove()
    {
        int calculatedMove = ai.CalculateBestMove() + 1;
        PlaceMarble("Move " + calculatedMove);
    }

    public void EndTurn()
    {
        isPlayer1Turn = !isPlayer1Turn;
        CheckWin();
        if (isPlayer1Turn)
        {
            turnText.text = "player 1 turn";
        }
        else
        {
            turnText.text = "player 2 turn";
        }
        if (isAI && !isPlayer1Turn)
        {
            if (aiLevel == "Easy")
            {
                RandomAIMove();
            }
            else
            {
                if (aiLevel == "Hard")
                {
                    if (p1Score.Score == (scoreToWin - 1))
                    {
                        ai.IsP1OnMatchPoint = true;
                    }
                    if (p2Score.Score == (scoreToWin - 2))
                    {
                        ai.IsP2OnMatchPoint = true;
                    }
                }
                CalculatedAIMove();
            }
        }
        canClick = true;
    }

    public void CheckWin()
    {
        if (p1Score.Score >= scoreToWin)
        {
            lm.LoadLevel("Player 1 Win");
        }
        else if (p2Score.Score >= scoreToWin)
        {
            lm.LoadLevel("Player 2 Win");
        }
    }

    public List<Ball> GetP1Balls()
    {
        return p1Balls;
    }

    public List<Ball> GetP2Balls()
    {
        return p2Balls;
    }
}
