using System;

using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;
using SDRGames.Whist.AbilitiesQueueModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.Managers
{
    public class AbilitySlotManager : MonoBehaviour
    {
        [SerializeField] private AbilitySlotView _abilitySlotView;
        
        public AbilityScriptableObject AbilityScriptableObject { get; private set; }

        public event EventHandler AbilitySlotUnbound;

        public void Initialize(UserInputController userInputController)
        {
            _abilitySlotView.Initialize(userInputController);
        }

        public void Bind(AbilityScriptableObject abilityScriptableObject)
        {
            AbilityScriptableObject = abilityScriptableObject;
            _abilitySlotView.SetIconSprite(abilityScriptableObject.Icon);
            _abilitySlotView.AbilitySlotUnbound += OnAbilitySlotCleared;
        }

        public void Unbind()
        {
            AbilityScriptableObject = null;
            _abilitySlotView.Unbind();
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
