using System;

using SDRGames.Whist.DialogueSystem.ScriptableObjects;
using SDRGames.Whist.DialogueSystem.Views;
using SDRGames.Whist.UserInputModule.Controller;

namespace SDRGames.Whist.DialogueSystem.Presenters
{
    public class DialogueAnswerLinearPresenter : IDisposable
    {
        public DialogueAnswerScriptableObject Dialogue { get; private set; }
        private DialogueLinearView _linearView;

        public event EventHandler Disposed;

        public DialogueAnswerLinearPresenter(DialogueAnswerScriptableObject dialogue, DialogueLinearView linearView, UserInputController userInputController)
        {
            Dialogue = dialogue;
            DialogueCharacterScriptableObject character = Dialogue.Character;

            _linearView = linearView;
            _linearView.Initialize(character.CharacterPortrait, character.CharacterNameLocalization.GetLocalizedString(), Dialogue.TextLocalization.GetLocalizedText(), userInputController);
            _linearView.Destroyed += OnViewDestroyed;
        }

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
            GC.SuppressFinalize(this);
        }

        private void OnViewDestroyed(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
