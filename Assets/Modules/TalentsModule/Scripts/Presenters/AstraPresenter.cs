using System;
using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Presenters
{
    [Serializable]
    public class AstraPresenter
    {
        private readonly Color ACTIVE_COLOR = Color.yellow;
        private readonly Color INACTIVE_COLOR = Color.gray;

        private AstraScriptableObject _data;
        private TalentView _talentView;

        public AstraPresenter(AstraScriptableObject data, TalentView talentView)
        {
            _data = data;

            _talentView = talentView;
            _talentView.Initialize(ACTIVE_COLOR, INACTIVE_COLOR, data.Position);

            //_data.ActiveChanged += ChangeActive;
        }

        //public void ChangeActive(object sender, ActiveChangedEventArgs e)
        //{
        //    _talentView.ChangeActive(e.IsActive);
        //}
    }
}
