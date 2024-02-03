using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PhraseView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameTMP;
    [SerializeField] private TextMeshProUGUI _textTMP;
    [SerializeField] private Image _portrait;

    public void Initialize(Phrase phrase)
    {
        _nameTMP.text = phrase.Name;
        _textTMP.text = phrase.Text;
        _portrait.sprite = phrase.Portrait;
    }
}
