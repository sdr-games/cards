using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.MusicModule.Managers;
using SDRGames.Whist.NotificationsModule;

using UnityEngine;

namespace SDRGames.Whist.DomainModule
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private MusicGlobalManager _musicGlobalManager;
        [SerializeField] private NotificationController _notificationController;
        //temp
        [SerializeField] private CombatSceneInitializer _combatSceneInitializer;

        public static GameInitializer Instance { get; private set; }

        private void OnEnable()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(Instance);

            _musicGlobalManager.Initialize();
            _notificationController.Initialize();
            _combatSceneInitializer.Initialize();
        }
    }
}
