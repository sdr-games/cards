using SDRGames.Whist.DialogueSystem.Views;
using SDRGames.Whist.BezierModule.Views;
using SDRGames.Whist.ChronotopMapModule.Controllers;
using SDRGames.Whist.ChronotopMapModule.Models;
using SDRGames.Whist.ChronotopMapModule.Views;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;
using SDRGames.Whist.DialogueSystem.Managers;

namespace SDRGames.Whist.ChronotopMapModule.Managers
{
    public class ChronotopMapPinManager : MonoBehaviour
    {
        [SerializeField] private ChronotopMapFightPinModel _chronotopMapFightPinModel;
        [SerializeField] private ChronotopMapTownPinModel _chronotopMapTownPinModel;
        [SerializeField] private ChronotopMapPinView _chronotopMapPinView;
        [SerializeField] private Button _button;

        [SerializeField] private BezierView _bezierView;

        [SerializeField] private DialogueManager dialogue;
        
        [field: SerializeField] public ChronotopMapPinController ChronotopMapPinController { get; private set; }

        public void Initialize()
        {
            _chronotopMapPinView.Initialize(_button);
            ChronotopMapPinController.Initialize(_chronotopMapPinView, _button, _bezierView);
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
    }
}
