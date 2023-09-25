using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    //[SerializeField] private ButtonsPanel _loginView;
    private const string AuthGuidKey = "auth_guid_key";
    private const string TIME_REWARD = "time_recive_daily_reward";

    private void Awake()
    {
        //_loginView.PressLogIn += ActiveLogIn;
        ActiveLogIn();
    }

    void ActiveLogIn()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "A823B";
        }

        var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());


        var request = new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = !needCreation
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            result =>
            {
                PlayerPrefs.SetString(AuthGuidKey, id);
                OnLoginSuccess(result);
            }, OnLoginFailure);
    }



    private void OnLoginSuccess(LoginResult result)
    {
        //_loginView.OnLoginSuccessPanel();

        Debug.Log("Congratulations, you made successful API call!");
        SetUserData(result.PlayFabId);
        //MakePurchase();
        GetInventory();
    }

    private void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            result => ShowInventory(result.Inventory), OnLoginFailure);
    }

    private void ShowInventory(List<ItemInstance> inventory)
    {
        var firstItem = inventory.First();
        Debug.Log($"{firstItem.ItemId}") ;
        ConsumePotion(firstItem.ItemInstanceId);
    }

    private void ConsumePotion(string itemInstanceId)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        {
            ConsumeCount = 1,
            ItemInstanceId = itemInstanceId
        },
        result =>
        {
            Debug.Log("ConsumePotion");
        },
            OnLoginFailure);
    }

    private void MakePurchase()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "main",
            ItemId = "wooden_potion_id",
            Price = 1,
            VirtualCurrency = "WC"
        },
          result =>
          {
              Debug.Log("Complete PurchaseItem");
          },
            OnLoginFailure);
    }

    private void SetUserData(string playFabId)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
        {
            {TIME_REWARD, DateTime.UtcNow.ToString() }
        }
        },
        result =>
        {
            Debug.Log("SetUserData");
            GetUserData(playFabId, TIME_REWARD);
        }, OnLoginFailure);
    }

    private void GetUserData(string playFabId, string keyData)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playFabId
        }, result =>
        {
            if (result.Data.ContainsKey(keyData))
                Debug.Log($"{keyData} : {result.Data[keyData].Value}");
        }, OnLoginFailure);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
        //_loginView.OnLoginFailurePanel();
    }
}
