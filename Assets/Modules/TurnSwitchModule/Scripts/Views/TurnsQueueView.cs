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
        [SerializeField] private int _portraitsLimit = 8;
        [SerializeField] private Sprite _restorationTurnIcon;

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
            for(int i = 0; i < _portraitsLimit; i++)
            {
                Sprite portrait = _queueTemplate[_currentTemplateIndex].CharacterPortrait;
                AddPortraitToQueue(portrait);
                _currentTemplateIndex = (_currentTemplateIndex + 1) % _queueTemplate.Count;
            }
        }

        public void NaturalShiftQueue()
        {
            Sprite portrait = _queueTemplate[_currentTemplateIndex].CharacterPortrait;
            _currentTemplateIndex = (_currentTemplateIndex + 1) % _queueTemplate.Count;
            StartCoroutine(NaturalShiftQueueCoroutine(portrait));
        }

        public void ForceShiftQueue(Sprite portrait)
        {
            StartCoroutine(NaturalShiftQueueCoroutine(portrait));
        }

        public void RestorationTurnShiftQueue()
        {
            StartCoroutine(ForceShiftQueueCoroutine(_restorationTurnIcon));
        }

        public void AddPortraitToQueue(Sprite portrait, bool force = false)
        {
            TurnsQueuePortraitView turnsQueuePortraitView = Instantiate(_turnsQueuePortraitViewPrefab, transform, false);
            turnsQueuePortraitView.Initialize(portrait);
            if (force)
            {
                turnsQueuePortraitView.transform.SetAsFirstSibling();
                _turnsQueuePortraitViewList.AddFirst(turnsQueuePortraitView);
                return;
            }
            _turnsQueuePortraitViewList.AddLast(turnsQueuePortraitView);
        }

        private IEnumerator NaturalShiftQueueCoroutine(Sprite portrait)
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
            AddPortraitToQueue(portrait);
            ShiftDone?.Invoke(this, new ShiftDoneEventArgs(_currentTemplateIndex));
        }

        private IEnumerator ForceShiftQueueCoroutine(Sprite portrait)
        {
            while (_horizontalLayoutGroup.padding.left > _shiftedLeftPadding)
            {
                yield return null;
                SetRectOffsetLeftPadding(_horizontalLayoutGroup.padding.left - _shiftingSpeed);
            }
            TurnsQueuePortraitView turnsQueuePortraitView = _turnsQueuePortraitViewList.First.Value;
            Destroy(turnsQueuePortraitView.gameObject);
            _turnsQueuePortraitViewList.RemoveFirst();
            AddPortraitToQueue(portrait, true);
            yield return new WaitForSeconds(0.5f);
            while (_horizontalLayoutGroup.padding.left < _defaultLeftPadding)
            {
                yield return null;
                SetRectOffsetLeftPadding(_horizontalLayoutGroup.padding.left + _shiftingSpeed);
            }
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
