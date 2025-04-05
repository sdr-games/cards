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
using SDRGames.Whist.NotificationsModule;
using SDRGames.Whist.SceneManagementModule.Initializers;

using UnityEngine;
using System;
using System.Collections;
using SDRGames.Whist.SceneManagementModule.Models;

namespace SDRGames.Whist.DomainModule
{
    public class CombatSceneInitializer : SceneInitializer
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
        private SceneData _sceneCurrentData;

        public override IEnumerator InitializeCoroutine()
        {
            _totalWeight = 13.5f + _enemyBehaviorManagers.Length * 0.5f;

            yield return InitializePart(() => _notificationController.Initialize(), 0.5f);
            yield return InitializePart(() => _characterParametersScalingSettings.Initialize(), 1f);
            yield return InitializePart(() => _cardsScalingScriptableObject.Initialize(), 1f);
            yield return InitializePart(() => _meleeAttacksScalingScriptableObject.Initialize(), 1f);
            yield return InitializePart(() => _floatingTextManager.Initialize(), 1f);
            yield return InitializePart(() => _playerCombatManager.Initialize(), 1f);

            _enemyCombatManagers = new List<EnemyCombatManager>();
            List<CharacterParamsModel> characterInfoScriptableObjects = new List<CharacterParamsModel>();
            characterInfoScriptableObjects.Add(_playerCombatManager.GetParams());
            foreach (EnemyBehaviorManager enemyBehaviorManager in _enemyBehaviorManagers)
            {
                yield return InitializePart(() => enemyBehaviorManager.Initialize(_playerCombatManager, UserInputController.Instance), 0.5f);
                _enemyCombatManagers.Add(enemyBehaviorManager.EnemyCombatManager);
                characterInfoScriptableObjects.Add(enemyBehaviorManager.EnemyCombatManager.GetParams());
            }
            yield return InitializePart(() => _combatUIManager.Initialize(UserInputController.Instance), 1f);
            yield return InitializePart(() => _turnsQueueManager.Initialize(characterInfoScriptableObjects), 1f);

            yield return InitializePart(() => _combatSceneManager.Initialize(_turnsQueueManager, _combatUIManager, _playerCombatManager, _enemyBehaviorManagers, _enemyCombatManagers), 6.5f);
        }

        public override void Run()
        {
            _combatSceneManager.StartCombat();
        }

        public void SetCurrentSceneData(SceneData sceneData)
        {
            _sceneCurrentData = sceneData;
        }

        private void OnEnable()
        { 
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
