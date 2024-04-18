using System;

using SDRGames.Whist.TalentsEditorModule.Models;
using SDRGames.Whist.TalentsEditorModule.Views;
using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEngine;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.TalentScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule.Presenters
{
    public class TalamusNodePresenter : BaseNodePresenter
    {
        private TalamusData _data;
        private TalamusNodeView _nodeView;

        public TalamusNodePresenter()
        {
            _nodeView = (TalamusNodeView)Activator.CreateInstance(typeof(TalamusNodeView));
            _nodeView.NodeNameTextFieldChanged += OnNodeNameTextFieldChanged;
        }

        public override void Initialize(string name, Vector2 position)
        {
            _data = new TalamusData(name);
            _data.SetNodeType(NodeTypes.Talamus);
            
            _nodeView.Initialize(_data.ID, _data.NodeName, position);
            _nodeView.SavedToSO += OnSavedToSO;
            _nodeView.Loaded += OnLoaded;
            _nodeView.CostTextFieldChanged += OnCostTextFieldChanged;
            _nodeView.CharactersticNameChanged += OnCharactersticNameChanged;
            _nodeView.CharactersticValueChanged += OnCharactersticValueChanged;
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

        protected void OnSavedToSO(object sender, SavedToSOEventArgs<TalamusScriptableObject> e)
        {
            _data.SaveToSO(e.TalentSO);
        }

        private void OnLoaded(object sender, TalamusLoadedEventArgs e)
        {
            _data.Load(e);
        }

        private void OnCharactersticNameChanged(object sender, CharacteristicNameChangedEventArgs e)
        {
            _data.SetCharacteristicName(e.CharacteristicName);
        }

        private void OnCharactersticValueChanged(object sender, CharacteristicValueChangedEventArgs e)
        {
            _data.SetCharacteristicValue(e.CharacteristicValue);
        }
    }
}
