using System;

using SDRGames.Whist.CharacterInfoModule.ScriptableObjects;
using SDRGames.Whist.DialogueModule.ScriptableObjects;
using SDRGames.Whist.DialogueModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

namespace SDRGames.Whist.DialogueModule.Presenters
{
    [Serializable]
    public class DialogueSpeechLinearPresenter : IDisposable
    {
        public DialogueSpeechScriptableObject Dialogue { get; private set; }
        private DialogueLinearView _linearView;

        public event EventHandler Disposed;

        public DialogueSpeechLinearPresenter(DialogueSpeechScriptableObject dialogue, DialogueLinearView linearView, UserInputController userInputController, float fadeSpeed, int rolloverSpeed)
        {
            Dialogue = dialogue;
            CharacterInfoScriptableObject character = Dialogue.Character;

            _linearView = linearView;
            _linearView.Initialize(character.CharacterPortrait, character.CharacterName.GetLocalizedString(), Dialogue.TextLocalization.GetLocalizedText(), userInputController, fadeSpeed, rolloverSpeed);
            _linearView.Destroyed += OnViewDestroyed;
        }

        public void Dispose()
        {
            _linearView.Destroyed -= OnViewDestroyed;
            Disposed?.Invoke(this, EventArgs.Empty);
            GC.SuppressFinalize(this);
        }

        private void OnViewDestroyed(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
