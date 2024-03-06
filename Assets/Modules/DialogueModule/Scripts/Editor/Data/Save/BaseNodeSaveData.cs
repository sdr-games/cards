using System;
using System.Collections.Generic;

using UnityEngine;

using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueScriptableObject;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    [Serializable]
    public class BaseNodeSaveData
    {
        [field: SerializeField] public string ID { get; protected set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public List<AnswerSaveData> Answers { get; protected set; }
        [field: SerializeField] public NodeTypes NodeType { get; protected set; }
        [field: SerializeField] public Vector2 Position { get; protected set; }

        public BaseNodeSaveData(string name, List<AnswerSaveData> answers, Vector2 position)
        {
            Name = name;
            Answers = answers;
            Position = position;
        }

        public void GenerateID()
        {
            ID = Guid.NewGuid().ToString();
        }

        public virtual void SetName(string name)
        {
            Name = name;
        }

        public void SetNodeType(NodeTypes nodeType)
        {
            NodeType = nodeType;
        }

        public void SetAnswers(List<AnswerSaveData> answers)
        {
            Answers = answers;
        }

        public void AddAnswer(AnswerSaveData answer)
        {
            if(!Answers.Contains(answer))
            {
                Answers.Add(answer);
            }
        }

        public void RemoveAnswer(AnswerSaveData answer)
        {
            if (Answers.Contains(answer))
            {
                Answers.Remove(answer);
            }
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        } 
    }
}