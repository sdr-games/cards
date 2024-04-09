using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule
{

    public class BranchManager : MonoBehaviour
    {
        [SerializeField] private TalentsBranchScriptableObject _talentBranch;
        [SerializeField] private AstraManager _astraPrefab;
        [SerializeField] private TalamusManager _talamusPrefab;
        [SerializeField] private UserInputController _userInputController;

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
            
            foreach(TalentScriptableObject talent in _talentBranch.Talents)
            {
                if(talent is TalamusScriptableObject talamus)
                {
                    TalamusManager talamusManager = Instantiate(_talamusPrefab, transform, false);
                    talamusManager.Initialize(_userInputController, talamus);
                    continue;
                }

                if (talent is AstraScriptableObject astra)
                {
                    AstraManager astraManager = Instantiate(_astraPrefab, transform, false);
                    astraManager.Initialize(_userInputController, astra);
                    continue;
                }
            }
        }
    }
}