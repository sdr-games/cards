using System;

using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;
using SDRGames.Whist.MeleeCombatModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.Managers
{
    public class AbilitySlotManager : MonoBehaviour
    {
        private MeleeAttackScriptableObject _meleeAbilityScriptableObject;
        [SerializeField] private AbilitySlotView _abilitySlotView;

        public event EventHandler AbilitySlotUnbound;

        public void Initialize(UserInputController userInputController)
        {
            _abilitySlotView.Initialize(userInputController);
        }

        public void Bind(MeleeAttackScriptableObject meleeAttackScriptableObject)
        {
            _meleeAbilityScriptableObject = meleeAttackScriptableObject;
            _abilitySlotView.SetIconSprite(_meleeAbilityScriptableObject.Icon);
            _abilitySlotView.AbilitySlotUnbound += OnAbilitySlotCleared;
        }

        private void OnAbilitySlotCleared(object sender, EventArgs e)
        {
            _abilitySlotView.AbilitySlotUnbound -= OnAbilitySlotCleared;
            AbilitySlotUnbound?.Invoke(this, EventArgs.Empty);
        }

        private void OnDisable()
        {
            if (_abilitySlotView == null)
            {
                return;
            }
            _abilitySlotView.AbilitySlotUnbound -= OnAbilitySlotCleared;
        }
    }
}
