using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ServerModels;


public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;


    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new PlayFab.ClientModels.GetCatalogItemsRequest(), OnGetCatalogSuccess, OnError);
        PlayFabServerAPI.GetRandomResultTables(new GetRandomResultTablesRequest(), OnGetRandomResultTables, OnError);
    }

    private void OnGetRandomResultTables(GetRandomResultTablesResult result)
    {
        Debug.Log(result.Tables.Keys);
    }

    private void OnGetCatalogSuccess(PlayFab.ClientModels.GetCatalogItemsResult result)
    {
        Debug.Log("OnGetCatalogSuccess");

        ShowItems(result.Catalog);
    }

    private void ShowItems(List<PlayFab.ClientModels.CatalogItem> catalog)
    {
        foreach(var item in catalog)
        {
            Debug.Log($"{item.ItemId}");
        }    
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
