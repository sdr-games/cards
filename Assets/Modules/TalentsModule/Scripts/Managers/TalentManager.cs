using System;
using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class TalentManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Talent _talent;

        protected List<TalentManager> _blockers;
        protected List<TalentManager> _dependencies;
        protected UserInputController _userInputController;

        [SerializeField] protected bool _isBlocked; //talent is not available because blockers are not fully increased
        [SerializeField] protected bool _isActive; //talent is fully increased

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] protected TalentView _talentView;

        public event EventHandler PointerEnterTalent;
        public event EventHandler PointerExitTalent;

        public void Initialize(UserInputController userInputController, Talent talent)
        {
            _userInputController = userInputController;
            _talent = talent;
            _dependencies = new List<TalentManager>();
            _blockers = new List<TalentManager>();

            _talent.CurrentPointsChanged += OnCurrentPointsChanged;

            ChangeAvailability(false);
        }

        public void SetDependencies(List<TalentManager> dependencies)
        {
            foreach (TalentManager talentManager in dependencies)
            {
                if(_dependencies.Contains(talentManager))
                {
                    continue;
                }
                _dependencies.Add(talentManager);
                _talentView.DrawDependencyLine(talentManager.transform.position);
            } 
        }

        public void AddBlocker(TalentManager blocker)
        {
            if(_blockers.Contains(blocker))
            {
                return;
            }
            _blockers.Add(blocker);
            SetBlock(true);
        }

        public virtual void SetBlock(bool isBlocked, bool silent = false)
        {
            _isBlocked = isBlocked;
            if(_isBlocked)
            {
                _isActive = false;
            }
            UpdateDependenciesBlockStatus();
            _talentView.ChangeAvailability(!_isBlocked);
        }

        public virtual void SetActive(bool isActive, bool silent = false)
        {
            _isActive = isActive;
            UpdateDependenciesBlockStatus();
        }

        public void ChangeAvailability(bool isActive)
        {
            isActive &= !_isBlocked;
            _talentView.ChangeAvailability(isActive);
        }

        public void SwitchTooltip(bool isVisible)
        {
            _talentView.SwitchTooltip(isVisible);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isBlocked)
            {
                return;
            } 
            PointerEnterTalent?.Invoke(this, EventArgs.Empty);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isBlocked)
            {
                return;
            }
            PointerExitTalent?.Invoke(this, EventArgs.Empty);
        }

        protected Vector2 GetSize()
        {
            return _rectTransform.rect.size / 1.5f;
        }

        private void UpdateDependenciesBlockStatus()
        {
            foreach(TalentManager talentManager in _dependencies)
            {
                talentManager.SetBlock(!_isActive);
            }
        }

        private void OnCurrentPointsChanged(object sender, CurrentPointsChangedEventArgs e)
        {
            SetActive(e.CurrentPoints == _talent.TotalCost, true);
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_talentView), _talentView);
        }
    }
}
