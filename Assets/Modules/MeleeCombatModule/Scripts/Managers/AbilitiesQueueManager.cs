using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;
using SDRGames.Whist.MeleeCombatModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.Managers
{
    public class AbilitiesQueueManager : MonoBehaviour
    {
        [SerializeField] private UserInputController _userInputController;
        [SerializeField] private AbilitySlotManager[] _abilitySlotManagers;
        [SerializeField] private AbilityQueueButtonView _applyButton; 
        [SerializeField] private AbilityQueueButtonView _cancelButton; 

        private Dictionary<AbilitySlotManager, MeleeAttackScriptableObject> _bindedAbilities;

        public void AddAbilityToQueue(MeleeAttackScriptableObject meleeAttackScriptableObject)
        {
            AbilitySlotManager abilitySlotManager = _bindedAbilities.FirstOrDefault(item => item.Value == null).Key;
            abilitySlotManager.Bind(meleeAttackScriptableObject);
            _bindedAbilities[abilitySlotManager] = meleeAttackScriptableObject;
            SwitchButtonsActivity();
        }

        private void OnAbilitySlotCleared(object sender, System.EventArgs e)
        {
            AbilitySlotManager abilitySlotManager = (AbilitySlotManager)sender;
            if (_bindedAbilities.ContainsKey(abilitySlotManager))
            {
                _bindedAbilities[abilitySlotManager] = null;
            }
            SwitchButtonsActivity();
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

        private void OnEnable()
        {
            _bindedAbilities = new Dictionary<AbilitySlotManager, MeleeAttackScriptableObject>();

            foreach (var abilitySlotManager in _abilitySlotManagers)
            {
                abilitySlotManager.Initialize(_userInputController);
                abilitySlotManager.AbilitySlotUnbound += OnAbilitySlotCleared;
                _bindedAbilities.Add(abilitySlotManager, null);
            }
            _applyButton.Initialize(_userInputController);
            _cancelButton.Initialize(_userInputController);
        }

        private void OnDisable()
        {
            foreach (var abilitySlotManager in _abilitySlotManagers)
            {
                abilitySlotManager.AbilitySlotUnbound -= OnAbilitySlotCleared;
            }
        }
    }
}
