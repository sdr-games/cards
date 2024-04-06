using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using SDRGames.Whist.TalentsEditorModule.Managers;
using SDRGames.Whist.TalentsEditorModule.Presenters;
using SDRGames.Whist.TalentsEditorModule.Views;
using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEditor;
using UnityEditor.Experimental.GraphView;

using UnityEngine;

namespace SDRGames.Whist.TalentsEditorModule
{
    public static class UtilityIO
    {
        private static GraphManager _graphView;

        private static string _graphFileName;
        private static string _containerFolderPath;

        private static List<BaseNodeView> _nodes;

        private static Dictionary<string, BaseNodeView> _loadedNodes;

        public static void Initialize(GraphManager graphView, string graphName)
        {
            _graphView = graphView;

            _graphFileName = graphName;
            _containerFolderPath = $"{TalentsEditorWindow.MODULE_ROOT}/ScriptableObjects/Branches/{graphName}";

            _nodes = new List<BaseNodeView>();

            _loadedNodes = new Dictionary<string, BaseNodeView>();
        }

        public static void Save(string path)
        {
            ClearFolder($"{_containerFolderPath}/Talents");

            GetElementsFromGraphView();
            GraphSaveDataScriptableObject graphData = CreateAsset<GraphSaveDataScriptableObject>(path);
            graphData.Initialize(_graphFileName);

            TalentsBranchScriptableObject talentsBranch = CreateAsset<TalentsBranchScriptableObject>(_containerFolderPath, _graphFileName);
            talentsBranch.Initialize(_graphFileName);

            SaveNodes(graphData, talentsBranch);

            SaveAsset(graphData);
            SaveAsset(talentsBranch);
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
            TalentsEditorWindow.UpdateFileName(graphData.FileName);

            _graphView.ClearGraph();
            LoadNodes(graphData.TalamusNodes);
            LoadNodes(graphData.AstraNodes);
            LoadNodesConnections();
        }

        private static void SaveNodes(GraphSaveDataScriptableObject graphData, TalentsBranchScriptableObject talentsBranch)
        {
            List<string> nodeNames = new List<string>();
            Dictionary<string, TalentScriptableObject> createdTalents = new Dictionary<string, TalentScriptableObject>();

            foreach (BaseNodeView node in _nodes)
            {
                node.SaveToGraph(graphData);
                TalentScriptableObject talentSO = node.SaveToSO(_containerFolderPath);
                createdTalents.Add(node.ID, talentSO);

                talentsBranch.Talents.Add(talentSO);
                nodeNames.Add(node.NodeName);
            }

            UpdateDialoguesChoicesConnections(createdTalents);
        }

        private static void UpdateDialoguesChoicesConnections(Dictionary<string, TalentScriptableObject> createdTalents)
        {
            foreach (BaseNodeView node in _nodes)
            {
                foreach (Port port in node.OutputPorts)
                {
                    foreach (Edge edge in port.connections)
                    {
                        string inputNodeID = ((BaseNodeView)edge.input.node).ID;
                        if (createdTalents[node.ID] is TalamusScriptableObject talamusSO)
                        {
                            talamusSO.Connections.Add(createdTalents[inputNodeID]);
                        }
                    }
                }
            }
        }

        private static void LoadNodes(List<AstraNodeView> nodes)
        {
            foreach (AstraNodeView node in nodes)
            {
                _graphView.AddElement(node);
                _loadedNodes.Add(node.ID, node);
            }
        }

        private static void LoadNodes(List<TalamusNodeView> nodes)
        {
            foreach (TalamusNodeView node in nodes)
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