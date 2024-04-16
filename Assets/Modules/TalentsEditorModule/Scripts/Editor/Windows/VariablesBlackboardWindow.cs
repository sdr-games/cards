using System;
using System.Collections.Generic;

using SDRGames.Whist.TalentsEditorModule.Models;
using SDRGames.Whist.TalentsEditorModule.Presenters;
using SDRGames.Whist.TalentsEditorModule.Views;

using UnityEditor;
using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.TalentsEditorModule.Models.VariableData;

namespace SDRGames.Whist.TalentsEditorModule
{
    public class VariablesBlackboardWindow : Blackboard
    {
        public List<VariableData> Variables { get; private set; }

        public VariablesBlackboardWindow(GraphView graphView) : base(graphView)
        {
            title = "Bonuses";
            subTitle = "";
            addItemRequested += CreateTypesSelectMenu;
            editTextRequested += OnEditTextRequested;
            style.left = 15;
            style.top = 50;

            Variables = new List<VariableData>();
        }

        public void CreateVariable(VariableTypes variableType)
        {
            VariablePresenter presenter = new VariablePresenter(variableType);
            CreateVariable(presenter);
        }

        public void CreateVariable(VariableData variable)
        {
            VariablePresenter presenter = new VariablePresenter(variable);
            CreateVariable(presenter);
        }

        private void CreateVariable(VariablePresenter presenter)
        {
            presenter.VariableView.VariableSelected += OnVariableSelected;
            presenter.VariableView.VariableDeselected += OnVariableDeselected;
            Add(presenter.VariableView);
            Variables.Add(presenter.Variable);
        }

        private void CreateTypesSelectMenu(Blackboard obj)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Half Astra bonuses"), false, () => CreateVariable(VariableTypes.HalfAstraBonus));
            genericMenu.AddItem(new GUIContent("Full Astra bonuses"), false, () => CreateVariable(VariableTypes.FullAstraBonus));
            genericMenu.AddItem(new GUIContent("Full Talamus bonuses"), false, () => CreateVariable(VariableTypes.FullTalamusBonus));
            genericMenu.ShowAsContext();
        }

        private void OnVariableSelected(object sender, VariableSelectedEventArgs e)
        {
            graphView.Add(e.DetailWindow);
        }

        private void OnVariableDeselected(object sender, VariableSelectedEventArgs e)
        {
            graphView.Remove(e.DetailWindow);
        }

        private void OnEditTextRequested(Blackboard blackboard, VisualElement element, string text)
        {
            ClearSelection();
        }
    }
}
