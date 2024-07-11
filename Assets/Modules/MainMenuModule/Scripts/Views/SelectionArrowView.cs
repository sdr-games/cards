using UnityEngine;

namespace SDRGames.Whist.MainMenuModule.Views
{
    public class SelectionArrowView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        
        public void SetWidth(float width)
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
    }
}
