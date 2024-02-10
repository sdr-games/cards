using SDRGames.Islands.PointsModule.Presenters;
using SDRGames.Whist.CharacterModule.Models;
using SDRGames.Whist.CharacterModule.Views;

namespace SDRGames.Whist.CharacterModule.Presenters
{
    public class CombatPlayerCharacterParamsPresenter : CombatCommonCharacterParamsPresenter
    {
        public CombatPlayerCharacterParamsPresenter(CommonCharacterParamsModel commonCharacterParamsModel, CombatPlayerCharacterParamsView combatPlayerCharacterParamsView) : base(commonCharacterParamsModel, combatPlayerCharacterParamsView)
        {
            new PointsBarPresenter(commonCharacterParamsModel.StaminaPoints, combatPlayerCharacterParamsView.StaminaPointsBarView);
            new PointsBarPresenter(commonCharacterParamsModel.BreathPoints, combatPlayerCharacterParamsView.BreathPointsBarView);
        }
    }
}
