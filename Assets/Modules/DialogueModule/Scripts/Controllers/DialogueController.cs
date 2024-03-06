using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using TMPro;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Controllers
{
    public class DialogueController : MonoBehaviour
    {
        private Transform dialogueTextScrollViewContent;
        private Transform dialogueAnswersScrollViewContent;

        public List<DialogueScriptableObject> dialogues;
        public void Run()
        {
            dialogueTextScrollViewContent = transform.Find("DialogueTextScrollView/Viewport/Content");
            dialogueAnswersScrollViewContent = transform.Find("DialogueAnswersScrollView/Viewport/Content");
            CreateDialogueText();
        }
        public void CreateDialogueText(DialogueSpeechScriptableObject dialogueSO = null, bool fullscreen = false)
        {
            if (dialogueSO != null)
            {
                GameObject dialogueText = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Dialogue/DialogueText", typeof(GameObject)), dialogueTextScrollViewContent);
                //StartCoroutine(ShowUIElementWithFading(dialogueText));
                dialogueText.GetComponent<TextMeshProUGUI>().text = dialogueSO.LocalizationData.SelectedLocalizedText;
                if (dialogueSO.Answers != null && dialogueSO.Answers.Count > 0)
                {
                    CreateAnswers(SortAnswersByConditions(dialogueSO.Answers), fullscreen);
                }

                //if (dialogueSO.Quest != null)
                //{
                //    if (dialogueSO.DialogueQuestAction == DialogueScriptableObject.DialogueQuestActions.Accept /*&& GameData.IsQuestAvailable(dialogueSO.Quest)*/)
                //    {
                //        dialogueSO.Quest.Start();
                //    }
                //    else if (dialogueSO.DialogueQuestAction == DialogueScriptableObject.DialogueQuestActions.Finish)
                //    {
                //        dialogueSO.Quest.Finish();
                //    }
                //}
            }
            else
            {
                CloseDialogueWindow();
            }
        }
        public List<DialogueAnswerData> SortAnswersByConditions(List<DialogueAnswerData> answers)
        {
            List<DialogueAnswerData> answersSorted = new List<DialogueAnswerData>();
            foreach (DialogueAnswerData answer in answers)
            {
                //if (answer.conditions.Count > 0 && !answer.CheckConditions())
                //    continue;
                answersSorted.Add(answer);
            }
            return answersSorted;
        }
        public void CreateAnswers(List<DialogueAnswerData> answers, bool fullscreen = false)
        {
            int i = 0;
            foreach (DialogueAnswerData answer in answers)
            {
                if (answer.CheckConditions())
                {
                    string text = "";
                    GameObject answerButton = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Dialogue/Answer", typeof(GameObject)), dialogueAnswersScrollViewContent);
                    if (fullscreen)
                    {
                        text = answer.LocalizationData.SelectedLocalizedText;
                    }
                    else
                    {
                        text = $"{i + 1}. {answer.LocalizationData.SelectedLocalizedText}";
                    }
                    answerButton.GetComponent<DialogueAnswerController>().Initialize(answer.NextDialogue, this, text);
                    i++;
                }
            }
        }
        public void ClearDialogueWindow(string invokeMethod = "")
        {
            foreach (Transform element in dialogueTextScrollViewContent)
            {
                /*if (SceneManager.GetActiveScene().name == "NewGame")
                    //StartCoroutine(HideUIElementWithFading(element.gameObject, invokeMethod));
                else*/
                Destroy(element.gameObject);
            }
            foreach (Transform element in dialogueAnswersScrollViewContent)
            {
                Destroy(element.gameObject);
            }
        }

        public void CloseDialogueWindow()
        {
            //GameData.GetPlayer()._isBusy = false;
            //GameState.rootUI.CloseDialogueWindow();
        }
    }
}