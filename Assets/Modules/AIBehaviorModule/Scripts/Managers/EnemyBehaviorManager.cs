using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AIBehaviorModule.Managers
{
    public class EnemyBehaviorManager : MonoBehaviour
    {
        [SerializeField] public EnemyCombatManager EnemyCombatManager { get; private set; }
        [SerializeField] private EnemyMeleeBehaviorManager _meleeBehaviorManager;

        private PlayerCombatManager _playerCombatManager;

        public void Initialize(PlayerCombatManager playerCombatManager)
        {
            _playerCombatManager = playerCombatManager;

            EnemyCombatManager.Initialize();
            _meleeBehaviorManager.Initialize(EnemyCombatManager, playerCombatManager);
        }
    }
}
