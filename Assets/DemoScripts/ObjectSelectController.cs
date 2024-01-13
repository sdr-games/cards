using System;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class ObjectSelectController : MonoBehaviour
{
    private const float COLOR_INTENSITY = 1f;

    private Color[] Colors = { new Color(0.1f, 0, 0), new Color(0.1f, 0.07f, 0), new Color(0, 0.07f, 0) };
    private Color _color;

    [SerializeField] protected Material _material;
    [SerializeField][Range(0, 100)] private float _hitChance;

    [field: SerializeField] public float DamageMultiplier { get; private set; }

    [field: SerializeField] public bool Selected { get; protected set; }

    public float HitChance => _hitChance;

    public event EventHandler ObjectSelected;
    public event EventHandler ObjectDeselected;

    public void Highlight()
    {
        _material.EnableKeyword("_EMISSION");
    }

    public void CancelHighlight()
    {
        _material.DisableKeyword("_EMISSION");
    }

    public bool CheckHitSuccess()
    {
        float random = UnityEngine.Random.Range(0, 100);
        return HitChance > random;
    }

    public void Select()
    {
        Selected = true;
    }

    public void Initialize()
    {
        if (_material != null)
        {
            int index = _hitChance < 66 ? _hitChance < 33 ? 0 : 1 : 2;
            _color = Colors[index] * COLOR_INTENSITY;
            _material.SetVector("_EmissionColor", _color);
            CancelHighlight();
        }
        enabled = false;
    }

    protected void OnMouseOver()
    {
        if(!enabled)
        {
            return;
        }

        Highlight();
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            Select();
            ObjectSelected?.Invoke(this, EventArgs.Empty);
        }
        else if(Mouse.current.rightButton.wasPressedThisFrame)
        {
            Selected = false;
            ObjectDeselected?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnMouseExit()
    {
        if(Selected || !enabled)
        {
            return;
        }
        CancelHighlight();
    }
}
