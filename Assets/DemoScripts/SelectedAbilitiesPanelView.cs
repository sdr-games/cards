using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedAbilitiesPanelView : MonoBehaviour
{
    [SerializeField] private Image _selectedAbilityIconPrefab;
    [SerializeField] private float _maxSelectedAbilitiesCount;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private CanvasGroup _applyButton;
    [SerializeField] private CanvasGroup _clearButton;

    private LinkedList<Image> _emptyAbilitiesIconsLinkedList;
    private LinkedList<Image> _selectedAbilitiesIconsLinkedList;

    private List<MeleeAbility> _selectedAbilitiesList;

    public event EventHandler<List<MeleeAbility>> AttackApplied;

    private void Start()
    {
        _emptyAbilitiesIconsLinkedList = new LinkedList<Image>();
        _selectedAbilitiesIconsLinkedList = new LinkedList<Image>();
        _selectedAbilitiesList = new List<MeleeAbility>();

        for (int i = 0; i < _maxSelectedAbilitiesCount; i++)
        {
            Image selectedAbility = Instantiate(_selectedAbilityIconPrefab, transform);
            _emptyAbilitiesIconsLinkedList.AddLast(selectedAbility);
        }
    }

    public void SwitchVisibility(bool showed)
    {
        if(!showed)
        {
            _canvasGroup.alpha = 1;
        }
        else
        {
            _canvasGroup.alpha = 0;
            DeselectAbilities();
        }
    }

    public void SelectAbility(object sender, MeleeAbility meleeAbility)
    {
        if(_emptyAbilitiesIconsLinkedList.Count == 0)
        {
            return;
        }
        _selectedAbilitiesIconsLinkedList.AddLast(_emptyAbilitiesIconsLinkedList.First.Value);
        _selectedAbilitiesIconsLinkedList.Last.Value.sprite = meleeAbility.Icon;
        _emptyAbilitiesIconsLinkedList.RemoveFirst();
        _selectedAbilitiesList.Add(meleeAbility);

        ShowButtons();
    }

    public void DeselectAbilities()
    {
        foreach(Image selectedAbility in _emptyAbilitiesIconsLinkedList)
        {
            _selectedAbilitiesIconsLinkedList.AddLast(selectedAbility);
        }
        _emptyAbilitiesIconsLinkedList.Clear();
        foreach(Image selectedAbility in _selectedAbilitiesIconsLinkedList)
        {
            _emptyAbilitiesIconsLinkedList.AddLast(selectedAbility);
            _emptyAbilitiesIconsLinkedList.Last.Value.sprite = _defaultSprite;
        }
        _selectedAbilitiesIconsLinkedList.Clear();
        _selectedAbilitiesList.Clear();
        HideButtons();
    }

    private void ShowButtons()
    {
        if(_applyButton.alpha > 0 || _clearButton.alpha > 0)
        {
            return;
        }
        _applyButton.alpha = 1;
        _applyButton.interactable = true;
        _applyButton.blocksRaycasts = true;
        _clearButton.alpha = 1;
        _clearButton.interactable = true;
        _clearButton.blocksRaycasts = true;
    }

    private void HideButtons()
    {
        if (_applyButton.alpha < 1 || _clearButton.alpha < 1)
        {
            return;
        }
        _applyButton.alpha = 0;
        _applyButton.interactable = false;
        _applyButton.blocksRaycasts = false;
        _clearButton.alpha = 0;
        _clearButton.interactable = false;
        _clearButton.blocksRaycasts = false;
    }

    public void Apply()
    {
        if(_selectedAbilitiesList.Count == 0)
        {
            return;
        }
        AttackApplied?.Invoke(this, _selectedAbilitiesList);
    }
}
