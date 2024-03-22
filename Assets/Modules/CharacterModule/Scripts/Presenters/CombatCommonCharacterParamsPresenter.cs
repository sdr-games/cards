using SDRGames.Whist.PointsModule.Presenters;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;

namespace SDRGames.Whist.CharacterModule.Presenters
{
    public class CombatCommonCharacterParamsPresenter
    {
        public CombatCommonCharacterParamsPresenter(CommonCharacterParamsModel commonCharacterParamsModel, CombatCommonCharacterParamsView combatCommonCharacterParamsView)
        {
            new PointsBarPresenter(commonCharacterParamsModel.HealthPoints, combatCommonCharacterParamsView.HealthPointsBarView);
            new PointsBarPresenter(commonCharacterParamsModel.Armor, combatCommonCharacterParamsView.ArmorPointsBarView);
            new PointsBarPresenter(commonCharacterParamsModel.Barrier, combatCommonCharacterParamsView.BarrierPointsBarView);
        }
    }
}
