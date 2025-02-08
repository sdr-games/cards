using SDRGames.Whist.PointsModule.Presenters;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;

namespace SDRGames.Whist.CharacterModule.Presenters
{
    public class CharacterCombatParamsPresenter
    {
        private CharacterParamsModel _characterParamsModel;

        public CharacterCombatParamsPresenter(CharacterParamsModel characterParamsModel, CharacterCombatParamsView characterCombatParamsView)
        {
            _characterParamsModel = characterParamsModel;

            new PointsBarPresenter(_characterParamsModel.HealthPoints, characterCombatParamsView.HealthPointsBarView);
            new PointsBarPresenter(_characterParamsModel.ArmorPoints, characterCombatParamsView.ArmorPointsBarView);
            new PointsBarPresenter(_characterParamsModel.BarrierPoints, characterCombatParamsView.BarrierPointsBarView);
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
