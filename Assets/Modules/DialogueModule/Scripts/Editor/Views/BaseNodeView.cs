using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Models;
using SDRGames.Whist.DialogueSystem.Helpers;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class BaseNodeView : Node
    {
        private Color _defaultBackgroundColor;

        public string NodeName { get; protected set; }
        public List<Port> InputPorts { get; protected set; }
        public List<Port> OutputPorts { get; protected set; }

        //public BaseData SaveData { get; protected set; }

        public event EventHandler<NodeNameChangedEventArgs> NodeNameTextFieldChanged;
        public event EventHandler AnswerPortRemoved;
        public event EventHandler PortDisconnected;


        public override void BuildContextualMenu(ContextualMenuPopulateEvent contextualMenuEvent)
        {
            contextualMenuEvent.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            contextualMenuEvent.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(contextualMenuEvent);
        }

        public virtual void Initialize(string nodeName, Vector2 position)
        {
            SetPosition(new Rect(position, Vector2.zero));

            NodeName = nodeName;

            InputPorts = new List<Port>();
            OutputPorts = new List<Port>();

            _defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* TITLE CONTAINER */
            TextField nodeNameTextField = UtilityElement.CreateTextField(NodeName, null, callback =>
            {
                string oldName = NodeName;
                TextField target = (TextField)callback.target;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
                NodeName = target.value;
                NodeNameTextFieldChanged?.Invoke(this, new NodeNameChangedEventArgs(oldName, this));
                if (string.IsNullOrEmpty(target.value))
                {
                    return;
                }
            });

            nodeNameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__filename-text-field"
            );

            titleContainer.Insert(0, nodeNameTextField);
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = _defaultBackgroundColor;
        }

        public virtual void SaveToGraph(GraphSaveDataScriptableObject graphData) { }

        public virtual DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<DialogueScriptableObject>($"{folderPath}/Dialogues", "");

            //dialogueSO.Initialize(
            //    SaveData.Name,
            //    null,
            //    SaveData.NodeType
            //);

            UtilityIO.SaveAsset(dialogueSO);
            return dialogueSO;
        }

        public void LoadData(BaseData nodeData)
        {
            //SaveData = nodeData;
        }

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        protected Port CreateAnswerPort(Type type, bool singlePort = false)
        {
            Port answerPort = this.CreatePort(type, "Output");
            answerPort.ClearClassList();
            answerPort.AddToClassList("ds-node__answer-port");
            //answerPort.userData = userData;

            //AnswerSaveData answerData = (AnswerSaveData)userData;
            //if (answerData.Conditions == null)
            //{
            //    answerData.Conditions = new List<AnswerConditionSaveData>();
            //}

            //Foldout answerFoldout = UtilityElement.CreateFoldout("Answer");

            //Box localizationBox = UtilityElement.CreateLocalizationBox(answerData.LocalizationSaveData, "ds-node__string-dropdown");
            //answerPort.Add(localizationBox);

            if (!singlePort)
            {
                Button deleteAnswerButton = UtilityElement.CreateButton("X", () =>
                {
                    if (OutputPorts.Count == 1)
                    {
                        return;
                    }
                    outputContainer.Remove(answerPort);
                    AnswerPortRemoved?.Invoke(answerPort, EventArgs.Empty);
                });
                deleteAnswerButton.AddToClassList("ds-node__button");
                answerPort.Add(deleteAnswerButton);
            }


            //Foldout conditionsFoldout = UtilityElement.CreateFoldout("Conditions", true);
            //conditionsFoldout.AddClasses("ds-node__foldout-right");

            //Button addConditionButton = UtilityElement.CreateButton("Add condition", () =>
            //{
            //    AnswerConditionSaveData conditionData = new AnswerConditionSaveData()
            //    {
            //        AnswerConditionType = AnswerConditionTypes.CharacteristicCheck
            //    };
            //    answerData.Conditions.Add(conditionData);
            //    UtilityElement.CreateConditionField(conditionsFoldout, conditionData);
            //});
            //conditionsFoldout.Add(addConditionButton);

            //foreach (AnswerConditionSaveData conditionData in answerData.Conditions)
            //{
            //    UtilityElement.CreateConditionField(conditionsFoldout, conditionData);
            //}

            //answerFoldout.Add(answerPort);
            //answerFoldout.Add(conditionsFoldout);

            OutputPorts.Add(answerPort);
            outputContainer.Add(answerPort);

            return answerPort;
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(InputPorts);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(OutputPorts);
        }

        private void DisconnectPorts(List<Port> ports)
        {
            foreach (Port port in ports)
            {
                if (!port.connected)
                {
                    continue;
                }
                PortDisconnected?.Invoke(port, EventArgs.Empty);
            }
        }
    }
}