using SDRGames.Whist.HelpersModule.Views;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.SceneManagementModule.Views
{
    public class ProgressBarView : HideableUIView
    {
        [SerializeField] private Image _fillerImage;

        public void FillBar(float percent)
        {
            _fillerImage.fillAmount = percent;
        }
    }
}
