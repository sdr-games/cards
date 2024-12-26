using System;

using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.TalentsModule.Views;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Presenters
{
    [Serializable]
    public class TalamusPresenter
    {
        private Talamus _talamus;
        private TalentView _talentView;

        public TalamusPresenter(Talamus talamus, TalentView talentView, Vector2 position)
        {
            _talamus = talamus;

            _talentView = talentView;
            _talentView.Initialize(talamus.TotalCost, talamus.Description, position);

            _talamus.CurrentPointsChanged += OnCurrentPointsChanged;
        }

        private void OnCurrentPointsChanged(object sender, CurrentPointsChangedEventArgs e)
        {
            _talentView.ChangeCurrentPoints($"{e.CurrentPoints}/{_talamus.TotalCost}");
        }
    }
}
