using System;

using SDRGames.Whist.AbilitiesQueueModule.Managers;
using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.NotificationsModule;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.DomainModule.Views
{
    public class CombatUIView : MonoBehaviour
    {
        [SerializeField] private ButtonView _endTurnButton;
        [SerializeField] private ButtonView _clearButton;

        [SerializeField] private LocalizedString _noTargetErrorMessage;

        public event EventHandler EndTurnButtonClicked;
        public event EventHandler ClearButtonClicked;

        public void Initialize(UserInputController userInputController)
        {
            _endTurnButton.Initialize(userInputController, true);
            _endTurnButton.ButtonClicked += OnEndTurnButtonClicked;

            _clearButton.Initialize(userInputController);
            _clearButton.ButtonClicked += OnClearButtonClicked;
        }

        public void OnSelectedCardsCountChanged(object sender, SelectedCardsCountChangedEventArgs e)
        {
            SwitchButtonsActivity(e.IsEmpty);
        }

        public void OnAbilityQueueCountChanged(object sender, AbilityQueueCountChangedEventArgs e)
        {
            SwitchButtonsActivity(e.IsEmpty);
        }

        public void ShowNoTargetError()
        {
            NotificationController.Show(_noTargetErrorMessage.GetLocalizedText());
        }

        private void OnEndTurnButtonClicked(object sender, EventArgs e)
        {
            EndTurnButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            ClearButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void SwitchButtonsActivity(bool isEmpty)
        {
            if (isEmpty)
            {
                _clearButton.Deactivate();
                return;
            }
            _clearButton.Activate();
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_endTurnButton), _endTurnButton);
            this.CheckFieldValueIsNotNull(nameof(_clearButton), _clearButton);
        }

        private void OnDestroy()
        {
            _endTurnButton.ButtonClicked -= OnEndTurnButtonClicked;
            _clearButton.ButtonClicked -= OnClearButtonClicked;
        }
    }
}
