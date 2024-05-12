using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.MeleeCombatModule.Managers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.DomainModule.Managers
{
    public class CombatUIInitializer : MonoBehaviour
    {
        [SerializeField] private UserInputController _userInputController;
        [Header("ABILITIES")][SerializeField] private AbilitiesQueueManager _abilitiesQueueManager;
        [SerializeField] private MeleeAttackListManager _meleeAttackListManager;
        [Header("PLAYER")][SerializeField] private PlayerCharacterCombatManager _playerCharacterCombatManager;

        private void OnEnable()
        {
            if (_userInputController == null)
            {
                Debug.LogError("User Input Controller не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_abilitiesQueueManager == null)
            {
                Debug.LogError("Abilities Queue Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_meleeAttackListManager == null)
            {
                Debug.LogError("Attack List Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_playerCharacterCombatManager == null)
            {
                Debug.LogError("Player Character Combat Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            _abilitiesQueueManager.Initialize(_userInputController);
            _abilitiesQueueManager.ApplyButtonClicked += OnApplyButtonClicked;
            _abilitiesQueueManager.AbilityQueueCleared += OnAbilityQueueCleared;
            _playerCharacterCombatManager.Initialize();
            _meleeAttackListManager.Initialize(_userInputController);
            _meleeAttackListManager.MeleeAttackClicked += OnMeleeAttackClicked;
        }

        private void OnMeleeAttackClicked(object sender, MeleeAttackClickedEventArgs e)
        {
            if(_abilitiesQueueManager.IsFull || !_playerCharacterCombatManager.HasEnoughStaminaPoints(e.MeleeAttackScriptableObject.Cost))
            {
                return;
            }
            _playerCharacterCombatManager.ReserveStaminaPoints(e.MeleeAttackScriptableObject.Cost);
            _abilitiesQueueManager.AddAbilityToQueue(e.MeleeAttackScriptableObject);
        }

        private void OnApplyButtonClicked(object sender, ApplyButtonClickedEventArgs e)
        {
            foreach(var ability in e.MeleeAttackScriptableObjects)
            {
                if(ability == null)
                {
                    continue;
                }
                //Do damage
            }
            _playerCharacterCombatManager.SpendStaminaPoints(e.TotalCost);
        }

        private void OnAbilityQueueCleared(object sender, AbilityQueueClearedEventArgs e)
        {
            _playerCharacterCombatManager.ResetReservedPoints(e.ReverseAmount);
        }
    }
}
