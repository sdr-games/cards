using System.IO;

using SDRGames.Whist.DialogueSystem.Helpers;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine.UIElements;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class DialogueEditorWindow : EditorWindow
    {
        private GraphView _graphView;

        private readonly string _defaultFileName = "";

        private static TextField _fileNameTextField;
        private Button _saveButton;
        private Button _miniMapButton;

        [MenuItem("Window/Dialogue/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<DialogueEditorWindow>("Dialogue System Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();

            AddStyles();
        }

        private void AddGraphView()
        {
            _graphView = new GraphView(this);
            _graphView.StretchToParentSize();

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
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.", "Roger!");
                return;
            }

            UtilityIO.Initialize(_graphView, _fileNameTextField.value);
            UtilityIO.Save();
        }

        private void Load()
        {
            string filepath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Resources/ScriptableObjectsAssets", "asset");

            if (string.IsNullOrEmpty(filepath))
            {
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

        public static void UpdateFileName(string newFileName)
        {
            _fileNameTextField.value = newFileName;
        }

        public void EnableSaving()
        {
            _saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            _saveButton.SetEnabled(false);
        }
    }
}