using System.IO;

using SDRGames.Whist.TalentsEditorModule.Managers;
using SDRGames.Whist.HelpersModule;

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace SDRGames.Whist.TalentsEditorModule
{
    public class TalentsEditorWindow : EditorWindow
    {
        public static readonly string MODULE_ROOT = "Assets/Modules/TalentsEditorModule";

        private GraphManager _graphView;

        private readonly string _defaultFileName = "";

        private static TextField _fileNameTextField;
        private Button _saveButton;
        private Button _miniMapButton;

        [MenuItem("Window/Talents/Talents Graph")]
        public static void Open()
        {
            GetWindow<TalentsEditorWindow>("Talents Graph");
        }

        public static void UpdateFileName(string newFileName)
        {
            _fileNameTextField.value = newFileName;
        }

        public void EnableSaving()
        {
            _saveButton?.SetEnabled(true);
        }

        public void DisableSaving()
        {
            _saveButton.SetEnabled(false);
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();

            AddStyles();
        }

        private void AddGraphView()
        {
            _graphView = new GraphManager(this);

            rootVisualElement.Add(_graphView);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            _fileNameTextField = UtilityElement.CreateTextField(_defaultFileName, "File Name:", callback =>
            {
                _fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });

            _saveButton = UtilityElement.CreateButton("Save", () => Save());

            Button loadButton = UtilityElement.CreateButton("Load", () => Load());
            Button clearButton = UtilityElement.CreateButton("Clear", () => Clear());
            Button resetButton = UtilityElement.CreateButton("Reset", () => ResetGraph());

            _miniMapButton = UtilityElement.CreateButton("Minimap", () => ToggleMiniMap());

            toolbar.Add(_fileNameTextField);
            toolbar.Add(_saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);
            toolbar.Add(_miniMapButton);

            toolbar.AddStyleSheets("TalentsEditorStyles/DSToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("TalentsEditorStyles/DSVariables.uss");
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_fileNameTextField.value))
            {
                _fileNameTextField.value = "NewTalentsGraph";
            }
            CreateDefaultFolders(_fileNameTextField.value);

            var path = EditorUtility.SaveFilePanel("Save talents graph", $"{MODULE_ROOT}/ScriptableObjects/TalentsGraphs", $"{_fileNameTextField.value}.asset", "asset");

            if (string.IsNullOrEmpty(path))
            {
                EditorUtility.DisplayDialog("Empty path", "You must select a path first", "OK");
                return;
            }
            path = $"Assets\\{Path.GetRelativePath("Assets", path)}";
            _fileNameTextField.value = Path.GetFileNameWithoutExtension(path);
            UtilityIO.Initialize(_graphView, _fileNameTextField.value);
            UtilityIO.Save(path);
        }

        private void Load()
        {
            string filepath = EditorUtility.OpenFilePanel("Talents Graphs", $"{MODULE_ROOT}/ScriptableObjects/TalentsGraphs", "asset");

            if (string.IsNullOrEmpty(filepath))
            {
                EditorUtility.DisplayDialog("Empty path", "You must select a path first", "OK");
                return;
            }

            Clear();

            UtilityIO.Initialize(_graphView, Path.GetFileNameWithoutExtension(filepath));
            UtilityIO.Load(filepath);
        }

        private void Clear()
        {
            _graphView.ClearGraph();
        }

        private void ResetGraph()
        {
            Clear();
            UpdateFileName(_defaultFileName);
        }

        private void ToggleMiniMap()
        {
            _graphView.ToggleMiniMap();
            _miniMapButton.ToggleInClassList("ds-toolbar__button__selected");
        }

        private void CreateDefaultFolders(string graphFileName)
        {
            string containerFolderPath = $"{MODULE_ROOT}/ScriptableObjects/Branches/{graphFileName}";
            CreateFolder($"{MODULE_ROOT}", "ScriptableObjects");
            CreateFolder($"{MODULE_ROOT}/ScriptableObjects", "TalentsGraphs");
            CreateFolder($"{MODULE_ROOT}/ScriptableObjects", "Branches");

            CreateFolder($"{MODULE_ROOT}/ScriptableObjects/Branches", graphFileName);
            CreateFolder(containerFolderPath, "Talents");
        }

        private void CreateFolder(string parentFolderPath, string newFolderName)
        {
            if (AssetDatabase.IsValidFolder($"{parentFolderPath}/{newFolderName}"))
            {
                return;
            }
            AssetDatabase.CreateFolder(parentFolderPath, newFolderName);
        }
    }
}