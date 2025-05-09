using System;

using SDRGames.Whist.CharacterCombatModule.Models;
using SDRGames.Whist.RestorationModule.Models;
using SDRGames.Whist.RestorationModule.Presenters;
using SDRGames.Whist.RestorationModule.ScriptableObjects;
using SDRGames.Whist.RestorationModule.Views;
using SDRGames.Whist.UserInputModule.Controller;


using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SDRGames.Whist.RestorationModule.Managers
{
    public class PotionManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private PotionView _potionView;

        private Potion _potion;
        private UserInputController _userInputController;

        public event EventHandler<PotionClickedEventArgs> PotionClicked;
        public event EventHandler<PotionClickedEventArgs> PotionPointerEnter;
        public event EventHandler<PotionClickedEventArgs> PotionPointerExit;

        public void Initialize(UserInputController userInputController, PotionScriptableObject potionScriptableObject, CharacterParamsModel characterParamsModel)
        {
            _potion = new Potion(potionScriptableObject);

            new PotionPresenter(_potion, _potionView, characterParamsModel);

            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PotionPointerEnter?.Invoke(this, new PotionClickedEventArgs(_potion));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PotionPointerExit?.Invoke(this, new PotionClickedEventArgs(_potion));
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                PotionClicked?.Invoke(this, new PotionClickedEventArgs(_potion));
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
