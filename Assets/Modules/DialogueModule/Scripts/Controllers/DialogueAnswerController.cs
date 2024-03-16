using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SDRGames.Whist.DialogueSystem.Controllers
{
    public class DialogueAnswerController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [HideInInspector] public DialogueScriptableObject nextDialogue;
        [HideInInspector] public DialogueController dialogueController;

        public void Initialize(DialogueScriptableObject nextDialogue, DialogueController dialogueController, string answerText)
        {

            this.nextDialogue = nextDialogue;
            this.dialogueController = dialogueController;
            this.GetComponent<TextMeshProUGUI>().text = answerText;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //if (nextDialogue != null)
            //{
            //    dialogueController.ClearDialogueWindow();
            //    dialogueController.CreateDialogueText((DialogueSpeechScriptableObject)nextDialogue);
            //}
            //else
            //{
            //    dialogueController.CloseDialogueWindow();
            //}
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //GetComponent<TextMeshProUGUI>().color = MessageColors.GetColor("AnswerHighlighted");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //GetComponent<TextMeshProUGUI>().color = MessageColors.GetColor("Answer");
        }
    }
}