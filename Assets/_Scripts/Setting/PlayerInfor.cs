using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using TMPro;

public class PlayerInfor : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform currentPlayerTransform;

    private void Start()
    {
        PlayFabLoginManager.OnLoginSuccess += () =>
        {
            GetPlayerRank();
            FindObjectOfType<ProfileSetting>().GetProfile();
        };
        Debug.Log("🟢 GetPlayerRank() is called.");
    }

   
    public void GetPlayerRank()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "HighScore",
            MaxResultsCount = 1
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, result =>
        {
            if (result.Leaderboard.Count > 0)
            {
                var playerEntry = result.Leaderboard[0];

                GameObject entry = Instantiate(entryPrefab, currentPlayerTransform);

                if (entry == null) Debug.Log("entry is null");

                TextMeshProUGUI rankText = entry.transform.Find("Rank/RankText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI playerNameText = entry.transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI scoreText = entry.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();

                rankText.text = (playerEntry.Position + 1).ToString();
                if(playerEntry.DisplayName != null)
                    playerNameText.text = playerEntry.DisplayName;
                else
                    playerNameText.text = playerEntry.PlayFabId;
                scoreText.text = playerEntry.StatValue.ToString();
            }
            else
            {
                Debug.LogWarning("⚠️ Player data not found!");
            }
        }, error =>
        {
            Debug.LogError("⚠️ Error retrieving player data: " + error.GenerateErrorReport());
        });
    }
}
