using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";

    [SerializeField] private ButtonsPanel _buttonsPanel;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        _buttonsPanel.PressPhotonConnect += Connect;

    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
        PanelSwichOn();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnetcedToMaster");
    }

    public void Disconnect()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        PanelSwichOff();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnetced");
    }

    public void PanelSwichOn()
    {
        _buttonsPanel.PressPhotonConnect -= Connect;
        _buttonsPanel.PressPhotonConnect += Disconnect;
        _buttonsPanel.ButtonSwithOn();
    }

    public void PanelSwichOff()
    {
        _buttonsPanel.PressPhotonConnect -= Disconnect;
        _buttonsPanel.PressPhotonConnect += Connect;
        _buttonsPanel.ButtonSwithOff();
    }
}
