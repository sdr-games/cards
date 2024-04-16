using System;

using SDRGames.Whist.TalentsEditorModule.Models;
using SDRGames.Whist.TalentsEditorModule.Views;

using static SDRGames.Whist.TalentsEditorModule.Models.VariableData;

namespace SDRGames.Whist.TalentsEditorModule.Presenters
{
    public class VariablePresenter
    {
        public VariableData Variable { get; private set; }

        public VariableView VariableView { get; private set; }

        public VariablePresenter(VariableTypes variableType)
        {
            Variable = new VariableData($"New {variableType}", null, variableType);

            VariableView = new VariableView(Variable);
            VariableView.NameFieldValueChanged += OnNameFieldValueChanged;
            VariableView.ValueFieldValueChanged += OnValueFieldValueChanged;
        }

        public VariablePresenter(VariableData variable)
        {
            Variable = variable;

            VariableView = new VariableView(Variable);
            VariableView.NameFieldValueChanged += OnNameFieldValueChanged;
            VariableView.ValueFieldValueChanged += OnValueFieldValueChanged;
        }

        private void OnNameFieldValueChanged(object sender, NameFieldValueChangedEventArgs e)
        {
            Variable.SetName(e.Name);
        }

        private void OnValueFieldValueChanged(object sender, ValueFieldValueChangedEventArgs e)
        {
            Variable.SetValue(e.Value);
        }
    }
}
