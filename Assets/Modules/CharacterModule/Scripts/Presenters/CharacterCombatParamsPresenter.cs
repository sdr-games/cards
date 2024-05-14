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
            new PointsBarPresenter(_characterParamsModel.Armor, characterCombatParamsView.ArmorPointsBarView);
            new PointsBarPresenter(_characterParamsModel.Barrier, characterCombatParamsView.BarrierPointsBarView);
        }

        public void TakeDamage(int damage)
        {
            _characterParamsModel.TakeDamage(damage);
        }
    }
}
