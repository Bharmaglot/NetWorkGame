using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectAndJoinRandomLB : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    [SerializeField] private ServerSettings _serverSetting;
    [SerializeField] private TMP_Text _stateUiText;

    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _createIvisibleRoomButton;
    [SerializeField] private Button _leaveRoomButton;

    [SerializeField] private Button _closeRoomButton;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TMP_Text _closeRoomButtonText;
    [SerializeField] private TMP_InputField _nameRoomInputField;
    [SerializeField] private Button _connectToInvisibleRoomButton;



    [SerializeField] private ListItem _roomItemPrefab;
    [SerializeField] private Transform _roomsWindow;

    private LoadBalancingClient _lbc;
    private TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);

    private List<ListItem> listItems = new List<ListItem>();

    private bool _clientIsConnected = false;


    private void Start()
    {
        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this);

        _lbc.ConnectUsingSettings(_serverSetting.AppSettings);

        _createRoomButton.onClick.AddListener(PressCreateRoom);
        _leaveRoomButton.onClick.AddListener(PressLeaveFromRoom);
        _closeRoomButton.onClick.AddListener(PressToCloseRoom);
        _startGameButton.onClick.AddListener(PressToStartGame);
        _createIvisibleRoomButton.onClick.AddListener(PressCreateInvisibleRoom);
        _connectToInvisibleRoomButton.onClick.AddListener(PressConnectToInvisibleRoom);

        _createRoomButton.gameObject.SetActive(false);
        _leaveRoomButton.gameObject.SetActive(false);
        _closeRoomButton.gameObject.SetActive(false);
        _startGameButton.gameObject.SetActive(false);
        _createIvisibleRoomButton.gameObject.SetActive(false);
        _connectToInvisibleRoomButton.gameObject.SetActive(false);
        _nameRoomInputField.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_lbc == null)
            return;

        _lbc.Service();

        var state = _lbc.State.ToString();
        _stateUiText.text = state;

        if (_clientIsConnected == false) CheckConnectionForActiveUI();
    }

    private void DeactiveAllButtons()
    {
        if (_createRoomButton.gameObject.activeSelf) _createRoomButton.gameObject.SetActive(false);
        if (_leaveRoomButton.gameObject.activeSelf) _leaveRoomButton.gameObject.SetActive(false);
        if (_closeRoomButton.gameObject.activeSelf) _closeRoomButton.gameObject.SetActive(false);
        if (_startGameButton.gameObject.activeSelf) _startGameButton.gameObject.SetActive(false);
        if (_createIvisibleRoomButton.gameObject.activeSelf) _createIvisibleRoomButton.gameObject.SetActive(false);
        if (_connectToInvisibleRoomButton.gameObject.activeSelf) _connectToInvisibleRoomButton.gameObject.SetActive(false);
        if (_nameRoomInputField.gameObject.activeSelf) _nameRoomInputField.gameObject.SetActive(false);
    }

    private void ActiveMainButtons()
    {
        _createRoomButton.gameObject.SetActive(true);
        _createIvisibleRoomButton.gameObject.SetActive(true);
        _connectToInvisibleRoomButton.gameObject.SetActive(true);
        _nameRoomInputField.gameObject.SetActive(true);
    }
    private void CheckConnectionForActiveUI()
    {
        if(_lbc.InLobby)
        {
            ActiveMainButtons();
            _clientIsConnected = true;
        }
    }

    private void OnDestroy()
    {
        _lbc.RemoveCallbackTarget(this);

        _createRoomButton.onClick.RemoveAllListeners();
        _leaveRoomButton.onClick.RemoveAllListeners();
        _createIvisibleRoomButton.onClick.RemoveAllListeners();
        _connectToInvisibleRoomButton.onClick.RemoveAllListeners();
        _startGameButton.onClick.RemoveAllListeners();
        _closeRoomButton.onClick.RemoveAllListeners();
    }

    private void PressCreateRoom()
    {
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        _lbc.OpCreateRoom(enterRoomParams);

        DeactiveAllButtons();
        _leaveRoomButton.gameObject.SetActive(true);
        _closeRoomButton.gameObject.SetActive(true);
    }

    private void PressCreateInvisibleRoom()
    {
        if (_nameRoomInputField != null)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = false;
            EnterRoomParams enterRoomParams = new EnterRoomParams();
            enterRoomParams.RoomOptions = roomOptions;
            enterRoomParams.RoomName = _nameRoomInputField.text;
            _lbc.OpCreateRoom(enterRoomParams);

            DeactiveAllButtons();

            _leaveRoomButton.gameObject.SetActive(true);
            _closeRoomButton.gameObject.SetActive(true);
        }
        else
        {
            _nameRoomInputField.text = "Write name";
        }
    }

    private void PressConnectToInvisibleRoom()
    {
        if(_nameRoomInputField != null)
        {
            EnterRoomParams enterRoomParams = new EnterRoomParams();
            enterRoomParams.RoomName = _nameRoomInputField.text;
            _lbc.OpJoinRoom(enterRoomParams);

            if (_lbc.InRoom)
            {
                DeactiveAllButtons();

                _leaveRoomButton.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("WrongName");
            }
        }
        else
        {
            _nameRoomInputField.text = "Write name";
        }
    }

    private void PressConnectToRoom(RoomInfo info)
    {
        Debug.Log($"ConnectToRoom {info.Name}");
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomName = info.Name;
        _lbc.OpJoinRoom(enterRoomParams);


        DeactiveAllButtons();

        _leaveRoomButton.gameObject.SetActive(true);
    }

    private void PressLeaveFromRoom()
    {
        DeactiveAllButtons();
        ActiveMainButtons();
        _lbc.OpLeaveRoom(true);
    }

    private void ConnectToLobby()
    {
        _lbc.OpJoinLobby(customLobby);
    }

    private void PressToCloseRoom()
    {
        _lbc.CurrentRoom.IsOpen = false;
        _closeRoomButtonText.text = "Open Room";
        _closeRoomButton.onClick.RemoveAllListeners();
        _closeRoomButton.onClick.AddListener(PressToOpenRoom);

        _startGameButton.gameObject.SetActive(true);
        _leaveRoomButton.gameObject.SetActive(false);
    }

    private void PressToOpenRoom()
    {
        _lbc.CurrentRoom.IsOpen = true;
        _closeRoomButtonText.text = "Close Room";
        _closeRoomButton.onClick.RemoveAllListeners();
        _closeRoomButton.onClick.AddListener(PressToCloseRoom);

        _startGameButton.gameObject.SetActive(false);
        _leaveRoomButton.gameObject.SetActive(true);
    }

   
    private void PressToStartGame()
    {
         PhotonNetwork.LoadLevel("PhotonGame");
    }

    public void OnConnected()
    {
       
    }

    public void OnConnectedToMaster()
    {
        ConnectToLobby();
    }

    public void OnCreatedRoom()
    {

    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
       
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
       
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
       
    }

    public void OnDisconnected(DisconnectCause cause)
    {
       
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
       
    }

    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }

    public void OnJoinedRoom()
    {
        Debug.Log($"OnJoinedRoom {_lbc.CurrentRoom.PlayerCount}");
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
       
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
       
    }

    public void OnLeftLobby()
    {
       
    }

    public void OnLeftRoom()
    {
        Debug.Log($"OnLeftRoom");
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
       
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
       
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (listItems != null)
        {
            foreach (ListItem item in listItems)
            {
                Destroy(item.gameObject);

            }
        }
        listItems.Clear();

        Debug.Log("OnRoomListUpdate");

        foreach(RoomInfo info in roomList)
        {
            Debug.Log("Room");
           ListItem listItem = Instantiate(_roomItemPrefab, _roomsWindow);
            listItems.Add(listItem);
            if(listItem != null)
            {
                listItem.SetInfo(info);
                listItem.JoinButton.onClick.AddListener(delegate { PressConnectToRoom(info);});
            }
        }
    }



}
