using System;
using System.Collections.Generic;

using SDRGames.Whist.CharacterModule.ScriptableObjects;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.TurnSwitchModule.Managers
{
    public class TurnsQueueManager : MonoBehaviour
    {
        [SerializeField] private int _portraitsLimit = 8;
        [SerializeField] private TurnsQueueView _turnsQueueView;
        [SerializeField] private TimerManager _timerManager;

        [SerializeField] private List<CharacterInfoScriptableObject> _characterInfoScriptableObjects;
        
        private bool _isCombatTurn;

        public event EventHandler<TurnSwitchedEventArgs> TurnSwitched;

        public void Initialize(List<CharacterInfoScriptableObject> characterInfoScriptableObjects = null)
        {
            _isCombatTurn = true;
            characterInfoScriptableObjects = _characterInfoScriptableObjects;
            _turnsQueueView.Initialize(characterInfoScriptableObjects);
            for (int i = 0; i < _portraitsLimit; i++)
            {
                _turnsQueueView.AddPortraitToQueue();
            }
            _turnsQueueView.ShiftDone += OnShiftDone;

            _timerManager.Initialize();
            _timerManager.TimeOver += OnTimeOver;
            _timerManager.StartCombatTimer();

            TurnSwitched?.Invoke(this, new TurnSwitchedEventArgs(_characterInfoScriptableObjects[0].IsPlayer, true));
        }

        public void SwitchTurn()
        {
            _timerManager.StopTimer();
            _turnsQueueView.NaturalShiftQueue();
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
