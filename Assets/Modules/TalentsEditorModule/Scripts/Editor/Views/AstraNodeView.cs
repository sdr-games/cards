using System;

using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.TalentsEditorModule.Models.AstraData;
using static SDRGames.Whist.TalentsEditorModule.Models.TalamusData;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    [Serializable]
    public class AstraNodeView : BaseNodeView
    {
        [field: SerializeField] public EquipmentNames Equipment { get; private set; }

        public new event EventHandler<SavedToSOEventArgs<AstraScriptableObject>> SavedToSO;
        public event EventHandler<AstraLoadedEventArgs> Loaded;
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

            TextField costTextField = UtilityElement.CreateTextField(
                Cost.ToString(),
                "Cost",
                callback =>
                {
                    Cost = int.Parse(callback.newValue);
                    CostChanged(new CostChangedEventArgs(Cost));
                }
            );

            DropdownField equipmentDropdown = UtilityElement.CreateDropdownField
            (
                typeof(EquipmentNames),
                Equipment.ToString(),
                null,
                callback =>
                {
                    Equipment = (EquipmentNames)Enum.Parse(typeof(EquipmentNames), callback.newValue);
                    EquipmentChanged?.Invoke(this, new EquipmentChangedEventArgs(callback.newValue));
                }
            );

            //ObjectField characterObjectField = UtilityElement.CreateObjectField(typeof(CharacterInfoScriptableObject), _character, "Character", callback =>
            //{
            //    _character = callback.newValue as CharacterInfoScriptableObject;
            //    CharacterUpdated?.Invoke(this, new CharacterUpdatedEventArgs(_character));
            //});
            //characterFoldout.Add(characterObjectField);

            customDataContainer.Add(costTextField);
            customDataContainer.Add(equipmentDropdown);
            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            base.SaveToGraph(graphData);
            graphData.AddAstraNode(this);
        }

        public override TalentScriptableObject SaveToSO(string folderPath)
        {
            AstraScriptableObject astraSO;

            astraSO = UtilityIO.CreateAsset<AstraScriptableObject>($"{folderPath}/Talents", NodeName);
            astraSO.SetPosition(Position);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<AstraScriptableObject>(astraSO));
            return astraSO;
        }

        public void Load(AstraNodeView node)
        {
            base.Load(node);
            Equipment = node.Equipment;
            Loaded?.Invoke(this, new AstraLoadedEventArgs(Equipment));
        }

        protected override Port CreateInputPort()
        {
            Port port = this.CreatePort(typeof(BaseNodeView), "Talamus", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            port.ClearClassList();
            port.AddToClassList($"ds-node__astra-input-port");
            InputPorts.Add(port);

            return port;
        }
    }
}