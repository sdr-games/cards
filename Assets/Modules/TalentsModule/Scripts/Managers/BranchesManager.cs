using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class BranchesManager : MonoBehaviour
    {
        [SerializeField] private float _startScale;
        [SerializeField] private BranchManager _branchManagerPrefab;
        [SerializeField] private UserInputController _userInputController;
        [SerializeField] private TalentsBranchScriptableObject[] _talentBranchesSO;

        private void OnEnable()
        {
            if (_startScale == 0)
            {
                Debug.LogError("Start Scale не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_branchManagerPrefab == null)
            {
                Debug.LogError("Branch Manager Prefab не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_userInputController == null)
            {
                Debug.LogError("User Input Controller не были назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_talentBranchesSO.Length == 0)
            {
                Debug.LogError("Talent Branches не были назначены");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
            //transform.localScale = new Vector3(_startScale, _startScale);

            for (int i = 0; i < _talentBranchesSO.Length; i++)
            {
                BranchManager branchManager = Instantiate(_branchManagerPrefab);
                branchManager.Initialize(_userInputController, _talentBranchesSO[i]);
                Vector2 position = CalculatePositionInRadius(i, _startScale);
                branchManager.BranchView.SetPositionAndSize(position, new Vector2(_startScale, _startScale));
                branchManager.transform.SetParent(transform, false);
                branchManager.BranchView.SetRotation();
            }
        }

        private Vector2 CalculatePositionInRadius(int index, float scale)
        {
            float radius = Screen.width * scale;
            float offsetY = (Screen.height / 2 + BranchView.PADDING.y / 2) * scale;
            float radiansOfSeparation = Mathf.PI / _talentBranchesSO.Length * (index + 0.5f);
            return new Vector2(Mathf.Cos(radiansOfSeparation) * radius, Mathf.Sin(radiansOfSeparation) * radius - offsetY);
        }
    }
}
