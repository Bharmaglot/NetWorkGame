using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ButtonsPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private Button _logInButton;
    [SerializeField] private Button _photonButton;


    private string _connectTrueText = "Login Success";
    private string _connectFalseText = "Login Failure";
    private string _notConectedText = "";
    private string _connectPhotonText = "Photon Connected";
    private string _disconnectPhotonText = "Photon Disconnected";


    private ColorBlock theColor;

    public event Action PressLogIn = () => { };
    public event Action PressPhotonConnect = () => { };

    private void Awake()
    {
        _textField.text = _notConectedText;
        _logInButton.onClick.AddListener(TryToLogIn);
        _photonButton.onClick.AddListener(ConnectPhoton);
        ButtonSwithOffColor(_photonButton);
    }
    public void OnLoginSuccessPanel()
    {
        _textField.text = _connectTrueText;
        _textField.color = new Color(0, 0, 255);
    }

    public void OnLoginFailurePanel()
    {
        _textField.text = _connectFalseText;
        _textField.color = new Color(255, 0, 0);
    }


    private void TryToLogIn()
    {
        PressLogIn();
    }

    private void ConnectPhoton()
    {
        PressPhotonConnect();
    }

    public void ButtonSwithOnColor(Button button)
    {
        theColor.highlightedColor = Color.blue;
        theColor.normalColor = Color.red;
        theColor.pressedColor = Color.green;
        button.colors = theColor;
    }

    public void ButtonSwithOffColor(Button button)
    {
        theColor.highlightedColor = Color.red;
        theColor.normalColor = Color.gray;
        theColor.pressedColor = Color.grey;
        button.colors = theColor;
    }

    public void ButtonSwithOn()
    {
        _textField.text = _connectPhotonText;
        ButtonSwithOnColor(_photonButton);

    }
    public void ButtonSwithOff()
    {
        _textField.text = _disconnectPhotonText;
        ButtonSwithOffColor(_photonButton);

    }
}
