using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Material p1Material, p2Material;

    Scoreboard[] scores = new Scoreboard[2];
    int player;

    bool hasStopped;
    bool isOnTopOfSwitch;
    float timeOnTopOfSwitch = 1f;
    string onSwitch = "";

    public int Player { get => player; set => player = value; }

    private void Start()
    {
        scores = FindObjectsOfType<Scoreboard>();
        Renderer r = GetComponent<Renderer>();
        if (Player == 1)
        {
            r.material = p1Material;
        }
        else if (Player == 2)
        {
            r.material = p2Material;
        }
        hasStopped = false;
    }

    private void Update()
    {
        if (isOnTopOfSwitch)
        {
            timeOnTopOfSwitch -= Time.deltaTime;
            if (timeOnTopOfSwitch < 0)
            {
                timeOnTopOfSwitch = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bottom Collider")
        {
            foreach(Scoreboard s in scores)
            {
                if (s.Player == Player.ToString())
                {
                    s.ScorePoint();
                }
            }
            hasStopped = true;
            Invoke("DestroyBall", 3f);
        }
        else if (collision.gameObject.GetComponentInChildren<BoxCollider>().name == "Top")
        {
            isOnTopOfSwitch = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // When the ball stays on the top of the switch, this means it's not moving
        if (collision.gameObject.GetComponentInChildren<BoxCollider>().name == "Top" && isOnTopOfSwitch)
        {
            if (timeOnTopOfSwitch <= 0) // If the ball has been on the switch for more than 1 second
            {
                hasStopped = true;
                onSwitch = collision.gameObject.name;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // When the ball no longer stays on the top of the switch, this means it's moving
        if (collision.gameObject.GetComponentInChildren<BoxCollider>().name == "Top")
        {
            hasStopped = false;
            timeOnTopOfSwitch = 1f;
            onSwitch = "";
        }
    }


    private void DestroyBall()
    {
        Destroy(gameObject);
    }

    public bool HasBallStopped()
    {
        return hasStopped;
    }

    public string GetSwitch()
    {
        return onSwitch;
    }
}
