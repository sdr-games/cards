using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.TalentsEditorModule
{
    public class ParametersWindow : GraphElement
    {
        private ObjectField _backgroundObjectField;

        public ParametersWindow()
        {
            capabilities = Capabilities.Resizable;

            /* HEADER */

            VisualElement header = new VisualElement();
            header.AddToClassList("header");

            Label title = new Label("Branch parameters");
            title.AddToClassList("title-label");

            header.Add(title);

            /* MAIN CONTAINER */

            VisualElement mainContainer = new VisualElement();
            mainContainer.AddToClassList("main-container");

            Foldout previewFoldout = UtilityElement.CreateFoldout("Preview", false);

            Box backgroundImagePreview = new Box();
            previewFoldout.Add(backgroundImagePreview);

            _backgroundObjectField = UtilityElement.CreateObjectField(typeof(Texture2D), null, "Background image:", callback =>
            {
                ((ObjectField)callback.target).value = callback.newValue;
                Background background = Background.FromTexture2D((Texture2D)callback.newValue);
                backgroundImagePreview.style.backgroundImage = background;

                if(background == null)
                {
                    backgroundImagePreview.style.width = 0;
                    backgroundImagePreview.style.height = 0;
                    return;
                }

                backgroundImagePreview.style.width = background.texture.width > previewFoldout.contentContainer.layout.width ? previewFoldout.contentContainer.layout.width : background.texture.width;
                float height = background.texture.height;
                if(background.texture.width > previewFoldout.contentContainer.layout.width)
                {
                    height = (height > background.texture.width) ? height / background.texture.width : background.texture.width / height;
                    height *= previewFoldout.contentContainer.layout.width;
                }
                backgroundImagePreview.style.height = height;
            });

            mainContainer.Add(_backgroundObjectField);
            mainContainer.Add(previewFoldout);
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

        public Texture2D GetBackgroundImageFieldValue()
        {
            return _backgroundObjectField.value as Texture2D;
        }

        public void SetBackgroundImageFieldValue(Texture2D backgroundImage)
        {
            _backgroundObjectField.value = backgroundImage;
        }
    }
}
