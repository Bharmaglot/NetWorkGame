using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SecondConnectAndJoinRandomLB : MonoBehaviourPunCallbacks
{
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

    [SerializeField] private int _maxPlayer = 4;

    private List<ListItem> listItems = new List<ListItem>();

    private bool _clientIsConnected = false;

    private void Start()
    {

        PhotonNetwork.ConnectUsingSettings();
        
        _createRoomButton.onClick.AddListener(PressCreateRoom);
        _leaveRoomButton.onClick.AddListener(PressLeaveFromRoom);
        _closeRoomButton.onClick.AddListener(PressToCloseRoom);
        _startGameButton.onClick.AddListener(PressToStartGame);
        _createIvisibleRoomButton.onClick.AddListener(PressCreateInvisibleRoom);
        _connectToInvisibleRoomButton.onClick.AddListener(PressConnectToInvisibleRoom);

        DeactiveAllButtons();
    }

    private void Update()
    {
        string state = PhotonNetwork.NetworkClientState.ToString();
        _stateUiText.text = state;


        if (_clientIsConnected == true) CheckConnectionForActiveUI();
    }

    private void CheckConnectionForActiveUI()
    {
            ActiveMainButtons();
            _clientIsConnected = false;
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

    private void OnDestroy()
    {
        _createRoomButton.onClick.RemoveAllListeners();
        _leaveRoomButton.onClick.RemoveAllListeners();
        _createIvisibleRoomButton.onClick.RemoveAllListeners();
        _connectToInvisibleRoomButton.onClick.RemoveAllListeners();
        _startGameButton.onClick.RemoveAllListeners();
        _closeRoomButton.onClick.RemoveAllListeners();
    }

    private void PressCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _maxPlayer;
        string name = _nameRoomInputField.text;
        PhotonNetwork.CreateRoom(name, roomOptions, TypedLobby.Default);

        DeactiveAllButtons();
        _leaveRoomButton.gameObject.SetActive(true);
        _closeRoomButton.gameObject.SetActive(true);
    }

    private void PressCreateInvisibleRoom()
    {
        if (_nameRoomInputField != null)
        {
           string roomName = _nameRoomInputField.text;
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = _maxPlayer;
            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);


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
        if (_nameRoomInputField != null)
        {
            string roomName = _nameRoomInputField.text;
            PhotonNetwork.JoinRoom(roomName);

            if (PhotonNetwork.InRoom)
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
        PhotonNetwork.JoinRoom(info.Name);

        DeactiveAllButtons();

        _leaveRoomButton.gameObject.SetActive(true);
    }

    private void PressLeaveFromRoom()
    {
        DeactiveAllButtons();
        ActiveMainButtons();
        PhotonNetwork.LeaveRoom();
    }

    private void PressToCloseRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        _closeRoomButtonText.text = "Open Room";
        _closeRoomButton.onClick.RemoveAllListeners();
        _closeRoomButton.onClick.AddListener(PressToOpenRoom);

        _startGameButton.gameObject.SetActive(true);
        _leaveRoomButton.gameObject.SetActive(false);
    }

    private void PressToOpenRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
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

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        _clientIsConnected = true;
        Debug.Log($"{_clientIsConnected}");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"OnJoinedRoom {PhotonNetwork.CurrentRoom.Name}");
        PhotonNetwork.LoadLevel("PhotonGame");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
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

        foreach (RoomInfo info in roomList)
        {
            ListItem listItem = Instantiate(_roomItemPrefab, _roomsWindow);
            listItems.Add(listItem);
            if (listItem != null)
            {
                listItem.SetInfo(info);
                listItem.JoinButton.onClick.AddListener(delegate { PressConnectToRoom(info); });
            }
        }
    }
}
