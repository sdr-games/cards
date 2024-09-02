using System;
using System.Collections.Generic;
using System.Linq;

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

            TurnSwitched?.Invoke(this, new TurnSwitchedEventArgs(_characterInfoScriptableObjects[0].IsPlayer, true));
        }

        public void SwitchTurn()
        {
            _timerManager.StopTimer();
            _turnsQueueView.NaturalShiftQueue();
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
