using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Models;
using SDRGames.Whist.DialogueSystem.Editor.Views;

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
            LocalizationSaveData characterNameLocalization = new LocalizationSaveData("CharacterNames", "Valior", "Valior");
            LocalizationSaveData textLocalization = new LocalizationSaveData("", "", "");
            List<AnswerConditionSaveData> conditions = new List<AnswerConditionSaveData>();

            Data = new AnswerData(name, position, characterNameLocalization, textLocalization, conditions);

            NodeView.Initialize(Data.Name, position, Data.CharacterNameLocalization, Data.TextLocalization);
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
