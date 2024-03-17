using SDRGames.Whist.DialogueSystem.Presenters;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;
using SDRGames.Whist.DialogueSystem.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Managers
{
    public class DialogueLinearManager : MonoBehaviour
    {
        [SerializeField] private UserInputController _userInputController;
        [SerializeField] private DialogueContainerScriptableObject _dialogueContainer;
        [SerializeField] private DialogueLinearView _speechLinearViewPrefab;
        [SerializeField] private DialogueLinearView _answerLinearViewPrefab;

        private void Start()
        {
            Initialize(_dialogueContainer);
        }

        public void Initialize(DialogueContainerScriptableObject dialogueContainer)
        {
            _dialogueContainer = dialogueContainer;
            CreateDialoguePresenter(_dialogueContainer.FirstSpeech);
        }

        private void CreateDialoguePresenter(DialogueSpeechScriptableObject dialogue)
        {
            DialogueLinearView linearView = Instantiate(_speechLinearViewPrefab, transform);

            DialogueSpeechLinearPresenter presenter = new DialogueSpeechLinearPresenter(dialogue, linearView, _userInputController);
            presenter.Disposed += OnSpeechPresenterDisposed;
        }

        private void CreateDialoguePresenter(DialogueAnswerScriptableObject dialogue)
        {
            DialogueLinearView linearView = Instantiate(_answerLinearViewPrefab, transform);

            DialogueAnswerLinearPresenter presenter = new DialogueAnswerLinearPresenter(dialogue, linearView, _userInputController);
            presenter.Disposed += OnAnswerPresenterDisposed;
        }

        private void OnSpeechPresenterDisposed(object sender, System.EventArgs e)
        {
            DialogueSpeechLinearPresenter presenter = sender as DialogueSpeechLinearPresenter;
            DialogueAnswerScriptableObject nextDialogue = presenter.Dialogue.Answers[0];
            if (nextDialogue == null)
            {
                return;
            }
            CreateDialoguePresenter(nextDialogue);
        }

        private void OnAnswerPresenterDisposed(object sender, System.EventArgs e)
        {
            DialogueAnswerLinearPresenter presenter = sender as DialogueAnswerLinearPresenter;
            DialogueSpeechScriptableObject nextDialogue = presenter.Dialogue.NextSpeech;
            if(nextDialogue == null)
            {
                return;
            }
            CreateDialoguePresenter(nextDialogue);
        }
    }
}