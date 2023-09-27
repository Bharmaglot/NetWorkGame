using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Linq;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;

    [SerializeField] private GameObject _newCharacterCreatePanel;

    [SerializeField] private Button _createCharacterButton;

    [SerializeField] private TMP_InputField _inputField;

    [SerializeField] private List<SlotCharacterWidget> _slots;

    private string _characterName;


    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new PlayFab.ClientModels.GetCatalogItemsRequest(), OnGetCatalogSuccess, OnError);
        PlayFabServerAPI.GetRandomResultTables(new PlayFab.ServerModels.GetRandomResultTablesRequest(), OnGetRandomResultTables, OnError);

        GetCharacters();

        foreach(var slot in _slots)
        {
            slot.SlotButton.onClick.AddListener(OpenCreateNewCharacter);
        }

        _inputField.onValueChanged.AddListener(OnNameChanged);
        _createCharacterButton.onClick.AddListener(CreateCharacter);
    }

    private void CreateCharacter()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _characterName,
            ItemId = "character_token"
        },
        result =>
        {
            UpdateCharactersStatistic(result.CharacterId);
        },
            OnError);
    }

    private void UpdateCharactersStatistic(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics( new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {"Level", 1 },
                {"Wood", 0  },
                {"Damage", 2 },
                {"Health", 3 },
                {"Exp", 4 }

            }
        },
        result =>
        {
            Debug.Log("Complete!!!");
            CloseCreateNewCharacter();
            GetCharacters();
        }, OnError);
    }


    private void OnNameChanged(string changedName)
    {
        _characterName = changedName;
    }

    private void OpenCreateNewCharacter()
    {
        _newCharacterCreatePanel.gameObject.SetActive(true);
    }


    private void CloseCreateNewCharacter()
    {
        _newCharacterCreatePanel.gameObject.SetActive(false);
    }

    private void GetCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new PlayFab.ClientModels.ListUsersCharactersRequest(), result =>
        {
            Debug.Log($"Character count: {result.Characters.Count}");
            ShowCharactersInSlot(result.Characters);
        }, OnError);
    }

    private void ShowCharactersInSlot(List<PlayFab.ClientModels.CharacterResult> characters)
    {
        if (characters.Count == 0)
        {
            foreach (var slot in _slots)
                slot.ShowEmptySlot();
        }
        else if(characters.Count > 0 && characters.Count <= _slots.Count)
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if (i < characters.Count)
                {
                    var name = characters[i].CharacterName;
                    var j = i;
                    PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                    {
                        CharacterId = characters[i].CharacterId, 
                    },
            result =>
            {
                
                var level = result.CharacterStatistics["Level"].ToString();
                var wood = result.CharacterStatistics["Wood"].ToString();
                var damage = result.CharacterStatistics["Damage"].ToString();
                var health = result.CharacterStatistics["Health"].ToString();
                var exp = result.CharacterStatistics["Exp"].ToString();


                _slots[j].ShowInfoCharacterSlot(name, level, wood, damage, health, exp);

            }, OnError);
                }
                else
                {
                    _slots[i].ShowEmptySlot();
                }
            }
            
            //PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
            //{
            //    CharacterId = characters.First().CharacterId
            //},
            //result =>
            //{
            //   var level = result.CharacterStatistics["Level"].ToString();
            //    var wood = result.CharacterStatistics["Wood"].ToString();
            //    var damage = result.CharacterStatistics["Damage"].ToString();
            //    var health = result.CharacterStatistics["Health"].ToString();
            //    var exp = result.CharacterStatistics["Exp"].ToString();


            //    _slots.First().ShowInfoCharacterSlot(characters.First().CharacterName, level, wood, damage, health, exp);

            //}, OnError);
         }
        else
        {
            Debug.LogError("Add slots for characters");
        }    
    }

    private void OnGetRandomResultTables(PlayFab.ServerModels.GetRandomResultTablesResult result)
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
