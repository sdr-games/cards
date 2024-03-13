using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Models;
using SDRGames.Whist.DialogueSystem.Editor.Views;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.Editor.Presenters
{
    public class StartNodePresenter : BaseNodePresenter
    {
        private BaseData _data;
        private StartNodeView _nodeView;

        public StartNodePresenter()
        {
            _nodeView = (StartNodeView)Activator.CreateInstance(typeof(StartNodeView));
            _nodeView.NodeNameTextFieldChanged += OnNodeNameTextFieldChanged;
        }

        public override void Initialize(string name, Vector2 position)
        {
            _data = new BaseData(name, position);
            _data.SetNodeType(NodeTypes.Start);
            
            _nodeView.Initialize(_data.ID, _data.NodeName, position);
            _nodeView.SavedToSO += OnSavedToSO;
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

        protected void OnSavedToSO(object sender, SavedToSOEventArgs<DialogueStartScriptableObject> e)
        {
            _data.SaveToSO(e.DialogueSO);
        }
    }
}
