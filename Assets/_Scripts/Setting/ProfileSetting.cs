using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;
using UnityEngine.SceneManagement;

public class ProfileSetting : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField Id;
    //public TextMeshProUGUI feedbackText; 

    void Start()
    {
        Debug.Log("Profile Setting Start");
        PlayFabLoginManager.Instance.Login();
    }
    public void SetProfile()
    {
        string newName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(newName))
        {
            Debug.Log("Name cannot be empty!");
            //feedbackText.text = "Name cannot be empty!";
            return;
        }

        UpdateUserTitleDisplayNameRequest request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdateSuccess, OnDisplayNameUpdateFailure);
    }

    public void GetProfile()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            var currentDisplayName = result.AccountInfo.TitleInfo.DisplayName;
            var playFabId = result.AccountInfo.PlayFabId;

            if(currentDisplayName == null)
            {
                currentDisplayName = "";
            }
            nameInput.text = currentDisplayName;
            Id.text = playFabId;
            Debug.Log("Display Name: " + currentDisplayName);
            Debug.Log("PlayFab ID: " + playFabId);
        }, error =>
        {
            Debug.LogError("Error fetching display name: " + error.GenerateErrorReport());
        });
    }

    private void OnDisplayNameUpdateSuccess(UpdateUserTitleDisplayNameResult result)
    {
        //feedbackText.text = "Name updated successfully!";
        Debug.Log("Name updated successfully!");

    }


    private void OnDisplayNameUpdateFailure(PlayFabError error)
    {
        //feedbackText.text = "Error: " + error.GenerateErrorReport();
        Debug.Log("Error: " + error.GenerateErrorReport());
    }

    public void CloseGameSetting()
    {
        SceneManager.LoadScene("BootScene");
    }
}
