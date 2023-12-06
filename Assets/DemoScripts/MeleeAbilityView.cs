using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MeleeAbilityView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;

    public void Initialize(Sprite sprite, string name)
    {
        _icon.sprite = sprite;
        _name.text = name;
    }
}
