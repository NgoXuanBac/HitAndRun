using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", 1);
            PlayerPrefs.Save();
        }
    }

    public void PlayGame()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        string levelName = "Level " + currentLevel; 
        SceneManager.LoadSceneAsync(levelName);
        Debug.Log(levelName);
    }
}