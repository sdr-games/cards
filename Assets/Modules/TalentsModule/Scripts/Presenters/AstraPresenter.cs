using System;

using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.TalentsModule.Views;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Presenters
{
    [Serializable]
    public class AstraPresenter
    {
        private Astra _astra;
        private TalentView _talentView;

        public AstraPresenter(Astra data, TalentView talentView, Vector2 position)
        {
            _astra = data;

            _talentView = talentView;
            _talentView.Initialize(data.TotalCost, data.Description, position);

            _astra.CurrentPointsChanged += OnCurrentPointsChanged;
        }

        private void OnCurrentPointsChanged(object sender, CurrentPointsChangedEventArgs e)
        {
            _talentView.ChangeCurrentPoints($"{e.CurrentPoints}/{_astra.TotalCost}");
            _talentView.SetFilled(e.CurrentPoints == _astra.TotalCost);
        }
    }
}
