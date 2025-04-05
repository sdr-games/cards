using System.Collections;

using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.MusicModule.Managers;
using SDRGames.Whist.MusicModule.ScriptableObjects;
using SDRGames.Whist.SceneManagementModule.Views;

using UnityEngine;

namespace SDRGames.Whist.SceneManagementModule.Initializers
{
    public class LoadingScreenInitializer : SceneInitializer
    {
        [SerializeField] private LoadingScreenUIView _loadingScreenUIView;
        [SerializeField] private LocalizedString[] _tooltips;

        public override IEnumerator InitializeCoroutine()
        {
            yield return null;
            if (_sceneInitializationReferenceParameters["backgroundMusic"])
            {
                MusicGlobalManager.Play((MusicClipScriptableObject)_sceneInitializationReferenceParameters["backgroundMusic"]);
            }
            _loadingScreenUIView.Initialize();
            _loadingScreenUIView.SetHeaderText(_sceneInitializationStringParameters["headerText"].GetLocalizedText());
            _loadingScreenUIView.SetTooltipText(GetTooltipString());
            _loadingScreenUIView.SetBackgroundSprite((Sprite)_sceneInitializationReferenceParameters["backgroundSprite"]);
        }

        public LoadingScreenUIView GetLoadingScreenUIView()
        {
            return _loadingScreenUIView;
        }

        public override void Run()
        {
            _loadingScreenUIView.Show();
        }

        private string GetTooltipString()
        {
            try
            {
                return _sceneInitializationStringParameters["tooltipText"].GetLocalizedText();
            }
            catch
            {
                return _tooltips[Random.Range(0, _tooltips.Length)].GetLocalizedText();
            }
        }
    }
}
