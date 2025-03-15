using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class UpdateDisplayName : MonoBehaviour
{
    public TMP_InputField nameInput;
    //public TextMeshProUGUI feedbackText; 

    void Start()
    {
        PlayFabLoginManager.OnLoginSuccess += GetDisplayName;
    }
    public void SaveDisplayName()
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

    public void GetDisplayName()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            var currentDisplayName = result.AccountInfo.TitleInfo.DisplayName ?? result.AccountInfo.PlayFabId;
            nameInput.text = currentDisplayName;
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
}
