using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Models;
using SDRGames.Whist.DialogueSystem.Editor.Views;
using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Presenters
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
            LocalizationData textLocalization = new LocalizationData("", "", "");

            _data = new AnswerData(name, position, textLocalization);

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

        protected void OnSavedToSO(object sender, SavedToSOEventArgs<DialogueAnswerScriptableObject> e)
        {
            _data.SaveToSO(e.DialogueSO);
        }

        private void OnCharacterUpdated(object sender, CharacterUpdatedEventArgs e)
        {
            _data.SetCharacter(e.Character);
        }
    }
}
