using System;

using SDRGames.Whist.TalentsEditorModule.Models;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class VariableView : VisualElement
    {
        private bool _isSelected;
        private BlackboardField _variableField;
        private VariableDetailWindow _variableDetailWindow;

        public event EventHandler<VariableSelectedEventArgs> VariableSelected; 
        public event EventHandler<VariableSelectedEventArgs> VariableDeselected; 
        public event EventHandler<NameFieldValueChangedEventArgs> NameFieldValueChanged;
        public event EventHandler<ValueFieldValueChangedEventArgs> ValueFieldValueChanged;

        public VariableView(VariableData variable)
        {
            _variableField = new BlackboardField { text = variable.Name, typeText = variable.Type.ToString(), userData = variable };
            _variableField.RegisterCallback<MouseDownEvent>(OnVariableSelected);
            _variableField.capabilities = Capabilities.Selectable | Capabilities.Deletable | Capabilities.Droppable;
            Add(_variableField);
        }

        private void OnVariableSelected(MouseDownEvent evt)
        {
            if (evt.clickCount == 1 && evt.button == (int)MouseButton.LeftMouse)
            {
                _isSelected = !_isSelected;
                if (_isSelected)
                {
                    Select((evt.currentTarget as BlackboardField).userData as VariableData, evt.mousePosition);
                    return;
                }
                Deselect();
            }
        }

        private void Select(VariableData variable, Vector2 position)
        {
            _variableDetailWindow = new VariableDetailWindow(variable, position);
            _variableDetailWindow.NameFieldValueChanged += OnNameFieldValueChanged;
            _variableDetailWindow.ValueFieldValueChanged += OnValueFieldValueChanged;
            VariableSelected?.Invoke(this, new VariableSelectedEventArgs(_variableDetailWindow));
        }

        private void Deselect()
        {
            _variableDetailWindow.NameFieldValueChanged -= OnNameFieldValueChanged;
            _variableDetailWindow.ValueFieldValueChanged -= OnValueFieldValueChanged;
            VariableDeselected?.Invoke(this, new VariableSelectedEventArgs(_variableDetailWindow));
        }

        private void OnNameFieldValueChanged(object sender, NameFieldValueChangedEventArgs e)
        {
            _variableField.text = e.Name;
            NameFieldValueChanged?.Invoke(this, e);
        }

        private void OnValueFieldValueChanged(object sender, ValueFieldValueChangedEventArgs e)
        {
            ValueFieldValueChanged?.Invoke(this, e);
        }
    }
}
