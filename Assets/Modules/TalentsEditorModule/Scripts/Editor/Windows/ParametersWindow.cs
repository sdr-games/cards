using SDRGames.Whist.TalentsEditorModule.Managers;

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SDRGames.Whist.TalentsEditorModule
{
    public class ParametersWindow : GraphElement
    {
        private ObjectField _backgroundObjectField;

        public ParametersWindow(GraphManager graphManager)
        {
            capabilities = Capabilities.Selectable | Capabilities.Resizable | Capabilities.Ascendable | Capabilities.Collapsible;

            /* HEADER */

            VisualElement header = new VisualElement();
            header.AddToClassList("header");

            Label title = new Label("Branch parameters");
            title.AddToClassList("title-label");

            header.Add(title);

            /* MAIN CONTAINER */

            VisualElement mainContainer = new VisualElement();
            mainContainer.AddToClassList("main-container");

            _backgroundObjectField = UtilityElement.CreateObjectField(typeof(Sprite), null, "Background image:", callback =>
            {
                ((ObjectField)callback.target).value = callback.newValue;
                graphManager.SetBackgroundImage((Sprite)callback.newValue);
            });

            mainContainer.Add(_backgroundObjectField);
            contentContainer.Add(header);
            contentContainer.Add(mainContainer);

            ClearClassList();
            AddToClassList("variable-detail-view");
            style.height = contentRect.height;
            style.top = 50;
        }

        public void ClearParameters()
        {
            _backgroundObjectField.value = null;
        }

        public Sprite GetBackgroundImageFieldValue()
        {
            return _backgroundObjectField.value as Sprite;
        }

        public void SetBackgroundImageFieldValue(Sprite backgroundImage)
        {
            _backgroundObjectField.value = backgroundImage;
        }
    }
}
