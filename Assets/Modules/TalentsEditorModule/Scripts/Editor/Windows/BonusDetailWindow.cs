using System;

using SDRGames.Whist.TalentsEditorModule.Models;
using SDRGames.Whist.TalentsEditorModule.Views;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.BonusScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule
{
    public class BonusDetailWindow : GraphElement
    {
        public event EventHandler<NameFieldValueChangedEventArgs> NameFieldValueChanged;
        public event EventHandler<ValueFieldValueChangedEventArgs> ValueFieldValueChanged;

        public BonusDetailWindow(BonusData bonus, Vector2 position)
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
                bonus.Name,
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

            switch (bonus.Type)
            {
                case BonusTypes.HalfAstraBonus:
                case BonusTypes.FullAstraBonus:
                case BonusTypes.FullTalamusBonus:
                    ObjectField valueObjectField = UtilityElement.CreateObjectField(
                        typeof(ScriptableObject),
                        bonus.Value,
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
