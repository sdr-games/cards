using System.Collections.Generic;

using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.SceneManagementModule.Initializers;
using SDRGames.Whist.SceneManagementModule.Models;
using SDRGames.Whist.SceneManagementModule.Views;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SDRGames.Whist.SceneManagementModule.Managers
{
    public class ScenesManager : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<ScenesNames, SceneData> _scenesData;

        private LoadingScreenInitializer _loadingScreenInitializer;
        private LoadingScreenUIView _loadingScreenUIView;
        private SceneData _currentSceneData;

        public enum ScenesNames { MainMenu, LocationMap, Combat, Talents, PlayerParameters }

        public static ScenesManager Instance { get; private set; }

        public static SceneData GetCurrentSceneData()
        {
            return Instance._currentSceneData;
        }

        public void Initialize()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        public static SceneData GetSceneData(ScenesNames sceneName)
        {
            SceneData sceneData = Instance._scenesData[sceneName];
            sceneData.SetName($"{sceneName}Scene");
            return sceneData;
        }

        public void LoadScene(SceneData sceneData)
        {
            StartCoroutine(LoadSceneAsync(sceneData));
        }

        private IEnumerator<object> LoadSceneAsync(SceneData sceneData)
        {
            yield return SceneManager.LoadSceneAsync("LoadingScreenScene");

            _loadingScreenInitializer = FindFirstObjectByType<LoadingScreenInitializer>();
            _loadingScreenInitializer.AddInitializationParameter("headerText", sceneData.LoadingScreenBackgroundHeader);
            _loadingScreenInitializer.AddInitializationParameter("tooltipText", sceneData.LoadingScreenBackgroundTooltip);
            _loadingScreenInitializer.AddInitializationParameter("backgroundSprite", sceneData.LoadingScreenBackgroundSprite);
            _loadingScreenInitializer.AddInitializationParameter("backgroundMusic", sceneData.LoadingScreenMusic);
            yield return _loadingScreenInitializer.InitializeCoroutine();
            _loadingScreenInitializer.Run();
            _loadingScreenUIView = _loadingScreenInitializer.GetLoadingScreenUIView();
            Destroy(_loadingScreenInitializer.gameObject);

            yield return null;

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneData.SceneName, LoadSceneMode.Additive);
            while (!operation.isDone)
            {
                float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
                _loadingScreenUIView.FillBar(progressValue);
                yield return null;
            }

            _loadingScreenUIView.FillBar(0);

            SceneInitializer sceneInitializer = FindFirstObjectByType<SceneInitializer>();
            sceneInitializer.SetInitializationParameters(sceneData.StringParameters);
            sceneInitializer.SetInitializationParameters(sceneData.NumericParameters);
            sceneInitializer.SetInitializationParameters(sceneData.ReferenceParameters);
            sceneInitializer.PartInitialized += OnPartInitialized;
            yield return sceneInitializer.InitializeCoroutine();
            yield return SceneManager.UnloadSceneAsync("LoadingScreenScene");
            sceneInitializer.PartInitialized -= OnPartInitialized;
            sceneInitializer.Run();
            _currentSceneData = sceneData;
        }

        private void OnPartInitialized(object sender, PartInitializedEventArgs e)
        {
            _loadingScreenUIView.FillBar(e.CurrentPercent);
        }
    }
}
