using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.DialogueSystem.Views
{
    public class SpeechLinearView : MonoBehaviour
    {
        [SerializeField] private Image _leftAlignedCharacterPortrait;
        [SerializeField] private Image _rightAlignedCharacterPortrait;
        [SerializeField] private TextMeshProUGUI _characterName;
        [SerializeField] private TextMeshProUGUI _speech;

        [SerializeField] private bool _rightAligned = false;
        private Image _currentCharacterPortrair;

        private void Start()
        {
            Initialize(null, "Valior", "Hello!", _rightAligned);
        }

        public void Initialize(Sprite characterPortrairSprite, string characterName, string speechText, bool rightAligned = false)
        {
            _leftAlignedCharacterPortrait.gameObject.SetActive(false);
            _rightAlignedCharacterPortrait.gameObject.SetActive(false);

            _rightAligned = rightAligned;

            _currentCharacterPortrair = _rightAligned ? _rightAlignedCharacterPortrait : _leftAlignedCharacterPortrait;
            _currentCharacterPortrair.sprite = characterPortrairSprite;
            _currentCharacterPortrair.gameObject.SetActive(true);

            _characterName.text = characterName;
            _characterName.alignment = _rightAligned ? TextAlignmentOptions.TopRight : TextAlignmentOptions.TopLeft;

            _speech.text = speechText;
            _speech.alignment = _rightAligned ? TextAlignmentOptions.TopRight : TextAlignmentOptions.TopLeft;
        }
    }
}
