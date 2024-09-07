using System;
using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.RestorationModule.Presenters;
using SDRGames.Whist.RestorationModule.ScriptableObjects;
using SDRGames.Whist.RestorationModule.Views;
using SDRGames.Whist.UserInputModule.Controller;


using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.RestorationModule.Managers
{
    public class PotionManager : MonoBehaviour
    {
        [SerializeField] private PotionView _potionView;

        private PotionScriptableObject _potionScriptableObject;
        private UserInputController _userInputController;

        public event EventHandler<PotionClickedEventArgs> PotionClicked;

        public void Initialize(UserInputController userInputController, PotionScriptableObject meleeAttackScriptableObject)
        {
            _potionScriptableObject = meleeAttackScriptableObject;

            new PotionPresenter(_potionScriptableObject, _potionView);

            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                PotionClicked?.Invoke(this, new PotionClickedEventArgs(_potionScriptableObject));
            }
        }

        private void OnEnable()
        {
            if (_potionView == null)
            {
                Debug.LogError("Potion View не был назначен");
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
