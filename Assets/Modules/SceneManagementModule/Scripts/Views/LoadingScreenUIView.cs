using SDRGames.Whist.HelpersModule.Views;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.SceneManagementModule.Views
{
    [RequireComponent(typeof(Canvas))]
    public class LoadingScreenUIView : HideableUIView
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private ProgressBarView _progressBar;
        [SerializeField] private TextMeshProUGUI _headerTMP;
        [SerializeField] private TextMeshProUGUI _tooltipTMP;

        public void Initialize()
        {
            FillBar(0);
        }

        public override void Show()
        {
            base.Show();
            _progressBar.Show();
        }

        public void FillBar(float percent)
        {
            _progressBar.FillBar(percent);
        }

        public void SetHeaderText(string text)
        {
            _headerTMP.text = text;
        }

        public void SetTooltipText(string text)
        {
            _tooltipTMP.text = text;
        }

        public void SetBackgroundSprite(Sprite sprite)
        {
            _backgroundImage.sprite = sprite;
        }
    }
}
