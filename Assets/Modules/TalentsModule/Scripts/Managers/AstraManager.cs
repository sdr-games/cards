using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.Presenters;
using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class AstraManager : TalentManager
    {
        private AstraScriptableObject _astraScriptableObject;
        private AstraPresenter _astraPresenter;

        public void Initialize(UserInputController userInputController, AstraScriptableObject astraScriptableObject)
        {
            base.Initialize(userInputController, astraScriptableObject);
            _astraScriptableObject = astraScriptableObject;

            _astraPresenter = new AstraPresenter(_astraScriptableObject, TalentView);
        }

        protected override void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                //_astraScriptableObject.ChangeActive();
            }
        }
    }
}
