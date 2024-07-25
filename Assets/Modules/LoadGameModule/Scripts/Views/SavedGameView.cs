using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.LoadGameModule.Views
{
    public class SavedGameView : MonoBehaviour
    {
        [SerializeField] private Image _thumbnailImage;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        public void Initialize(Sprite thumbnailSprite, string description)
        {
            _thumbnailImage.sprite = thumbnailSprite;
            _descriptionText.text = description;
        }

        private void OnEnable()
        {
            if (_thumbnailImage == null)
            {
                Debug.LogError("Thumbnail Image не был назначен");
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
        }
    }
}
