using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
    }

    private void OnGetAccount(GetAccountInfoResult result)
    {
        _titleLabel.text = $"PlayFab id: {result.AccountInfo.PlayFabId}. Player Title{result.AccountInfo.TitleInfo}";
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }
}
