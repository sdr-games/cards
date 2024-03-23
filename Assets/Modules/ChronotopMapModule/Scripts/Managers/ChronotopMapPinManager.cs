using SDRGames.Whist.BezierModule.Views;
using SDRGames.Whist.ChronotopMapModule.Controllers;
using SDRGames.Whist.ChronotopMapModule.Models;
using SDRGames.Whist.ChronotopMapModule.Views;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;
using System;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.ChronotopMapModule.Presenters;

namespace SDRGames.Whist.ChronotopMapModule.Managers
{
    public class ChronotopMapPinManager : MonoBehaviour
    {
        [SerializeField] private ChronotopMapFightPinModel _chronotopMapFightPinModel;
        [SerializeField] private ChronotopMapTownPinModel _chronotopMapTownPinModel;
        [SerializeField] private ChronotopMapPinView _chronotopMapPinView;
        [SerializeField] private Button _button;

        [SerializeField] private BezierView _bezierView;

        [SerializeField] private bool _autofinish = false;

        private ChronotopMapPinModalPresenter _modalPresenter;
        
        [field: SerializeField] public ChronotopMapPinController ChronotopMapPinController { get; private set; }

        public event EventHandler<AvailablePinClickedEventArgs> AvailablePinClicked;
        public event EventHandler ReadyPinClicked;
        public event EventHandler DonePinClicked;

        public void Initialize(UserInputController userInputController, ChronotopMapPinModalView modalView)
        {
            _chronotopMapPinView.Initialize(_button);
            ChronotopMapPinController.Initialize(_chronotopMapPinView, userInputController, _bezierView);            
        }

        public void MarkAsAvailable()
        {
            ChronotopMapPinController.MarkAsAvailable();
            ChronotopMapPinController.AvailablePinClicked += OnAvailablePinClick;
            _chronotopMapPinView.MarkAsAvailable();
        }

        public void MarkAsReady(UserInputController userInputController, ChronotopMapPinModalView modalView)
        {
            ChronotopMapPinController.MarkAsReady();
            ChronotopMapPinController.AvailablePinClicked -= OnAvailablePinClick;
            ChronotopMapPinController.ReadyPinClicked += OnReadyPinClick;

            _chronotopMapPinView.MarkAsReady();

            _modalPresenter = new ChronotopMapPinModalPresenter(_chronotopMapFightPinModel.EnemyCharacterParams, modalView, userInputController);
        }

        public void MarkAsDone()
        {
            _modalPresenter.FightButtonClicked -= OnFightButtonClick;
            if (_autofinish)
            {
                MarkAsFinished();
                return;
            }

            ChronotopMapPinController.MarkAsDone();
            ChronotopMapPinController.ReadyPinClicked -= OnReadyPinClick;
            ChronotopMapPinController.DonePinClicked += OnDonePinClick;
            _chronotopMapPinView.MarkAsDone();
        }

        public void MarkAsFinished()
        {
            ChronotopMapPinController.MarkAsFinished();
            ChronotopMapPinController.DonePinClicked -= OnDonePinClick;
            _chronotopMapPinView.MarkAsFinished();
        }

        private void OnFightButtonClick(object sender, EventArgs e)
        {
            //start battle
        }

        private void OnEnable()
        {
            if (_chronotopMapPinView == null)
            {
                Debug.LogError("Chronotop Map Pin View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (ChronotopMapPinController == null)
            {
                Debug.LogError("Chronotop Map Pin Controller не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_button == null)
            {
                Debug.LogError("Button не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }

        private void OnDisable()
        {
            ChronotopMapPinController.AvailablePinClicked -= OnAvailablePinClick;
            if (_modalPresenter != null)
            {
                _modalPresenter.FightButtonClicked -= OnFightButtonClick;
            }
        }

        private void OnAvailablePinClick(object sender, AvailablePinClickedEventArgs e)
        {
            AvailablePinClicked?.Invoke(this, new AvailablePinClickedEventArgs(e.BezierView, _chronotopMapFightPinModel.DialogueContainerScriptableObject));
        }

        private void OnReadyPinClick(object sender, EventArgs e)
        {
            _modalPresenter.ShowView();
            _modalPresenter.FightButtonClicked += OnFightButtonClick;
            ReadyPinClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnDonePinClick(object sender, EventArgs e)
        {
            DonePinClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
