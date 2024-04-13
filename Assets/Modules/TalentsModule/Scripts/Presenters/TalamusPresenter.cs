using System;

using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Presenters
{
    [Serializable]
    public class TalamusPresenter
    {
        private readonly Color ACTIVE_COLOR = Color.blue;
        private readonly Color INACTIVE_COLOR = Color.cyan;

        private Talamus _talamus;
        private TalentView _talentView;

        public TalamusPresenter(Talamus data, TalentView talentView, Vector2 position)
        {
            _talamus = data;

            _talentView = talentView;
            _talentView.Initialize(ACTIVE_COLOR, INACTIVE_COLOR, position);

            //_data.ActiveChanged += ChangeActive;
        }

        public void ChangeActive(object sender, ActiveChangedEventArgs e)
        {
            _talentView.ChangeActive(e.IsActive);
        }
    }
}
