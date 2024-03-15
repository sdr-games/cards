using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.DialogueSystem.Editor.Views;
using SDRGames.Whist.DialogueSystem.Models;

using UnityEditor.Experimental.GraphView;
using UnityEditor.Localization;

using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public static class UtilityElement
    {
        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };
            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };
            return foldout;
        }

        public static PortView CreatePort(this BaseNodeView node, Type nodeType, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            PortView port = (PortView)node.InstantiatePort(orientation, direction, capacity, nodeType);
            port.portName = portName;
            return port;
        }

        public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null, bool isReadOnly = false)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label,
                maxLength = 20,
                isReadOnly = isReadOnly
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }
            return textField;
        }

        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null, bool isReadOnly = false)
        {
            TextField textArea = CreateTextField(value, label, onValueChanged, isReadOnly);
            textArea.multiline = true;
            return textArea;
        }

        public static Toggle CreateBoolField(bool value = false, string label = null, EventCallback<ChangeEvent<bool>> onValueChanged = null)
        {
            Toggle toggle = new Toggle()
            {
                value = value,
                text = " " + label
            };

            if (onValueChanged != null)
            {
                toggle.RegisterValueChangedCallback(onValueChanged);
            }
            return toggle;
        }

        public static DropdownField CreateDropdownField(Type enumType, string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            DropdownField dropdownField = new DropdownField()
            {
                value = value,
                label = label,
                choices = Enum.GetNames(enumType).OfType<string>().ToList()
            };

            if (onValueChanged != null)
            {
                dropdownField.RegisterValueChangedCallback(onValueChanged);
            }
            return dropdownField;
        }

        public static DropdownField CreateDropdownField(List<string> choices, string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            DropdownField dropdownField = new DropdownField()
            {
                value = value,
                label = label,
                choices = choices
            };

            if (onValueChanged != null)
            {
                dropdownField.RegisterValueChangedCallback(onValueChanged);
            }
            return dropdownField;
        }

        public static Box CreateLocalizationBox(LocalizationData localizationSaveData, string uss_class = "")
        {
            List<string> stringTablesNames = new List<string>();
            Box box = new Box();

            foreach (var stringTable in LocalizationEditorSettings.GetStringTableCollections())
            {
                //if (stringTable.name.Contains("Dialogue"))
                //{
                stringTablesNames.Add(stringTable.name);
                //}
            }
            if (stringTablesNames.Count > 0)
            {
                if (string.IsNullOrEmpty(localizationSaveData.SelectedLocalizationTable))
                {
                    localizationSaveData.SetLocalizationTable(stringTablesNames[0]);
                }
                Box subBox = new Box();

                DropdownField localizationTableDropdown = CreateDropdownField
                (
                    stringTablesNames,
                    localizationSaveData.SelectedLocalizationTable,
                    null,
                    callback =>
                    {
                        localizationSaveData.SetLocalizationTable(callback.newValue);
                        OnLocalizationDropdownChange(localizationSaveData, subBox, uss_class);
                    }
                );
                localizationTableDropdown.AddClasses(uss_class);

                DropdownField localizationTextDropdown = CreateLocalizationEntriesDropdown(localizationSaveData, subBox, uss_class);
                Foldout localizationTextFoldout = CreateFoldout("Localized text", true);
                TextField localizationText = CreateTextArea(localizationSaveData.LocalizedText, isReadOnly: true);

                localizationTextFoldout.Add(localizationText);
                box.Add(localizationTableDropdown);
                if (localizationTextDropdown != null)
                {
                    subBox.Add(localizationTextDropdown);
                    subBox.Add(localizationTextFoldout);
                }
                box.Add(subBox);
            }
            return box;
        }

        //public static void CreateConditionField(Foldout foldout, AnswerConditionSaveData conditionData)
        //{
        //    Foldout conditionFoldout = CreateFoldout($"{conditionData.AnswerConditionType}", true);
        //    Box box = new Box();

        //    DropdownField dropdownField = CreateDropdownField(typeof(AnswerConditionTypes), conditionData.AnswerConditionType.ToString(), null, callback =>
        //    {
        //        conditionData.AnswerConditionType = Enum.Parse<AnswerConditionTypes>(callback.newValue);
        //        conditionFoldout.text = $"{conditionData.AnswerConditionType}";
        //        box.Clear();
        //        box = CreateConditionCheckField(box, conditionData);
        //    });

        //    Toggle reverseToggle = CreateBoolField(conditionData.Reversed, "Reversed", callback => conditionData.Reversed = callback.newValue);

        //    box = CreateConditionCheckField(box, conditionData);

        //    conditionFoldout.Add(dropdownField);
        //    conditionFoldout.Add(reverseToggle);
        //    conditionFoldout.Add(box);
        //    foldout.Add(conditionFoldout);
        //}

        //public static Box CreateConditionCheckField(Box box, AnswerConditionSaveData conditionData)
        //{
        //    switch (conditionData.AnswerConditionType)
        //    {
        //        case AnswerConditionTypes.CharacteristicCheck:
        //            conditionData.Skill = SkillsNames.No;

        //            DropdownField dropdownField = CreateDropdownField(
        //                typeof(Characteristics),
        //                conditionData.Characteristic.ToString(),
        //                null,
        //                callback =>
        //                {
        //                    conditionData.Characteristic = (Characteristics)Enum.Parse(typeof(Characteristics), callback.newValue);
        //                }
        //            );
        //            Label label = new Label()
        //            {
        //                text = "more or equal"
        //            };
        //            label.AddToClassList("ds-node__label");
        //            TextField valueTextField = CreateTextField(
        //                conditionData.RequiredValue.ToString(),
        //                null,
        //                callback => conditionData.RequiredValue = int.Parse(callback.newValue)
        //            );
        //            box.Add(dropdownField);
        //            box.Add(label);
        //            box.Add(valueTextField);

        //            break;
        //        case AnswerConditionTypes.SkillCheck:
        //            dropdownField = CreateDropdownField(
        //                typeof(SkillsNames),
        //                conditionData.Skill.ToString(),
        //                null,
        //                callback => conditionData.Skill = (SkillsNames)Enum.Parse(typeof(SkillsNames), callback.newValue)
        //            );
        //            label = new Label()
        //            {
        //                text = "more or equal"
        //            };
        //            label.AddToClassList("ds-node__label");
        //            valueTextField = CreateTextField(
        //                conditionData.RequiredValue.ToString(),
        //                null,
        //                callback => conditionData.RequiredValue = int.Parse(callback.newValue)
        //            );
        //            box.Add(dropdownField);
        //            box.Add(label);
        //            box.Add(valueTextField);

        //            break;
        //        default:
        //            break;
        //    }
        //    return box;
        //}

        private static void OnLocalizationDropdownChange(LocalizationData localizationSaveData, Box box, string uss_class = "")
        {
            box.Clear();
            DropdownField localizationTextDropdown = CreateLocalizationEntriesDropdown(localizationSaveData, box, uss_class);
            Foldout localizationTextFoldout = CreateFoldout("Localized text", true);
            TextField localizationText = CreateTextArea(localizationSaveData.LocalizedText, isReadOnly: true);
            localizationTextFoldout.Add(localizationText);
            if (localizationTextDropdown != null)
            {
                box.Add(localizationTextDropdown);
                box.Add(localizationTextFoldout);
            }
        }
        private static DropdownField CreateLocalizationEntriesDropdown(LocalizationData localizationSaveData, Box box, string uss_class)
        {
            Dictionary<string, string> localizationEntries = new Dictionary<string, string>();
            var currentLocale = LocalizationSettings.ProjectLocale.Formatter.ToString();
            var collection = LocalizationEditorSettings.GetStringTableCollection(localizationSaveData.SelectedLocalizationTable);
            var table = collection.GetTable(currentLocale) as StringTable;

            foreach (var entry in table.Values)
            {
                localizationEntries.Add(entry.SharedEntry.Key, entry.Value);
            }
            if (localizationEntries.Count > 0)
            {
                if (string.IsNullOrEmpty(localizationSaveData.SelectedEntryKey) || !localizationEntries.ContainsKey(localizationSaveData.SelectedEntryKey))
                {
                    localizationSaveData.SetEntryKey(localizationEntries.Keys.First());
                }
                localizationSaveData.SetText(localizationEntries[localizationSaveData.SelectedEntryKey]);
                DropdownField localizationTextDropdown = CreateDropdownField
                (
                    localizationEntries.Keys.ToList(),
                    localizationSaveData.SelectedEntryKey,
                    null,
                    callback =>
                    {
                        localizationSaveData.SetEntryKey(callback.newValue);
                        localizationSaveData.SetText(localizationEntries[localizationSaveData.SelectedEntryKey]);
                        OnLocalizationDropdownChange(localizationSaveData, box, uss_class);
                    }
                );
                localizationTextDropdown.AddClasses(uss_class);
                return localizationTextDropdown;
            }
            return null;
        }
    }
}