using System;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesQueueModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.Managers
{
    public class AbilitySlotManager : MonoBehaviour
    {
        [SerializeField] private AbilitySlotView _abilitySlotView;
        private UserInputController _userInputController;
        
        [field:SerializeField] public Ability Ability { get; private set; }

        public event EventHandler<AbilityQueueClearedEventArgs> AbilitySlotUnbindManually;

        public void Initialize(UserInputController userInputController)
        {
            _abilitySlotView.Initialize();
            _userInputController = userInputController;
        }

        public void Bind(Ability ability)
        {
            Ability = ability;
            _abilitySlotView.SetIconSprite(ability.Icon);
            if(ability.Icon != null)
            {
                _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            }
        }

        public void Unbind()
        {
            Ability = null;
            _abilitySlotView.SetIconSprite();
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject && _abilitySlotView.interactable)
            {
                float cost = Ability.Cost;
                Unbind();
                AbilitySlotUnbindManually?.Invoke(this, new AbilityQueueClearedEventArgs(cost));
            }
        }

        private void OnDisable()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }
    }
}
