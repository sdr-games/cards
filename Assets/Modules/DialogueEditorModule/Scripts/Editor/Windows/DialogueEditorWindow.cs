using System.IO;

using SDRGames.Whist.DialogueEditorModule.Managers;
using SDRGames.Whist.HelpersModule;

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace SDRGames.Whist.DialogueEditorModule
{
    public class DialogueEditorWindow : EditorWindow
    {
        private GraphManager _graphView;

        private readonly string _defaultFileName = "";

        private static TextField _fileNameTextField;
        private Button _saveButton;
        private Button _miniMapButton;

        [MenuItem("Window/Dialogue/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<DialogueEditorWindow>("Dialogue System Graph");
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
            _graphView.CreateStartNode();
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

            toolbar.AddStyleSheets("DialogueSystem/DSToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables.uss");
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_fileNameTextField.value))
            {
                _fileNameTextField.value = "NewDialogueGraph";
            }

            var path = EditorUtility.SaveFilePanel("Save dialogue graph", "", $"{_fileNameTextField.value}.asset", "asset");

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
            string filepath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Modules/DialogueModule/ScriptableObjects/DialogueGraphs", "asset");

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
    }
}