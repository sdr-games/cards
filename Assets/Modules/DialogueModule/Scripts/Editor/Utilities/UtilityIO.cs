using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using SDRGames.Whist.DialogueSystem.Editor.Managers;
using SDRGames.Whist.DialogueSystem.Editor.Models;
using SDRGames.Whist.DialogueSystem.Editor.Presenters;
using SDRGames.Whist.DialogueSystem.Editor.Views;
using SDRGames.Whist.DialogueSystem.Helpers;
using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor;
using UnityEditor.Experimental.GraphView;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public static class UtilityIO
    {
        private static GraphManager _graphView;

        private static string _graphFileName;
        private static string _containerFolderPath;

        private static List<BaseNodeView> _nodes;

        private static Dictionary<string, DialogueScriptableObject> _createdDialogues;

        private static Dictionary<string, BaseNodeView> _loadedNodes;

        public static void Initialize(GraphManager dsGraphView, string graphName)
        {
            _graphView = dsGraphView;

            _graphFileName = graphName;
            _containerFolderPath = $"Assets/Modules/DialogueModule/ScriptableObjects/Dialogues/{graphName}";

            _nodes = new List<BaseNodeView>();

            _createdDialogues = new Dictionary<string, DialogueScriptableObject>();

            _loadedNodes = new Dictionary<string, BaseNodeView>();
        }

        public static void Save(string path)
        {
            CreateDefaultFolders();

            GetElementsFromGraphView();
            GraphSaveDataScriptableObject graphData = CreateAsset<GraphSaveDataScriptableObject>(path);
            graphData.Initialize(_graphFileName);

            DialogueContainerScriptableObject dialogueContainer = CreateAsset<DialogueContainerScriptableObject>(_containerFolderPath, _graphFileName);
            dialogueContainer.Initialize(_graphFileName);

            SaveNodes(graphData, dialogueContainer);

            SaveAsset(graphData);
            SaveAsset(dialogueContainer);
        }

        private static void SaveNodes(GraphSaveDataScriptableObject graphData, DialogueContainerScriptableObject dialogueContainer)
        {
            List<string> nodeNames = new List<string>();
            Dictionary<string, DialogueScriptableObject> createdDialogues = new Dictionary<string, DialogueScriptableObject>();

            foreach (BaseNodeView node in _nodes)
            {
                node.SaveToGraph(graphData);
                DialogueScriptableObject dialogue = node.SaveToSO(_containerFolderPath);
                createdDialogues.Add(node.ID, dialogue);

                dialogueContainer.Dialogues.Add(dialogue);
                nodeNames.Add(node.NodeName);
            }

            UpdateDialoguesChoicesConnections(createdDialogues);
            UpdateOldNodes(nodeNames, graphData);
        }

        public static List<DialogueAnswerData> ConvertNodeAnswersToDialogueAnswers(List<AnswerData> nodeAnswers)
        {
            List<DialogueAnswerData> dialogueAnswers = new List<DialogueAnswerData>();

            foreach (AnswerData nodeAnswer in nodeAnswers)
            {
                DialogueAnswerData answerData = new DialogueAnswerData(
                    new DialogueLocalizationData(nodeAnswer.CharacterNameLocalization.SelectedLocalizationTable, nodeAnswer.CharacterNameLocalization.SelectedEntryKey),
                    new DialogueLocalizationData(nodeAnswer.TextLocalization.SelectedLocalizationTable, nodeAnswer.TextLocalization.SelectedEntryKey),
                    ConvertNodeAnswersConditionsToDialogueAnswersConditions(nodeAnswer.Conditions)
                );

                dialogueAnswers.Add(answerData);
            }

            return dialogueAnswers;
        }

        private static List<DialogueAnswerCondition> ConvertNodeAnswersConditionsToDialogueAnswersConditions(List<AnswerConditionSaveData> nodeAnswerConditions)
        {
            List<DialogueAnswerCondition> dialogueAnswerConditions = new List<DialogueAnswerCondition>();

            foreach (AnswerConditionSaveData nodeAnswerCondition in nodeAnswerConditions)
            {
                DialogueAnswerCondition conditionData = new DialogueAnswerCondition()
                {
                    Characteristic = nodeAnswerCondition.Characteristic,
                    Skill = nodeAnswerCondition.Skill,
                    RequiredValue = nodeAnswerCondition.RequiredValue,
                    AnswerConditionType = nodeAnswerCondition.AnswerConditionType,
                    Reversed = nodeAnswerCondition.Reversed,
                };

                dialogueAnswerConditions.Add(conditionData);
            }

            return dialogueAnswerConditions;
        }

        private static void UpdateDialoguesChoicesConnections(Dictionary<string, DialogueScriptableObject> createdDialogues)
        {
            foreach (BaseNodeView node in _nodes)
            {
                foreach (Port port in node.OutputPorts)
                {
                    foreach (Edge edge in port.connections)
                    {
                        string inputNodeID = ((BaseNodeView)edge.input.node).ID;
                        if (createdDialogues[node.ID] is DialogueStartScriptableObject dialogueStart)
                        {
                            dialogueStart.SetNextSpech(createdDialogues[inputNodeID] as DialogueSpeechScriptableObject);
                        } 
                        else if (createdDialogues[node.ID] is DialogueSpeechScriptableObject dialogueSpeech)
                        {
                            dialogueSpeech.Answers.Add(createdDialogues[inputNodeID] as DialogueAnswerScriptableObject);
                        }
                        else if (createdDialogues[node.ID] is DialogueAnswerScriptableObject dialogueAnswer)
                        {
                            dialogueAnswer.SetNextSpech(createdDialogues[inputNodeID] as DialogueSpeechScriptableObject);
                        }
                    }
                }

                //for (int choiceIndex = 0; choiceIndex < node.OutputPorts.Count; ++choiceIndex)
                //{
                //    //AnswerSaveData nodeChoice = node.SaveData.OutputPorts[choiceIndex];
                //    //if (string.IsNullOrEmpty(nodeChoice.NodeID))
                //    //{
                //    //    continue;
                //    //}

                //    //dialogue.Answers[choiceIndex].NextDialogue = _createdDialogues[nodeChoice.NodeID];
                //}
                //SaveAsset(dialogue);
            }
        }

        private static void UpdateOldNodes(List<string> currentUngroupedNodeNames, GraphSaveDataScriptableObject graphData)
        {
            if (graphData.OldNodeNames != null && graphData.OldNodeNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldNodeNames.Except(currentUngroupedNodeNames).ToList();
                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{_containerFolderPath}/Dialogues", nodeToRemove);
                }
            }

            graphData.SetOldNodeNames(new List<string>(currentUngroupedNodeNames));
        }

        public static void Load(string filepath)
        {
            filepath = Path.GetDirectoryName(filepath).Replace(Environment.CurrentDirectory + "\\", "");
            GraphSaveDataScriptableObject graphData = LoadAsset<GraphSaveDataScriptableObject>(filepath, _graphFileName);

            if (graphData == null)
            {
                EditorUtility.DisplayDialog(
                    "Could not find the file!",
                    "The file at the following path could not be found:\n\n" +
                    $"\"{filepath}\\{_graphFileName}\".\n\n" +
                    "Make sure you choose the right file and it's placed at the folder path mentioned above.",
                    "Thanks!"
                );
                return;
            }

            DialogueEditorWindow.UpdateFileName(graphData.FileName);

            LoadNode(graphData.StartNode);
            LoadNodes(graphData.AnswerNodes);
            LoadNodes(graphData.SpeechNodes);
            LoadNodesConnections();
        }

        private static void LoadNode(StartNodeView node)
        {

            //List<AnswerSaveData> answers = CloneNodeAnswers(nodeData.Answers);

            //StartNodeView node = _graphView.CreateNode<StartNodePresenter>(nodeData.NodeName, nodeData.Position, false) as StartNodeView;
            _graphView.AddElement(node);
            //node.LoadData(nodeData);
            node.Draw();

            //_graphView.AddElement(node);

            _loadedNodes.Add(node.ID, node);
        }

        private static void LoadNodes(List<SpeechNodeView> nodes)
        {
            foreach (SpeechNodeView node in nodes)
            {
                //List<AnswerSaveData> answers = CloneNodeAnswers(nodeData.Answers);

                //SpeechNodeView node = _graphView.CreateNode<SpeechNodePresenter>(nodeData.NodeName, nodeData.Position, false) as SpeechNodeView;
                //node.LoadData(nodeData);

                _graphView.AddElement(node);
                node.Draw();

                //_loadedNodes.Add(node.SaveData.ID, node);
                _loadedNodes.Add(node.ID, node);
            }
        }

        private static void LoadNodes(List<AnswerNodeView> nodes)
        {
            foreach (AnswerNodeView node in nodes)
            {
                //List<AnswerSaveData> answers = CloneNodeAnswers(nodeData.Answers);

                //AnswerNodeView node = _graphView.CreateNode<AnswerNodePresenter>(nodeData.NodeName, nodeData.Position, false) as AnswerNodeView;
                //node.LoadData(nodeData);

                _graphView.AddElement(node);
                node.Draw();

                //_loadedNodes.Add(node.SaveData.ID, node);
                _loadedNodes.Add(node.ID, node);
            }
        }

        private static void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, BaseNodeView> loadedNode in _loadedNodes)
            {
                var foldouts = loadedNode.Value.outputContainer.Children();
                foreach (var foldout in foldouts)
                {
                    foreach (var choicePort in foldout.Children())
                    {
                        if (choicePort.GetType() == typeof(Port))
                        {
                            AnswerSaveData choiceData = (AnswerSaveData)choicePort.userData;

                            if (string.IsNullOrEmpty(choiceData.NodeID))
                            {
                                continue;
                            }

                            BaseNodeView nextNode = _loadedNodes[choiceData.NodeID];

                            Port nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();

                            Edge edge = ((Port)choicePort).ConnectTo(nextNodeInputPort);

                            _graphView.AddElement(edge);

                            loadedNode.Value.RefreshPorts();
                        }
                    }
                }
            }
        }

        private static void CreateDefaultFolders()
        {
            CreateFolder("Assets/Modules/DialogueModule/ScriptableObjects", "DialogueGraphs");
            CreateFolder("Assets/Modules/DialogueModule/ScriptableObjects", "Dialogues");

            CreateFolder("Assets/Modules/DialogueModule/ScriptableObjects/Dialogues", _graphFileName);
            CreateFolder(_containerFolderPath, "Dialogues");
        }

        private static void GetElementsFromGraphView()
        {
            _graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is BaseNodeView node)
                {
                    _nodes.Add(node);
                    return;
                }
            });
        }

        public static void CreateFolder(string parentFolderPath, string newFolderName)
        {
            if (AssetDatabase.IsValidFolder($"{parentFolderPath}/{newFolderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(parentFolderPath, newFolderName);
        }

        public static void RemoveFolder(string path)
        {
            FileUtil.DeleteFileOrDirectory($"{path}.meta");
            FileUtil.DeleteFileOrDirectory($"{path}/");
        }

        public static T CreateAsset<T>(string path, string assetName = "") where T : ScriptableObject
        {
            string fullPath = $"{path}";
            if (!string.IsNullOrEmpty(assetName))
            {
                fullPath += $"/{assetName}.asset";
            }
            fullPath = Regex.Replace(fullPath, @"^.*?Assets", "Assets");

            T asset = LoadAsset<T>(path, assetName);

            if(asset != null)
            {
                RemoveAsset(path, assetName);
            }

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, fullPath);
            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName = "") where T : ScriptableObject
        {
            string fullPath = $"{path}";
            if (!string.IsNullOrEmpty(assetName))
            {
                fullPath += $"/{assetName}.asset";
            }
            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        private static List<AnswerSaveData> CloneNodeAnswers(List<AnswerSaveData> nodeAnswers)
        {
            List<AnswerSaveData> answers = new List<AnswerSaveData>();

            foreach (AnswerSaveData nodeAnswer in nodeAnswers)
            {
                AnswerSaveData answerData = new AnswerSaveData()
                {
                    LocalizationSaveData = nodeAnswer.LocalizationSaveData,
                    NodeID = nodeAnswer.NodeID,
                    Conditions = nodeAnswer.Conditions,
                };

                answers.Add(answerData);
            }

            return answers;
        }
    }
}