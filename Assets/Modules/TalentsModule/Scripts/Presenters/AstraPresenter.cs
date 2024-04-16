using System;

using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.TalentsModule.Views;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Presenters
{
    [Serializable]
    public class AstraPresenter
    {
        private readonly Color ACTIVE_COLOR = Color.yellow;
        private readonly Color INACTIVE_COLOR = Color.gray;

        private Astra _astra;
        private TalentView _talentView;

        public AstraPresenter(Astra data, TalentView talentView, Vector2 position)
        {
            _astra = data;

            _talentView = talentView;
            _talentView.Initialize(ACTIVE_COLOR, INACTIVE_COLOR, position);
        }
    }
}
