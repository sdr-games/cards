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
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private AstraManager _astraPrefab;
        [SerializeField] private TalamusManager _talamusPrefab;

        private UserInputController _userInputController;
        private TalentsBranchScriptableObject _talentBranchSO;
        private Dictionary<string, TalentManager> _createdTalents;

        public void Initialize(UserInputController userInputController, TalentsBranchScriptableObject talentsBranchSO)
        {
            _userInputController = userInputController;
            _talentBranchSO = talentsBranchSO;

            foreach (TalentScriptableObject talent in _talentBranchSO.StartTalents)
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

        public void SetPosition(Vector2 position, Vector2 padding = default)
        {
            _rectTransform.sizeDelta = new Vector2(Screen.width / 2 + padding.x, Screen.height / 2 + padding.y);
            transform.position = position;
            foreach (TalentManager talentManager in _createdTalents.Values)
            {
                talentManager.TalentView.SetParent(transform);
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
            TalamusManager talamusManager = Instantiate(_talamusPrefab);
            talamusManager.Initialize(_userInputController, talamus);
            talamusManager.TalentView.SetDependencies(dependencies);
            _createdTalents.Add(talamus.Name, talamusManager);
            return talamusManager;
        }

        private AstraManager CreateAstra(AstraScriptableObject astra)
        {
            List<TalentView> dependencies = CreateDependencies(astra.Dependencies);
            AstraManager astraManager = Instantiate(_astraPrefab);
            astraManager.Initialize(_userInputController, astra);
            astraManager.TalentView.SetDependencies(dependencies);
            _createdTalents.Add(astra.Name, astraManager);
            return astraManager;
        }

        public void CheckForChanges()
        {

            float minX = Screen.width;
            float minY = Screen.height;
            float maxX = 0;
            float maxY = 0;

            foreach (TalentManager talentManager in _createdTalents.Values)
            {
                Vector2 talentViewPosition = talentManager.TalentView.transform.position;
                Debug.Log(talentViewPosition);
                Vector2 elementSize = ((RectTransform)talentManager.TalentView.transform).sizeDelta;
                if (minX > talentViewPosition.x)
                {
                    minX = talentViewPosition.x - elementSize.x / 2;
                }
                if (maxX < talentViewPosition.x)
                {
                    maxX = talentViewPosition.x + elementSize.x / 2;
                }

                if (minY > talentViewPosition.y)
                {
                    minY = talentViewPosition.y - elementSize.y / 2;
                }
                if (maxY < talentViewPosition.y)
                {
                    maxY = talentViewPosition.y - elementSize.y / 2;
                }
            }
            ((RectTransform)transform).sizeDelta = new Vector2(maxX - minX, maxY - minY);
        }
    }
}