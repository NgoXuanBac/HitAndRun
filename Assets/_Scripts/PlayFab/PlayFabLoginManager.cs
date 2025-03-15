using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayFabLoginManager : MonoBehaviour
{
    public static PlayFabLoginManager Instance;
    public static event Action OnLoginSuccess;

    private void Start()
    {
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Login();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Login()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.Log("✅ Already logged in!");
            OnLoginSuccess?.Invoke();
            return;
        }

        string deviceId = SystemInfo.deviceUniqueIdentifier;
        Debug.Log("🆔 Device ID: " + deviceId);

        var request = new LoginWithCustomIDRequest
        {
            CustomId = deviceId,
            CreateAccount = false
        };

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            Debug.Log("✅ Login successful! PlayFabID: " + result.PlayFabId);
            OnLoginSuccess?.Invoke();
        }, error =>
        {
            Debug.LogError("❌ Login failed: " + error.GenerateErrorReport());

            if (error.HttpCode == 409)
            {
                Debug.LogWarning("⚠️ Tài khoản chưa tồn tại, tạo mới...");
                CreateNewAccount(deviceId);
            }
        });

    }

    void CreateNewAccount(string deviceId)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = deviceId,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            Debug.Log("✅ Account created and logged in! PlayFabID: " + result.PlayFabId);
            OnLoginSuccess?.Invoke();
        }, error =>
        {
            Debug.LogError("❌ Failed to create account: " + error.GenerateErrorReport());
        });
    }

}
