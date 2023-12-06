using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelectController : ObjectSelectController
{
    [SerializeField] private MeleeAbilitiesPanelView _meleeAbilitiesPanelView;

    private void OnMouseOver()
    {
        base.OnMouseOver();
        if(_selected && Mouse.current.leftButton.wasPressedThisFrame)
        {
            _meleeAbilitiesPanelView.Initialize();
        }
    }
}
