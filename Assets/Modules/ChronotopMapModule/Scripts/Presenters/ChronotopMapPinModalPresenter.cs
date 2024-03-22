using System;
using SDRGames.Whist.CharacterModule.ScriptableObjects;

namespace SDRGames.Whist.ChronotopMapModule.Presenters
{
    public class ChronotopMapPinModalPresenter
    {
        //view

        public event EventHandler FightButtonClicked;

        public ChronotopMapPinModalPresenter(CommonCharacterParamsModel enemyParams)
        {
            //view initialization
            //view events subscr
        }

        private void OnFightButtonClick(object sender, EventArgs e)
        {
            FightButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
