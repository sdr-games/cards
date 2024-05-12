using SDRGames.Whist.PointsModule.Presenters;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;

namespace SDRGames.Whist.CharacterModule.Presenters
{
    public class CommonCharacterCombatParamsPresenter
    {
        public CommonCharacterCombatParamsPresenter(CommonCharacterParamsModel commonCharacterParamsModel, CombatCommonCharacterParamsView combatCommonCharacterParamsView)
        {
            new PointsBarPresenter(commonCharacterParamsModel.HealthPoints, combatCommonCharacterParamsView.HealthPointsBarView);
            new PointsBarPresenter(commonCharacterParamsModel.Armor, combatCommonCharacterParamsView.ArmorPointsBarView);
            new PointsBarPresenter(commonCharacterParamsModel.Barrier, combatCommonCharacterParamsView.BarrierPointsBarView);
        }
    }
}
