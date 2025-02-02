using System;

using SDRGames.Whist.MeleeCombatModule.Models;
using SDRGames.Whist.MeleeCombatModule.Presenters;
using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;
using SDRGames.Whist.MeleeCombatModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.Managers
{
    public class MeleeAttackManager : MonoBehaviour
    {
        [SerializeField] private MeleeAttackView _meleeAttackView;

        private MeleeAttack _meleeAttack;
        private UserInputController _userInputController;

        public event EventHandler<MeleeAttackClickedEventArgs> MeleeAttackClicked;

        public void Initialize(UserInputController userInputController, MeleeAttackScriptableObject meleeAttackScriptableObject)
        {
            _meleeAttack = new MeleeAttack(meleeAttackScriptableObject);

            new MeleeAttackPresenter(_meleeAttack, _meleeAttackView);

            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
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
