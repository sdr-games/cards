using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class TalentManager : MonoBehaviour
    {
        [field: SerializeField] public TalentView TalentView { get; protected set; }

        protected UserInputController _userInputController;

        public void Initialize(UserInputController userInputController, TalentScriptableObject talentScriptableObject)
        {
            if (TalentView == null)
            {
                Debug.LogError("Talent View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
        }

        protected virtual void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            return;
        }

        private void OnDisable()
        {
            if (_userInputController != null)
            {
                _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
            }
        }
    }
}
