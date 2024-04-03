using SDRGames.Whist.HelpersModule;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine.UIElements;

namespace SDRGames.Whist.HelpersEditorModule
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var propertyField = new PropertyField(property);
            propertyField.SetEnabled(false);
            return propertyField;
        }
    }
}
