using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AIBehaviorModule.Managers
{
    public class EnemyBehaviorManager : MonoBehaviour
    {
        [SerializeField] private EnemyMeleeBehaviorManager _meleeBehaviorManager;

        [field: SerializeField] public EnemyCombatManager EnemyCombatManager { get; private set; }

        private PlayerCombatManager _playerCombatManager;

        public void Initialize(PlayerCombatManager playerCombatManager)
        {
            _playerCombatManager = playerCombatManager;

            EnemyCombatManager.Initialize();
            _meleeBehaviorManager.Initialize(EnemyCombatManager, playerCombatManager);
        }
    }
}
