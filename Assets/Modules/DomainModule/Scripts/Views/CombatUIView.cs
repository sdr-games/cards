using System;

using SDRGames.Whist.AbilitiesQueueModule.Managers;
using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.DomainModule.Views
{
    public class CombatUIView : MonoBehaviour
    {
        [SerializeField] private ButtonView _endTurnButton; 

        public event EventHandler EndTurnButtonClicked;
        public void Initialize(UserInputController userInputController)
        {
            _endTurnButton.Initialize(userInputController);
            _endTurnButton.ButtonClicked += OnEndTurnButtonClicked;
        }

        public void OnSelectedCardsCountChanged(object sender, SelectedCardsCountChangedEventArgs e)
        {
            SwitchButtonsActivity(e.IsEmpty);
        }

        public void OnAbilityQueueCountChanged(object sender, AbilityQueueCountChangedEventArgs e)
        {
            SwitchButtonsActivity(e.IsEmpty);
        }

        private void OnEndTurnButtonClicked(object sender, EventArgs e)
        {
            EndTurnButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void SwitchButtonsActivity(bool isEmpty)
        {
            if (isEmpty)
            {
                _endTurnButton.Deactivate();
                return;
            }
            _endTurnButton.Activate();
        }
    }
}
