using System;
using System.Collections.Generic;

using SDRGames.Islands.PointsModule.Model;
using SDRGames.Islands.PointsModule.Presenter;
using SDRGames.Islands.PointsModule.View;

using UnityEngine;

public class DefenceController : MonoBehaviour
{
    public List<ObjectSelectController> ObjectSelectedParts { get; private set; }

    [SerializeField] private Points _healthPoints;
    [SerializeField] private Points _armorPoints;
    [SerializeField] private Points _shieldPoints;

    [SerializeField] private PointsView _healthPointsView;
    [SerializeField] private PointsView _armorPointsView;
    [SerializeField] private PointsView _shieldPointsView;

    [SerializeField] private CanvasGroup _applyButton;
    [SerializeField] private CanvasGroup _clearButton;

    public bool IsDead => _healthPoints.CurrentValue <= 0;

    [field: SerializeField] public ObjectSelectController[] ObjectSelectableParts { get; private set; }

    public event EventHandler<ObjectSelectController[]> PlayerDefenceApplied;

    public void Initialize()
    {
        new PointsPresenter(_healthPoints, _healthPointsView);
        new PointsPresenter(_armorPoints, _armorPointsView);
        new PointsPresenter(_shieldPoints, _shieldPointsView);

        ObjectSelectedParts = new List<ObjectSelectController>();

        foreach (ObjectSelectController part in ObjectSelectableParts)
        {
            part.Initialize();
            part.ObjectSelected += OnObjectSelect;
            part.ObjectDeselected += OnObjectDeselect;
        }
    }

    public void EnableParts()
    {
        foreach (ObjectSelectController part in ObjectSelectableParts)
        {
            part.enabled = true;
        }
    }

    public void SetSelectedParts(List<ObjectSelectController> objectSelectedParts)
    {
        ObjectSelectedParts = objectSelectedParts;
    }

    public void TakeDamage(float damage)
    {
        _healthPoints.DecreaseCurrentValue(damage);
    }

    public void StartDefence()
    {
        EnableParts();
        ShowButtons();
    }

    public void ClearSelectedParts()
    {
        foreach (ObjectSelectController part in ObjectSelectableParts)
        {
            part.CancelHighlight();
            part.enabled = false;
        }
        ObjectSelectedParts.Clear();
    }

    public void Apply()
    {
        HideButtons();
        foreach (ObjectSelectController part in ObjectSelectableParts)
        {
            part.enabled = false;
        }
        PlayerDefenceApplied?.Invoke(this, ObjectSelectableParts);
    }

    private void OnObjectDeselect(object sender, EventArgs e)
    {
        if(ObjectSelectedParts.Count == 0)
        {
            return;
        }
        ObjectSelectController objectSelectController = (ObjectSelectController)sender;
        ObjectSelectedParts.Remove(objectSelectController);
        if(ObjectSelectedParts.Contains(objectSelectController)) {
            objectSelectController.Select();
        }
    }

    private void OnObjectSelect(object sender, EventArgs e)
    {
        if (ObjectSelectedParts.Count == 3)
        {
            return;
        }
        ObjectSelectController objectSelectController = (ObjectSelectController)sender;
        ObjectSelectedParts.Add(objectSelectController);
        objectSelectController.Highlight();
    }

    private void ShowButtons()
    {
        if (_applyButton.alpha > 0 || _clearButton.alpha > 0)
        {
            return;
        }
        _applyButton.alpha = 1;
        _applyButton.interactable = true;
        _applyButton.blocksRaycasts = true;
        _clearButton.alpha = 1;
        _clearButton.interactable = true;
        _clearButton.blocksRaycasts = true;
    }

    private void HideButtons()
    {
        if (_applyButton.alpha < 1 || _clearButton.alpha < 1)
        {
            return;
        }
        _applyButton.alpha = 0;
        _applyButton.interactable = false;
        _applyButton.blocksRaycasts = false;
        _clearButton.alpha = 0;
        _clearButton.interactable = false;
        _clearButton.blocksRaycasts = false;
    }
}
