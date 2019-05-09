using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    string aiLevel;

    public string AiLevel { get => aiLevel; set => aiLevel = value; }
    public static string PreviousScene = "";

    private void Awake()
    {
        int numLevelManagers = FindObjectsOfType<LevelManager>().Length;
        if (numLevelManagers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
        
    }

    public void LoadLevel(string name)
    {
        PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(name);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadPreviousLevel()
    {
        if (PreviousScene == "") { return; }
        
        SceneManager.LoadScene(PreviousScene);
    }

    public void ResetAILevel()
    {
        AiLevel = null;
    }

    public void SetAILevel(string a)
    {
        if (a == "Reset")
        {
            AiLevel = null;
        }
        else
        {
            AiLevel = a;
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
