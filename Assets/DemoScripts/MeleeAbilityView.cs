using System;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MeleeAbilityView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;

    private MeleeAbility _meleeAbility;

    public event EventHandler<MeleeAbility> MeleeAbilitySelected;

    public void Initialize(MeleeAbility meleeAbility)
    {
        _meleeAbility = meleeAbility;
        _icon.sprite = _meleeAbility.Icon;
        _name.text = _meleeAbility.Name;
    }

    public void SelectAbility()
    {
        MeleeAbilitySelected?.Invoke(this, _meleeAbility);
    }
}
