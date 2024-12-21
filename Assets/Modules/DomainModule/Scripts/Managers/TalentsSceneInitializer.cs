using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.TalentsModule.Managers;
using SDRGames.Whist.CharacterModule.ScriptableObjects;

using UnityEngine;

using static SDRGames.Whist.TalentsModule.Models.Talamus;

namespace SDRGames.Whist.DomainModule
{
    public class TalentsSceneInitializer : MonoBehaviour
    {
        [SerializeField] private UserInputController _userInputController;
        [SerializeField] private PlayerCharacterParamsModel _playerCharacterParamsModel;
        [SerializeField] private BranchesManager _branchesManager;

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_userInputController), _userInputController);
            this.CheckFieldValueIsNotNull(nameof(_playerCharacterParamsModel), _playerCharacterParamsModel);
            this.CheckFieldValueIsNotNull(nameof(_branchesManager), _branchesManager);

            _branchesManager.Initialize(_userInputController);
            _branchesManager.AstraChanged += OnAstraChanged;
            _branchesManager.TalamusChanged += OnTalamusChanged;
        }

        private void OnAstraChanged(object sender, AstraChangedEventArgs e)
        {
            
        }

        private void OnTalamusChanged(object sender, TalamusChangedEventArgs e)
        {
            if(e.TotalPoints == 0)
            {
                return;
            }

            int amount = e.TotalPoints * e.Talamus.CharacteristicValuePerPoint;
            switch (e.Talamus.Characteristic)
            {
                case CharacteristicNames.Strength:
                    _playerCharacterParamsModel.ChangeStrength(amount);
                    break;
                case CharacteristicNames.Agility:
                    _playerCharacterParamsModel.ChangeAgility(amount);
                    break;
                case CharacteristicNames.Stamina:
                    _playerCharacterParamsModel.ChangeStamina(amount);
                    break;
                case CharacteristicNames.Intellegence:
                    _playerCharacterParamsModel.ChangeIntelligence(amount);
                    break;
                default:
                    break;
            } 
        }
    }
}
