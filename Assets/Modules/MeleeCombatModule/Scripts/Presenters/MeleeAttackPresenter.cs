using SDRGames.Whist.MeleeCombatModule.Views;
using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;

using SDRGames.Whist.UserInputModule.Controller;
using System;

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
        }
    }
}
