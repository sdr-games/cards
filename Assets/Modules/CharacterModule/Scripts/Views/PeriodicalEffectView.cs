using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CharacterModule.Views
{
    public class PeriodicalEffectView : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _fillerImage;
        [SerializeField] private TextMeshProUGUI _roundsText;

        private float _totalDuration;

        public void Initialize(Sprite backgroundIcon, int rounds)
        {
            _backgroundImage.sprite = backgroundIcon;
            _roundsText.text = rounds.ToString();

            _totalDuration = rounds;
        }

        public void UpdateDuration(int duration)
        {
            _roundsText.text = duration.ToString();
            if(duration >= _totalDuration)
            {
                _totalDuration = duration;
            }
            _fillerImage.fillAmount = 1 - duration / _totalDuration;
        }

        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}
