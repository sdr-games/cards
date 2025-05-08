using System.Collections;

using UnityEngine;

namespace SDRGames.Whist.HelpersModule.Views
{
    [RequireComponent(typeof(CanvasGroup))]
    public class HideableUIView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _appearingSpeed = 0;

        public virtual void Show()
        {
            if (_canvasGroup.alpha == 1)
            {
                return;
            }

            if (_appearingSpeed > 0)
            {
                StartCoroutine(ShowSmoothly());
                return;
            }
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public virtual void Hide()
        {
            if(_canvasGroup.alpha == 0)
            {
                return;
            } 

            if (_appearingSpeed > 0)
            {
                StartCoroutine(HideSmoothly());
                return;
            }
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private IEnumerator ShowSmoothly()
        {
            while(_canvasGroup.alpha < 1)
            {
                yield return null;
                _canvasGroup.alpha += _appearingSpeed / 100;
            }
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private IEnumerator HideSmoothly()
        {
            while (_canvasGroup.alpha > 0)
            {
                yield return null;
                _canvasGroup.alpha -= _appearingSpeed / 100;
            }
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
