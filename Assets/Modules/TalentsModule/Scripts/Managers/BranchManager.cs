using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Managers
{

    public class BranchManager : MonoBehaviour
    {
        [SerializeField] private TalentsBranchScriptableObject _talentBranch;
        [SerializeField] private AstraManager _astraPrefab;
        [SerializeField] private TalamusManager _talamusPrefab;
        [SerializeField] private UserInputController _userInputController;

        private Dictionary<string, TalentManager> _createdTalents;

        private void OnEnable()
        {
            if(_talentBranch == null)
            {
                Debug.LogError("Talent Branch не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            _createdTalents =  new Dictionary<string, TalentManager>();

            foreach (TalentScriptableObject talent in _talentBranch.StartTalents)
            {
                if (talent is AstraScriptableObject astraTalent)
                {
                    CreateAstra(astraTalent);
                    continue;
                }

                if (talent is TalamusScriptableObject talamusTalent)
                {
                    CreateTalamus(talamusTalent);
                    continue;
                }
            }
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

                if (talent is AstraScriptableObject astraTalent)
                {
                    CreateAstra(astraTalent);
                }
                else if (talent is TalamusScriptableObject talamusTalent)
                {
                    CreateTalamus(talamusTalent);
                }
                dependencies.Add(_createdTalents[talent.Name].TalentView);
            }
            return dependencies;
        }

        private TalamusManager CreateTalamus(TalamusScriptableObject talamus)
        {
            List<TalentView> dependencies = CreateDependencies(talamus.Dependencies);
            TalamusManager talamusManager = Instantiate(_talamusPrefab, transform, false);
            talamusManager.Initialize(_userInputController, talamus);
            talamusManager.TalentView.SetDependencies(dependencies);
            _createdTalents.Add(talamus.Name, talamusManager);
            return talamusManager;
        }

        private AstraManager CreateAstra(AstraScriptableObject astra)
        {
            List<TalentView> dependencies = CreateDependencies(astra.Dependencies);
            AstraManager astraManager = Instantiate(_astraPrefab, transform, false);
            astraManager.Initialize(_userInputController, astra);
            astraManager.TalentView.SetDependencies(dependencies);
            _createdTalents.Add(astra.Name, astraManager);
            return astraManager;
        }
    }
}