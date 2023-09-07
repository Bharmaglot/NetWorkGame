using UnityEngine;
using UnityEngine.UI;

public class EnterInGameWindow : MonoBehaviour
{
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _createAccontButton;
    [SerializeField] private Canvas _enterInGameCanvas;
    [SerializeField] private Canvas _createAccountCanvas;
    [SerializeField] private Canvas _signInCanvas;

    private void Start()
    {
        _signInButton.onClick.AddListener(OpenSignInWindow);
        _createAccontButton.onClick.AddListener(CreateAccountWindow);
    }

    private void OpenSignInWindow()
    {
        _signInCanvas.enabled = true;
        _enterInGameCanvas.enabled = false;
         
    }

    private void CreateAccountWindow()
    {
        
        _createAccountCanvas.enabled = true;
        _enterInGameCanvas.enabled = false;
        Debug.Log(_createAccontButton.enabled);
    }
}
