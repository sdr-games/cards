using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        private static List<SpeechNode> _answerNodes;
        private static List<Group> _groups;

        private static Dictionary<string, DialogueGroupScriptableObject> _createdDialogueGroups;
        private static Dictionary<string, DialogueScriptableObject> _createdDialogues;

        private static Dictionary<string, Group> _loadedGroups;
        private static Dictionary<string, BaseNode> _loadedNodes;

        public static void Initialize(GraphView dsGraphView, string graphName)
        {
            _graphView = dsGraphView;

            _graphFileName = graphName;
            _containerFolderPath = $"Assets/Modules/DialogueModule/ScriptableObjects/Dialogues/{graphName}";

            _nodes = new List<BaseNode>();
            _groups = new List<Group>();

            _createdDialogueGroups = new Dictionary<string, DialogueGroupScriptableObject>();
            _createdDialogues = new Dictionary<string, DialogueScriptableObject>();

            _loadedGroups = new Dictionary<string, Group>();
            _loadedNodes = new Dictionary<string, BaseNode>();
        }

        public static void Save()
        {
            CreateDefaultFolders();

            GetElementsFromGraphView();
            GraphSaveDataScriptableObject graphData = CreateAsset<GraphSaveDataScriptableObject>("Assets/Modules/DialogueModule/ScriptableObjects/DialogueGraphs", $"{_graphFileName}Graph");
            graphData.Initialize(_graphFileName);

            DialogueContainerScriptableObject dialogueContainer = CreateAsset<DialogueContainerScriptableObject>(_containerFolderPath, _graphFileName);
            dialogueContainer.Initialize(_graphFileName);

            SaveGroups(graphData, dialogueContainer);
            SaveNodes(graphData, dialogueContainer);

            SaveAsset(graphData);
            SaveAsset(dialogueContainer);
        }

        private static void SaveGroups(GraphSaveDataScriptableObject graphData, DialogueContainerScriptableObject dialogueContainer)
        {
            List<string> groupNames = new List<string>();

            foreach (Group group in _groups)
            {
                group.SaveToGraph(graphData);
                var dialogueGroup = group.SaveToSO(_containerFolderPath);
                dialogueContainer.DialogueGroups.Add(dialogueGroup, dialogueGroup.GroupedDialogues);
                _createdDialogueGroups.Add(group.ID, dialogueGroup);

                groupNames.Add(group.title);
            }

            UpdateOldGroups(groupNames, graphData);
        }

        private static void UpdateOldGroups(List<string> currentGroupNames, GraphSaveDataScriptableObject graphData)
        {
            if (graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();
                foreach (string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{_containerFolderPath}/Groups/{groupToRemove}");
                }
            }

            graphData.OldGroupNames = new List<string>(currentGroupNames);
        }

        private static void SaveNodes(GraphSaveDataScriptableObject graphData, DialogueContainerScriptableObject dialogueContainer)
        {
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach (BaseNode node in _nodes)
            {
                node.SaveToGraph(graphData);
                DialogueScriptableObject dialogue = node.SaveToSO(_containerFolderPath);
                _createdDialogues.Add(node.ID, dialogue);

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.DialogueName);
                    dialogueContainer.DialogueGroups.AddItem(_createdDialogueGroups[node.Group.ID], dialogue);
                    continue;
                }
                dialogueContainer.UngroupedDialogues.Add(dialogue);
                ungroupedNodeNames.Add(node.DialogueName);
            }

            UpdateDialoguesChoicesConnections();

            UpdateOldGroupedNodes(groupedNodeNames, graphData);
            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
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
                    Quest = nodeAnswerCondition.Quest,
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
                DialogueScriptableObject dialogue = _createdDialogues[node.ID];

                for (int choiceIndex = 0; choiceIndex < node.Answers.Count; ++choiceIndex)
                {
                    AnswerSaveData nodeChoice = node.Answers[choiceIndex];
                    if (string.IsNullOrEmpty(nodeChoice.NodeID))
                    {
                        continue;
                    }

                    dialogue.Answers[choiceIndex].NextDialogue = _createdDialogues[nodeChoice.NodeID];
                    SaveAsset(dialogue);
                }
            }
        }

        private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, GraphSaveDataScriptableObject graphData)
        {
            if (graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count != 0)
            {
                foreach (KeyValuePair<string, List<string>> oldGroupedNode in graphData.OldGroupedNodeNames)
                {
                    List<string> nodesToRemove = new List<string>();

                    if (currentGroupedNodeNames.ContainsKey(oldGroupedNode.Key))
                    {
                        nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeNames[oldGroupedNode.Key]).ToList();
                    }

                    foreach (string nodeToRemove in nodesToRemove)
                    {
                        RemoveAsset($"{_containerFolderPath}/Groups/{oldGroupedNode.Key}/Dialogues", nodeToRemove);
                    }
                }
            }

            graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
        }

        private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, GraphSaveDataScriptableObject graphData)
        {
            if (graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();
                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{_containerFolderPath}/Global/Dialogues", nodeToRemove);
                }
            }

            graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
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

            LoadGroups(graphData.Groups);
            LoadNodes(graphData.StartNodes);
            LoadNodes(graphData.SpeechNodes);
            LoadNodesConnections();
        }

        private static void LoadGroups(List<GroupSaveData> groups)
        {
            foreach (GroupSaveData groupData in groups)
            {
                Group group = _graphView.CreateGroup(groupData.Name, groupData.Position);
                group.ID = groupData.ID;
                _loadedGroups.Add(group.ID, group);
            }
        }

        private static void LoadNodes(List<BaseNodeSaveData> nodes)
        {
            foreach (BaseNodeSaveData nodeData in nodes)
            {
                List<AnswerSaveData> answers = CloneNodeAnswers(nodeData.Answers);

                StartNode node = _graphView.CreateNode(nodeData.Name, nodeData.DialogueType, nodeData.Position, false) as StartNode;

                node.ID = nodeData.ID;
                node.Answers = answers;
                node.DialogueType = nodeData.DialogueType;

                node.Draw();

                _graphView.AddElement(node);

                _loadedNodes.Add(node.ID, node);

                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                Group group = _loadedGroups[nodeData.GroupID];

                node.Group = group;

                group.AddElement(node);
            }
        }

        private static void LoadNodes(List<SpeechNodeSaveData> nodes)
        {
            foreach (SpeechNodeSaveData nodeData in nodes)
            {
                List<AnswerSaveData> answers = CloneNodeAnswers(nodeData.Answers);

                SpeechNode node = _graphView.CreateNode(nodeData.Name, nodeData.DialogueType, nodeData.Position, false) as SpeechNode;

                node.ID = nodeData.ID;
                node.DialogueType = nodeData.DialogueType;
                node.Answers = answers;
                node.LocalizationSaveData = nodeData.LocalizationSaveData;
                node.Quest = nodeData.Quest;
                node.DialogueQuestAction = nodeData.DialogueQuestAction;
                node.DialogueAction = nodeData.DialogueAction;

                node.Draw();

                _graphView.AddElement(node);
                _loadedNodes.Add(node.ID, node);

                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                Group group = _loadedGroups[nodeData.GroupID];
                node.Group = group;
                group.AddElement(node);
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
            CreateFolder(_containerFolderPath, "Global");
            CreateFolder(_containerFolderPath, "Groups");
            CreateFolder($"{_containerFolderPath}/Global", "Dialogues");
        }

        private static void GetElementsFromGraphView()
        {
            Type groupType = typeof(Group);

            _graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is BaseNode node)
                {
                    _nodes.Add(node);
                    return;
                }

                if (graphElement is SpeechNode answerNode)
                {
                    _answerNodes.Add(answerNode);
                    return;
                }

                if (graphElement.GetType() == groupType)
                {
                    Group group = (Group)graphElement;
                    _groups.Add(group);
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

        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            var a = AssetDatabase.LoadAssetAtPath<T>(fullPath);

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