using System;

using SDRGames.Whist.DialogueSystem.Editor.Models;
using SDRGames.Whist.DialogueSystem.Editor.Views;

using UnityEngine;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.Editor.Presenters
{
    public class StartNodePresenter : BaseNodePresenter
    {
        public BaseData Data { get; private set; }
        public StartNodeView NodeView { get; private set; }

        public StartNodePresenter()
        {
            NodeView = (StartNodeView)Activator.CreateInstance(typeof(StartNodeView));
            NodeView.NodeNameTextFieldChanged += OnNodeNameTextFieldChanged;
        }

        public override void Initialize(string name, Vector2 position)
        {
            Data = new BaseData(name, position);
            Data.SetNodeType(NodeTypes.Start);
            
            NodeView.Initialize(name, position);
        }

        public override BaseNodeView GetNodeView()
        {
            return NodeView;
        }

        public override BaseData GetData()
        {
            return Data;
        }

        protected override void OnNodeNameTextFieldChanged(object sender, NodeNameChangedEventArgs e)
        {
            Data.SetName(e.NewNode.NodeName);
        }
    }
}
