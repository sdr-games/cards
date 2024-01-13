using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelectController : ObjectSelectController
{
    [SerializeField] private MeleeAbilitiesPanelView _meleeAbilitiesPanelView;

    public event EventHandler<List<MeleeAbility>> AttackApplied;

    public void Initialize()
    {
        base.Initialize();
        _meleeAbilitiesPanelView.AttackApplied += OnAttackApplied;
    }

    private void OnAttackApplied(object sender, List<MeleeAbility> e)
    {
        AttackApplied?.Invoke(this, e);
    }

    public void Deselect()
    {
        _material.DisableKeyword("_EMISSION");
        Selected = false;
        _meleeAbilitiesPanelView.Hide();
    }

    private void OnMouseOver()
    {
        if(!enabled)
        {
            return;
        }
        base.OnMouseOver();
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            _meleeAbilitiesPanelView.SwitchVisibility();
        }
    }
}
