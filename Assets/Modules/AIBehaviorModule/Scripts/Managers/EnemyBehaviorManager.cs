using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.AIBehaviorModule.Managers
{
    public class EnemyBehaviorManager : MonoBehaviour
    {
        [SerializeField] private EnemyMeleeBehaviorManager _meleeBehaviorManager;

        [field: SerializeField] public EnemyCombatManager EnemyCombatManager { get; private set; }

        private PlayerCombatManager _playerCombatManager;

        public void Initialize(PlayerCombatManager playerCombatManager, UserInputController userInputController)
        {
            _playerCombatManager = playerCombatManager;

            EnemyCombatManager.Initialize(userInputController);
            _meleeBehaviorManager.Initialize(EnemyCombatManager, playerCombatManager);
        }
    }
}
