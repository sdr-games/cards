using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Models;
using SDRGames.Whist.DialogueSystem.Editor.Views;
using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Presenters
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
            LocalizationData characterNameLocalization = new LocalizationData("CharacterNames", "Valior", "Valior");
            LocalizationData textLocalization = new LocalizationData("", "", "");

            _data = new SpeechData(name, position, characterNameLocalization, textLocalization);

            _nodeView.Initialize(_data.ID, _data.NodeName, position, _data.CharacterNameLocalization, _data.TextLocalization);
            _nodeView.SavedToSO += OnSavedToSO;
        }

        public void CreateInputRelationship(BaseNodePresenter outputNodePresenter)
        {
            //_data.AddAnswer(outputNodePresenter.GetData());

            //if (!_relationships.ContainsKey(inputNodeView))
            //{
            //    _relationships.Add(inputNodeView, outputNodeView);
            //    RelationshipAdded?.Invoke(this, new RelationshipAddedEventArgs(inputNodeView, outputNodeView));
            //}
        }

        public void RemoveRelationship(BaseNodeView inputNodeView)
        {
            //if (_relationships.ContainsKey(inputNodeView))
            //{
            //    _relationships.Remove(inputNodeView);
            //}
        }

        public override BaseNodeView GetNodeView()
        {
            return _nodeView;
        }

        public override BaseData GetData()
        {
            return _data;
        }

        protected override void OnNodeNameTextFieldChanged(object sender, NodeNameChangedEventArgs e)
        {
            _data.SetNodeName(e.NewNode.NodeName);
        }

        protected void OnSavedToSO(object sender, SavedToSOEventArgs<DialogueSpeechScriptableObject> e)
        {
            _data.SaveToSO(e.DialogueSO);
        }
    }
}
