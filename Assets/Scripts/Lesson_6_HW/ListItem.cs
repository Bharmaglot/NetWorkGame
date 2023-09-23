using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] public Button JoinButton;

    public void SetInfo(RoomInfo info)
    {
        _name.text = $"Player {info.PlayerCount}/{info.MaxPlayers} room {info.Name}";
        JoinButton.onClick.RemoveAllListeners();
    }

    private void OnDestroy()
    {
        JoinButton.onClick.RemoveAllListeners();
    }

}
