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
        private UserInputController _userInputController;
        
        [field:SerializeField] public AbilityScriptableObject AbilityScriptableObject { get; private set; }

        public void Initialize(UserInputController userInputController)
        {
            _abilitySlotView.Initialize();
            _userInputController = userInputController;
        }

        public void Bind(AbilityScriptableObject abilityScriptableObject)
        {
            AbilityScriptableObject = abilityScriptableObject;
            _abilitySlotView.SetIconSprite(abilityScriptableObject.Icon);
            if(abilityScriptableObject.Icon != null)
            {
                _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            }
        }

        public void Unbind()
        {
            AbilityScriptableObject = null;
            _abilitySlotView.SetIconSprite();
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject && _abilitySlotView.interactable)
            {
                Unbind();
            }
        }

        private void OnDisable()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }
    }
}
