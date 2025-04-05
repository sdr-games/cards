using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.SceneManagementModule.Managers;
using SDRGames.Whist.SceneManagementModule.Models;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SDRGames.Whist.DomainModule
{
    public class EndBattlePanelView : HideableUIView
    {
        [SerializeField] private TextMeshProUGUI _endHeaderTMP;
        [SerializeField] private TextMeshProUGUI _endDescriptionTMP;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TextMeshProUGUI _restartButtonTextTMP;
        [SerializeField] private Button _endButton;
        [SerializeField] private TextMeshProUGUI _endButtonTextTMP;

        [SerializeField] private LocalizedString _victoryHeader;
        [SerializeField] private LocalizedString _victoryDescription;
        [SerializeField] private LocalizedString _victoryEndButton;

        [SerializeField] private LocalizedString _defeatHeader;
        [SerializeField] private LocalizedString _defeatDescription;
        [SerializeField] private LocalizedString _defeatRestartButton;
        [SerializeField] private LocalizedString _defeatEndButton;

        public void ShowVictory()
        {
            _endHeaderTMP.text = _victoryHeader.GetLocalizedText();
            _endDescriptionTMP.text = _victoryDescription.GetLocalizedText();
            _endButtonTextTMP.text = _victoryEndButton.GetLocalizedText();

            _endButton.onClick.RemoveAllListeners();
            _endButton.onClick.AddListener(() => EndVictoriousGame());
            _restartButton.onClick.RemoveAllListeners();
            _restartButton.gameObject.SetActive(false);
            Show();
        }

        public void ShowDefeat()
        {
            _endHeaderTMP.text = _defeatHeader.GetLocalizedText();
            _endDescriptionTMP.text = _defeatDescription.GetLocalizedText();
            _restartButtonTextTMP.text = _defeatRestartButton.GetLocalizedText();
            _endButtonTextTMP.text = _defeatEndButton.GetLocalizedText();

            _restartButton.onClick.RemoveAllListeners();
            _restartButton.onClick.AddListener(() => RestartGame());
            _endButton.onClick.RemoveAllListeners();
            _endButton.onClick.AddListener(() => LeaveLostGame());
            Show();
        }

        private void EndVictoriousGame()
        {
            SceneManager.LoadScene("LocationMapScene");
        }

        private void RestartGame()
        {
            SceneData currentSceneData = ScenesManager.GetCurrentSceneData();
            ScenesManager.Instance.LoadScene(currentSceneData);
        }

        private void LeaveLostGame()
        {
            SceneManager.LoadScene("LocationMapScene");
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveAllListeners();
            _endButton.onClick.RemoveAllListeners();
        }
    }
}
