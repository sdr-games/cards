using System;

using SDRGames.Whist.TalentsEditorModule.Models;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class BonusView : VisualElement
    {
        private bool _isSelected;
        private BlackboardField _bonusField;
        private BonusDetailWindow _bonusDetailWindow;

        public event EventHandler<BonusSelectedEventArgs> BonusSelected; 
        public event EventHandler<BonusSelectedEventArgs> BonusDeselected; 
        public event EventHandler<NameFieldValueChangedEventArgs> NameFieldValueChanged;
        public event EventHandler<ValueFieldValueChangedEventArgs> ValueFieldValueChanged;

        public BonusView(BonusData bonus)
        {
            _bonusField = new BlackboardField { text = bonus.Name, typeText = bonus.Type.ToString(), userData = bonus };
            _bonusField.RegisterCallback<MouseDownEvent>(onusOnBSelected);
            _bonusField.capabilities = Capabilities.Selectable | Capabilities.Deletable | Capabilities.Droppable;
            Add(_bonusField);
        }

        private void onusOnBSelected(MouseDownEvent evt)
        {
            if (evt.clickCount == 1 && evt.button == (int)MouseButton.LeftMouse)
            {
                _isSelected = !_isSelected;
                if (_isSelected)
                {
                    Select((evt.currentTarget as BlackboardField).userData as BonusData, evt.mousePosition);
                    return;
                }
                Deselect();
            }
        }

        private void Select(BonusData bonus, Vector2 position)
        {
            _bonusDetailWindow = new BonusDetailWindow(bonus, position);
            _bonusDetailWindow.NameFieldValueChanged += OnNameFieldValueChanged;
            _bonusDetailWindow.ValueFieldValueChanged += OnValueFieldValueChanged;
            BonusSelected?.Invoke(this, new BonusSelectedEventArgs(_bonusDetailWindow));
        }

        private void Deselect()
        {
            _bonusDetailWindow.NameFieldValueChanged -= OnNameFieldValueChanged;
            _bonusDetailWindow.ValueFieldValueChanged -= OnValueFieldValueChanged;
            BonusDeselected?.Invoke(this, new BonusSelectedEventArgs(_bonusDetailWindow));
        }

        private void OnNameFieldValueChanged(object sender, NameFieldValueChangedEventArgs e)
        {
            _bonusField.text = e.Name;
            NameFieldValueChanged?.Invoke(this, e);
        }

        private void OnValueFieldValueChanged(object sender, ValueFieldValueChangedEventArgs e)
        {
            ValueFieldValueChanged?.Invoke(this, e);
        }
    }
}
