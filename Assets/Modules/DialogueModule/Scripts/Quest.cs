using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[Serializable]
public class Quest : ScriptableObject
{
    public enum QuestStatuses { Progress, Completed, Finished, Failed, Declined };
    private QuestStatuses _status;

    [SerializeField] private int _id;
    [SerializeField] private string _title;
    [SerializeField] private LocalizedString _description;
    [SerializeField] private int _experienceReward;
    [SerializeField] private bool _autoComplete, _chainAutoStart;
    public int order;


    private void OnValidate()
    {
        
    }

    public void Start()
    {
        
    }
    public void Finish()
    {
        
    }
    public void Decline()
    {
        
    }

    public void UpdateCondition(int conditionIndex)
    {
        
    }
    private void CheckQuestStatus()
    {
        
    }
    public static Quest GetActiveQuestById(int questId)
    {
        return null;
    }
    public string GetString()
    {
        return "";

    }
}
