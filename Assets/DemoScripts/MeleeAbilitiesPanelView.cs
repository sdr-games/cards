using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeAbilitiesPanelView : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private MeleeAbilityView _meleeAvilityViewPrefab;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private SelectedAbilitiesPanelView _selectedAbilitiesPanelView;

    [SerializeField] private MeleeAbility[] _meleeAbilities;

    private RectTransform _scrollRectContent;

    private bool _showed;

    public event EventHandler<List<MeleeAbility>> AttackApplied;

    private void Start()
    {
        _scrollRectContent = _scrollRect.content;
        foreach (MeleeAbility ability in _meleeAbilities)
        {
            MeleeAbilityView meleeAbilityView = Instantiate(_meleeAvilityViewPrefab, _scrollRectContent);
            meleeAbilityView.Initialize(ability);
            meleeAbilityView.MeleeAbilitySelected += _selectedAbilitiesPanelView.SelectAbility;
        }

        _selectedAbilitiesPanelView.AttackApplied += OnAttackApplied;
    }

    private void OnAttackApplied(object sender, List<MeleeAbility> e)
    {
        AttackApplied?.Invoke(sender, e);
    }

    public void SwitchVisibility()
    {
        if(!_showed)
        {
            _canvasGroup.alpha = 1;
        }
        else
        {
            _canvasGroup.alpha = 0;
        }
        _selectedAbilitiesPanelView.SwitchVisibility(_showed);
        _showed = !_showed;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _showed = false;
        _selectedAbilitiesPanelView.SwitchVisibility(true);
    }
}
