using System;
using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.Managers
{
    public class MeleeAttackListManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private MeleeAttackScriptableObject[] _meleeAttackScriptableObjects;
        [SerializeField] private MeleeAttackManager _meleeAttackPrefab;
        [SerializeField] private RectTransform _contentRectTransform;

        private List<MeleeAttackManager> _createdManagers;

        public event EventHandler<MeleeAttackClickedEventArgs> MeleeAttackClicked;

        public void Initialize(UserInputController userInputController)
        {
            _createdManagers = new List<MeleeAttackManager>();
            foreach (MeleeAttackScriptableObject meleeAttackScriptableObject in _meleeAttackScriptableObjects)
            {
                MeleeAttackManager meleeAttackManager = Instantiate(_meleeAttackPrefab, _contentRectTransform);
                meleeAttackManager.Initialize(userInputController, meleeAttackScriptableObject);
                meleeAttackManager.MeleeAttackClicked += OnMeleeAttackClicked;
                _createdManagers.Add(meleeAttackManager);
            }
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private void OnMeleeAttackClicked(object sender, MeleeAttackClickedEventArgs e)
        {
            MeleeAttackClicked?.Invoke(this, e);
        }

        private void OnEnable()
        {
            if (_meleeAttackPrefab == null)
            {
                Debug.LogError("Melee Attack Prefab не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_contentRectTransform == null)
            {
                Debug.LogError("Content Rect Transform не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_canvasGroup == null)
            {
                Debug.LogError("Canvas Group не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }

        private void OnDisable()
        {
            foreach(MeleeAttackManager meleeAttackManager in _createdManagers)
            {
                meleeAttackManager.MeleeAttackClicked -= OnMeleeAttackClicked;
            }
        }
    }
}
