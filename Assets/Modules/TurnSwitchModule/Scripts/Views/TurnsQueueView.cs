using System;
using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.TurnSwitchModule.Views;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.TurnSwitchModule
{
    public class TurnsQueueView : MonoBehaviour
    {
        [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;
        [SerializeField] private TurnsQueuePortraitView _turnsQueuePortraitViewPrefab;
        [SerializeField] private int _shiftingSpeed = 2;

        private int _defaultLeftPadding;
        private int _shiftedLeftPadding = -50;
        private int _currentTemplateIndex;
        private LinkedList<TurnsQueuePortraitView> _turnsQueuePortraitViewList;
        private List<CharacterInfoScriptableObject> _queueTemplate;

        public event EventHandler<ShiftDoneEventArgs> ShiftDone;

        public void Initialize(List<CharacterInfoScriptableObject> characterInfoScriptableObjects)
        {
            _currentTemplateIndex = 0;
            _defaultLeftPadding = _horizontalLayoutGroup.padding.left;
            _turnsQueuePortraitViewList = new LinkedList<TurnsQueuePortraitView>();
            _queueTemplate = characterInfoScriptableObjects;
        }

        public void NaturalShiftQueue()
        {
            StartCoroutine(NaturalShiftQueueCoroutine());
        }

        public void ForceShiftQueue()
        {

        }

        public void AddPortraitToQueue()
        {
            TurnsQueuePortraitView turnsQueuePortraitView = Instantiate(_turnsQueuePortraitViewPrefab, transform, false);
            turnsQueuePortraitView.Initialize(_queueTemplate[_currentTemplateIndex].CharacterPortrait);
            _turnsQueuePortraitViewList.AddLast(turnsQueuePortraitView);
            _currentTemplateIndex = (_currentTemplateIndex + 1) % _queueTemplate.Count;
        }

        private IEnumerator NaturalShiftQueueCoroutine()
        {
            while (_horizontalLayoutGroup.padding.left > _shiftedLeftPadding)
            {
                yield return null;
                SetRectOffsetLeftPadding(_horizontalLayoutGroup.padding.left - _shiftingSpeed);
            }
            TurnsQueuePortraitView turnsQueuePortraitView = _turnsQueuePortraitViewList.First.Value;
            Destroy(turnsQueuePortraitView.gameObject);
            _turnsQueuePortraitViewList.RemoveFirst();
            SetRectOffsetLeftPadding(_defaultLeftPadding);
            AddPortraitToQueue();
            ShiftDone?.Invoke(this, new ShiftDoneEventArgs(_currentTemplateIndex));
        }

        private void SetRectOffsetLeftPadding(int leftPadding)
        {
            RectOffset tmpRectOffset = new RectOffset(
                    leftPadding,
                    _horizontalLayoutGroup.padding.right,
                    _horizontalLayoutGroup.padding.top,
                    _horizontalLayoutGroup.padding.bottom
                );
            _horizontalLayoutGroup.padding = tmpRectOffset;
        }

        private void OnEnable()
        {
            if (_horizontalLayoutGroup == null)
            {
                Debug.LogError("Horizontal Layout Group не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_turnsQueuePortraitViewPrefab == null)
            {
                Debug.LogError("Turns Queue Portrait View Prefab не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }
    }
}
