using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PinView : MonoBehaviour
{
    private readonly Color DONE_COLOR = new Color(0, 0.8f, 0);
    private readonly Color ACTIVE_COLOR = new Color(0.8f, 0.4f, 0.1f);

    [field: SerializeField] public RectTransform RectTransform { get; private set; }
    [field: SerializeField] public string Text { get; private set; }

    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private PinTipView _tipView;
    [SerializeField] private PinTip _tipData;
    [SerializeField] private bool _autoFinish;

    private enum Status { Blocked, Finished, Done, Active }
    private Status _status;

    public event EventHandler<bool> CTAButtonClicked;
    public event EventHandler PinClicked;

    public void Initialize()
    {
        _button.interactable = false;
        _status = Status.Blocked;
        _tipView.Initialize(_tipData);
    }

    public void MarkAsActive()
    {
        _status = Status.Active;
        _button.interactable = true;
        _image.color = ACTIVE_COLOR;
    }

    public void MarkAsDone()
    {
        _image.color = DONE_COLOR;
        if (_autoFinish)
        {
            MarkAsFinished();
            return;
        }
        _status = Status.Done;
    }

    public void MarkAsFinished()
    {
        _status = Status.Finished;
        _button.interactable = false;
    }

    public void PinClick()
    {
        switch (_status)
        {
            case Status.Done:
                transform.SetAsLastSibling();
                _tipView.CTAButtonDoneStatusClicked += OnCTAButtonClick;
                ChangeTipVisibility();
                break;
            case Status.Active:
                transform.SetAsLastSibling();
                _tipView.CTAButtonActiveStatusClicked += OnCTAButtonClick;
                ChangeTipVisibility(true);
                break;
            default:
                break;
        }
        PinClicked?.Invoke(this, EventArgs.Empty);
    }

    private void ChangeTipVisibility(bool isActive = false)
    {
        _tipView.ChangeVisilibity(isActive);
    }

    private void OnCTAButtonClick(object sender, bool isActive)
    {
        CTAButtonClicked?.Invoke(this, isActive);
    }
}
