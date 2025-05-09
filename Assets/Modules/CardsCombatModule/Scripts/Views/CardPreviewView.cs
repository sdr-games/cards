using SDRGames.Whist.LocalizationModule.Models;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CardsCombatModule.Views
{
    public class CardPreviewView : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected TextMeshProUGUI _descriptionText;
        [SerializeField] protected Image _illustrationImage;

        public virtual void Initialize(LocalizedString nameText, string descriptionText, Sprite illustrationSprite)
        {
            _nameText.text = nameText.GetLocalizedText();
            _descriptionText.text = descriptionText;
            _illustrationImage.sprite = illustrationSprite;
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
