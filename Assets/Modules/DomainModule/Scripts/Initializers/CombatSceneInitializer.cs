using System.Collections.Generic;

using SDRGames.Whist.AIBehaviorModule.Managers;
using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.DomainModule.Managers;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;
using SDRGames.Whist.TurnSwitchModule.Managers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.DomainModule
{
    public class CombatSceneInitializer : MonoBehaviour
    {
        [SerializeField] private CombatSceneManager _combatSceneManager;
        [SerializeField] private UserInputController _userInputController;
        [SerializeField] private TurnsQueueManager _turnsQueueManager;

        [Header("UI")][SerializeField] private CombatUIManager _combatUIManager;
        [Header("PLAYER")][SerializeField] private PlayerCombatManager _playerCombatManager;
        [Header("ENEMIES")][SerializeField] private EnemyBehaviorManager[] _enemyBehaviorManagers;

        [Header("SCALINGS")][SerializeField] private CharacterParametersScalingScriptableObject _characterParametersScalingSettings;
        [SerializeField] private CardsScalingScriptableObject _cardsScalingScriptableObject;
        [SerializeField] private MeleeAttacksScalingScriptableObject _meleeAttacksScalingScriptableObject;

        private List<EnemyCombatManager> _enemyCombatManagers;

        public void Initialize()
        {
            _characterParametersScalingSettings.Initialize();
            _cardsScalingScriptableObject.Initialize();
            _meleeAttacksScalingScriptableObject.Initialize();

            _playerCombatManager.Initialize();

            _enemyCombatManagers = new List<EnemyCombatManager>();
            List<CharacterParamsModel> characterInfoScriptableObjects = new List<CharacterParamsModel>();
            characterInfoScriptableObjects.Add(_playerCombatManager.GetParams());
            foreach (EnemyBehaviorManager enemyBehaviorManager in _enemyBehaviorManagers)
            {
                enemyBehaviorManager.Initialize(_playerCombatManager, _userInputController);
                _enemyCombatManagers.Add(enemyBehaviorManager.EnemyCombatManager);
                characterInfoScriptableObjects.Add(enemyBehaviorManager.EnemyCombatManager.GetParams());
            }
            _combatUIManager.Initialize(_userInputController);
            _turnsQueueManager.Initialize(characterInfoScriptableObjects);

            _combatSceneManager.Initialize(_userInputController, _turnsQueueManager, _combatUIManager, _playerCombatManager, _enemyBehaviorManagers, _enemyCombatManagers);
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_userInputController), _userInputController);
            this.CheckFieldValueIsNotNull(nameof(_turnsQueueManager), _turnsQueueManager);
            this.CheckFieldValueIsNotNull(nameof(_characterParametersScalingSettings), _characterParametersScalingSettings);
            this.CheckFieldValueIsNotNull(nameof(_cardsScalingScriptableObject), _cardsScalingScriptableObject);
            this.CheckFieldValueIsNotNull(nameof(_meleeAttacksScalingScriptableObject), _meleeAttacksScalingScriptableObject);
            this.CheckFieldValueIsNotNull(nameof(_combatUIManager), _combatUIManager);
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
