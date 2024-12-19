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
        private float _rotationOffset;
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

            Vector2 totalSize = CalculateBranchesTotalSize();

            _createdBranches = new List<BranchManager>();
            _startScale = 0.6f;
            _rotationOffset = transform.localEulerAngles.z;

            for (int i = 0; i < _talentBranchesSO.Length; i++)
            {
                BranchManager branchManager = Instantiate(_branchManagerPrefab);
                Vector2 position = CalculatePositionInRadius(i, totalSize * 0.3f);
                branchManager.Initialize(_userInputController, _talentBranchesSO[i], position, _startScale, transform);
                branchManager.BranchView.BranchZoomInStarted += OnBranchZoomIn;
                branchManager.BranchView.BranchZoomOutStarted += OnBranchZoomOut;
                _createdBranches.Add(branchManager);
            }
        }

        private void OnDestroy()
        {
            foreach (BranchManager branchManager in _createdBranches)
            {
                branchManager.BranchView.BranchZoomInStarted -= OnBranchZoomIn;
                branchManager.BranchView.BranchZoomOutStarted -= OnBranchZoomOut;
                StopAllCoroutines();
            }
        }

        private Vector2 CalculatePositionInRadius(int index, Vector2 size)
        {
            float radius = size.x / 2;
            float angle = Mathf.PI * index / _talentBranchesSO.Length;
            return new Vector2(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius);
        }

        private void OnBranchZoomIn(object sender, BranchZoomedEventArgs e)
        {
            foreach (BranchManager branchManager in _createdBranches)
            {
                branchManager.BranchView.SetSizeSmoothly(1, e.Time);
                if(sender as BranchView == branchManager.BranchView)
                {
                    branchManager.BranchView.Show();
                    continue;
                }
                branchManager.BranchView.Hide();
            }
            StartCoroutine(RotateSmoothlyCoroutine(e.Angle, e.Time));
            StartCoroutine(MoveSmoothlyCoroutine(-180, e.Time));
        }

        private void OnBranchZoomOut(object sender, BranchZoomedEventArgs e)
        {
            foreach (BranchManager branchManager in _createdBranches)
            {
                branchManager.BranchView.SetSizeSmoothly(_startScale, e.Time);
                branchManager.BranchView.Show();
            }
            StartCoroutine(RotateSmoothlyCoroutine(e.Angle, e.Time));
            StartCoroutine(MoveSmoothlyCoroutine(180, e.Time));
        }

        private IEnumerator RotateSmoothlyCoroutine(float targetAngle, float time)
        {
            yield return null;
            float step = _rotationOffset / time;
            float currentAngle = _rotationOffset;
            if(targetAngle < 0)
            {
                targetAngle = _rotationOffset;
                currentAngle = transform.localEulerAngles.z;
            }
            Quaternion a = Quaternion.Euler(0, 0, currentAngle);
            Quaternion b = Quaternion.Euler(0, 0, targetAngle);
            float currentTime = 0;
            while (currentTime < time)
            {
                yield return null;
                transform.rotation = Quaternion.Lerp(a, b, currentTime / time);
                currentTime++;
            }
            Debug.Log(currentTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, targetAngle);
        }

        private IEnumerator MoveSmoothlyCoroutine(float distance, float time)
        {
            yield return null;
            float step = distance / time;
            float currentTime = 0;
            Vector3 newPosition = transform.localPosition;
            while (currentTime < time)
            {
                yield return null;
                newPosition.y += step;
                transform.localPosition = newPosition;
                currentTime++;
            }
            Debug.Log(currentTime);
        }

        private Vector2 CalculateBranchesTotalSize()
        {
            Vector2 totalSize = Vector2.zero;
            foreach(TalentsBranchScriptableObject talentsBranchScriptableObject in _talentBranchesSO)
            {
                totalSize += talentsBranchScriptableObject.Background.rect.size;
            }
            return totalSize;
        }
    }
}
