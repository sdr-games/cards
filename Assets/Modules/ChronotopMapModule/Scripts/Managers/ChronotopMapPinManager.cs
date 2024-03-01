using SDRGames.Whist.ChronotopMapModule.Controllers;
using SDRGames.Whist.ChronotopMapModule.Models;
using SDRGames.Whist.ChronotopMapModule.Views;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.ChronotopMapModule.Managers
{
    public class ChronotopMapPinManager : MonoBehaviour
    {
        [SerializeField] private ChronotopMapFightPinModel _chronotopMapFightPinModel;
        [SerializeField] private ChronotopMapTownPinModel _chronotopMapTownPinModel;
        [SerializeField] private ChronotopMapPinView _chronotopMapPinView;
        [SerializeField] private ChronotopMapPinController _choronotopMapPinController;
        [SerializeField] private Button _button;

        public void Initialize()
        {
            _chronotopMapPinView.Initialize(_button);
            _choronotopMapPinController.Initialize(_chronotopMapPinView, _button);
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

            if (_choronotopMapPinController == null)
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
