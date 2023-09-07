using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


public class CreateAccountWindow : AccountDataWindowBase
{
    [SerializeField] private InputField _mailField;
    [SerializeField] private Button _createAccountButton;

    private string _mail;

    protected override void SubscriptionElementUI()
    {
        base.SubscriptionElementUI();

        _mailField.onValueChanged.AddListener(UpdateMail);
        _createAccountButton.onClick.AddListener(CreateAccount);
    }

    private void CreateAccount()
    {
        _loadLabelImage.enabled = true;
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _username,
            Email = _mail,
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
    
    private void UpdateMail(string mail)
    {
        _mail = mail;
    }
}
