using UnityEngine;

namespace SDRGames.Whist.HelpersModule.Views
{
    [RequireComponent(typeof(CanvasGroup))]
    public class HideableUIView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public virtual void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public virtual void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
