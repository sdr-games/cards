using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueScriptableObject;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class StartNode : BaseNode
    {
        public override void Initialize(string nodeName, GraphView dsGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dsGraphView, position);

            DialogueType = DialogueTypes.Start;
            Answers = new List<AnswerSaveData>();

            AnswerSaveData answerData = new AnswerSaveData()
            {
                LocalizationSaveData = new LocalizationSaveData("", "Answer", "Answer"),
                Conditions = new List<AnswerConditionSaveData>()
            };
            Answers.Add(answerData);
        }

        public override void Draw()
        {
            /* MAIN CONTAINER */

            Button addAnswerButton = UtilityElement.CreateButton("Add answer", () =>
            {
                AnswerSaveData answerData = new AnswerSaveData()
                {
                    LocalizationSaveData = new LocalizationSaveData("", "Answer", "Answer"),
                    Conditions = new List<AnswerConditionSaveData>()
                };
                Answers.Add(answerData);
                CreateAnswerPort(answerData, Answers);
            });

            addAnswerButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1, addAnswerButton);

            /* OUTPUT CONTAINER */

            foreach (AnswerSaveData answer in Answers)
            {
                CreateAnswerPort(answer, Answers);
            }
            RefreshExpandedState();

            base.Draw();
        }
    }
}