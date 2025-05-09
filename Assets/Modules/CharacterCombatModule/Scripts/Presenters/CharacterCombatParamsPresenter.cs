using SDRGames.Whist.PointsModule.Presenters;
using SDRGames.Whist.CharacterCombatModule.Views;
using SDRGames.Whist.CharacterCombatModule.Models;

namespace SDRGames.Whist.CharacterCombatModule.Presenters
{
    public class CharacterCombatParamsPresenter
    {
        protected CharacterParamsModel _characterParamsModel;
        private CharacterCombatUIView _characterCombatUIView;

        public CharacterCombatParamsPresenter(CharacterParamsModel characterParamsModel, CharacterCombatUIView characterCombatUIView)
        {
            _characterParamsModel = characterParamsModel;
            _characterCombatUIView = characterCombatUIView;

            new PointsBarPresenter(_characterParamsModel.HealthPoints, _characterCombatUIView.HealthPointsBarView);
            new PointsBarPresenter(_characterParamsModel.ArmorPoints, _characterCombatUIView.ArmorPointsBarView);
            new PointsBarPresenter(_characterParamsModel.BarrierPoints, _characterCombatUIView.BarrierPointsBarView);
        }

        public void TakePhysicalDamage(int damage, bool isCritical)
        {
            _characterParamsModel.TakePhysicalDamage(damage, isCritical);
        }

        public void TakeMagicalDamage(int damage, bool isCritical)
        {
            _characterParamsModel.TakeMagicalDamage(damage, isCritical);
        }

        public void TakeTrueDamage(int damage, bool isCritical)
        {
            _characterParamsModel.TakeTrueDamage(damage, isCritical);
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
