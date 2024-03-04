using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor.Experimental.GraphView;
using UnityEditor.Localization;
using UnityEditor.UIElements;

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

        public static Port CreatePort(this BaseNode node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(SpeechNode));
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

        public static ObjectField CreateObjectField(Type objectType, UnityEngine.Object value = null, string label = null, EventCallback<ChangeEvent<UnityEngine.Object>> onValueChanged = null)
        {
            ObjectField objectField = new ObjectField()
            {
                value = value,
                label = label,
                objectType = objectType,
            };

            if (onValueChanged != null)
            {
                objectField.RegisterCallback(onValueChanged);
            }

            return objectField;
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

        public static Box CreateLocalizationBox(LocalizationSaveData localizationSaveData, string uss_class = "")
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
                    localizationSaveData.SelectedLocalizationTable = stringTablesNames[0];
                }
                Box subBox = new Box();

                DropdownField localizationTableDropdown = CreateDropdownField
                (
                    stringTablesNames,
                    localizationSaveData.SelectedLocalizationTable,
                    null,
                    callback =>
                    {
                        localizationSaveData.SelectedLocalizationTable = callback.newValue;
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

        private static void OnLocalizationDropdownChange(LocalizationSaveData localizationSaveData, Box box, string uss_class = "")
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

        public static DropdownField CreateLocalizationEntriesDropdown(LocalizationSaveData localizationSaveData, Box box, string uss_class)
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
                if(string.IsNullOrEmpty(localizationSaveData.SelectedEntryKey) || !localizationEntries.ContainsKey(localizationSaveData.SelectedEntryKey))
                {
                    localizationSaveData.SelectedEntryKey = localizationEntries.Keys.First();
                }
                localizationSaveData.LocalizedText = localizationEntries[localizationSaveData.SelectedEntryKey];
                DropdownField localizationTextDropdown = CreateDropdownField
                (
                    localizationEntries.Keys.ToList(),
                    localizationSaveData.SelectedEntryKey,
                    null,
                    callback =>
                    {
                        localizationSaveData.SelectedEntryKey = callback.newValue;
                        localizationSaveData.LocalizedText = localizationEntries[localizationSaveData.SelectedEntryKey];
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