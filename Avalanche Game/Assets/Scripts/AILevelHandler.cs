using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AILevelHandler : MonoBehaviour
{

    LevelManager lm;
    [SerializeField] string level;

    // Start is called before the first frame update
    void Start()
    {
        lm = FindObjectOfType<LevelManager>();
        GetComponent<Button>().onClick.AddListener(SetAILevel);
        
    }

    public void SetAILevel()
    {
        lm.SetAILevel(level);
    }
}
