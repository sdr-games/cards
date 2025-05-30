using System;

using SDRGames.Whist.DialogueEditorModule.Models;
using SDRGames.Whist.DialogueEditorModule.Views;
using SDRGames.Whist.DialogueModule.ScriptableObjects;

using UnityEngine;

using static SDRGames.Whist.DialogueModule.ScriptableObjects.DialogueScriptableObject;

namespace SDRGames.Whist.DialogueEditorModule.Presenters
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
            _data = new BaseData(name);
            _data.SetNodeType(NodeTypes.Start);
            
            _nodeView.Initialize(_data.ID, _data.NodeName, position);
            _nodeView.SavedToSO += OnSavedToSO;
        }

        public override BaseNodeView GetNodeView()
        {
            return _nodeView;
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
