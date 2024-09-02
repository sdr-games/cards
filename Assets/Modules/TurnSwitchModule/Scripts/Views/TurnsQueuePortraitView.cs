using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.TurnSwitchModule.Views
{
    public class TurnsQueuePortraitView : MonoBehaviour
    {
        [SerializeField] private Image _portraitImage;

        public void Initialize(Sprite portraitSprite)
        {
            _portraitImage.sprite = portraitSprite;
        }

        private void OnEnable()
        {
            if (_portraitImage == null)
            {
                Debug.LogError("Portrait Image не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }
    }
}
