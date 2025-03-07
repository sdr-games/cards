using System;
using System.Collections.Generic;

using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SDRGames.Whist.MeleeCombatModule.Managers
{
    public class MeleeAttackListManager : HideableUIView
    {
        [SerializeField] private MeleeAttackManager _meleeAttackPrefab;
        [SerializeField] private GridLayoutGroup _buttonsGridLayoutGroup;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private MeleeAttackScriptableObject[] _meleeAttackScriptableObjects;

        private List<MeleeAttackManager> _createdManagers;

        public event EventHandler<MeleeAttackClickedEventArgs> MeleeAttackClicked;

        public void Initialize(UserInputController userInputController)
        {
            _createdManagers = new List<MeleeAttackManager>();
            foreach (MeleeAttackScriptableObject meleeAttackScriptableObject in _meleeAttackScriptableObjects)
            {
                MeleeAttackManager meleeAttackManager = Instantiate(_meleeAttackPrefab, _buttonsGridLayoutGroup.transform);
                meleeAttackManager.Initialize(userInputController, meleeAttackScriptableObject);
                meleeAttackManager.MeleeAttackPointerEnter += OnMeleeAttackPointerEnter;
                meleeAttackManager.MeleeAttackPointerExit += OnMeleeAttackPointerExit;
                meleeAttackManager.MeleeAttackClicked += OnMeleeAttackClicked;
                _createdManagers.Add(meleeAttackManager);
            }
        }

        private void OnMeleeAttackPointerEnter(object sender, MeleeAttackClickedEventArgs e)
        {
            _descriptionText.text = e.MeleeAttack.GetLocalizedDescription();
            _costText.text = e.MeleeAttack.Cost.ToString();
        }

        private void OnMeleeAttackPointerExit(object sender, MeleeAttackClickedEventArgs e)
        {
            _descriptionText.text = "";
            _costText.text = "";
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

            if (_buttonsGridLayoutGroup == null)
            {
                Debug.LogError("Content Rect Transform не был назначен");
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
