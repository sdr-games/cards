using SDRGames.Whist.MeleeCombatModule.Views;
using SDRGames.Whist.MeleeCombatModule.Models;

namespace SDRGames.Whist.MeleeCombatModule.Presenters
{
    public class MeleeAttackPresenter
    {
        private MeleeAttack _meleeAttack;
        private MeleeAttackView _meleeAttackView;

        public MeleeAttackPresenter(MeleeAttack meleeAttack, MeleeAttackView meleeAttackView)
        {
            _meleeAttack = meleeAttack;

            _meleeAttackView = meleeAttackView;
            _meleeAttackView.Initialize(_meleeAttack);
        }
    }
}
