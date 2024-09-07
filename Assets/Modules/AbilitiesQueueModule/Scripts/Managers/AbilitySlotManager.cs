using System;

using SDRGames.Whist.AbilitiesQueueModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.Managers
{
    public class AbilitySlotManager : MonoBehaviour
    {
        [SerializeField] private AbilitySlotView _abilitySlotView;

        public event EventHandler AbilitySlotUnbound;

        public void Initialize(UserInputController userInputController)
        {
            _abilitySlotView.Initialize(userInputController);
        }

        public void Bind(Sprite icon)
        {
            _abilitySlotView.SetIconSprite(icon);
            _abilitySlotView.AbilitySlotUnbound += OnAbilitySlotCleared;
        }

        public void Unbind()
        {
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
