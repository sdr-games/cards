using System;
using System.Linq;

using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.TalentsEditorModule.Models.TalamusData;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class TalamusNodeView : BaseNodeView
    {
        private CharacteristicNames _characteristicName;
        private int _characteristicValue;

        public new event EventHandler<SavedToSOEventArgs<TalamusScriptableObject>> SavedToSO;
        public event EventHandler<CharacteristicNameChangedEventArgs> CharactersticNameChanged;
        public event EventHandler<CharacteristicValueChangedEventArgs> CharactersticValueChanged;

        public void Initialize(string id, string nodeName, Vector2 position)
        {
            base.Initialize(id, nodeName, position);

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

            DropdownField characteristicDropdown = UtilityElement.CreateDropdownField
            (
                typeof(CharacteristicNames),
                _characteristicName.ToString(),
                null,
                callback =>
                {
                    _characteristicName = (CharacteristicNames)Enum.Parse(typeof(CharacteristicNames), callback.newValue);
                    CharactersticNameChanged?.Invoke(this, new CharacteristicNameChangedEventArgs(callback.newValue));
                }
            );

            TextField characteristicValueTextField = UtilityElement.CreateTextField(_characteristicValue.ToString(), null, callback =>
            {
                TextField target = (TextField)callback.target;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
                _characteristicValue = int.Parse(target.value);
                CharactersticValueChanged?.Invoke(this, new CharacteristicValueChangedEventArgs(_characteristicValue));
            });

            //ObjectField characterObjectField = UtilityElement.CreateObjectField(typeof(CharacterInfoScriptableObject), _character, "Character", callback =>
            //{
            //    _character = callback.newValue as CharacterInfoScriptableObject;
            //    CharacterUpdated?.Invoke(this, new CharacterUpdatedEventArgs(_character));
            //});
            //characterFoldout.Add(characterObjectField);

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

        public override TalentScriptableObject SaveToSO(string folderPath)
        {
            TalamusScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<TalamusScriptableObject>($"{folderPath}/Talents", NodeName);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<TalamusScriptableObject>(dialogueSO));
            return dialogueSO;
        }

        public override Port CreateInputPort()
        {
            Port port = this.CreatePort(typeof(BaseNodeView), "Talamus", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            port.ClearClassList();
            port.AddToClassList($"ds-node__talamus-input-port");
            InputPorts.Add(port);

            return port;
        }

        public override Port CreateOutputPort()
        {
            Port port = this.CreatePort(typeof(BaseNodeView), "Astra/Talamus", capacity: Port.Capacity.Multi);
            port.ClearClassList();
            port.AddToClassList($"ds-node__talamus-output-port");

            OutputPorts.Add(port);

            return port;
        }
    }
}
