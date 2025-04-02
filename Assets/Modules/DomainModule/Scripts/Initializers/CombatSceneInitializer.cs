using System.Collections.Generic;

using SDRGames.Whist.EnemyBehaviorModule.Managers;
using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.DomainModule.Managers;
using SDRGames.Whist.FloatingTextModule.Managers;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;
using SDRGames.Whist.TurnSwitchModule.Managers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;
using SDRGames.Whist.NotificationsModule;

namespace SDRGames.Whist.DomainModule
{
    public class CombatSceneInitializer : MonoBehaviour
    {
        [SerializeField] private CombatSceneManager _combatSceneManager;
        [SerializeField] private NotificationController _notificationController;
        [SerializeField] private TurnsQueueManager _turnsQueueManager;

        [Header("UI")][SerializeField] private CombatUIManager _combatUIManager;
        [SerializeField] private FloatingTextManager _floatingTextManager;
        [Header("PLAYER")][SerializeField] private PlayerCombatManager _playerCombatManager;
        [Header("ENEMIES")][SerializeField] private EnemyBehaviorManager[] _enemyBehaviorManagers;

        [Header("SCALINGS")][SerializeField] private CharacterParametersScalingScriptableObject _characterParametersScalingSettings;
        [SerializeField] private CardsScalingScriptableObject _cardsScalingScriptableObject;
        [SerializeField] private MeleeAttacksScalingScriptableObject _meleeAttacksScalingScriptableObject;

        private List<EnemyCombatManager> _enemyCombatManagers;

        public void OnEnable()
        {
            _notificationController.Initialize();

            _characterParametersScalingSettings.Initialize();
            _cardsScalingScriptableObject.Initialize();
            _meleeAttacksScalingScriptableObject.Initialize();
            _floatingTextManager.Initialize();

            _playerCombatManager.Initialize();

            _enemyCombatManagers = new List<EnemyCombatManager>();
            List<CharacterParamsModel> characterInfoScriptableObjects = new List<CharacterParamsModel>();
            characterInfoScriptableObjects.Add(_playerCombatManager.GetParams());
            foreach (EnemyBehaviorManager enemyBehaviorManager in _enemyBehaviorManagers)
            {
                enemyBehaviorManager.Initialize(_playerCombatManager, UserInputController.Instance);
                _enemyCombatManagers.Add(enemyBehaviorManager.EnemyCombatManager);
                characterInfoScriptableObjects.Add(enemyBehaviorManager.EnemyCombatManager.GetParams());
            }
            _combatUIManager.Initialize(UserInputController.Instance);
            _turnsQueueManager.Initialize(characterInfoScriptableObjects);

            _combatSceneManager.Initialize(UserInputController.Instance, _turnsQueueManager, _combatUIManager, _playerCombatManager, _enemyBehaviorManagers, _enemyCombatManagers);

            this.CheckFieldValueIsNotNull(nameof(_combatSceneManager), _combatSceneManager);
            this.CheckFieldValueIsNotNull(nameof(_notificationController), _notificationController);
            this.CheckFieldValueIsNotNull(nameof(_turnsQueueManager), _turnsQueueManager);
            this.CheckFieldValueIsNotNull(nameof(_characterParametersScalingSettings), _characterParametersScalingSettings);
            this.CheckFieldValueIsNotNull(nameof(_cardsScalingScriptableObject), _cardsScalingScriptableObject);
            this.CheckFieldValueIsNotNull(nameof(_meleeAttacksScalingScriptableObject), _meleeAttacksScalingScriptableObject);
            this.CheckFieldValueIsNotNull(nameof(_combatUIManager), _combatUIManager);
            this.CheckFieldValueIsNotNull(nameof(_floatingTextManager), _floatingTextManager);
            this.CheckFieldValueIsNotNull(nameof(_playerCombatManager), _playerCombatManager);
            this.CheckFieldValueIsNotNull(nameof(_enemyBehaviorManagers), _enemyBehaviorManagers);
        }

        private void OnValidate()
        {
            _characterParametersScalingSettings.Initialize();
            _cardsScalingScriptableObject.Initialize();
            _meleeAttacksScalingScriptableObject.Initialize();
        }
    }
}
