using System;

using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.TalentsEditorModule.Models.AstraData;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class AstraNodeView : BaseNodeView
    {
        private EquipmentName _equipment;

        public new event EventHandler<SavedToSOEventArgs<AstraScriptableObject>> SavedToSO;
        public event EventHandler<EquipmentChangedEventArgs> EquipmentChanged;

        public void Initialize(string id, string nodeName, Vector2 position)
        {
            base.Initialize(id, nodeName, position);

            CreateInputPort();
        }

        public override void Draw()
        {
            base.Draw();
            titleContainer.AddToClassList("ds-node__astra-background");

            /* INPUT CONTAINER */

            inputContainer.Add(InputPorts[0]);

            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            DropdownField equipmentDropdown = UtilityElement.CreateDropdownField
            (
                typeof(EquipmentName),
                _equipment.ToString(),
                null,
                callback =>
                {
                    _equipment = (EquipmentName)Enum.Parse(typeof(EquipmentName), callback.newValue);
                    EquipmentChanged?.Invoke(this, new EquipmentChangedEventArgs(callback.newValue));
                }
            );

            //ObjectField characterObjectField = UtilityElement.CreateObjectField(typeof(CharacterInfoScriptableObject), _character, "Character", callback =>
            //{
            //    _character = callback.newValue as CharacterInfoScriptableObject;
            //    CharacterUpdated?.Invoke(this, new CharacterUpdatedEventArgs(_character));
            //});
            //characterFoldout.Add(characterObjectField);

            customDataContainer.Add(equipmentDropdown);
            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            base.SaveToGraph(graphData);
            graphData.AddAstrahNode(this);
        }

        public override TalentScriptableObject SaveToSO(string folderPath)
        {
            AstraScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<AstraScriptableObject>($"{folderPath}/Talents", NodeName);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<AstraScriptableObject>(dialogueSO));
            return dialogueSO;
        }

        public override Port CreateInputPort()
        {
            Port port = this.CreatePort(typeof(BaseNodeView), "Talamus", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            port.ClearClassList();
            port.AddToClassList($"ds-node__astra-input-port");
            InputPorts.Add(port);

            return port;
        }
    }
}