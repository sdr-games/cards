using SDRGames.Whist.RestorationModule.ScriptableObjects;
using SDRGames.Whist.RestorationModule.Views;

namespace SDRGames.Whist.RestorationModule.Presenters
{
    public class PotionPresenter
    {
        private PotionScriptableObject _potionScriptableObject;
        private PotionView _potionView;

        public PotionPresenter(PotionScriptableObject meleeAttackScriptableObject, PotionView meleeAttackView)
        {
            _potionScriptableObject = meleeAttackScriptableObject;

            _potionView = meleeAttackView;
            _potionView.Initialize(_potionScriptableObject);
        }
    }
}
