using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.HelpersModule;

using UnityEngine;
using System;

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
                if (talent is AstraScriptableObject astraTalent)
                {
                    Vector2 talentPosition = talent.CalculatePositionInContainer(_branchView.GetBackgroundSize() - _astraPrefab.GetSize());
                    CreateAstra(astraTalent, talentPosition);
                    continue;
                }

                if (talent is TalamusScriptableObject talamusTalent)
                {
                    Vector2 talentPosition = talent.CalculatePositionInContainer(_branchView.GetBackgroundSize() - _talamusPrefab.GetSize());
                    CreateTalamus(talamusTalent, talentPosition);
                    continue;
                }
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

            _createdTalents =  new Dictionary<string, TalentManager>();
        }

        private List<TalentView> CreateDependencies(List<TalentScriptableObject> talents)
        {
            List<TalentView> dependencies = new List<TalentView>();
            foreach (TalentScriptableObject talent in talents)
            {
                if (_createdTalents.ContainsKey(talent.Name))
                {
                    dependencies.Add(_createdTalents[talent.Name].GetView());
                    continue;
                }

                if (talent is AstraScriptableObject astraTalent)
                {
                    Vector2 talentPosition = talent.CalculatePositionInContainer(_branchView.GetBackgroundSize() - _astraPrefab.GetSize());
                    CreateAstra(astraTalent, talentPosition);
                }
                else if (talent is TalamusScriptableObject talamusTalent)
                {
                    Vector2 talentPosition = talent.CalculatePositionInContainer(_branchView.GetBackgroundSize() - _talamusPrefab.GetSize());
                    CreateTalamus(talamusTalent, talentPosition);
                }
                dependencies.Add(_createdTalents[talent.Name].GetView());
            }
            return dependencies;
        }

        private TalamusManager CreateTalamus(TalamusScriptableObject talamus, Vector2 position)
        {
            List<TalentView> dependencies = CreateDependencies(talamus.Dependencies);
            TalamusManager talamusManager = Instantiate(_talamusPrefab, transform, false);
            talamusManager.Initialize(_userInputController, talamus, position);
            talamusManager.GetView().SetDependencies(dependencies);
            talamusManager.TalamusChanged += OnTalamusChanged;
            _createdTalents.Add(talamus.Name, talamusManager);
            return talamusManager;
        }

        private AstraManager CreateAstra(AstraScriptableObject astra, Vector2 position)
        {
            List<TalentView> dependencies = CreateDependencies(astra.Dependencies);
            AstraManager astraManager = Instantiate(_astraPrefab, transform, false);
            astraManager.Initialize(_userInputController, astra, position);
            astraManager.GetView().SetDependencies(dependencies);
            astraManager.AstraChanged += OnAstraChanged;
            _createdTalents.Add(astra.Name, astraManager);
            return astraManager;
        }

        private void OnAstraChanged(object sender, AstraChangedEventArgs e)
        {
            AstraChanged?.Invoke(this, e);
        }

        private void OnTalamusChanged(object sender, TalamusChangedEventArgs e)
        {
            TalamusChanged?.Invoke(this, e);
        }

        private void OnBranchVisibilityChanged(object sender, BranchVisibilityChangedEventArgs e)
        {
            foreach(TalentManager talentManager in _createdTalents.Values)
            {
                if(e.IsVisible && _branchView.IsZoomed)
                {
                    talentManager.GetView().ChangeBlock();
                    continue;
                }
                talentManager.GetView().ChangeVisibility(false);
            }
        }
    }
}