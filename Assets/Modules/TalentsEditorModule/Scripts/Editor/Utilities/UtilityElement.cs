using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.TalentsEditorModule.Views;

using UnityEditor.Experimental.GraphView;
using UnityEditor.Localization;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEditor.UIElements;

using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

namespace SDRGames.Whist.TalentsEditorModule
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
                maxLength = 50,
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

        public static Box CreateLocalizationBox(LocalizationData localizationSaveData, string uss_class = "", EventHandler<LocalizationDataChangedEventArgs> onValueChangedEvent = null)
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
                        OnLocalizationDropdownChange(localizationSaveData, subBox, uss_class, onValueChangedEvent);                        
                    }
                );
                localizationTableDropdown.AddClasses(uss_class);

                DropdownField localizationTextDropdown = CreateLocalizationEntriesDropdown(localizationSaveData, subBox, uss_class, onValueChangedEvent);
                TextField localizationText = CreateTextArea(localizationSaveData.LocalizedTextPreview, isReadOnly: true);

                box.Add(localizationTableDropdown);
                if (localizationTextDropdown != null)
                {
                    subBox.Add(localizationTextDropdown);
                    subBox.Add(localizationText);
                }
                box.Add(subBox);
            }
            return box;
        }

        private static void OnLocalizationDropdownChange(LocalizationData localizationSaveData, Box box, string uss_class, EventHandler<LocalizationDataChangedEventArgs> onValueChangedEvent)
{
            box.Clear();
            DropdownField localizationTextDropdown = CreateLocalizationEntriesDropdown(localizationSaveData, box, uss_class, onValueChangedEvent);
            TextField localizationText = CreateTextArea(localizationSaveData.LocalizedTextPreview, isReadOnly: true);
            onValueChangedEvent?.Invoke(null, new LocalizationDataChangedEventArgs(localizationSaveData));
            if (localizationTextDropdown != null)
            {
                box.Add(localizationTextDropdown);
                box.Add(localizationText);
            }
        }
        private static DropdownField CreateLocalizationEntriesDropdown(LocalizationData localizationSaveData, Box box, string uss_class, EventHandler<LocalizationDataChangedEventArgs> onValueChangedEvent)
        {
            if(LocalizationSettings.ProjectLocale == null)
            {
                LocalizationSettings.Instance.GetSelectedLocale();
            }
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
                localizationSaveData.SetPreviewText(localizationEntries[localizationSaveData.SelectedEntryKey]);
                DropdownField localizationTextDropdown = CreateDropdownField
                (
                    localizationEntries.Keys.ToList(),
                    localizationSaveData.SelectedEntryKey,
                    null,
                    callback =>
                    {
                        localizationSaveData.SetEntryKey(callback.newValue);
                        localizationSaveData.SetPreviewText(localizationEntries[localizationSaveData.SelectedEntryKey]);
                        OnLocalizationDropdownChange(localizationSaveData, box, uss_class, onValueChangedEvent);
                    }
                );
                localizationTextDropdown.AddClasses(uss_class);
                return localizationTextDropdown;
            }
            return null;
        }
    }
}