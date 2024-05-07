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
        [SerializeField] private MeleeAttackScriptableObject[] _meleeAttackScriptableObjects;
        [SerializeField] private MeleeAttackManager _meleeAttackPrefab;
        [SerializeField] private RectTransform _contentRectTransform;
        [SerializeField] private UserInputController _userInputController;

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

            if (_userInputController == null)
            {
                Debug.LogError("User Input Controller не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            foreach (MeleeAttackScriptableObject meleeAttackScriptableObject in _meleeAttackScriptableObjects)
            {
                MeleeAttackManager meleeAttackManager = Instantiate(_meleeAttackPrefab, _contentRectTransform);
                meleeAttackManager.Initialize(_userInputController, meleeAttackScriptableObject);
            }
        }
    }
}
