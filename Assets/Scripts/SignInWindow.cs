using System;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField] private Button _signInButton;

    protected override void SubscriptionElementUI()
    {
        base.SubscriptionElementUI();

        _signInButton.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        _loadLabelImage.enabled = true;
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password
        },
        result =>
        {
            Debug.Log($"Success: {_username}");
            EnterInGameScene();
        },

        error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}");
        });
        _loadLabelImage.enabled = false;
    }
}
 