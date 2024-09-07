using SDRGames.Whist.RestorationModule.ScriptableObjects;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.RestorationModule.Views
{
    public class PotionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _effectText;
        [SerializeField] private Image _iconImage;

        public void Initialize(PotionScriptableObject potionScriptableObject)
        {
            _nameText.text = potionScriptableObject.Name.GetLocalizedText();
            _effectText.text = potionScriptableObject.GetEffectDescription();
            _iconImage.sprite = potionScriptableObject.Icon;
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

            if (_effectText == null)
            {
                Debug.LogError("Effect Text не был назначен");
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
        }
    }
}
