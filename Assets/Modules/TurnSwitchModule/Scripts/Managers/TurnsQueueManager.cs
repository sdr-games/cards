using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.CharacterInfoModule.ScriptableObjects;
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
        private int _playerTurnIndex;
        private bool _isCombatTurn;
        private List<Points> _charactersPoints;
        private List<CharacterInfoScriptableObject> _charactersInfos;

        public event EventHandler<TurnSwitchedEventArgs> TurnSwitched;

        public void Initialize(List<CharacterScriptableObject> characters)
        {
            _isCombatTurn = true;

            _charactersInfos = OrderByInitiative(characters);
            _charactersPoints = GetPointsFromParams(characters);
            _playerTurnIndex = _charactersInfos.FindIndex(info => info.IsPlayer);

            _turnsQueueView.Initialize(_charactersInfos);
            _turnsQueueView.ShiftDone += OnShiftDone;

            _timerManager.Initialize();
            _timerManager.TimeOver += OnTimeOver;
        }

        public void Run()
        {
            bool isPlayerTurn = _charactersInfos[0].IsPlayer;
            int enemyIndex = isPlayerTurn ? -1 : 0;
            string turnSwitchMessage = isPlayerTurn ? _playerTurnSwitchMessage.GetLocalizedText() : _enemyTurnSwitchMessage.GetLocalizedText();
            NotificationController.Show(turnSwitchMessage);
            _timerManager.StartCombatTimer();

            TurnSwitched?.Invoke(this, new TurnSwitchedEventArgs(isPlayerTurn, _isCombatTurn, enemyIndex));
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

        private List<CharacterInfoScriptableObject> OrderByInitiative(List<CharacterScriptableObject> characters)
        {
            List<CharacterInfoScriptableObject> result = new List<CharacterInfoScriptableObject>();
            List<CharacterScriptableObject> sortedParams = characters.OrderByDescending(x => x.CharacterParams.Initiative.CheckRoll()).ToList();
            foreach (CharacterScriptableObject characterParamsModel in sortedParams)
            {
                result.Add(characterParamsModel.CharacterInfo);
            }
            return result;
        }

        private List<Points> GetPointsFromParams(List<CharacterScriptableObject> characters)
        {
            List<Points> result = new List<Points>();
            foreach (CharacterScriptableObject characterParamsModel in characters)
            {
                result.Add(characterParamsModel.CharacterParams.ArmorPoints);
                result.Add(characterParamsModel.CharacterParams.BarrierPoints);
                result.Add(characterParamsModel.CharacterParams.HealthPoints);
            }
            return result;
        }

        private void OnShiftDone(object sender, ShiftDoneEventArgs e)
        {
            bool isPlayerTurn;
            if (_isCombatTurn)
            {
                isPlayerTurn = _charactersInfos[e.CurrentIndex].IsPlayer;
                _timerManager.StartCombatTimer();
                string turnSwitchMessage = isPlayerTurn ? _playerTurnSwitchMessage.GetLocalizedText() : _enemyTurnSwitchMessage.GetLocalizedText();
                NotificationController.Show(turnSwitchMessage);
            }
            else
            {
                isPlayerTurn = true;
                _timerManager.StartRestorationTimer();
                NotificationController.Show(_restorationTurnSwitchMessage.GetLocalizedText());
            }
            int enemyIndex = isPlayerTurn ? -1 : e.CurrentIndex;
            if(enemyIndex > _playerTurnIndex)
            {
                enemyIndex--;
            }
            TurnSwitched?.Invoke(this, new TurnSwitchedEventArgs(isPlayerTurn, _isCombatTurn, enemyIndex));
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
