using System;

using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    [Serializable]
    public class AnswerNodeView : BaseNodeView
    {
        [SerializeField] private LocalizationData _characterNameLocalization;
        [SerializeField] private LocalizationData _textLocalization;
        [SerializeField] private string _nextSpeechNodeID;

        public new event EventHandler<SavedToSOEventArgs<DialogueAnswerScriptableObject>> SavedToSO;

        public void Initialize(string id, string nodeName, Vector2 position, LocalizationData characterNameLocalization, LocalizationData textLocalization)
        {
            base.Initialize(id, nodeName, position);

            _characterNameLocalization = characterNameLocalization;
            _textLocalization = textLocalization;

            CreateInputPort(typeof(SpeechNodeView), NodeTypes.Speech);
            CreateOutputPort(typeof(AnswerNodeView), NodeTypes.Answer, true);
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

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            base.SaveToGraph(graphData);
            graphData.AddAnswerNode(this);
        }

        public override DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueAnswerScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<DialogueAnswerScriptableObject>($"{folderPath}/Dialogues", NodeName);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<DialogueAnswerScriptableObject>(dialogueSO));
            return dialogueSO;
        }

        public void SetNextSpeechNodeID(string speechNodeViewID)
        {
            _nextSpeechNodeID = speechNodeViewID;
        }

        public void UnsetNextSpeechNodeID()
        {
            _nextSpeechNodeID = "";
        }
    }
}
