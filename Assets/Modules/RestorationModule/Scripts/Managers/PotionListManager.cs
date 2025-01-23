using System;
using System.Collections.Generic;

using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.RestorationModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.RestorationModule.Managers
{
    public class PotionListManager : HideableUIView
    {
        [SerializeField] private PotionScriptableObject[] _potionScriptableObjects;
        [SerializeField] private PotionManager _potionPrefab;
        [SerializeField] private RectTransform _contentRectTransform;

        private List<PotionManager> _createdManagers;

        public event EventHandler<PotionClickedEventArgs> PotionClicked;

        public void Initialize(UserInputController userInputController)
        {
            _createdManagers = new List<PotionManager>();
            foreach (PotionScriptableObject potionScriptableObject in _potionScriptableObjects)
            {
                PotionManager PotionManager = Instantiate(_potionPrefab, _contentRectTransform);
                PotionManager.Initialize(userInputController, potionScriptableObject);
                PotionManager.PotionClicked += OnPotionClicked;
                _createdManagers.Add(PotionManager);
            }
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

            if (_contentRectTransform == null)
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
