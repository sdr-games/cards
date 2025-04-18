using System.Collections;
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
using SDRGames.Whist.AIBehaviorModule.ScriptableObjects;

using UnityEngine;

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
        [Header("ENEMIES")][SerializeField] private EnemyBehaviorManager _enemyBehaviorManagerPrefab;

        [Header("SCALINGS")][SerializeField] private CharacterParametersScalingScriptableObject _characterParametersScalingSettings;
        [SerializeField] private CardsScalingScriptableObject _cardsScalingScriptableObject;
        [SerializeField] private MeleeAttacksScalingScriptableObject _meleeAttacksScalingScriptableObject;

        private List<EnemyBehaviorManager> _enemyBehaviorManagers;

        public override IEnumerator InitializeCoroutine()
        {
            PlayerParamsScriptableObject playerParamsScriptableObject = (PlayerParamsScriptableObject)_sceneInitializationReferenceParameters["playerParams"];
            EnemiesListScriptableObject enemiesListScriptableObject = (EnemiesListScriptableObject)_sceneInitializationReferenceParameters["enemiesList"];

            _totalWeight = 13.5f + enemiesListScriptableObject.EnemiesData.Length;

            yield return InitializePart(() => _notificationController.Initialize(), 0.5f);
            yield return InitializePart(() => _characterParametersScalingSettings.Initialize(), 1f);
            yield return InitializePart(() => _cardsScalingScriptableObject.Initialize(), 1f);
            yield return InitializePart(() => _meleeAttacksScalingScriptableObject.Initialize(), 1f);
            yield return InitializePart(() => _floatingTextManager.Initialize(), 1f);
            yield return InitializePart(() => _playerCombatManager.Initialize(playerParamsScriptableObject), 1f);

            List<CharacterParamsScriptableObject> characterParamsScriptableObjects = new List<CharacterParamsScriptableObject>();
            characterParamsScriptableObjects.Add(playerParamsScriptableObject);

            _enemyBehaviorManagers = new List<EnemyBehaviorManager>();
            List<EnemyCombatManager> enemyCombatManagers = new List<EnemyCombatManager>();
            for(int i = 0; i < enemiesListScriptableObject.EnemiesData.Length; i++)
            {
                yield return InitializePart(() => CreateAndInitializeEnemy(enemiesListScriptableObject.EnemiesData[i], enemyCombatManagers, characterParamsScriptableObjects), 1f);
            }
            yield return InitializePart(() => _combatUIManager.Initialize(UserInputController.Instance), 1f);
            yield return InitializePart(() => _turnsQueueManager.Initialize(characterParamsScriptableObjects), 1f);

            yield return InitializePart(() => _combatSceneManager.Initialize(_turnsQueueManager, _combatUIManager, _playerCombatManager, _enemyBehaviorManagers, enemyCombatManagers), 6.5f);
        }

        public override void Run()
        {
            _combatSceneManager.StartCombat();
        }

        private void CreateAndInitializeEnemy(EnemyDataScriptableObject enemyDataScriptableObject, List<EnemyCombatManager> enemyCombatManagers, List<CharacterParamsScriptableObject> characterParamsScriptableObjects)
        {
            EnemyBehaviorManager enemyBehaviorManager = Instantiate(_enemyBehaviorManagerPrefab);
            enemyBehaviorManager.Initialize(enemyDataScriptableObject, _playerCombatManager, UserInputController.Instance);
            _enemyBehaviorManagers.Add(enemyBehaviorManager);
            enemyCombatManagers.Add(enemyBehaviorManager.EnemyCombatManager);
            characterParamsScriptableObjects.Add(enemyDataScriptableObject.EnemyParamsScriptableObject);
            _combatUIManager.AddEnemyBars(enemyBehaviorManager.EnemyCombatManager.GetView().gameObject);
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
            this.CheckFieldValueIsNotNull(nameof(_enemyBehaviorManagerPrefab), _enemyBehaviorManagerPrefab);
        }

        private void OnValidate()
        {
            _characterParametersScalingSettings.Initialize();
            _cardsScalingScriptableObject.Initialize();
            _meleeAttacksScalingScriptableObject.Initialize();
        }
    }
}
