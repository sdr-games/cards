using System;
using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class BranchesManager : MonoBehaviour
    {
        [SerializeField] private BranchManager _branchManagerPrefab;
        [SerializeField] private UserInputController _userInputController;
        [SerializeField] private TalentsBranchScriptableObject[] _talentBranchesSO;

        private float _startScale;
        private List<BranchManager> _createdBranches;

        private void OnEnable()
        {
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

            _createdBranches = new List<BranchManager>();
            _startScale = 1 - Screen.width / (_talentBranchesSO.Length * BranchView.SIZE.x);

            for (int i = 0; i < _talentBranchesSO.Length; i++)
            {
                BranchManager branchManager = Instantiate(_branchManagerPrefab);
                Vector2 position = CalculatePositionInRadius(i, _startScale);
                branchManager.Initialize(_userInputController, _talentBranchesSO[i], position, _startScale, transform);
                branchManager.BranchView.BranchZoomed += OnBranchZoomed;
                branchManager.BranchView.BranchUnzoomed += OnBranchUnzoomed;
                _createdBranches.Add(branchManager);
            }
        }

        private void OnDestroy()
        {
            foreach (BranchManager branchManager in _createdBranches)
            {
                branchManager.BranchView.BranchZoomed -= OnBranchZoomed;
                branchManager.BranchView.BranchZoomed -= OnBranchUnzoomed;
                StopAllCoroutines();
            }
        }

        private Vector2 CalculatePositionInRadius(int index, float scale)
        {
            float radius = (Screen.width - BranchView.SIZE.x / _talentBranchesSO.Length) * scale;
            float offsetY = BranchView.PADDING.y / 2 * scale;
            float radiansOfSeparation = Mathf.PI / _talentBranchesSO.Length * (index + 0.5f);
            return new Vector2(Mathf.Cos(radiansOfSeparation) * radius, Mathf.Sin(radiansOfSeparation) * radius - offsetY);
        }

        private void OnBranchZoomed(object sender, BranchZoomedEventArgs e)
        {
            foreach (BranchManager branchManager in _createdBranches)
            {
                branchManager.BranchView.SetSizeSmoothly(1);
                if(sender as BranchView != branchManager.BranchView)
                {
                    branchManager.BranchView.Hide();
                } 
            }
            StartCoroutine(RotateSmoothlyCoroutine(e.Angle, e.Time));
        }

        private void OnBranchUnzoomed(object sender, BranchZoomedEventArgs e)
        {
            foreach (BranchManager branchManager in _createdBranches)
            {
                branchManager.BranchView.SetSizeSmoothly(_startScale);
                branchManager.BranchView.Show();
            }
            StartCoroutine(RotateSmoothlyCoroutine(e.Angle, e.Time));
        }

        private IEnumerator RotateSmoothlyCoroutine(float angle, float time)
        {
            yield return null;
            Debug.Log(angle);
            float destinationAngle = angle != 0 ? 360 - transform.localEulerAngles.z - Math.Abs(angle) : 0;
            float direction = angle > 0 ? -1 : 1;
            float speed = Math.Abs(angle / time);
            Debug.Log(speed);
            while(Math.Abs(transform.localEulerAngles.z - destinationAngle) > speed)
            {
                yield return null;
                transform.RotateAround(transform.TransformPoint(((RectTransform)transform).rect.center), Vector3.forward * direction, speed);
            } 
        }
    }
}
