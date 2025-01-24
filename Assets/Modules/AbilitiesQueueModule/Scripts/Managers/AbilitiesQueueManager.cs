using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.NotificationsModule;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.Managers
{
    public class AbilitiesQueueManager : HideableUIView
    {
        [SerializeField] private AbilitySlotManager[] _abilitySlotManagers;
        [SerializeField] private LocalizedString _errorMessage;

        public bool IsFull => FindFirstEmptySlot() == null;

        public event EventHandler<AbilityQueueClearedEventArgs> AbilityQueueCleared;
        public event EventHandler<AbilityQueueCountChangedEventArgs> AbilityQueueCountChanged;

        public void Initialize(UserInputController userInputController)
        {
            foreach (AbilitySlotManager abilitySlotManager in _abilitySlotManagers)
            {
                abilitySlotManager.Initialize(userInputController);
                abilitySlotManager.AbilitySlotUnbindManually += OnAbilitySlotUnbindManually;
            }
        }

        public bool TryAddAbilityToQueue(AbilityScriptableObject abilityScriptableObject)
        {
            AbilitySlotManager abilitySlotManager = FindFirstEmptySlot();
            if(abilitySlotManager == null)
            {
                return false;
            }
            abilitySlotManager.Bind(abilityScriptableObject);
            AbilityQueueCountChanged?.Invoke(this, new AbilityQueueCountChangedEventArgs(_abilitySlotManagers.All(item => item.AbilityScriptableObject == null)));
            return true;
        }

        public List<AbilityScriptableObject> PopSelectedAbilities()
        {
            if(_abilitySlotManagers.All(item => item.AbilityScriptableObject == null))
            {
                return null;
            }

            List<AbilityScriptableObject> selectedAbilities = new List<AbilityScriptableObject>();
            foreach (AbilitySlotManager abilitySlotManager in _abilitySlotManagers)
            {
                selectedAbilities.Add(abilitySlotManager.AbilityScriptableObject);
                abilitySlotManager.Unbind();
            }
            return selectedAbilities;
        }

        public void ClearBindedAbilities()
        {
            float reverseAmount = 0;
            foreach (AbilitySlotManager abilitySlotManager in _abilitySlotManagers)
            {
                if (abilitySlotManager.AbilityScriptableObject == null)
                {
                    continue;
                }
                reverseAmount += abilitySlotManager.AbilityScriptableObject.Cost;
                abilitySlotManager.Unbind();
            }
            AbilityQueueCleared?.Invoke(this, new AbilityQueueClearedEventArgs(reverseAmount));
            AbilityQueueCountChanged?.Invoke(this, new AbilityQueueCountChangedEventArgs(_abilitySlotManagers.All(item => item.AbilityScriptableObject == null)));
        }

        private AbilitySlotManager FindFirstEmptySlot()
        {
            AbilitySlotManager abilitySlotManager = _abilitySlotManagers.FirstOrDefault(item => item.AbilityScriptableObject == null);
            if(abilitySlotManager == null)
            {
                Notification.Show(_errorMessage.GetLocalizedText());
            }
            return abilitySlotManager;
        }

        private void OnAbilitySlotUnbindManually(object sender, AbilityQueueClearedEventArgs e)
        {
            AbilityQueueCleared?.Invoke(this, e);
        }

        private void OnEnable()
        {
            if (_abilitySlotManagers.Length == 0)
            {
                Debug.LogError("Ability Slot Managers пустой");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
