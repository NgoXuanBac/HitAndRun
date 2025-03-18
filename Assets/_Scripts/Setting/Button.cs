using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    internal object onClick;

    private void Start()
    {
        ClickSound();
    }
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "HighScore", Value = 0 },   
                new StatisticUpdate { StatisticName = "TotalGamesPlayed", Value = 0 },
                new StatisticUpdate { StatisticName = "TotalWins", Value = 0 }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, result =>
        {
            Debug.Log("Successfully reset PlayFab statistics.");
        },
        error =>
        {
            Debug.LogError("Error resetting PlayFab statistics: " + error.GenerateErrorReport());
        });
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ClickSound()
    {
        if (PlayerPrefs.GetInt("SFX", 1) == 1)
        {
            AudioManager.Instance.PlaySFX(SFXType.ButtonClick);
        }
    }

}
