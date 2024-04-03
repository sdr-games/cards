using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using SDRGames.Whist.DialogueEditorModule.Managers;
using SDRGames.Whist.DialogueEditorModule.Presenters;
using SDRGames.Whist.DialogueEditorModule.Views;
using SDRGames.Whist.DialogueModule.ScriptableObjects;

using UnityEditor;
using UnityEditor.Experimental.GraphView;

using UnityEngine;

namespace SDRGames.Whist.DialogueEditorModule
{
    public static class UtilityIO
    {
        private static GraphManager _graphView;

        private static string _graphFileName;
        private static string _containerFolderPath;

        private static List<BaseNodeView> _nodes;

        private static Dictionary<string, BaseNodeView> _loadedNodes;

        public static void Initialize(GraphManager dsGraphView, string graphName)
        {
            _graphView = dsGraphView;

            _graphFileName = graphName;
            _containerFolderPath = $"Assets/Modules/DialogueModule/ScriptableObjects/Dialogues/{graphName}";

            _nodes = new List<BaseNodeView>();

            _loadedNodes = new Dictionary<string, BaseNodeView>();
        }

        public static void Save(string path)
        {
            CreateDefaultFolders();
            ClearFolder($"{_containerFolderPath}/Dialogues");

            GetElementsFromGraphView();
            GraphSaveDataScriptableObject graphData = CreateAsset<GraphSaveDataScriptableObject>(path);
            graphData.Initialize(_graphFileName);

            DialogueContainerScriptableObject dialogueContainer = CreateAsset<DialogueContainerScriptableObject>(_containerFolderPath, _graphFileName);
            dialogueContainer.Initialize(_graphFileName);

            SaveNodes(graphData, dialogueContainer);

            SaveAsset(graphData);
            SaveAsset(dialogueContainer);
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

            if (asset != null)
            {
                RemoveAsset(path, assetName);
            }

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, fullPath);
            return asset;
        }

        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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

            _graphView.ClearGraph(true);
            LoadNode(graphData.StartNode);
            LoadNodes(graphData.AnswerNodes);
            LoadNodes(graphData.SpeechNodes);
            LoadNodesConnections();
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
            }
        }

        private static void LoadNode(StartNodeView node)
        {
            StartNodeView newNode = _graphView.CreateNode<StartNodePresenter>(node.NodeName, node.Position) as StartNodeView;
            newNode.LoadData(node);
            _graphView.AddElement(node);
            _loadedNodes.Add(node.ID, node);
        }

        private static void LoadNodes(List<SpeechNodeView> nodes)
        {
            foreach (SpeechNodeView node in nodes)
            {
                _graphView.AddElement(node);
                _loadedNodes.Add(node.ID, node);
            }
        }

        private static void LoadNodes(List<AnswerNodeView> nodes)
        {
            foreach (AnswerNodeView node in nodes)
            {
                _graphView.AddElement(node);
                _loadedNodes.Add(node.ID, node);
            }
        }

        private static void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, BaseNodeView> loadedNode in _loadedNodes)
            {
                BaseNodeView node = loadedNode.Value;
                foreach(Port port in node.OutputPorts)
                {
                    List<Edge> connections = port.connections.ToList();
                    foreach(Edge edge in connections)
                    {
                        _graphView.AddElement(edge);
                        node.RefreshPorts();
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

        private static void CreateFolder(string parentFolderPath, string newFolderName)
        {
            if (AssetDatabase.IsValidFolder($"{parentFolderPath}/{newFolderName}"))
            {
                return;
            }
            AssetDatabase.CreateFolder(parentFolderPath, newFolderName);
        }

        private static void ClearFolder(string dialoguesFolder)
        {
            if (Directory.Exists(dialoguesFolder))
            {
                foreach (string filename in Directory.GetFiles(dialoguesFolder))
                {
                    RemoveAsset(filename);
                }
            }
        }

        private static T LoadAsset<T>(string path, string assetName = "") where T : ScriptableObject
        {
            string fullPath = $"{path}";
            if (!string.IsNullOrEmpty(assetName))
            {
                fullPath += $"/{assetName}.asset";
            }
            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        private static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        private static void RemoveAsset(string asset)
        {
            AssetDatabase.DeleteAsset($"{asset}");
        }
    }
}