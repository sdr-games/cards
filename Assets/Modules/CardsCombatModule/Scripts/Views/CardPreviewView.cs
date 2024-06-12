using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CardsCombatModule.Views
{
    public class CardPreviewView : MonoBehaviour
    {
        [SerializeField] private Image _illustrationImage;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        public void Initialize(Sprite illustrationSprite, string descriptionText)
        {
            _illustrationImage.sprite = illustrationSprite;
            _descriptionText.text = descriptionText;
        }

        private void OnEnable()
        {
            if (_illustrationImage == null)
            {
                Debug.LogError("Illustration Image не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_descriptionText == null)
            {
                Debug.LogError("Description Text не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
