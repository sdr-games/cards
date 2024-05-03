using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.TalentsEditorModule.Views;

using UnityEngine;

namespace SDRGames.Whist.TalentsEditorModule.Presenters
{
    public abstract class BaseNodePresenter
    {
        public BaseNodePresenter() { }

        public abstract BaseNodeView GetNodeView();

        public abstract void Initialize(string name, Vector2 position);

        protected abstract void OnNodeNameTextFieldChanged(object sender, NodeNameChangedEventArgs e);

        protected abstract void OnDescriptionLocalizationFieldChanged(object sender, LocalizationDataChangedEventArgs e);

        protected abstract void OnCostTextFieldChanged(object sender, CostChangedEventArgs e);
    }
}
