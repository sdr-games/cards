using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.Views;
using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class PlayerCharacterManager : MonoBehaviour
    {
        [SerializeField] private PlayerCharacterParamsModel _playerCharacterParamsModel;
        [SerializeField] private PlayerCharacterParamsView _playerCharacterParamsView;

        private PlayerCharacterParamsPresenter _playerCharacterParamsPresenter;

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_playerCharacterParamsView), _playerCharacterParamsView);
        }

        public void Initialize()
        {
            _playerCharacterParamsPresenter = new PlayerCharacterParamsPresenter(_playerCharacterParamsModel, _playerCharacterParamsView);
        }

        public void Test()
        {
            _playerCharacterParamsModel.IncreaseLevel(1);
            _playerCharacterParamsModel.IncreaseExperience(10);
            _playerCharacterParamsModel.ChangeStrength(1);
            _playerCharacterParamsModel.ChangeAgility(1);
            _playerCharacterParamsModel.ChangeStamina(1);
            _playerCharacterParamsModel.ChangeIntelligence(1);
        }
    }
}
