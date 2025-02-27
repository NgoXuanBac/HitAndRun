using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab.ClientModels;
using PlayFab;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform contentTransform;
    public Transform currentPlayerTransform;

    private void Start()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogWarning("⚠️ Chưa đăng nhập PlayFab! Tiến hành đăng nhập...");
            LoginAndGetLeaderboard();
        }
        else
        {
            GetLeaderboard();
            GetCurrentPlayerRank();
        }
    }

    private void LoginAndGetLeaderboard()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            Debug.Log("✅ Đăng nhập thành công! Lấy leaderboard...");
            GetLeaderboard();
            GetCurrentPlayerRank();
        }, error =>
        {
            Debug.LogError("❌ Lỗi đăng nhập: " + error.GenerateErrorReport());
        });
    }

    void GetLeaderboard()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };


        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            Debug.Log("🏆 Lấy leaderboard thành công!");
            foreach (var e in result.Leaderboard)
            {
                GameObject entry = Instantiate(entryPrefab, contentTransform);

                TMPro.TextMeshProUGUI rankText = entry.transform.Find("RankText").GetComponent<TMPro.TextMeshProUGUI>();
                TMPro.TextMeshProUGUI playerNameText = entry.transform.Find("PlayerNameText").GetComponent<TMPro.TextMeshProUGUI>();
                TMPro.TextMeshProUGUI scoreText = entry.transform.Find("ScoreText").GetComponent<TMPro.TextMeshProUGUI>();

                rankText.text = (e.Position + 1).ToString();
                playerNameText.text = e.PlayFabId;
                scoreText.text = e.StatValue.ToString();
            }
        }, error =>
        {
            Debug.LogError("⚠️ Lỗi khi lấy leaderboard: " + error.GenerateErrorReport());
        });

    }

    void GetCurrentPlayerRank()
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

                if(entry == null) Debug.Log("entry is null"); 
                
                TextMeshProUGUI rankText = entry.transform.Find("RankText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI playerNameText = entry.transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI scoreText = entry.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();

                rankText.text = (playerEntry.Position + 1).ToString();
                playerNameText.text = playerEntry.PlayFabId;
                scoreText.text = playerEntry.StatValue.ToString();
            }
            else
            {
                Debug.LogWarning("⚠️ Không tìm thấy thông tin người chơi!");
            }
        }, error =>
        {
            Debug.LogError("⚠️ Lỗi khi lấy thông tin người chơi: " + error.GenerateErrorReport());
        });
    }

    public void CloseLeaderboard()
    {
        gameObject.SetActive(false);
    }

}