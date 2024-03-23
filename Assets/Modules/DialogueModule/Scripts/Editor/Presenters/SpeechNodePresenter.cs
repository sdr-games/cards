using System;

using SDRGames.Whist.DialogueModule.Editor.Models;
using SDRGames.Whist.DialogueModule.Editor.Views;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.DialogueModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.DialogueModule.Editor.Presenters
{
    public class SpeechNodePresenter : BaseNodePresenter
    {
        private SpeechData _data;
        private SpeechNodeView _nodeView;

        public SpeechNodePresenter() 
        {
            _nodeView = (SpeechNodeView)Activator.CreateInstance(typeof(SpeechNodeView));
            _nodeView.NodeNameTextFieldChanged += OnNodeNameTextFieldChanged;
        }

        public override void Initialize(string name, Vector2 position)
        {
            LocalizationData textLocalization = new LocalizationData("", "", "");

            _data = new SpeechData(name, position, textLocalization);

            _nodeView.Initialize(_data.ID, _data.NodeName, position, _data.Character, _data.TextLocalization);
            _nodeView.SavedToSO += OnSavedToSO;
            _nodeView.CharacterUpdated += OnCharacterUpdated;
        }

        public override BaseNodeView GetNodeView()
        {
            return _nodeView;
        }

        protected override void OnNodeNameTextFieldChanged(object sender, NodeNameChangedEventArgs e)
        {
            _data.SetNodeName(e.NewNode.NodeName);
        }

        protected void OnSavedToSO(object sender, SavedToSOEventArgs<DialogueSpeechScriptableObject> e)
        {
            _data.SaveToSO(e.DialogueSO);
        }

        private void OnCharacterUpdated(object sender, CharacterUpdatedEventArgs e)
        {
            _data.SetCharacter(e.Character);
        }
    }
}
