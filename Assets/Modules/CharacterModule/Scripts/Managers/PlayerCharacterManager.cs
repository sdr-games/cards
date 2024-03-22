using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.Views;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class PlayerCharacterManager : MonoBehaviour
    {
        public static PlayerCharacterManager Instance { get; private set; }

        [SerializeField] private PlayerCharacterParamsModel _playerCharacterParamsModel;
        [SerializeField] private PlayerCharacterParamsView _playerCharacterParamsView;
        [SerializeField] private CombatPlayerCharacterParamsView _combatPlayerCharacterParamsView;

        private PlayerCharacterParamsPresenter _playerCharacterParamsPresenter;

        private void OnEnable()
        {
            if (_playerCharacterParamsView == null)
            {
                Debug.LogError("Player Character Params View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_combatPlayerCharacterParamsView == null)
            {
                Debug.LogError("Combat Player Character Params View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            Initialize();
        }

        public void Initialize()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void InitializePlayerCharacterParamsPresenter()
        {
            _playerCharacterParamsPresenter = new PlayerCharacterParamsPresenter(_playerCharacterParamsModel, _playerCharacterParamsView);
        }

        public void InitializeCombatPlayerCharacterParamsPresenter()
        {
            new CombatPlayerCharacterParamsPresenter(_playerCharacterParamsModel, _combatPlayerCharacterParamsView);
        }
    }
}
