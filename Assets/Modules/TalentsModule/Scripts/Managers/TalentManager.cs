using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule
{
    public class TalentManager : MonoBehaviour
    {
        [SerializeField] protected TalentView _talentView;

        protected UserInputController _userInputController;

        public void Initialize(UserInputController userInputController)
        {
            if (_talentView == null)
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
    }
}
