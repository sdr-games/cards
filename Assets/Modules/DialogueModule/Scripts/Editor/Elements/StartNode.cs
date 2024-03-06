using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueScriptableObject;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class StartNode : BaseNode
    {
        public override void Initialize(string nodeName, Vector2 position)
        {
            base.Initialize(nodeName, position);
            SaveData.SetNodeType(NodeTypes.Start);
        }

        public override void Draw()
        {
            /* MAIN CONTAINER */
            string defaultText = "Answer";
            Button addAnswerButton = UtilityElement.CreateButton("Add answer", () =>
            {
                AnswerSaveData answerData = new AnswerSaveData()
                {
                    LocalizationSaveData = new LocalizationSaveData("", defaultText, defaultText),
                    Conditions = new List<AnswerConditionSaveData>()
                };
                SaveData.AddAnswer(answerData);
                CreateAnswerPort(answerData);
            });

            addAnswerButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1, addAnswerButton);

            /* OUTPUT CONTAINER */

            foreach (AnswerSaveData answer in SaveData.Answers)
            {
                CreateAnswerPort(answer);
            }
            RefreshExpandedState();

            base.Draw();
        }

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            SaveData.SetPosition(GetPosition().position);
            graphData.StartNodes.Add(SaveData);
        }
    }
}