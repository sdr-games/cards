using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicSelectController : ObjectSelectController
{
    [SerializeField] private DecksPanelView _deckPanelView;

    // Start is called before the first frame update
    public void Initialize()
    {
        base.Initialize();
        _deckPanelView.Initialize();
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
}
