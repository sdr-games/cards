using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Models
{
    [Serializable]
    public class DialogueAnswerData
    {
        [field: SerializeField] public DialogueLocalizationData LocalizationData { get; set; }
        [field: SerializeField] public DialogueScriptableObject NextDialogue { get; set; }
        [field: SerializeField] public List<DialogueAnswerCondition> Conditions { get; set; }

        public bool CheckConditions()
        {
            bool isAnswerShown = true;
            //if (Conditions != null && Conditions.Count > 0)
            //{
            //    foreach (DialogueAnswerCondition condition in Conditions)
            //    {
            //        switch (condition.AnswerConditionType)
            //        {
            //            case AnswerConditionType.CharacteristicCheck:
            //                if (condition.Reversed)
            //                {
            //                    if (condition.RequiredValue < GameData.GetPlayer().CharacterParams.Characteristics.GetCharacteristicByName(condition.Characteristic.ToString()).GetCriticalValue())
            //                    {
            //                        isAnswerShown = false;
            //                    }
            //                }
            //                else
            //                {
            //                    if (condition.RequiredValue > GameData.GetPlayer().CharacterParams.Characteristics.GetCharacteristicByName(condition.Characteristic.ToString()).GetCriticalValue())
            //                    {
            //                        isAnswerShown = false;
            //                    }
            //                }

            //                break;
            //            case AnswerConditionType.SkillCheck:
            //                if (condition.Reversed)
            //                {
            //                    if (condition.RequiredValue < GameData.GetPlayer().CharacterParams.skills.GetByName(condition.Skill.ToString()).dice.GetCriticalValue())
            //                    {
            //                        isAnswerShown = false;
            //                    }
            //                }
            //                else
            //                {
            //                    if (condition.RequiredValue > GameData.GetPlayer().CharacterParams.skills.GetByName(condition.Skill.ToString()).dice.GetCriticalValue())
            //                    {
            //                        isAnswerShown = false;
            //                    }
            //                }

            //                break;
            //            case AnswerConditionType.QuestAccepted:
            //                if (condition.Reversed)
            //                {
            //                    if (GameData.Instance.progressQuests.Contains(condition.Quest))
            //                    {
            //                        isAnswerShown = false;
            //                    }
            //                }
            //                else
            //                {
            //                    if (!GameData.Instance.progressQuests.Contains(condition.Quest))
            //                    {
            //                        isAnswerShown = false;
            //                    }
            //                }

            //                break;
            //            case AnswerConditionType.QuestFinished:
            //                if (condition.Reversed)
            //                {
            //                    if (GameData.Instance.finishedQuests.Contains(condition.Quest))
            //                    {
            //                        isAnswerShown = false;
            //                    }
            //                }
            //                else
            //                {
            //                    if (!GameData.Instance.finishedQuests.Contains(condition.Quest))
            //                    {
            //                        isAnswerShown = false;
            //                    }
            //                }

            //                break;
            //            case AnswerConditionType.QuestCompleted:
            //                if (condition.Reversed)
            //                {
            //                    if (GameData.Instance.failedQuests.Contains(condition.Quest))
            //                    {
            //                        isAnswerShown = false;
            //                    }
            //                }
            //                else
            //                {
            //                    if (!GameData.Instance.failedQuests.Contains(condition.Quest))
            //                    {
            //                        isAnswerShown = false;
            //                    }
            //                }

            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}
            return isAnswerShown;
        }
    }
}