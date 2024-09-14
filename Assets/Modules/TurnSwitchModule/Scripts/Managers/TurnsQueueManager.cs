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
        [SerializeField] private int _portraitsLimit = 8;
        [SerializeField] private TurnsQueueView _turnsQueueView;
        [SerializeField] private TimerManager _timerManager;
        [SerializeField] private LocalizedString _playerTurnSwitchMessage;
        [SerializeField] private LocalizedString _enemyTurnSwitchMessage;
        [SerializeField] private LocalizedString _restorationTurnSwitchMessage;
        [SerializeField] private int _restorationTurnCooldown = 10;

        private float _currentRestorationTurnChance = 0;
        private int _currentRestorationTurnCooldown = 0;
        private bool _isCombatTurn;
        private List<CharacterInfoScriptableObject> _characterInfoScriptableObjects;

        public event EventHandler<TurnSwitchedEventArgs> TurnSwitched;

        public void Initialize(List<CharacterParamsModel> characterParamsModels)
        {
            _isCombatTurn = true;
            _characterInfoScriptableObjects = OrderByInitiative(characterParamsModels);
            _turnsQueueView.Initialize(_characterInfoScriptableObjects);
            for (int i = 0; i < _portraitsLimit; i++)
            {
                _turnsQueueView.AddPortraitToQueue();
            }
            _turnsQueueView.ShiftDone += OnShiftDone;

            _timerManager.Initialize();
            _timerManager.TimeOver += OnTimeOver;
            _timerManager.StartCombatTimer();

            foreach(CharacterParamsModel characterParamsModel in characterParamsModels)
            {
                characterParamsModel.Armor.CurrentValueChanged += ChangeRestorationTurnChance;
                characterParamsModel.Barrier.CurrentValueChanged += ChangeRestorationTurnChance;
                characterParamsModel.HealthPoints.CurrentValueChanged += ChangeRestorationTurnChance;
            }

            string turnSwitchMessage = _characterInfoScriptableObjects[0].IsPlayer ? _playerTurnSwitchMessage.GetLocalizedText() : _enemyTurnSwitchMessage.GetLocalizedText();
            Notification.Show(turnSwitchMessage);
            TurnSwitched?.Invoke(this, new TurnSwitchedEventArgs(_characterInfoScriptableObjects[0].IsPlayer, true));
        }

        public void SwitchTurn()
        {
            _timerManager.StopTimer();
            if(_currentRestorationTurnCooldown < _restorationTurnCooldown)
            {
                _currentRestorationTurnCooldown++;
                _turnsQueueView.NaturalShiftQueue();
                return;
            }

            if(_currentRestorationTurnChance >= UnityEngine.Random.Range(0, 100))
            {
                _currentRestorationTurnChance = 0;
                _currentRestorationTurnCooldown = 0;
                Notification.Show(_restorationTurnSwitchMessage.GetLocalizedText());
            }
        }

        public void ChangeRestorationTurnChance(object sender, ValueChangedEventArgs e)
        {
            string pointsName = (sender as Points).Name;
            switch (pointsName)
            {
                case "Armor":
                case "Barrier":
                    if(e.CurrentValue == 0)
                    {
                        _currentRestorationTurnChance += 12.5f;
                    }
                    else
                    {
                        _currentRestorationTurnChance -= 12.5f;
                    }
                    break;
                case "HealthPoints":
                    if(e.CurrentValueInPercents < 50)
                    {
                        _currentRestorationTurnChance += 25;
                    }
                    else
                    {
                        _currentRestorationTurnChance -= 25;
                    }
                    break;
                default:
                    break;
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

        private void OnShiftDone(object sender, ShiftDoneEventArgs e)
        {
            bool isPlayerTurn = _characterInfoScriptableObjects[e.CurrentIndex].IsPlayer;
            //if (isPlayerTurn)
            //{
            if (_isCombatTurn)
            {
                _timerManager.StartCombatTimer();
            }
            else
            {
                _timerManager.StartRestorationTimer();
            }
            //}
            string turnSwitchMessage = isPlayerTurn ? _playerTurnSwitchMessage.GetLocalizedText() : _enemyTurnSwitchMessage.GetLocalizedText();
            Notification.Show(turnSwitchMessage);
            TurnSwitched?.Invoke(this, new TurnSwitchedEventArgs(isPlayerTurn, true));
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
