using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Models
{
    [Serializable]
    public class SpeechData : BaseData
    {
        [field: SerializeField] public LocalizationData CharacterNameLocalization { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }
        [field: SerializeField] public List<string> Connections { get; private set; }

        public SpeechData(string name, Vector2 position, LocalizationData characterNameLocalization, LocalizationData textLocalization, List<string> answers) : base(name, position)
        {
            NodeType = Managers.GraphManager.NodeTypes.Speech;
            CharacterNameLocalization = characterNameLocalization;
            TextLocalization = textLocalization;
            Connections = answers;
        }

        public void SetID(string id)
        {
            ID = id;
        }

        public override void SetNodeName(string name)
        {
            NodeName = name;
        }

        public void SetCharacterNameLocalization(LocalizationData characterNameLocalization)
        {
            CharacterNameLocalization = characterNameLocalization;
        }

        public void SetTextLocalization(LocalizationData textLocalization)
        {
            TextLocalization = textLocalization;
        }

        public void SetConnections(List<string> connections)
        {
            Connections = connections;
        }

        public void AddConnection(string connection)
        {
            if (!Connections.Contains(connection))
            {
                Connections.Add(connection);
            }
        }

        public void RemoveConnection(string connection)
        {
            if (Connections.Contains(connection))
            {
                Connections.Remove(connection);
            }
        }

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData, Vector2 position)
        {
            SetPosition(position);
            graphData.SpeechNodes.Add(this);
        }

        public DialogueSpeechScriptableObject SaveToSO(DialogueSpeechScriptableObject dialogueSO)
        {
            dialogueSO.Initialize(
                NodeName,
                NodeType,
                CharacterNameLocalization,
                TextLocalization
            );
            UtilityIO.SaveAsset(dialogueSO);
            return dialogueSO;
        }
    }
}
