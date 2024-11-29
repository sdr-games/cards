using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class BranchManager : MonoBehaviour
    {
        [SerializeField] private AstraManager _astraPrefab;
        [SerializeField] private TalamusManager _talamusPrefab;

        private UserInputController _userInputController;
        private TalentsBranchScriptableObject _talentBranchSO;
        private Dictionary<string, TalentManager> _createdTalents;

        [field: SerializeField] public BranchView BranchView { get; private set; }

        public void Initialize(UserInputController userInputController, TalentsBranchScriptableObject talentsBranchSO, Vector3 position, float startScale, Transform parent)
        {
            _userInputController = userInputController;
            _talentBranchSO = talentsBranchSO;

            BranchView.Initialize(userInputController, position, startScale, parent);
            BranchView.SetBackground(talentsBranchSO.Background);
            BranchView.BranchVisibilityChanged += OnBranchVisibilityChanged;

            foreach (TalentScriptableObject talent in _talentBranchSO.StartTalents)
            {
                Vector2 talentPosition = talent.CalculatePositionInContainer(BranchView.GetBackgroundSize());
                if (talent is AstraScriptableObject astraTalent)
                {
                    CreateAstra(astraTalent, talentPosition);
                    continue;
                }

                if (talent is TalamusScriptableObject talamusTalent)
                {
                    CreateTalamus(talamusTalent, talentPosition);
                    continue;
                }
            }
        }

        private void OnEnable()
        {
            _createdTalents =  new Dictionary<string, TalentManager>();
        }

        private List<TalentView> CreateDependencies(List<TalentScriptableObject> talents)
        {
            List<TalentView> dependencies = new List<TalentView>();
            foreach (TalentScriptableObject talent in talents)
            {
                if (_createdTalents.ContainsKey(talent.Name))
                {
                    dependencies.Add(_createdTalents[talent.Name].TalentView);
                    continue;
                }

                Vector2 talentPosition = talent.CalculatePositionInContainer(BranchView.GetBackgroundSize());
                if (talent is AstraScriptableObject astraTalent)
                {
                    CreateAstra(astraTalent, talentPosition);
                }
                else if (talent is TalamusScriptableObject talamusTalent)
                {
                    CreateTalamus(talamusTalent, talentPosition);
                }
                dependencies.Add(_createdTalents[talent.Name].TalentView);
            }
            return dependencies;
        }

        private TalamusManager CreateTalamus(TalamusScriptableObject talamus, Vector2 position)
        {
            List<TalentView> dependencies = CreateDependencies(talamus.Dependencies);
            TalamusManager talamusManager = Instantiate(_talamusPrefab, transform, false);
            talamusManager.Initialize(_userInputController, talamus, position);
            talamusManager.TalentView.SetDependencies(dependencies);
            _createdTalents.Add(talamus.Name, talamusManager);
            return talamusManager;
        }

        private AstraManager CreateAstra(AstraScriptableObject astra, Vector2 position)
        {
            List<TalentView> dependencies = CreateDependencies(astra.Dependencies);
            AstraManager astraManager = Instantiate(_astraPrefab, transform, false);
            astraManager.Initialize(_userInputController, astra, position);
            astraManager.TalentView.SetDependencies(dependencies);
            _createdTalents.Add(astra.Name, astraManager);
            return astraManager;
        }

        private void OnBranchVisibilityChanged(object sender, BranchVisibilityChangedEventArgs e)
        {
            foreach(TalentManager talentManager in _createdTalents.Values)
            {
                if(e.IsVisible && BranchView.IsZoomed)
                {
                    talentManager.TalentView.ChangeBlock();
                    continue;
                }
                talentManager.TalentView.ChangeVisibility(false);
            }
        }

    }
}