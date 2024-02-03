using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicSelectController : ObjectSelectController
{
    [SerializeField] private DecksPanelView _deckPanelView;
    [SerializeField] private HandsView _handsView;

    public event EventHandler<List<Card>> AttackApplied;
    // Start is called before the first frame update
    public void Initialize()
    {
        base.Initialize();
        _deckPanelView.Initialize();
        _handsView.CardsApplied += ApplyMagicAttack;
    }

    private void OnMouseOver()
    {
        if (!enabled)
        {
            return;
        }
        base.OnMouseOver();
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _deckPanelView.SwitchVisibility();
        }
    }

    private void ApplyMagicAttack(object sender, List<Card> e)
    {
        AttackApplied?.Invoke(sender, e);
    }

    public void Deselect()
    {
        _material.DisableKeyword("_EMISSION");
        Selected = false;
        _handsView.Hide();
    }
}
