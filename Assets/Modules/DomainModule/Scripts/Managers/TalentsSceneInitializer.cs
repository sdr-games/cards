using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.TalentsModule.Managers;
using SDRGames.Whist.CharacterModule.ScriptableObjects;

using UnityEngine;

using static SDRGames.Whist.TalentsModule.Models.Talamus;
using System;

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
            _branchesManager.AstraIncreased += OnAstraIncreased;
            _branchesManager.AstraDecreased += OnAstraDecreased;
            _branchesManager.TalamusIncreased += OnTalamusIncreased;
            _branchesManager.TalamusDecreased += OnTalamusDecreased;
        }

        private void OnAstraIncreased(object sender, AstraChangedEventArgs e)
        {
            
        }

        private void OnAstraDecreased(object sender, AstraChangedEventArgs e)
        {
            
        }

        private void OnTalamusIncreased(object sender, TalamusChangedEventArgs e)
        {
            if(e.TotalPoints == 0 || _playerCharacterParamsModel.TalentPoints == 0)
            {
                return;
            }

            int amount = e.TotalPoints * e.Talamus.CharacteristicValuePerPoint;
            e.Talamus.IncreaseCurrentPoints();
            switch (e.Talamus.Characteristic)
            {
                case CharacteristicNames.Strength:
                    _playerCharacterParamsModel.ChangeStrength(amount);
                    _playerCharacterParamsModel.DecreaseTalentPoints();
                    break;
                case CharacteristicNames.Agility:
                    _playerCharacterParamsModel.ChangeAgility(amount);
                    _playerCharacterParamsModel.DecreaseTalentPoints();
                    break;
                case CharacteristicNames.Stamina:
                    _playerCharacterParamsModel.ChangeStamina(amount);
                    _playerCharacterParamsModel.DecreaseTalentPoints();
                    break;
                case CharacteristicNames.Intellegence:
                    _playerCharacterParamsModel.ChangeIntelligence(amount);
                    _playerCharacterParamsModel.DecreaseTalentPoints();
                    break;
                default:
                    break;
            }
        }

        private void OnTalamusDecreased(object sender, TalamusChangedEventArgs e)
        {
            if(e.TotalPoints == 0)
            {
                return;
            }

            int amount = e.TotalPoints * e.Talamus.CharacteristicValuePerPoint;
            e.Talamus.DecreaseCurrentPoints();
            switch (e.Talamus.Characteristic)
            {
                case CharacteristicNames.Strength:
                    _playerCharacterParamsModel.ChangeStrength(amount);
                    _playerCharacterParamsModel.IncreaseTalentPoints();
                    break;
                case CharacteristicNames.Agility:
                    _playerCharacterParamsModel.ChangeAgility(amount);
                    _playerCharacterParamsModel.IncreaseTalentPoints();
                    break;
                case CharacteristicNames.Stamina:
                    _playerCharacterParamsModel.ChangeStamina(amount);
                    _playerCharacterParamsModel.IncreaseTalentPoints();
                    break;
                case CharacteristicNames.Intellegence:
                    _playerCharacterParamsModel.ChangeIntelligence(amount);
                    _playerCharacterParamsModel.IncreaseTalentPoints();
                    break;
                default:
                    break;
            }
        }
    }
}
