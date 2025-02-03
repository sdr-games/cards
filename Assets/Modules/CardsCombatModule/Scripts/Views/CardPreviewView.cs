using SDRGames.Whist.LocalizationModule.Models;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CardsCombatModule.Views
{
    public class CardPreviewView : MonoBehaviour
    {
        [SerializeField] protected Image _illustrationImage;
        [SerializeField] protected TextMeshProUGUI _descriptionText;

        public virtual void Initialize(LocalizedString nameText, LocalizedString descriptionText, Sprite illustrationSprite)
        {
            _illustrationImage.sprite = illustrationSprite;
            _descriptionText.text = descriptionText.GetLocalizedText();
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
