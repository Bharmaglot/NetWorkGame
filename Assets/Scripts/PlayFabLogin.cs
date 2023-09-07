using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    //[SerializeField] private ButtonsPanel _loginView;
    private const string AuthGuidKey = "auth_guid_key";


    private void Awake()
    {
        //_loginView.PressLogIn += ActiveLogIn;
        ActiveLogIn();
    }

    void ActiveLogIn()
    {
    if(string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "A823B";
        }

        var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());
        

        var request = new LoginWithCustomIDRequest 
        {
            CustomId = id,
            CreateAccount = !needCreation 
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            result => {
                PlayerPrefs.SetString(AuthGuidKey, id);
                OnLoginSuccess(result);
            }, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        //_loginView.OnLoginSuccessPanel();

        Debug.Log("Congratulations, you made successful API call!");
    }
    private void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
        //_loginView.OnLoginFailurePanel();
    }

}
