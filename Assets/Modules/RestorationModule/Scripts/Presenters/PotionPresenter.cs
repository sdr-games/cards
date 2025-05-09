using SDRGames.Whist.CharacterCombatModule.Models;
using SDRGames.Whist.RestorationModule.Models;
using SDRGames.Whist.RestorationModule.Views;

namespace SDRGames.Whist.RestorationModule.Presenters
{
    public class PotionPresenter
    {
        private Potion _potion;
        private PotionView _potionView;

        public PotionPresenter(Potion potion, PotionView meleeAttackView, CharacterParamsModel characterParamsModel)
        {
            _potion = potion;

            _potionView = meleeAttackView;
            _potionView.Initialize(_potion, characterParamsModel);
        }
    }
}
