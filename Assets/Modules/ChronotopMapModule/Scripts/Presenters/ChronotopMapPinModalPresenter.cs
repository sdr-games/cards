using System;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.ChronotopMapModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

namespace SDRGames.Whist.ChronotopMapModule.Presenters
{
    [Serializable]
    public class ChronotopMapPinModalPresenter : IDisposable
    {
        private ChronotopMapPinModalView _modalView;

        public event EventHandler FightButtonClicked;
        public event EventHandler Disposed;

        public ChronotopMapPinModalPresenter(CommonCharacterParamsModel enemyParams, ChronotopMapPinModalView modalView, UserInputController userInputController)
        {
            _modalView = modalView;
            _modalView.Initialize(
                enemyParams.CharacterInfo.CharacterPortrait,
                enemyParams.CharacterInfo.CharacterNameLocalization.GetLocalizedString(),
                enemyParams.CharacterInfo.CharacterDescriptionLocalization.GetLocalizedString(),
                userInputController
            );
        }

        public void ShowView()
        {
            _modalView.Show();
            _modalView.FightButtonClicked += OnFightButtonClick;
            _modalView.ViewClosed += OnCloseButtonClick;
        }

        public void HideView()
        {
            _modalView.Hide();
            _modalView.FightButtonClicked -= OnFightButtonClick;
            _modalView.ViewClosed -= OnCloseButtonClick;
        }

        public void Dispose()
        {
            _modalView.FightButtonClicked -= OnFightButtonClick;
            _modalView.ViewClosed -= OnCloseButtonClick;
            Disposed?.Invoke(this, EventArgs.Empty);
            GC.SuppressFinalize(this);
        }

        private void OnFightButtonClick(object sender, EventArgs e)
        {
            FightButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
