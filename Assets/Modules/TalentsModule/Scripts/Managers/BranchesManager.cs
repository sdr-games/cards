using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.HelpersModule;

using UnityEngine;
using System;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class BranchesManager : MonoBehaviour
    {
        [SerializeField] private BranchManager _branchManagerPrefab;
        [SerializeField] private TalentsBranchScriptableObject[] _talentBranchesSO;

        private UserInputController _userInputController;
        private float _startScale;
        private float _rotationOffset;
        private List<BranchManager> _createdBranches;

        public event EventHandler<AstraChangedEventArgs> AstraIncreased;
        public event EventHandler<AstraChangedEventArgs> AstraDecreased;
        public event EventHandler<TalamusChangedEventArgs> TalamusIncreased;
        public event EventHandler<TalamusChangedEventArgs> TalamusDecreased;

        public void Initialize(UserInputController userInputController)
        {
            _userInputController = userInputController;

            Vector2 totalSize = CalculateBranchesTotalSize();

            _createdBranches = new List<BranchManager>();
            _startScale = 0.6f;
            _rotationOffset = transform.localEulerAngles.z;

            for (int i = 0; i < _talentBranchesSO.Length; i++)
            {
                BranchManager branchManager = Instantiate(_branchManagerPrefab);
                Vector2 position = CalculatePositionInRadius(i, totalSize * 0.3f);
                branchManager.Initialize(_userInputController, _talentBranchesSO[i], position, _startScale, transform);
                branchManager.AstraChanged += OnAstraChanged;
                branchManager.TalamusChanged += OnTalamusChanged;
                branchManager.GetView().BranchZoomInStarted += OnBranchZoomIn;
                branchManager.GetView().BranchZoomOutStarted += OnBranchZoomOut;
                _createdBranches.Add(branchManager);
            }
        }

        private void OnAstraChanged(object sender, AstraChangedEventArgs e)
        {
            if (e.TotalPoints > 0)
            {
                AstraIncreased?.Invoke(this, e);
                return;
            }
            AstraDecreased?.Invoke(this, e);
        }

        private void OnTalamusChanged(object sender, TalamusChangedEventArgs e)
        {
            if (e.TotalPoints > 0)
            {
                TalamusIncreased?.Invoke(this, e);
                return;
            }
            TalamusDecreased?.Invoke(this, e);
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_branchManagerPrefab), _branchManagerPrefab);
            this.CheckFieldValueIsNotNull(nameof(_talentBranchesSO), _talentBranchesSO);
            this.CheckFieldValueIsNotEmpty(nameof(_talentBranchesSO), _talentBranchesSO);
        }

        private void OnDestroy()
        {
            foreach (BranchManager branchManager in _createdBranches)
            {
                branchManager.AstraChanged -= OnAstraChanged;
                branchManager.TalamusChanged -= OnTalamusChanged;
                branchManager.GetView().BranchZoomInStarted -= OnBranchZoomIn;
                branchManager.GetView().BranchZoomOutStarted -= OnBranchZoomOut;
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
                BranchView branchView = branchManager.GetView();
                branchView.SetSizeSmoothly(1, e.Time);
                if(sender as BranchView == branchView)
                {
                    branchManager.GetView().Show();
                    continue;
                }
                branchView.Hide();
            }
            StartCoroutine(RotateSmoothlyCoroutine(e.Angle, e.Time));
            StartCoroutine(MoveSmoothlyCoroutine(-180, e.Time));
        }

        private void OnBranchZoomOut(object sender, BranchZoomedEventArgs e)
        {
            foreach (BranchManager branchManager in _createdBranches)
            {
                BranchView branchView = branchManager.GetView();
                branchView.SetSizeSmoothly(_startScale, e.Time);
                branchView.Show();
            }
            StartCoroutine(RotateSmoothlyCoroutine(e.Angle, e.Time));
            StartCoroutine(MoveSmoothlyCoroutine(180, e.Time));
        }

        private IEnumerator RotateSmoothlyCoroutine(float targetAngle, float time)
        {
            yield return null;
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
