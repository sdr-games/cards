using SDRGames.Whist.MeleeCombatModule.Views;
using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;

using SDRGames.Whist.UserInputModule.Controller;

namespace SDRGames.Whist.MeleeCombatModule.Presenters
{
    public class MeleeAttackPresenter
    {
        private MeleeAttackScriptableObject _meleeAttackScriptableObject;
        private MeleeAttackView _meleeAttackView;

        public MeleeAttackPresenter(UserInputController userInputController, MeleeAttackScriptableObject meleeAttackScriptableObject, MeleeAttackView meleeAttackView)
        {
            _meleeAttackScriptableObject = meleeAttackScriptableObject;

            _meleeAttackView = meleeAttackView;
            _meleeAttackView.Initialize(userInputController, _meleeAttackScriptableObject);
            _meleeAttackView.MeleeAttackClicked += OnMeleeAttackClicked;
        }

        private void OnMeleeAttackClicked(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
