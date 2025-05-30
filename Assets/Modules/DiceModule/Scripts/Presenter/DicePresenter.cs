using System;
using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.DiceModule.Models;
using SDRGames.Whist.DiceModule.Views;

using UnityEngine;

namespace SDRGames.Whist.DiceModule.Presenters
{
    public class DicePresenter
    {
        private Dice _dice;
        private DiceView _diceView;

        public DicePresenter(Dice dice, DiceView diceView)
        {
            _dice = dice;
            _diceView = diceView;

            _diceView.Initialize(_dice.Name, _dice.GetString(true));

            _dice.DiceChanged += OnDiceChanged;
        }

        private void OnDiceChanged(object sender, DiceChangedEventArgs e)
        {
            _diceView.SetDiceText(e.Dice.GetString(true));
        }
    }
}
