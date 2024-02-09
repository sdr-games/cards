using SDRGames.Islands.PointsModule.Presenters;
using SDRGames.Whist.CharacterModule.Models;
using SDRGames.Whist.CharacterModule.Views;

namespace SDRGames.Whist.CharacterModule.Presenters
{
    public class CombatCommonCharacterParamsPresenter
    {
        private CommonCharacterParamsModel _commonCharacterParamsModel;
        private CombatCommonCharacterParamsView _combatCommonCharacterParamsView;

        public CombatCommonCharacterParamsPresenter(CommonCharacterParamsModel commonCharacterParamsModel, CombatCommonCharacterParamsView combatCommonCharacterParamsView)
        {
            _commonCharacterParamsModel = commonCharacterParamsModel;
            _combatCommonCharacterParamsView = combatCommonCharacterParamsView;

            new PointsBarPresenter(_commonCharacterParamsModel.HealthPoints, _combatCommonCharacterParamsView.HealthPointsBarView);
            new PointsBarPresenter(_commonCharacterParamsModel.Armor, _combatCommonCharacterParamsView.ArmorPointsBarView);
            new PointsBarPresenter(_commonCharacterParamsModel.Barrier, _combatCommonCharacterParamsView.BarrierPointsBarView);
        }
    }
}
