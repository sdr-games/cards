using SDRGames.Whist.MusicModule.Managers;
using SDRGames.Whist.SoundModule.Managers;
using SDRGames.Whist.HelpersModule;

using UnityEngine;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.SceneManagementModule.Managers;
using SDRGames.Whist.SceneManagementModule.Models;
using System.Collections;

namespace SDRGames.Whist.DomainModule
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private ScenesManager _scenesManager;
        [SerializeField] private MusicGlobalManager _musicGlobalManager;
        [SerializeField] private SoundGlobalManager _soundGlobalManager;
        [SerializeField] private UserInputController _userInputController;
        [SerializeField] private SceneData _entryScene;

        public static GameInitializer Instance { get; private set; }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_scenesManager), _scenesManager);
            this.CheckFieldValueIsNotNull(nameof(_musicGlobalManager), _musicGlobalManager);
            this.CheckFieldValueIsNotNull(nameof(_soundGlobalManager), _soundGlobalManager);
            this.CheckFieldValueIsNotNull(nameof(_userInputController), _userInputController);

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(Instance);

            _scenesManager.Initialize();
            _musicGlobalManager.Initialize();
            _soundGlobalManager.Initialize();
            _userInputController.Initialize();
        }

        private IEnumerator Start()
        {
            yield return null;
            ScenesManager.Instance.LoadScene(_entryScene);
        }
    }
}
