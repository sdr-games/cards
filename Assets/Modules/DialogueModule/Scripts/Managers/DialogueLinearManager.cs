using System;

using SDRGames.Whist.DialogueModule.Presenters;
using SDRGames.Whist.DialogueModule.ScriptableObjects;
using SDRGames.Whist.DialogueModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.DialogueModule.Managers
{
    public class DialogueLinearManager : MonoBehaviour
    {
        [SerializeField] private DialogueLinearView _speechLinearViewPrefab;
        [SerializeField] private DialogueLinearView _answerLinearViewPrefab;

        private UserInputController _userInputController;
        private DialogueContainerScriptableObject _dialogueContainer;
        private DialogueLinearView _linearView;
        private DialogueSpeechLinearPresenter _speechLinearPresenter;
        private DialogueAnswerLinearPresenter _answerLinearPresenter;
        private int _dialoguesCharactersCount;
        private int _lastCharacterPosition;

        public event EventHandler<CharacterVisibleSyncedEventArgs> CharacterVisibleSynced;

        public void Initialize(DialogueContainerScriptableObject dialogueContainer, UserInputController userInputController)
        {
            _lastCharacterPosition = -1;
            _userInputController = userInputController;
            _dialogueContainer = dialogueContainer;
            CreateDialoguePresenter(_dialogueContainer.FirstSpeech);

            _dialoguesCharactersCount = 0;
            foreach (DialogueScriptableObject dialogue in _dialogueContainer.Dialogues)
            {
                _dialoguesCharactersCount += dialogue.GetCharactersCount();
            }
        }

        private void CreateDialoguePresenter(DialogueSpeechScriptableObject dialogue)
        {
            if(dialogue == null)
            {
                return;
            } 

            _linearView = Instantiate(_speechLinearViewPrefab, transform);
            _linearView.CharacterVisible += OnCharacterVisible;

            _speechLinearPresenter = new DialogueSpeechLinearPresenter(dialogue, _linearView, _userInputController);
            _speechLinearPresenter.Disposed += OnSpeechPresenterDisposed;
        }

        private void OnCharacterVisible(object sender, Views.CharacterVisibleAddedEventArgs e)
        {
            if(_dialoguesCharactersCount == 0)
            {
                return;
            }
            _lastCharacterPosition += e.CharactersAdded;
            CharacterVisibleSynced?.Invoke(this, new CharacterVisibleSyncedEventArgs((float)_lastCharacterPosition / (float)_dialoguesCharactersCount));
        }

        private void CreateDialoguePresenter(DialogueAnswerScriptableObject dialogue)
        {
            if (dialogue == null)
            {
                return;
            }

            _linearView = Instantiate(_answerLinearViewPrefab, transform);
            _linearView.CharacterVisible += OnCharacterVisible;

            _answerLinearPresenter = new DialogueAnswerLinearPresenter(dialogue, _linearView, _userInputController);
            _answerLinearPresenter.Disposed += OnAnswerPresenterDisposed;
        }

        private void OnSpeechPresenterDisposed(object sender, EventArgs e)
        {
            DialogueSpeechLinearPresenter presenter = sender as DialogueSpeechLinearPresenter;
            DialogueAnswerScriptableObject nextDialogue = presenter.Dialogue.Answers[0];
            if (nextDialogue == null)
            {
                UnsetEvents();
                return;
            }
            CreateDialoguePresenter(nextDialogue);
        }

        private void OnAnswerPresenterDisposed(object sender, EventArgs e)
        {
            DialogueAnswerLinearPresenter presenter = sender as DialogueAnswerLinearPresenter;
            DialogueSpeechScriptableObject nextDialogue = presenter.Dialogue.NextSpeech;
            if(nextDialogue == null)
            {
                UnsetEvents();
                return;
            }
            CreateDialoguePresenter(nextDialogue);
        }

        private void UnsetEvents()
        {
            if (_linearView != null)
            {
                _linearView.CharacterVisible -= OnCharacterVisible;
            }
            if (_speechLinearPresenter != null)
            {
                _speechLinearPresenter.Disposed -= OnSpeechPresenterDisposed;
            }
            if (_answerLinearPresenter != null)
            {
                _answerLinearPresenter.Disposed -= OnAnswerPresenterDisposed;
            }
        }

        private void OnDisable()
        {
            UnsetEvents();
        }
    }
}