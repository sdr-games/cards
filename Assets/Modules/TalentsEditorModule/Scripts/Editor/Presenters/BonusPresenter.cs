using SDRGames.Whist.TalentsEditorModule.Models;
using SDRGames.Whist.TalentsEditorModule.Views;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.BonusScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule.Presenters
{
    public class BonusPresenter
    {
        public BonusData Bonus { get; private set; }

        public BonusView BonusView { get; private set; }

        public BonusPresenter(BonusTypes variableType)
        {
            Bonus = new BonusData($"New {variableType}", null, variableType);

            BonusView = new BonusView(Bonus);
            BonusView.NameFieldValueChanged += OnNameFieldValueChanged;
            BonusView.ValueFieldValueChanged += OnValueFieldValueChanged;
        }

        public BonusPresenter(BonusData bonus)
        {
            Bonus = bonus;

            BonusView = new BonusView(Bonus);
            BonusView.NameFieldValueChanged += OnNameFieldValueChanged;
            BonusView.ValueFieldValueChanged += OnValueFieldValueChanged;
        }

        private void OnNameFieldValueChanged(object sender, NameFieldValueChangedEventArgs e)
        {
            Bonus.SetName(e.Name);
        }

        private void OnValueFieldValueChanged(object sender, ValueFieldValueChangedEventArgs e)
        {
            Bonus.SetValue(e.Value);
        }
    }
}
