using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;
using SDRGames.Whist.MeleeCombatModule.Views;
using SDRGames.Whist.NotificationsModule;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.Managers
{
    public class AbilitiesQueueManager : MonoBehaviour
    {
        [SerializeField] private AbilitySlotManager[] _abilitySlotManagers;
        [SerializeField] private AbilityQueueButtonView _applyButton;
        [SerializeField] private AbilityQueueButtonView _cancelButton;
        [SerializeField] private LocalizedString _errorMessage;

        public bool IsFull => FindFirstEmptySlot() == null;

        private Dictionary<AbilitySlotManager, MeleeAttackScriptableObject> _bindedAbilities;

        public event EventHandler<ApplyButtonClickedEventArgs> ApplyButtonClicked;
        public event EventHandler<AbilityQueueClearedEventArgs> AbilityQueueCleared;

        public void Initialize(UserInputController userInputController)
        {
            _bindedAbilities = new Dictionary<AbilitySlotManager, MeleeAttackScriptableObject>();

            foreach (var abilitySlotManager in _abilitySlotManagers)
            {
                abilitySlotManager.Initialize(userInputController);
                abilitySlotManager.AbilitySlotUnbound += OnAbilitySlotCleared;
                _bindedAbilities.Add(abilitySlotManager, null);
            }
            _applyButton.Initialize(userInputController);
            _cancelButton.Initialize(userInputController);

            _applyButton.ButtonClicked += OnApplyButtonClicked;
            _cancelButton.ButtonClicked += OnCancelButtonClicked;
        }

        public void AddAbilityToQueue(MeleeAttackScriptableObject meleeAttackScriptableObject)
        {
            AbilitySlotManager abilitySlotManager = FindFirstEmptySlot();
            abilitySlotManager.Bind(meleeAttackScriptableObject);
            _bindedAbilities[abilitySlotManager] = meleeAttackScriptableObject;
            SwitchButtonsActivity();
        }

        private AbilitySlotManager FindFirstEmptySlot()
        {
            AbilitySlotManager abilitySlotManager = _bindedAbilities.FirstOrDefault(item => item.Value == null).Key;
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
            if (_bindedAbilities.ContainsKey(abilitySlotManager))
            {
                reverseAmount = _bindedAbilities[abilitySlotManager].Cost;
                _bindedAbilities[abilitySlotManager] = null;
            }
            SwitchButtonsActivity();
            AbilityQueueCleared?.Invoke(this, new AbilityQueueClearedEventArgs(reverseAmount));
        }

        private void SwitchButtonsActivity()
        {
            if(_bindedAbilities.Values.FirstOrDefault(item => item != null))
            {
                _applyButton.Activate();
                _cancelButton.Activate();
                return;
            }
            _applyButton.Deactivate();
            _cancelButton.Deactivate();
        }

        private void OnApplyButtonClicked(object sender, EventArgs e)
        {
            Dictionary<AbilitySlotManager, MeleeAttackScriptableObject> bindedAbilities = new Dictionary<AbilitySlotManager, MeleeAttackScriptableObject>(_bindedAbilities);
            float totalCost = bindedAbilities.Values.Where(item => item != null).Sum(item => item.Cost);
            ClearBindedAbilities(bindedAbilities);
            ApplyButtonClicked?.Invoke(this, new ApplyButtonClickedEventArgs(totalCost, bindedAbilities.Values.ToList()));
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            Dictionary<AbilitySlotManager, MeleeAttackScriptableObject> bindedAbilities = new Dictionary<AbilitySlotManager, MeleeAttackScriptableObject>(_bindedAbilities);
            ClearBindedAbilities(bindedAbilities);
        }

        private void ClearBindedAbilities(Dictionary<AbilitySlotManager, MeleeAttackScriptableObject> bindedAbilities)
        {
            foreach (KeyValuePair<AbilitySlotManager, MeleeAttackScriptableObject> item in bindedAbilities)
            {
                if (item.Value == null)
                {
                    continue;
                }
                item.Key.Unbind();
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

            if (_applyButton == null)
            {
                Debug.LogError("Apply Button не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_cancelButton == null)
            {
                Debug.LogError("Cancel Button не был назначен");
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

            _applyButton.ButtonClicked -= OnApplyButtonClicked;
            _cancelButton.ButtonClicked -= OnCancelButtonClicked;
        }
    }
}
