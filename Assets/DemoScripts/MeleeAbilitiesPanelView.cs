using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeAbilitiesPanelView : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private MeleeAbilityView _meleeAvilityViewPrefab;
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private MeleeAbility[] _meleeAbilities;

    private RectTransform _scrollRectContent;

    private bool _showed;

    public void Initialize()
    {
        _scrollRectContent = _scrollRect.content;
        if(!_showed)
        {
            foreach(MeleeAbility ability in _meleeAbilities)
            {
                MeleeAbilityView meleeAbilityView = Instantiate(_meleeAvilityViewPrefab, _scrollRectContent);
                meleeAbilityView.Initialize(ability.Icon, ability.Name);
            }
            _canvasGroup.alpha = 1;
        }
    }
}
