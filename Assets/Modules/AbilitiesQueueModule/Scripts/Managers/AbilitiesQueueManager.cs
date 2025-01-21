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
        [SerializeField] private ButtonView _cancelButton;
        [SerializeField] private LocalizedString _errorMessage;

        public bool IsFull => FindFirstEmptySlot() == null;
        public bool IsEmpty => _bindedAbilities.All(item => item == null);

        private List<AbilitySlotManager> _bindedAbilities;

        public event EventHandler<AbilityQueueCountChangedEventArgs> AbilityQueueCountChanged;
        public event EventHandler<MeleeEndTurnEventArgs> ApplyButtonClicked;
        public event EventHandler<AbilityQueueClearedEventArgs> AbilityQueueCleared;

        public void Initialize(UserInputController userInputController)
        {
            _bindedAbilities = new List<AbilitySlotManager>();

            foreach (var abilitySlotManager in _abilitySlotManagers)
            {
                abilitySlotManager.Initialize(userInputController);
                abilitySlotManager.AbilitySlotUnbound += OnAbilitySlotCleared;
            }
            _cancelButton.Initialize(userInputController);
            _cancelButton.ButtonClicked += OnCancelButtonClicked;
        }

        public bool TryAddAbilityToQueue(AbilityScriptableObject abilityScriptableObject)
        {
            AbilitySlotManager abilitySlotManager = FindFirstEmptySlot();
            if(abilitySlotManager == null)
            {
                return false;
            }
            abilitySlotManager.Bind(abilityScriptableObject);
            _bindedAbilities.Add(abilitySlotManager);
            AbilityQueueCountChanged?.Invoke(this, new AbilityQueueCountChangedEventArgs(_bindedAbilities.FirstOrDefault(item => item == null)));
            return true;
        }

        private AbilitySlotManager FindFirstEmptySlot()
        {
            AbilitySlotManager abilitySlotManager = _bindedAbilities.FirstOrDefault(item => item.AbilityScriptableObject == null);
            if(abilitySlotManager == null)
            {
                Notification.Show(_errorMessage.GetLocalizedText());
            }
            return abilitySlotManager;
        }

        private void OnAbilitySlotCleared(object sender, EventArgs e)
        {
            AbilitySlotManager abilitySlotManager = (AbilitySlotManager)sender;
            float reverseAmount = 0;
            if (_bindedAbilities.Contains(abilitySlotManager))
            {
                reverseAmount = abilitySlotManager.AbilityScriptableObject.Cost;
                _bindedAbilities.Remove(abilitySlotManager);
            }
            AbilityQueueCountChanged?.Invoke(this, new AbilityQueueCountChangedEventArgs(_bindedAbilities.FirstOrDefault(item => item == null)));
            AbilityQueueCleared?.Invoke(this, new AbilityQueueClearedEventArgs(reverseAmount));
        }

        public List<AbilitySlotManager> GetSelectedAbilities()
        {
            if(_bindedAbilities.All(item => item is null))
            {
                return null;
            }
            List<AbilitySlotManager> abilities = new List<AbilitySlotManager>(_bindedAbilities); 
            ClearBindedAbilities();
            return abilities;
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            ClearBindedAbilities();
        }

        public void ClearBindedAbilities()
        {
            List<AbilitySlotManager> bindedAbilities = new List<AbilitySlotManager>(_bindedAbilities);
            foreach (AbilitySlotManager bindedAbility in bindedAbilities)
            {
                if (bindedAbility == null)
                {
                    continue;
                }
                bindedAbility.Unbind();
            }
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

        private void OnDisable()
        {
            foreach (var abilitySlotManager in _abilitySlotManagers)
            {
                abilitySlotManager.AbilitySlotUnbound -= OnAbilitySlotCleared;
            }
            _cancelButton.ButtonClicked -= OnCancelButtonClicked;
        }
    }
}
