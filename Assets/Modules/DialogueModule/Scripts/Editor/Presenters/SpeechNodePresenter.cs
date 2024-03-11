using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Models;
using SDRGames.Whist.DialogueSystem.Editor.Views;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Presenters
{
    public class SpeechNodePresenter : BaseNodePresenter
    {
        public SpeechData Data { get; private set; }
        public SpeechNodeView NodeView { get; private set; }

        public SpeechNodePresenter() 
        {
            NodeView = (SpeechNodeView)Activator.CreateInstance(typeof(SpeechNodeView));
            NodeView.NodeNameTextFieldChanged += OnNodeNameTextFieldChanged;
        }

        public override void Initialize(string name, Vector2 position)
        {
            LocalizationSaveData characterNameLocalization = new LocalizationSaveData("CharacterNames", "Valior", "Valior");
            LocalizationSaveData textLocalization = new LocalizationSaveData("", "", "");
            List<AnswerData> answerNodes = new List<AnswerData>();

            Data = new SpeechData(name, position, characterNameLocalization, textLocalization, answerNodes);

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
