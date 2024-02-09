using SDRGames.Whist.CharacterModule.Models;
using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.Views;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class PlayerCharacterManager : MonoBehaviour
    {
        public static PlayerCharacterManager Instance { get; private set; }

        [SerializeField] private PlayerCharacterParamsModel _playerCharacterParamsModel;
        [SerializeField] private PlayerCharacterParamsView _playerCharacterParamsView;

        private PlayerCharacterParamsPresenter _playerCharacterParamsPresenter;

        private void OnEnable()
        {
            Initialize();
            InitializePlayerCharacterParamsPresenter();
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
    }
}
