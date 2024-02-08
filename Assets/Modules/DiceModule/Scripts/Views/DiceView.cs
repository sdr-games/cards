using TMPro;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Islands.DiceModule.Views
{
    public class DiceView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _diceNameText;
        [SerializeField] private TextMeshProUGUI _diceValueText;

        public void Initialize(string name, string value)
        {
            _diceNameText.text = name;
            SetDiceText(value);
        }

        public void SetDiceText(string value)
        {
            _diceValueText.text = value;
        }

        #region MonoBehaviour methods
        private void OnEnable()
        {
            if (_diceNameText == null)
            {
                Debug.LogError("Dice Name TextMeshPro не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_diceValueText == null)
            {
                Debug.LogError("Dice Value TextMeshPro не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
        #endregion
    }
}
