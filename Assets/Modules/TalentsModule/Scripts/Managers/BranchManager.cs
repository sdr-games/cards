using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.HelpersModule;

using UnityEngine;
using System;
using SDRGames.Whist.TalentsModule.Models;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class BranchManager : MonoBehaviour
    {
        [SerializeField] private BranchView _branchView;
        [SerializeField] private AstraManager _astraPrefab;
        [SerializeField] private TalamusManager _talamusPrefab;

        private UserInputController _userInputController;
        private TalentsBranchScriptableObject _talentBranchSO;
        private Dictionary<string, TalentManager> _createdTalents;

        public event EventHandler<AstraChangedEventArgs> AstraChanged;
        public event EventHandler<TalamusChangedEventArgs> TalamusChanged;

        public void Initialize(UserInputController userInputController, TalentsBranchScriptableObject talentsBranchSO, Vector3 position, float startScale, Transform parent)
        {
            _userInputController = userInputController;
            _talentBranchSO = talentsBranchSO;

            _branchView.Initialize(userInputController, position, startScale, parent);
            _branchView.SetBackground(talentsBranchSO.Background);
            _branchView.BranchVisibilityChanged += OnBranchVisibilityChanged;

            foreach (TalentScriptableObject talent in _talentBranchSO.StartTalents)
            {
                CreateTalent(talent);
            }
        }

        public BranchView GetView()
        {
            return _branchView;
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_astraPrefab), _astraPrefab);
            this.CheckFieldValueIsNotNull(nameof(_talamusPrefab), _talamusPrefab);

            _createdTalents = new Dictionary<string, TalentManager>();
        }

        private void CreateTalent(TalentScriptableObject talent)
        {
            TalentManager talentManager = null;
            if (talent is AstraScriptableObject astraTalent)
            {
                talentManager = CreateAstra(astraTalent);
            }
            else if (talent is TalamusScriptableObject talamusTalent)
            {
                talentManager = CreateTalamus(talamusTalent);
            }

            if(talentManager is null)
            {
                return;
            }
            List<TalentManager> dependencies = CreateDependencies(talent.Dependencies, talentManager);
            talentManager.SetDependencies(dependencies);
            talentManager.PointerEnterTalent += OnPointerEnterTalent;
            talentManager.PointerExitTalent += OnPointerExitTalent;
            _createdTalents.Add(talent.Name, talentManager);
        }

        private TalamusManager CreateTalamus(TalamusScriptableObject talamus)
        {
            TalamusManager talamusManager = Instantiate(_talamusPrefab, transform, false);
            talamusManager.Initialize(_userInputController, talamus, _branchView.GetBackgroundSize());
            talamusManager.TalamusChanged += OnTalamusChanged;
            return talamusManager;
        }

        private AstraManager CreateAstra(AstraScriptableObject astra)
        {
            AstraManager astraManager = Instantiate(_astraPrefab, transform, false);
            astraManager.Initialize(_userInputController, astra, _branchView.GetBackgroundSize());
            astraManager.AstraChanged += OnAstraChanged;
            return astraManager;
        }

        private List<TalentManager> CreateDependencies(List<TalentScriptableObject> talents, TalentManager blocker)
        {
            List<TalentManager> dependencies = new List<TalentManager>();
            foreach (TalentScriptableObject talent in talents)
            {
                if (!_createdTalents.ContainsKey(talent.Name))
                {
                    CreateTalent(talent);
                }
                TalentManager talentManager = _createdTalents[talent.Name];
                talentManager.AddBlocker(blocker);
                dependencies.Add(talentManager);
            }
            return dependencies;
        }

        private void OnAstraChanged(object sender, AstraChangedEventArgs e)
        {
            if(!_branchView.IsZoomed || _branchView.IsMoving)
            {
                return;
            }
            AstraChanged?.Invoke(this, e);
        }

        private void OnTalamusChanged(object sender, TalamusChangedEventArgs e)
        {
            if (!_branchView.IsZoomed || _branchView.IsMoving)
            {
                return;
            }
            TalamusChanged?.Invoke(this, e);
        }

        private void OnBranchVisibilityChanged(object sender, BranchVisibilityChangedEventArgs e)
        {
            foreach(TalentManager talentManager in _createdTalents.Values)
            {
                talentManager.ChangeAvailability(e.IsVisible && _branchView.IsZoomed);
            }
        }

        private void OnPointerExitTalent(object sender, EventArgs e)
        {
            if (!_branchView.IsZoomed || _branchView.IsMoving)
            {
                return;
            }
            ((TalentManager)sender).SwitchTooltip(false);
        }

        private void OnPointerEnterTalent(object sender, EventArgs e)
        {
            if (!_branchView.IsZoomed || _branchView.IsMoving)
            {
                return;
            }
            ((TalentManager)sender).SwitchTooltip(true);
        }
    }
}