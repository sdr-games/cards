using System;

using SDRGames.Whist.MeleeCombatModule.Models;
using SDRGames.Whist.MeleeCombatModule.Presenters;
using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;
using SDRGames.Whist.MeleeCombatModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SDRGames.Whist.MeleeCombatModule.Managers
{
    public class MeleeAttackManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private MeleeAttackView _meleeAttackView;

        private MeleeAttack _meleeAttack;
        private UserInputController _userInputController;

        public event EventHandler<MeleeAttackClickedEventArgs> MeleeAttackClicked;
        public event EventHandler<MeleeAttackClickedEventArgs> MeleeAttackPointerEnter;
        public event EventHandler<MeleeAttackClickedEventArgs> MeleeAttackPointerExit;

        public void Initialize(UserInputController userInputController, MeleeAttackScriptableObject meleeAttackScriptableObject)
        {
            _meleeAttack = new MeleeAttack(meleeAttackScriptableObject);

            new MeleeAttackPresenter(_meleeAttack, _meleeAttackView);

            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            MeleeAttackPointerEnter?.Invoke(this, new MeleeAttackClickedEventArgs(_meleeAttack));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MeleeAttackPointerExit?.Invoke(this, new MeleeAttackClickedEventArgs(_meleeAttack));
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                MeleeAttackClicked?.Invoke(this, new MeleeAttackClickedEventArgs(_meleeAttack));
            }
        }

        private void OnEnable()
        {
            if (_meleeAttackView == null)
            {
                Debug.LogError("Melee Attack View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }

        private void OnDisable()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }
    }
}
