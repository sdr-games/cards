using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;
using System;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class TalentManager : MonoBehaviour
    {
        private Talent _talent;
        protected UserInputController _userInputController;

        [SerializeField] protected TalentView _talentView;
        [SerializeField] private RectTransform _rectTransform;

        public void Initialize(UserInputController userInputController, Talent talent)
        {
            _talent = talent;

            _talentView.BlockChanged += OnBlockChanged;

            _userInputController = userInputController;
        }

        public Vector2 GetSize()
        {
            return _rectTransform.rect.size / 1.5f;
        }

        public TalentView GetView()
        {
            return _talentView;
        }

        private void OnBlockChanged(object sender, EventArgs e)
        {
            _talent.ResetCurrentPoints();
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_talentView), _talentView);
        }

        protected virtual void OnDisable()
        {
            _talentView.BlockChanged -= OnBlockChanged;
        }
    }
}
