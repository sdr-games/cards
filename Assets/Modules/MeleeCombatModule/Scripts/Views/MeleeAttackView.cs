using System;

using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDRGames.Whist.MeleeCombatModule.Views
{
    public class MeleeAttackView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private CanvasGroup _tooltipCanvasGroup;

        public void Initialize(UserInputController userInputController, MeleeAttackScriptableObject meleeAttackScriptableObject)
        {
            _nameText.text = meleeAttackScriptableObject.Name.GetLocalizedText();
            _descriptionText.text = meleeAttackScriptableObject.Description.GetLocalizedText();
            _costText.text = $"Cost :{meleeAttackScriptableObject.Cost}";
            _iconImage.sprite = meleeAttackScriptableObject.Icon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tooltipCanvasGroup.alpha = 1;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltipCanvasGroup.alpha = 0;
        }

        private void OnEnable()
        {
            if (_nameText == null)
            {
                Debug.LogError("Name Text не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_descriptionText == null)
            {
                Debug.LogError("Description Text не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_costText == null)
            {
                Debug.LogError("Cost Text не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_iconImage == null)
            {
                Debug.LogError("Icon Image не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_tooltipCanvasGroup == null)
            {
                Debug.LogError("Tooltip Canvas Group не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }

    }
}
