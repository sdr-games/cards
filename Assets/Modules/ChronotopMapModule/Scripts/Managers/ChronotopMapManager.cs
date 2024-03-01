using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.ChronotopMapModule.Managers
{
    public class ChronotopMapManager : MonoBehaviour
    {
        [SerializeField] private ChronotopMapPinManager[] _chronotopMapPinManagers;

        private void OnEnable()
        {
            if (_chronotopMapPinManagers.Length == 0)
            {
                Debug.LogError("Chronotop Map Pin Managers не были назначены");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            foreach (var pinManager in _chronotopMapPinManagers)
            {
                pinManager.Initialize();
            } 
        }
    }
}
