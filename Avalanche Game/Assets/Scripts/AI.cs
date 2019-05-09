using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class AI : MonoBehaviour
{
    List<Switch> switches;
    float[] switchRotations = new float[14];
    int[] moves = new int[6];
    List<Ball> p1Balls, p2Balls;
    bool isHardDifficulty, isP1OnMatchPoint, isP2OnMatchPoint;
    
    public bool IsHardDifficulty { get => isHardDifficulty; set => isHardDifficulty = value; }
    public bool IsP1OnMatchPoint { get => isP1OnMatchPoint; set => isP1OnMatchPoint = value; }
    public bool IsP2OnMatchPoint { get => isP2OnMatchPoint; set => isP2OnMatchPoint = value; }

    private void Start()
    {
        switches = FindObjectsOfType<Switch>().OrderBy(n => n.name).ToList<Switch>();
    }

    public void ResetMoveScores()
    {
        Array.Clear(moves, 0, moves.Length);
    }

    public void ScoreMoves()
    {
        CalculateMove(0, 0, ">", 3, "<", 7, ">", 10, "<");
        CalculateMove(1, 0, "<", 4, ">", 7, "<", 11, ">");
        CalculateMove(2, 1, ">", 4, "<", 8, ">", 11, "<");
        CalculateMove(3, 1, "<", 5, ">", 8, "<", 12, ">");
        CalculateMove(4, 2, ">", 5, "<", 9, ">", 12, "<");
        CalculateMove(5, 2, "<", 6, ">", 9, "<", 13, ">");
    }

    public int CalculateBestMove()
    {
        ScoreMoves();
        return Array.IndexOf(moves, moves.Max());
    }

    private void CalculateMove(int m, int s1, string c1, int s2, string c2, int s3, string c3, int s4, string c4)
    {
        for (int i = 0; i < 14; i++)
        {
            switchRotations[i] = switches[i].transform.rotation.z;
        }
        p1Balls = Game.Instance.GetP1Balls();
        p2Balls = Game.Instance.GetP2Balls();

        bool check1 = true, check2 = true, check3 = true, check4 = true;
        if (c1 == ">")
        {
            check1 = switchRotations[s1] > 0;
        }
        else if (c1 == "<")
        {
            check1 = switchRotations[s1] < 0;
        }
        if (c2 == ">")
        {
            check2 = switchRotations[s2] > 0;
        }
        else if (c2 == "<")
        {
            check2 = switchRotations[s2] < 0;
        }
        if (c3 == ">")
        {
            check3 = switchRotations[s3] > 0;
        }
        else if (c3 == "<")
        {
            check3 = switchRotations[s3] < 0;
        }
        if (c4 == ">")
        {
            check4 = switchRotations[s4] > 0;
        }
        else if (c4 == "<")
        {
            check4 = switchRotations[s4] < 0;
        }

        if (check1)
        {
            moves[m] += 5;
            if (IsHardDifficulty)
            {
                CheckForBallsOnSwitch(m, s1, 1, 4);
            }
        }
        if (check2)
        {
            moves[m] += 4;
            if (IsHardDifficulty)
            {
                CheckForBallsOnSwitch(m, s2, 2, 3);
            }
        }
        if (check3)
        {
            moves[m] += 3;
            if (IsHardDifficulty)
            {
                CheckForBallsOnSwitch(m, s3, 3, 2);
            }
        }
        if (check4)
        {
            moves[m] += 2;
            if (IsHardDifficulty)
            {
                CheckForBallsOnSwitch(m, s4, 4, 1);
            }
        }
    }

    private void CheckForBallsOnSwitch(int m, int s, int r, int n)
    {
        foreach (Ball b in p1Balls)
        {
            if (b.GetSwitch() == switches[s].name)
            {
                if (IsP1OnMatchPoint)
                {
                    moves[m] -= (n + r);
                }
                else
                {
                    moves[m] -= n;
                }
                
            }
        }
        foreach (Ball b in p2Balls)
        {
            if (b.GetSwitch() == switches[s].name)
            {
                if (IsP2OnMatchPoint)
                {
                    moves[m] += (n + r);
                }
                else
                {
                    moves[m] += n;
                }
            }
        }
    }
}
