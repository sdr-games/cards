using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.NotificationsModule;
using SDRGames.Whist.PointsModule.Models;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.TurnSwitchModule.Managers
{
    public class TurnsQueueManager : MonoBehaviour
    {
        [SerializeField] private TurnsQueueView _turnsQueueView;
        [SerializeField] private TimerManager _timerManager;
        [SerializeField] private LocalizedString _playerTurnSwitchMessage;
        [SerializeField] private LocalizedString _enemyTurnSwitchMessage;
        [SerializeField] private LocalizedString _restorationTurnSwitchMessage;
        [SerializeField] private int _restorationTurnCooldown = 10;

        private float _currentRestorationTurnChance = 0;
        private int _currentRestorationTurnCooldown = 0;
        private bool _isCombatTurn;
        private List<Points> _charactersPoints;
        private List<CharacterInfoScriptableObject> _characterInfoScriptableObjects;

        public event EventHandler<TurnSwitchedEventArgs> TurnSwitched;

        public void Initialize(List<CharacterParamsModel> characterParamsModels)
        {
            _isCombatTurn = true;
            _characterInfoScriptableObjects = OrderByInitiative(characterParamsModels);
            _charactersPoints = GetPointsFromParams(characterParamsModels);
            _turnsQueueView.Initialize(_characterInfoScriptableObjects);
            _turnsQueueView.ShiftDone += OnShiftDone;

            _timerManager.Initialize();
            _timerManager.TimeOver += OnTimeOver;
            _timerManager.StartCombatTimer();

            string turnSwitchMessage = _characterInfoScriptableObjects[0].IsPlayer ? _playerTurnSwitchMessage.GetLocalizedText() : _enemyTurnSwitchMessage.GetLocalizedText();
            Notification.Show(turnSwitchMessage);
            TurnSwitched?.Invoke(this, new TurnSwitchedEventArgs(_characterInfoScriptableObjects[0].IsPlayer, true));
        }

        public void SwitchTurn()
        {
            _timerManager.StopTimer();
            CalculateRestorationTurnChance();
            _isCombatTurn = _currentRestorationTurnCooldown > 0 || _currentRestorationTurnChance < UnityEngine.Random.Range(0, 100);
            if (_isCombatTurn)
            {
                _currentRestorationTurnCooldown--;
                _turnsQueueView.NaturalShiftQueue();
                return;
            }
            _turnsQueueView.RestorationTurnShiftQueue();
            _currentRestorationTurnCooldown = _restorationTurnCooldown;
        }

        public void CalculateRestorationTurnChance()
        {
            _currentRestorationTurnChance = 0;
            foreach (Points points in _charactersPoints)
            {
                switch (points.Name)
                {
                    case "Armor":
                    case "Barrier":
                        if (points.CurrentValue == 0 && points.MaxValue > 0)
                        {
                            _currentRestorationTurnChance += 12.5f;
                        }
                        break;
                    case "HealthPoints":
                        _currentRestorationTurnChance += (points.MaxValue - points.CurrentValue) / points.MaxValue * 100;
                        break;
                    default:
                        break;
                }
            }
            if(_currentRestorationTurnChance < 0)
            {
                _currentRestorationTurnChance = 0;
                return;
            }
            if(_currentRestorationTurnChance > 50)
            {
                _currentRestorationTurnChance = 50;
            }
        }

        private List<CharacterInfoScriptableObject> OrderByInitiative(List<CharacterParamsModel> characterParamsModels)
        {
            List<CharacterInfoScriptableObject> result = new List<CharacterInfoScriptableObject>();
            List<CharacterParamsModel> sortedParams = characterParamsModels.OrderByDescending(x => x.Initiative).ToList();
            foreach (CharacterParamsModel characterParamsModel in sortedParams)
            {
                result.Add(characterParamsModel.CharacterInfo);
            }
            return result;
        }

        private List<Points> GetPointsFromParams(List<CharacterParamsModel> characterParamsModels)
        {
            List<Points> result = new List<Points>();
            foreach (CharacterParamsModel characterParamsModel in characterParamsModels)
            {
                result.Add(characterParamsModel.Armor);
                result.Add(characterParamsModel.Barrier);
                result.Add(characterParamsModel.HealthPoints);
            }
            return result;
        }

        private void OnShiftDone(object sender, ShiftDoneEventArgs e)
        {
            bool isPlayerTurn;
            //if (isPlayerTurn)
            //{
            if (_isCombatTurn)
            {
                isPlayerTurn = _characterInfoScriptableObjects[e.CurrentIndex].IsPlayer;
                _timerManager.StartCombatTimer();
                string turnSwitchMessage = isPlayerTurn ? _playerTurnSwitchMessage.GetLocalizedText() : _enemyTurnSwitchMessage.GetLocalizedText();
                Notification.Show(turnSwitchMessage);
            }
            else
            {
                isPlayerTurn = true;
                _timerManager.StartRestorationTimer();
                Notification.Show(_restorationTurnSwitchMessage.GetLocalizedText());
            }
            //}
            TurnSwitched?.Invoke(this, new TurnSwitchedEventArgs(isPlayerTurn, _isCombatTurn));
        }

        private void OnTimeOver(object sender, EventArgs e)
        {
            SwitchTurn();
        }

        private void OnEnable()
        {
            if (_turnsQueueView == null)
            {
                Debug.LogError("Turns Queue View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }
    }
}
