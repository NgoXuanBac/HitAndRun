using UnityEngine;
using TMPro;
using PlayFab.ClientModels;
using PlayFab;

namespace HitAndRun.Leaderboard
{
    public class LeaderboardManager : MonoBehaviour
    {
        public GameObject entryPrefab;
        public Transform contentTransform;
        public Transform currentPlayerTransform;

        private void Start()
        {
            if (!PlayFabClientAPI.IsClientLoggedIn())
            {
                Debug.Log("⚠️ Not logged into PlayFab! Attempting login...");
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
                Debug.Log("✅ Login successful! Fetching leaderboard...");
                GetLeaderboard();
                GetCurrentPlayerRank();
            }, error =>
            {
                Debug.LogError("❌ Login failed: " + error.GenerateErrorReport());
            });
        }

        private void GetLeaderboard()
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
                Debug.Log("🏆 Leaderboard retrieved successfully!");
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
                Debug.LogError("⚠️ Error retrieving leaderboard: " + error.GenerateErrorReport());
            });
        }

        private void GetCurrentPlayerRank()
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

                    TextMeshProUGUI rankText = entry.transform.Find("RankText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI playerNameText = entry.transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI scoreText = entry.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();

                    rankText.text = (playerEntry.Position + 1).ToString();
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

        public void CloseLeaderboard()
        {
            gameObject.SetActive(false);
        }
    }
}
