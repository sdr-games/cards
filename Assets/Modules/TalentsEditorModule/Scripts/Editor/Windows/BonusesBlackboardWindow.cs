using System;
using System.Collections.Generic;

using SDRGames.Whist.TalentsEditorModule.Models;
using SDRGames.Whist.TalentsEditorModule.Presenters;
using SDRGames.Whist.TalentsEditorModule.Views;

using UnityEditor;
using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.BonusScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule
{
    public class BonusesBlackboardWindow : Blackboard
    {
        public List<BonusData> Bonuses { get; private set; }

        public BonusesBlackboardWindow(GraphView graphView) : base(graphView)
        {
            title = "Bonuses";
            subTitle = "";
            addItemRequested += CreateTypesSelectMenu;
            editTextRequested += OnEditTextRequested;
            style.left = 15;
            style.top = 50;

            Bonuses = new List<BonusData>();
        }

        public void CreateBonus(BonusTypes bonusType)
        {
            BonusPresenter presenter = new BonusPresenter(bonusType);
            CreateBonus(presenter);
        }

        public void CreateBonus(BonusData variable)
        {
            BonusPresenter presenter = new BonusPresenter(variable);
            CreateBonus(presenter);
        }

        public void Clear()
        {
            base.Clear();
            Bonuses.Clear();
        }

        private void CreateBonus(BonusPresenter presenter)
        {
            presenter.BonusView.BonusSelected += OnVariableSelected;
            presenter.BonusView.BonusDeselected += OnVariableDeselected;
            Add(presenter.BonusView);
            Bonuses.Add(presenter.Bonus);
        }

        private void CreateTypesSelectMenu(Blackboard obj)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Half Astra bonuses"), false, () => CreateBonus(BonusTypes.HalfAstraBonus));
            genericMenu.AddItem(new GUIContent("Full Astra bonuses"), false, () => CreateBonus(BonusTypes.FullAstraBonus));
            genericMenu.AddItem(new GUIContent("Full Talamus bonuses"), false, () => CreateBonus(BonusTypes.FullTalamusBonus));
            genericMenu.ShowAsContext();
        }

        private void OnVariableSelected(object sender, BonusSelectedEventArgs e)
        {
            graphView.Add(e.DetailWindow);
        }

        private void OnVariableDeselected(object sender, BonusSelectedEventArgs e)
        {
            graphView.Remove(e.DetailWindow);
        }

        private void OnEditTextRequested(Blackboard blackboard, VisualElement element, string text)
        {
            ClearSelection();
        }
    }
}
