using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PinTipView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _banner;
    [SerializeField] private TextMeshProUGUI _headline;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Button _ctaButton;
    [SerializeField] private TextMeshProUGUI _ctaButtonText;

    public event EventHandler<bool> CTAButtonActiveStatusClicked;
    public event EventHandler<bool> CTAButtonDoneStatusClicked;

    private bool _isOpened;

    public void Initialize(PinTip tipData)
    {
        _banner.sprite = tipData.Sprite;
        _headline.text = tipData.Name;
        _description.text = tipData.Description;
    }

    public void ChangeVisilibity(bool isActive = false)
    {
        Debug.Log(_isOpened);
        if(_isOpened)
        {
            Hide();
            return;
        }
        Show(isActive);
    }

    public void Show(bool isActive)
    {
        ChangeVisibility(true);
        _ctaButton.image.color = isActive ? Color.red : Color.green;
        _ctaButtonText.text = isActive ? "Бой" : "Город";
        _isOpened = true;
        if (isActive)
        {
            _ctaButton.onClick.AddListener(CTAButtonActiveStatusClick);
            return;
        }
        _ctaButton.onClick.AddListener(CTAButtonDoneStatusClick);
    }

    public void Hide()
    {
        ChangeVisibility(false);
        _ctaButton.onClick.RemoveAllListeners();
        _isOpened = false;
    }

    private void ChangeVisibility(bool state)
    {
        _canvasGroup.alpha = state ? 1 : 0;
        _canvasGroup.interactable = state;
        _canvasGroup.blocksRaycasts = state;
    }

    private void CTAButtonActiveStatusClick()
    {
        CTAButtonActiveStatusClicked?.Invoke(this, true);
        Hide();
    }

    private void CTAButtonDoneStatusClick()
    {
        CTAButtonDoneStatusClicked?.Invoke(this, false);
        Hide();
    }
}
