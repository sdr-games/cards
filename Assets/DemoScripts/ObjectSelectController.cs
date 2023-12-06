using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectSelectController : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private Color _color;

    protected bool _selected;

    private void Start()
    {
        _material.DisableKeyword("_EMISSION");
    }

    protected void OnMouseOver()
    {
        _material.SetColor("_EmissionColor", _color);
        _material.EnableKeyword("_EMISSION");
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            _selected = !_selected;
        }
    }

    private void OnMouseExit()
    {
        if(_selected)
        {
            return;
        }
        _material.DisableKeyword("_EMISSION");
    }
}
