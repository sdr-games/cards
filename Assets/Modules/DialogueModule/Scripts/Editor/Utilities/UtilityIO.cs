using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
        private static GraphView _graphView;

        private static string _graphFileName;
        private static string _containerFolderPath;

        private static List<BaseNode> _nodes;

        private static Dictionary<string, DialogueScriptableObject> _createdDialogues;

        private static Dictionary<string, BaseNode> _loadedNodes;

        public static void Initialize(GraphView dsGraphView, string graphName)
        {
            _graphView = dsGraphView;

            _graphFileName = graphName;
            _containerFolderPath = $"Assets/Modules/DialogueModule/ScriptableObjects/Dialogues/{graphName}";

            _nodes = new List<BaseNode>();

            _createdDialogues = new Dictionary<string, DialogueScriptableObject>();

            _loadedNodes = new Dictionary<string, BaseNode>();
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

            foreach (BaseNode node in _nodes)
            {
                node.SaveToGraph(graphData);
                DialogueScriptableObject dialogue = node.SaveToSO(_containerFolderPath);
                _createdDialogues.Add(node.SaveData.ID, dialogue);

                dialogueContainer.Dialogues.Add(dialogue);
                nodeNames.Add(node.SaveData.Name);
            }

            UpdateDialoguesChoicesConnections();
            UpdateOldNodes(nodeNames, graphData);
        }

        public static List<DialogueAnswerData> ConvertNodeAnswersToDialogueAnswers(List<AnswerSaveData> nodeAnswers)
        {
            List<DialogueAnswerData> dialogueAnswers = new List<DialogueAnswerData>();

            foreach (AnswerSaveData nodeAnswer in nodeAnswers)
            {
                DialogueAnswerData answerData = new DialogueAnswerData()
                {
                    LocalizationData = new DialogueLocalizationData(nodeAnswer.LocalizationSaveData.SelectedLocalizationTable, nodeAnswer.LocalizationSaveData.SelectedEntryKey),
                    Conditions = ConvertNodeAnswersConditionsToDialogueAnswersConditions(nodeAnswer.Conditions),
                };

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

        private static void UpdateDialoguesChoicesConnections()
        {
            foreach (BaseNode node in _nodes)
            {
                DialogueScriptableObject dialogue = _createdDialogues[node.SaveData.ID];

                for (int choiceIndex = 0; choiceIndex < node.SaveData.Answers.Count; ++choiceIndex)
                {
                    AnswerSaveData nodeChoice = node.SaveData.Answers[choiceIndex];
                    if (string.IsNullOrEmpty(nodeChoice.NodeID))
                    {
                        continue;
                    }

                    dialogue.Answers[choiceIndex].NextDialogue = _createdDialogues[nodeChoice.NodeID];
                    SaveAsset(dialogue);
                }
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

            graphData.OldNodeNames = new List<string>(currentUngroupedNodeNames);
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

            LoadNodes(graphData.StartNodes);
            LoadNodes(graphData.SpeechNodes);
            LoadNodesConnections();
        }

        private static void LoadNodes(List<BaseNodeSaveData> nodes)
        {
            foreach (BaseNodeSaveData nodeData in nodes)
            {
                List<AnswerSaveData> answers = CloneNodeAnswers(nodeData.Answers);

                StartNode node = _graphView.CreateNode(nodeData.Name, nodeData.NodeType, nodeData.Position, false) as StartNode;
                node.LoadData(nodeData, answers);
                node.Draw();

                _graphView.AddElement(node);

                _loadedNodes.Add(node.SaveData.ID, node);
            }
        }

        private static void LoadNodes(List<SpeechNodeSaveData> nodes)
        {
            foreach (SpeechNodeSaveData nodeData in nodes)
            {
                List<AnswerSaveData> answers = CloneNodeAnswers(nodeData.Answers);

                SpeechNode node = _graphView.CreateNode(nodeData.Name, nodeData.NodeType, nodeData.Position, false) as SpeechNode;
                node.LoadData(nodeData, answers);
                node.Draw();

                _graphView.AddElement(node);
                _loadedNodes.Add(node.SaveData.ID, node);
            }
        }

        private static void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, BaseNode> loadedNode in _loadedNodes)
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

                            BaseNode nextNode = _loadedNodes[choiceData.NodeID];

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
                if (graphElement is BaseNode node)
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