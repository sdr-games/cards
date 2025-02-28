using SDRGames.Whist.MusicModule.Managers;
using SDRGames.Whist.NotificationsModule;
using SDRGames.Whist.SoundModule.Managers;
using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.DomainModule
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private MusicGlobalManager _musicGlobalManager;
        [SerializeField] private SoundGlobalManager _soundGlobalManager;
        [SerializeField] private NotificationController _notificationController;
        //temp
        [SerializeField] private CombatSceneInitializer _combatSceneInitializer;

        public static GameInitializer Instance { get; private set; }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_musicGlobalManager), _musicGlobalManager);
            this.CheckFieldValueIsNotNull(nameof(_soundGlobalManager), _soundGlobalManager);
            this.CheckFieldValueIsNotNull(nameof(_notificationController), _notificationController);

            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(Instance);

            _musicGlobalManager.Initialize();
            _soundGlobalManager.Initialize();
            _notificationController.Initialize();
            _combatSceneInitializer.Initialize();
        }
    }
}
