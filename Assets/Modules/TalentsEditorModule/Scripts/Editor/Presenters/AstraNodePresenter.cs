using System;

using SDRGames.Whist.TalentsEditorModule.Models;
using SDRGames.Whist.TalentsEditorModule.Views;
using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEngine;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.TalentScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule.Presenters
{
    public class AstraNodePresenter : BaseNodePresenter
    {
        private AstraData _data;
        private AstraNodeView _nodeView;

        public AstraNodePresenter() 
        {
            _nodeView = (AstraNodeView)Activator.CreateInstance(typeof(AstraNodeView));
            _nodeView.NodeNameTextFieldChanged += OnNodeNameTextFieldChanged;
        }

        public override void Initialize(string name, Vector2 position)
        {
            _data = new AstraData(name);
            _data.SetNodeType(NodeTypes.Astra);

            _nodeView.Initialize(_data.ID, _data.NodeName, position);
            _nodeView.SavedToSO += OnSavedToSO;
            _nodeView.Loaded += OnLoaded;
            _nodeView.CostTextFieldChanged += OnCostTextFieldChanged;
            _nodeView.EquipmentChanged += OnEquipmentChanged;
        }

        public override BaseNodeView GetNodeView()
        {
            return _nodeView;
        }

        protected override void OnNodeNameTextFieldChanged(object sender, NodeNameChangedEventArgs e)
        {
            _data.SetNodeName(e.NewNode.NodeName);
        }

        protected override void OnCostTextFieldChanged(object sender, CostChangedEventArgs e)
        {
            _data.SetCost(e.Cost);
        }

        protected void OnSavedToSO(object sender, SavedToSOEventArgs<AstraScriptableObject> e)
        {
            _data.SaveToSO(e.TalentSO);
        }

        private void OnLoaded(object sender, AstraLoadedEventArgs e)
        {
            _data.Load(e.EquipmentName);
        }

        private void OnEquipmentChanged(object sender, EquipmentChangedEventArgs e)
        {
            _data.SetEquipment(e.Equipment);
        }
    }
}
