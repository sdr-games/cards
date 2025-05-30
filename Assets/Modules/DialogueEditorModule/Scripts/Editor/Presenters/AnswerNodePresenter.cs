using System;

using SDRGames.Whist.DialogueEditorModule.Models;
using SDRGames.Whist.DialogueEditorModule.Views;
using SDRGames.Whist.DialogueModule.ScriptableObjects;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.DialogueEditorModule.Presenters
{
    public class AnswerNodePresenter : BaseNodePresenter
    {
        private AnswerData _data;
        private AnswerNodeView _nodeView;

        public AnswerNodePresenter() 
        {
            _nodeView = (AnswerNodeView)Activator.CreateInstance(typeof(AnswerNodeView));
            _nodeView.NodeNameTextFieldChanged += OnNodeNameTextFieldChanged;
        }

        public override void Initialize(string name, Vector2 position)
        {
            _data = new AnswerData(name);

            _nodeView.Initialize(_data.ID, _data.NodeName, position);
            _nodeView.SavedToSO += OnSavedToSO;
            _nodeView.Loaded += OnLoaded;
            _nodeView.TextLocalizationFieldChanged += OnTextLocalizationFieldChanged;
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

        protected void OnSavedToSO(object sender, SavedToSOEventArgs<DialogueAnswerScriptableObject> e)
        {
            _data.SaveToSO(e.DialogueSO);
        }

        private void OnLoaded(object sender, AnswerLoadedEventArgs e)
        {
            _data.Load(e.Character, e.TextLocalization);
        }

        private void OnCharacterUpdated(object sender, CharacterUpdatedEventArgs e)
        {
            _data.SetCharacter(e.Character);
        }
    }
}
