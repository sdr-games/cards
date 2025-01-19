using System.Collections.Generic;
using System.Linq;
using System;

using SDRGames.Whist.AbilitiesQueueModule.Managers;
using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.DomainModule.Views;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.MeleeCombatModule.Managers;
using SDRGames.Whist.RestorationModule.Managers;
using SDRGames.Whist.TurnSwitchModule.Managers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.DomainModule.Managers
{
    public class CombatUIManager : MonoBehaviour
    {
        private UserInputController _userInputController;
        private TurnsQueueManager _turnsQueueManager;
        private PlayerCharacterCombatManager _playerCharacterCombatManager;
        private EnemyCharacterCombatManager _enemyCharacterCombatManager;

        [SerializeField] private CombatUIView _combatUIView;

        [Header("MELEE ABILITIES")][SerializeField] private AbilitiesQueueManager _abilitiesQueueManager;
        [SerializeField] private MeleeAttackListManager _meleeAttackListManager;

        [Header("CARDS")][SerializeField] private SelectedDeckManager _selectedDeckManager;
        [SerializeField] private DecksPreviewWindowManager _decksPreviewWindowManager;
        [SerializeField] private DeckOnHandsManager _deckOnHandsManager;

        [Header("RESTORATION")][SerializeField] private PotionListManager _potionListManager;

        [Header("PLAYER")][SerializeField] private HideableUIView _playerSwitchableUI;

        public event EventHandler<CardClickedEventArgs> CardClicked;

        public void Initialize(UserInputController userInputController, TurnsQueueManager turnsQueueManager, PlayerCharacterCombatManager playerCharacterCombatManager, EnemyCharacterCombatManager enemyCharacterCombatManager)
        {
            _userInputController = userInputController;
            _turnsQueueManager = turnsQueueManager;
            _playerCharacterCombatManager = playerCharacterCombatManager;
            _enemyCharacterCombatManager = enemyCharacterCombatManager;

            _abilitiesQueueManager.Initialize(_userInputController);
            _meleeAttackListManager.Initialize(_userInputController);
            _potionListManager.Initialize(_userInputController);
            _selectedDeckManager.Initialize(_userInputController);
            _decksPreviewWindowManager.Initialize(_userInputController);

            _deckOnHandsManager.Initialize(_userInputController);
            _deckOnHandsManager.CardClicked += OnCardClicked;

            _combatUIView.Initialize();
        }

        public void ShowPlayerUI(bool isCombatTurn)
        {
            _abilitiesQueueManager.Show();
            if (isCombatTurn)
            {
                _meleeAttackListManager.Show();
                _selectedDeckManager.Show();
                _potionListManager.Hide();
            }
            else
            {
                _meleeAttackListManager.Hide();
                _deckOnHandsManager.HideView();
                _selectedDeckManager.Hide();
                _potionListManager.Show();
            }
            _playerSwitchableUI.Show();
        }

        public void HidePlayerUI()
        {
            _playerSwitchableUI.Hide();
            _deckOnHandsManager.HideView();
            _potionListManager.Hide();
            _decksPreviewWindowManager.Hide();
        }

        public void TrySelectCard(CardManager cardManager)
        {
            _deckOnHandsManager.AddSelectedCard(cardManager);
            _playerCharacterCombatManager.ReserveBreathPoints(cardManager.CardScriptableObject.Cost);            
        }

        public void TryDeselectCard(CardManager cardManager)
        {
        if (_deckOnHandsManager.RemoveSelectedCard(cardManager))
        {
            _playerCharacterCombatManager.ResetBreathReservedPoints(cardManager.CardScriptableObject.Cost);
        }
    }

        #region Events methods

        private void OnCardClicked(object sender, CardClickedEventArgs e)
        {
            CardClicked?.Invoke(this, e);
        }

        private void OnMeleeAttackClicked(object sender, MeleeAttackClickedEventArgs e)
{
            if (_abilitiesQueueManager.IsFull || !_playerCharacterCombatManager.HasEnoughStaminaPoints(e.MeleeAttackScriptableObject.Cost))
{
return;
}
            _playerCharacterCombatManager.ReserveStaminaPoints(e.MeleeAttackScriptableObject.Cost);
            _abilitiesQueueManager.AddAbilityToQueue(e.MeleeAttackScriptableObject);
        }

        private void OnPotionClicked(object sender, PotionClickedEventArgs e)
        {
            if (_abilitiesQueueManager.IsFull)
            {
                return;
            }
            _abilitiesQueueManager.AddAbilityToQueue(e.PotionScriptableObject);
        }

        private void OnQueueApplyButtonClicked(object sender, AbilitiesQueueModule.Managers.ApplyButtonClickedEventArgs e)
        {
            if (e.AbilityScriptableObjects.Count == 0 && e.AbilityScriptableObjects.Any(item => item is null))
            {
                return;
}

            List<CharacterCombatManager> enemyCharacterCombatManagers = new List<CharacterCombatManager>() { _enemyCharacterCombatManager };

            foreach (var ability in e.AbilityScriptableObjects)
            {
                if (ability == null)
                {
                    continue;
                }
                ability.ApplyLogics(
_playerCharacterCombatManager,
                    enemyCharacterCombatManagers,
                    e.AbilityScriptableObjects.Count,
                    new List<int>() { 0 });
            }
            _playerCharacterCombatManager.SpendStaminaPoints(e.TotalCost);
            _turnsQueueManager.SwitchTurn();
        }

        private void OnDeckApplyButtonClicked(object sender, CardsCombatModule.Managers.ApplyButtonClickedEventArgs e)
        {
            if (e.CardManagers.Count == 0)
            {
                return;
            }

            List<CharacterCombatManager> enemyCharacterCombatManagers = new List<CharacterCombatManager>() { _enemyCharacterCombatManager };

            foreach (CardManager cardManager in e.CardManagers)
            {
                _deckOnHandsManager.RemoveSelectedCard(cardManager);

            }
        }

        //private void OnDeckApplyButtonClicked(object sender, CardsCombatModule.Managers.ApplyButtonClickedEventArgs e)
        //{
        //    if(e.Managers.Count == 0)
        //    {
        //        return;
        //    }

        //    List<CharacterCombatManager> enemyCharacterCombatManagers = new List<CharacterCombatManager>() { _enemyCharacterCombatManager };

        //    List<CardScriptableObject> cards = new List<CardScriptableObject>();
        //    foreach(CardManager card in e.Managers)
        //    {
        //        cards.Add(card.CardScriptableObject);
        //    }

        //    foreach (CardManager card in e.Managers)
        //    {
        //        if (card == null)
        //        {
        //            continue;
        //        }
        //        _deckOnHandsManager.RemoveSelectedCard(card);

        //        if(card.CardScriptableObject.AbilityModifiers.Length > 0)
        //        {
        //            card.CardScriptableObject.ApplyModifiers(_playerCharacterCombatManager,
        //            enemyCharacterCombatManagers,
        //            e.Managers.Count,
        //            cards
        //            );
        //        }

        //        card.CardScriptableObject.ApplyLogics(
        //            _playerCharacterCombatManager,
        //            enemyCharacterCombatManagers,
        //            e.Managers.Count,
        //            new List<int>() { 0 });
        //    }
        //    _playerCharacterCombatManager.SpendBreathPoints(e.TotalCost);
        //    _turnsQueueManager.SwitchTurn();
        //}

        private void OnAbilityQueueCleared(object sender, AbilityQueueClearedEventArgs e)
        {
            _playerCharacterCombatManager.ResetStaminaReservedPoints(e.ReverseAmount);
        }

        private void OnSelectedDeckViewClicked(object sender, EventArgs e)
        {
            _abilitiesQueueManager.Hide();
            _meleeAttackListManager.Hide();
            _potionListManager.Hide();
            _deckOnHandsManager.ShowView();
        }

        private void OnEmptyDeckViewClicked(object sender, EventArgs e)
        {
            _abilitiesQueueManager.Hide();
            _meleeAttackListManager.Hide();
            _decksPreviewWindowManager.Show();
        }

        private void OnDeckSelected(object sender, DeckPreviewClickedEventArgs e)
        {
            _abilitiesQueueManager.Show();
            _meleeAttackListManager.Show();
            _decksPreviewWindowManager.Hide();
            _selectedDeckManager.SetSelectedDeck(e.DeckScriptableObject);
            _deckOnHandsManager.SetSelectedDeck(e.DeckScriptableObject);
        }

        #endregion
    }
}
