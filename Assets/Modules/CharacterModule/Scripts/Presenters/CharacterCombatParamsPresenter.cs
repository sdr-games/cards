using SDRGames.Whist.PointsModule.Presenters;
using SDRGames.Whist.PointsModule.Models;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;

namespace SDRGames.Whist.CharacterModule.Presenters
{
    public class CharacterCombatParamsPresenter
    {
        private CharacterParamsModel _characterParamsModel;
        private CharacterCombatUIView _characterCombatUIView;

        public CharacterCombatParamsPresenter(CharacterParamsModel characterParamsModel, CharacterCombatUIView characterCombatUIView)
        {
            _characterParamsModel = characterParamsModel;
            _characterCombatUIView = characterCombatUIView;

            new PointsBarPresenter(_characterParamsModel.HealthPoints, _characterCombatUIView.HealthPointsBarView);
            new PointsBarPresenter(_characterParamsModel.ArmorPoints, _characterCombatUIView.ArmorPointsBarView);
            new PointsBarPresenter(_characterParamsModel.BarrierPoints, _characterCombatUIView.BarrierPointsBarView);
        }

        public void TakePhysicalDamage(int damage)
        {
            _characterParamsModel.TakePhysicalDamage(damage);
        }

        public void TakeMagicalDamage(int damage)
        {
            _characterParamsModel.TakeMagicalDamage(damage);
        }

        public void TakeTrueDamage(int damage)
        {
            _characterParamsModel.TakeTrueDamage(damage);
        }

        public void RestoreArmor(float restoration)
        {
            _characterParamsModel.RestoreArmor(restoration);
        }

        public void RestoreBarrier(float restoration)
        {
            _characterParamsModel.RestoreBarrier(restoration);
        }

        public void RestoreHealth(float restoration)
        {
            _characterParamsModel.RestoreHealth(restoration);
        }

        public void RestoreStamina(float restoration)
        {
            _characterParamsModel.RestoreStamina(restoration);
        }

        public void RestoreBreath(float restoration)
        {
            _characterParamsModel.RestoreBreath(restoration);
        }
    }
}
