using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    private void Start()
    {
        Physics.IgnoreLayerCollision(9, 10);
        Physics.IgnoreLayerCollision(10, 11);
        Physics.IgnoreLayerCollision(11, 12);
        int r = Random.Range(0, 2);
        if (r == 0)
        {
            transform.Rotate(0f, 0f, 25f);
        }
        else
        {
            transform.Rotate(0f, 0f, -25f);
        }
    }
}
