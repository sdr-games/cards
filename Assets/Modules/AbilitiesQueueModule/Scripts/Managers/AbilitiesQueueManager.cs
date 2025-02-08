using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesModule.Models;
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
        public EventHandler<AbilityQueueCountChangedEventArgs> AbilityQueueCountChanged { get; set; }

        public void Initialize(UserInputController userInputController)
        {
            foreach (AbilitySlotManager abilitySlotManager in _abilitySlotManagers)
            {
                abilitySlotManager.Initialize(userInputController);
                abilitySlotManager.AbilitySlotUnbindManually += OnAbilitySlotUnbindManually;
            }
        }

        public bool TryAddAbilityToQueue(Ability ability)
        {
            AbilitySlotManager abilitySlotManager = FindFirstEmptySlot();
            if(abilitySlotManager == null)
            {
                return false;
            }
            abilitySlotManager.Bind(ability);
            AbilityQueueCountChanged?.Invoke(this, new AbilityQueueCountChangedEventArgs(_abilitySlotManagers.All(item => item.Ability == null)));
            return true;
        }

        public List<Ability> PopSelectedAbilities()
        {   List<Ability> selectedAbilities = new List<Ability>();
            foreach (AbilitySlotManager abilitySlotManager in _abilitySlotManagers)
            {
                selectedAbilities.Add(abilitySlotManager.Ability);
                abilitySlotManager.Unbind();
            }
            return selectedAbilities;
        }

        public void ClearBindedAbilities()
        {
            float reverseAmount = 0;
            foreach (AbilitySlotManager abilitySlotManager in _abilitySlotManagers)
            {
                if (abilitySlotManager.Ability == null)
                {
                    continue;
                }
                reverseAmount += abilitySlotManager.Ability.Cost;
                abilitySlotManager.Unbind();
            }
            AbilityQueueCleared?.Invoke(this, new AbilityQueueClearedEventArgs(reverseAmount));
            AbilityQueueCountChanged?.Invoke(this, new AbilityQueueCountChangedEventArgs(_abilitySlotManagers.All(item => item.Ability == null)));
        }

        private AbilitySlotManager FindFirstEmptySlot()
        {
            AbilitySlotManager abilitySlotManager = _abilitySlotManagers.FirstOrDefault(item => item.Ability == null);
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
