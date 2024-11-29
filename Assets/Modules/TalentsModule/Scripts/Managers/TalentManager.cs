using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class TalentManager : MonoBehaviour
    {
        private Talent _talent;
        private UserInputController _userInputController;

        [SerializeField] private RectTransform _rectTransform;

        [field: SerializeField] public TalentView TalentView { get; protected set; }


        public void Initialize(UserInputController userInputController, Talent talent)
        {
            if (TalentView == null)
            {
                Debug.LogError("Talent View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            _talent = talent;

            TalentView.BlockChanged += OnBlockChanged;

            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI += OnRightMouseButtonClickedOnUI;
        }

        public Vector2 GetSize()
        {
            return _rectTransform.rect.size;
        }

        private void OnBlockChanged(object sender, System.EventArgs e)
        {
            _talent.ResetCurrentPoints();
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if(e.GameObject != gameObject || _talent.CurrentPoints == _talent.TotalCost || TalentView.IsBlocked)
            {
                return;
            }

            _talent.IncreaseCurrentPoints();
            if (_talent.CurrentPoints == _talent.TotalCost)
            {
                TalentView.ChangeActive();
            }
        }

        private void OnRightMouseButtonClickedOnUI(object sender, RightMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject != gameObject || _talent.CurrentPoints == 0 || TalentView.IsBlocked)
            {
                return;
            }

            _talent.DecreaseCurrentPoints();
            if (_talent.CurrentPoints < _talent.TotalCost)
            {
                TalentView.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (_userInputController != null)
            {
                _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
                _userInputController.RightMouseButtonClickedOnUI -= OnRightMouseButtonClickedOnUI;
            }
        }
    }
}
