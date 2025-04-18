using System;

using SDRGames.Whist.AbilitiesQueueModule.Managers;
using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.NotificationsModule;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.DomainModule.Views
{
    public class CombatUIView : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _enemiesBarsGrid;

        [SerializeField] private ButtonView _endTurnButton;
        [SerializeField] private ButtonView _clearButton;

        [SerializeField] private EndBattlePanelView _endBattlePanelView;

        [SerializeField] private LocalizedString _noTargetErrorMessage;
        [SerializeField] private LocalizedString _comaStartMessage;
        [SerializeField] private LocalizedString _comaStopMessage;

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

        public void SetAsParent(Transform enemyBarsTransform)
        {
            enemyBarsTransform.SetParent(_enemiesBarsGrid.transform, false);
        }

        public void ShowNoTargetError()
        {
            NotificationController.Show(_noTargetErrorMessage.GetLocalizedText());
        }

        public void ShowComaStartNotification()
        {
            NotificationController.Show(_comaStartMessage.GetLocalizedText());
        }

        public void ShowComaStopNotification()
        {
            NotificationController.Show(_comaStopMessage.GetLocalizedText());
        }

        public void ShowVictoryPanel()
        {
            _endBattlePanelView.ShowVictory();
        }

        public void ShowDefeatPanel()
        {
            _endBattlePanelView.ShowDefeat();
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
