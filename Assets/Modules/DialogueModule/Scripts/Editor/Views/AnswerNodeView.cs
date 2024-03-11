using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class AnswerNodeView : BaseNodeView
    {
        private LocalizationSaveData _characterNameLocalization;
        private LocalizationSaveData _textLocalization;

        public void Initialize(string nodeName, Vector2 position, LocalizationSaveData characterNameLocalization, LocalizationSaveData textLocalization)
        {
            base.Initialize(nodeName, position);

            _characterNameLocalization = characterNameLocalization;
            _textLocalization = textLocalization;

            CreateAnswerPort(typeof(AnswerNodeView), true);

            Port inputPort = this.CreatePort(typeof(SpeechNodeView), "Speech Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            InputPorts.Add(inputPort);
        }

        public override void Draw()
        {
            base.Draw();

            /* INPUT CONTAINER */

            inputContainer.Add(InputPorts[0]);

            /* OUTPUT CONTAINER */

            foreach (Port port in OutputPorts)
            {
                outputContainer.Add(port);
            }

            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout characterNameFoldout = UtilityElement.CreateFoldout("Character Name", true);

            Box characterNameLocalizationBox = UtilityElement.CreateLocalizationBox(_characterNameLocalization);
            characterNameFoldout.Add(characterNameLocalizationBox);

            Foldout textFoldout = UtilityElement.CreateFoldout("Answer Text", true);

            Box localizationBox = UtilityElement.CreateLocalizationBox(_textLocalization);
            textFoldout.Add(localizationBox);

            customDataContainer.Add(characterNameFoldout);
            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }

        //public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        //{
        //    //SaveData.SetPosition(GetPosition().position);
        //    //graphData.AnswerNodes.Add(SaveData);
        //}
    }
}
