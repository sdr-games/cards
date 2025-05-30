using System;

using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEditor;
using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.TalentsEditorModule.Models.TalamusData;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    [Serializable]
    public class TalamusNodeView : BaseNodeView
    {
        [field: SerializeField] public CharacteristicNames CharacteristicName { get; private set; }
        [field: SerializeField] public int CharacteristicValue { get; private set; }

        public new event EventHandler<SavedToSOEventArgs<TalamusScriptableObject>> SavedToSO;
        public event EventHandler<TalamusLoadedEventArgs> Loaded;
        public event EventHandler<CharacteristicNameChangedEventArgs> CharactersticNameChanged;
        public event EventHandler<CharacteristicValueChangedEventArgs> CharactersticValueChanged;

        public void Initialize(string id, string nodeName, Vector2 position)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Modules/TalentsModule/Sprites/Thalamus.png");
            base.Initialize(id, nodeName, position, sprite);
            CharacteristicName = default;
            CharacteristicValue = 0;

            CreateInputPort();
            CreateOutputPort();
        }

        public override void Draw()
        {
            base.Draw();
            titleContainer.AddToClassList("ds-node__talamus-background");

            /* INPUT CONTAINER */

            inputContainer.Add(InputPorts[0]);

            /* OUTPUT CONTAINER */

            foreach (Port port in OutputPorts)
            {
                outputContainer.Add(port);
            }

            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            TextField costTextField = UtilityElement.CreateTextField(
                Cost.ToString(), 
                "Cost", 
                callback => 
                {
                    Cost = int.Parse(callback.newValue);
                    CostChanged(new CostChangedEventArgs(Cost));
                }
            );

            Foldout textFoldout = UtilityElement.CreateFoldout("Description");
            Box localizationBox = UtilityElement.CreateLocalizationBox(DescriptionLocalization, "", DescriptionLocalizationFieldChanged);
            textFoldout.Add(localizationBox);

            DropdownField characteristicDropdown = UtilityElement.CreateDropdownField
            (
                typeof(CharacteristicNames),
                CharacteristicName.ToString(),
                null,
                callback =>
                {
                    CharacteristicName = (CharacteristicNames)Enum.Parse(typeof(CharacteristicNames), callback.newValue);
                    CharactersticNameChanged?.Invoke(this, new CharacteristicNameChangedEventArgs(callback.newValue));
                }
            );

            TextField characteristicValueTextField = UtilityElement.CreateTextField(CharacteristicValue.ToString(), null, callback =>
            {
                TextField target = (TextField)callback.target;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
                CharacteristicValue = int.Parse(target.value);
                CharactersticValueChanged?.Invoke(this, new CharacteristicValueChangedEventArgs(CharacteristicValue));
            });

            //ObjectField characterObjectField = UtilityElement.CreateObjectField(typeof(CharacterInfoScriptableObject), _character, "Character", callback =>
            //{
            //    _character = callback.newValue as CharacterInfoScriptableObject;
            //    CharacterUpdated?.Invoke(this, new CharacterUpdatedEventArgs(_character));
            //});
            //characterFoldout.Add(characterObjectField);

            customDataContainer.Add(costTextField);
            customDataContainer.Add(textFoldout);
            customDataContainer.Add(characteristicDropdown);
            customDataContainer.Add(characteristicValueTextField);
            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            base.SaveToGraph(graphData);
            graphData.AddTalamusNode(this);
        }

        public override TalentScriptableObject SaveToSO(string folderPath, Rect graphRect)
        {
            TalamusScriptableObject talamusSO;

            talamusSO = UtilityIO.CreateAsset<TalamusScriptableObject>($"{folderPath}/Talents", NodeName);
            Vector2 positionPercentages = CalculateLocalPositionPercentages(graphRect);
            talamusSO.SetPositionPercentages(positionPercentages);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<TalamusScriptableObject>(talamusSO));
            return talamusSO;
        }

        public void Load(TalamusNodeView node)
        {
            base.Load(node);
            CharacteristicName = node.CharacteristicName;
            CharacteristicValue = node.CharacteristicValue;
            Loaded?.Invoke(this, new TalamusLoadedEventArgs(ID, NodeName, DescriptionLocalization, Cost, CharacteristicName, CharacteristicValue));
        }

        protected override Port CreateInputPort()
        {
            Port port = this.CreatePort(typeof(BaseNodeView), "", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            port.ClearClassList();
            port.AddToClassList($"ds-node__talamus-input-port");
            InputPorts.Add(port);

            return port;
        }

        protected override Port CreateOutputPort()
        {
            Port port = this.CreatePort(typeof(BaseNodeView), "", capacity: Port.Capacity.Multi);
            port.ClearClassList();
            port.AddToClassList($"ds-node__talamus-output-port");

            OutputPorts.Add(port);

            return port;
        }
    }
}
