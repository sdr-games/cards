using System;
using System.Collections.Generic;

using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.RestorationModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.RestorationModule.Managers
{
    public class PotionListManager : HideableUIView
    {
        [SerializeField] private PotionManager _potionPrefab; 
        [SerializeField] private GridLayoutGroup _buttonsGridLayoutGroup;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private PotionScriptableObject[] _potionScriptableObjects;

        private List<PotionManager> _createdManagers;

        public event EventHandler<PotionClickedEventArgs> PotionClicked;

        public void Initialize(UserInputController userInputController)
        {
            _createdManagers = new List<PotionManager>();
            foreach (PotionScriptableObject potionScriptableObject in _potionScriptableObjects)
            {
                PotionManager potionManager = Instantiate(_potionPrefab, _buttonsGridLayoutGroup.transform);
                potionManager.Initialize(userInputController, potionScriptableObject);
                potionManager.PotionPointerEnter += OnPotionPointerEnter;
                potionManager.PotionPointerExit += OnPotionPointerExit;
                potionManager.PotionClicked += OnPotionClicked;
                _createdManagers.Add(potionManager);
            }
        }

        private void OnPotionPointerEnter(object sender, PotionClickedEventArgs e)
        {
            _descriptionText.text = e.Potion.GetLocalizedDescription();
        }

        private void OnPotionPointerExit(object sender, PotionClickedEventArgs e)
        {
            _descriptionText.text = "";
        }

        private void OnPotionClicked(object sender, PotionClickedEventArgs e)
        {
            PotionClicked?.Invoke(this, e);
        }

        private void OnEnable()
        {
            if (_potionPrefab == null)
            {
                Debug.LogError("Potion Prefab не был назначен");
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
            foreach (PotionManager potionManager in _createdManagers)
            {
                potionManager.PotionClicked -= OnPotionClicked;
            }
        }
    }
}
