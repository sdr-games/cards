using System;

using SDRGames.Whist.TalentsEditorModule.Models;
using SDRGames.Whist.TalentsEditorModule.Views;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.TalentsEditorModule
{
    public class VariableDetailWindow : GraphElement
    {
        public event EventHandler<NameFieldValueChangedEventArgs> NameFieldValueChanged;
        public event EventHandler<ValueFieldValueChangedEventArgs> ValueFieldValueChanged;

        public VariableDetailWindow(VariableData variable, Vector2 position)
        {
            capabilities |= (Capabilities.Resizable | Capabilities.Movable);
            Dragger dragger = new Dragger
            {
                clampToParentEdges = true
            };
            this.AddManipulator(dragger);

            /* HEADER */

            VisualElement header = new VisualElement();
            header.AddToClassList("header");

            Label title = new Label("Variable values");
            title.AddToClassList("title-label");

            header.Add(title);

            /* MAIN CONTAINER */

            VisualElement mainContainer = new VisualElement();
            mainContainer.AddToClassList("main-container");

            TextField nameTextField = UtilityElement.CreateTextField(
                variable.Name,
                "Name",
                callback =>
                {
                    TextField target = (TextField)callback.target;
                    target.value = callback.newValue;
                    NameFieldValueChanged?.Invoke(this, new NameFieldValueChangedEventArgs(target.value));
                }
            );
            nameTextField.ClearClassList();

            mainContainer.Add(nameTextField);

            switch (variable.Type)
            {
                case VariableData.VariableTypes.HalfAstraBonus:
                case VariableData.VariableTypes.FullAstraBonus:
                case VariableData.VariableTypes.FullTalamusBonus:
                    ObjectField valueObjectField = UtilityElement.CreateObjectField(
                        typeof(ScriptableObject),
                        variable.Value,
                        "Value",
                        callback =>
                        {
                            ObjectField target = (ObjectField)callback.target;
                            target.value = callback.newValue;
                            ValueFieldValueChanged?.Invoke(this, new ValueFieldValueChangedEventArgs(target.value as ScriptableObject));
                        }
                    );
                    valueObjectField.ClearClassList();
                    mainContainer.Add(valueObjectField);
                    break;
                default:
                    break;
            }

            contentContainer.Add(header);
            contentContainer.Add(mainContainer);

            ClearClassList();
            AddToClassList("variable-detail-view");
            style.height = contentRect.height;
            style.left = position.x;
            style.top = position.y;
        }
    }
}
