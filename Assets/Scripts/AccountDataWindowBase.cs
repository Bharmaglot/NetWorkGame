using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountDataWindowBase : MonoBehaviour
{
    [SerializeField] private InputField _usernameField;
    [SerializeField] private InputField _passwordField;
    [SerializeField] protected Image _loadLabelImage;

    protected string _username;
    protected string _password;


    private void Start()
    {
        SubscriptionElementUI();
    }

    protected virtual void SubscriptionElementUI()
    {
        _usernameField.onValueChanged.AddListener(UpdateUsername);
        _passwordField.onValueChanged.AddListener(UpdatePassword);
    }

    private void UpdatePassword(string password)
    {
        _password = password; 
    }

    private void UpdateUsername(string username)
    {
        _username = username;
    }

    protected void EnterInGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
