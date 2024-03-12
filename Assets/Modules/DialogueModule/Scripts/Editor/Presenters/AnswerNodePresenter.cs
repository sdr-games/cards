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
        public AnswerData Data { get; private set; }
        public AnswerNodeView NodeView { get; private set; }

        public AnswerNodePresenter() 
        {
            NodeView = (AnswerNodeView)Activator.CreateInstance(typeof(AnswerNodeView));
            NodeView.NodeNameTextFieldChanged += OnNodeNameTextFieldChanged;
        }

        public override void Initialize(string name, Vector2 position)
        {
            LocalizationData characterNameLocalization = new LocalizationData("CharacterNames", "Valior", "Valior");
            LocalizationData textLocalization = new LocalizationData("", "", "");
            List<AnswerConditionSaveData> conditions = new List<AnswerConditionSaveData>();

            Data = new AnswerData(name, position, characterNameLocalization, textLocalization, conditions);

            NodeView.Initialize(Data.ID, Data.NodeName, position, Data.CharacterNameLocalization, Data.TextLocalization);
            NodeView.SavedToGraph += OnSavedToGraph;
            NodeView.SavedToSO += OnSavedToSO;
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
            Data.SetNodeName(e.NewNode.NodeName);
        }

        protected void OnSavedToGraph(object sender, SavedToGraphEventArgs e)
        {
            Data.SaveToGraph(e.GraphData, e.Position);
        }

        protected void OnSavedToSO(object sender, SavedToSOEventArgs<DialogueAnswerScriptableObject> e)
        {
            Data.SaveToSO(e.DialogueSO);
        }
    }
}
