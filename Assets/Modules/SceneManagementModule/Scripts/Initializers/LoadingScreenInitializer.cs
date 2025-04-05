using System.Collections;

using SDRGames.Whist.MusicModule.Managers;
using SDRGames.Whist.MusicModule.ScriptableObjects;
using SDRGames.Whist.SceneManagementModule.Views;

using UnityEngine;

namespace SDRGames.Whist.SceneManagementModule.Initializers
{
    public class LoadingScreenInitializer : SceneInitializer
    {
        [SerializeField] private LoadingScreenUIView _loadingScreenUIView;

        public override IEnumerator InitializeCoroutine()
        {
            yield return null;
            if (_sceneInitializationReferenceParameters["backgroundMusic"])
            {
                MusicGlobalManager.Play((MusicClipScriptableObject)_sceneInitializationReferenceParameters["backgroundMusic"]);
            }
            _loadingScreenUIView.Initialize();
            _loadingScreenUIView.SetHeaderText(_sceneInitializationStringParameters["headerText"].GetLocalizedText());
            _loadingScreenUIView.SetTooltipText(_sceneInitializationStringParameters["tooltipText"].GetLocalizedText());
            _loadingScreenUIView.SetBackgroundSprite((Sprite)_sceneInitializationReferenceParameters["backgroundSprite"]);
            yield return null;
        }

        public LoadingScreenUIView GetLoadingScreenUIView()
        {
            return _loadingScreenUIView;
        }

        public override void Run()
        {
            _loadingScreenUIView.ShowProgressBar();
            _loadingScreenUIView.gameObject.SetActive(true);
        }
    }
}
