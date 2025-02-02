using SDRGames.Whist.MeleeCombatModule.Views;
using SDRGames.Whist.MeleeCombatModule.Models;

namespace SDRGames.Whist.MeleeCombatModule.Presenters
{
    public class MeleeAttackPresenter
    {
        private MeleeAttack _meleeAttack;
        private MeleeAttackView _meleeAttackView;

        public MeleeAttackPresenter(MeleeAttack meleeAttackScriptableObject, MeleeAttackView meleeAttackView)
        {
            _meleeAttack = meleeAttackScriptableObject;

            _meleeAttackView = meleeAttackView;
            _meleeAttackView.Initialize(_meleeAttack);
        }
    }
}
