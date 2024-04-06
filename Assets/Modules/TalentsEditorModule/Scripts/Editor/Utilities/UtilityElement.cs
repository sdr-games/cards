using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.TalentsEditorModule.Views;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

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
    }
}