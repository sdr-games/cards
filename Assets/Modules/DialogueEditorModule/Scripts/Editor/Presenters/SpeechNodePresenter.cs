using System;

using SDRGames.Whist.DialogueEditorModule.Models;
using SDRGames.Whist.DialogueEditorModule.Views;
using SDRGames.Whist.DialogueModule.ScriptableObjects;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.DialogueEditorModule.Presenters
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
            _data = new SpeechData(name);

            _nodeView.Initialize(_data.ID, _data.NodeName, position);
            _nodeView.SavedToSO += OnSavedToSO;
            _nodeView.Loaded += OnLoaded;
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

        private void OnTextLocalizationFieldChanged(object sender, LocalizationDataChangedEventArgs e)
        {
            _data.SetTextLocalization(e.TextLocalization);
        }

        protected void OnSavedToSO(object sender, SavedToSOEventArgs<DialogueSpeechScriptableObject> e)
        {
            _data.SaveToSO(e.DialogueSO);
        }

        private void OnLoaded(object sender, SpeechLoadedEventArgs e)
        {
            _data.Load(e.Character, e.TextLocalization);
        }

        private void OnCharacterUpdated(object sender, CharacterUpdatedEventArgs e)
        {
            _data.SetCharacter(e.Character);
        }
    }
}
