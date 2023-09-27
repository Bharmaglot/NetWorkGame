using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotCharacterWidget : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _emptySlot;
    [SerializeField] private GameObject _infoCharacterSlot;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _levelLabel;
    [SerializeField] private TMP_Text _woodLabel;
    [SerializeField] private TMP_Text _damageLabel;
    [SerializeField] private TMP_Text _healthLabel;
    [SerializeField] private TMP_Text _expLabel;

    public Button SlotButton => _button;

    public void ShowInfoCharacterSlot(string name, string level, string wood, string damage, string health, string exp)
    {
        _nameLabel.text = name;
        _levelLabel.text = $"Level: {level}";
        _woodLabel.text = $"Wood: {wood}";
        _damageLabel.text = $"Power: {damage}";
        _healthLabel.text = $"Health: {health}";
        _expLabel.text = $"Exp: {exp}";


        _infoCharacterSlot.gameObject.SetActive(true);
        _emptySlot.gameObject.SetActive(false);
    }

    public void ShowEmptySlot()
    {
        _infoCharacterSlot.gameObject.SetActive(false);
        _emptySlot.gameObject.SetActive(true);
    }
}
