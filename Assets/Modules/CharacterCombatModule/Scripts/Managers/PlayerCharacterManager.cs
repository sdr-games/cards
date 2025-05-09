using SDRGames.Whist.CharacterCombatModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Presenters;
using SDRGames.Whist.CharacterCombatModule.Views;
using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.CharacterCombatModule.Managers
{
    public class PlayerCharacterManager : MonoBehaviour
    {
        [SerializeField] private PlayerParamsScriptableObject _playerCharacterParamsModel;
        [SerializeField] private PlayerCharacterParamsView _playerCharacterParamsView;

        private PlayerCharacterParamsPresenter _playerCharacterParamsPresenter;

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_playerCharacterParamsView), _playerCharacterParamsView);
            Initialize();
        }

        public void Initialize()
        {
            _playerCharacterParamsPresenter = new PlayerCharacterParamsPresenter(_playerCharacterParamsModel, _playerCharacterParamsView);
        }

        public void Test()
        {
            _playerCharacterParamsModel.IncreaseLevel(1);
            _playerCharacterParamsModel.IncreaseExperience(10);
            _playerCharacterParamsModel.IncreaseStrength(1);
            _playerCharacterParamsModel.IncreaseAgility(1);
            _playerCharacterParamsModel.IncreaseStamina(1);
            _playerCharacterParamsModel.IncreaseIntelligence(1);
        }
    }
}
